using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YamlDotNet.Serialization;
using CEEdit.Core.Models.Project;
using CEEdit.Core.Services.Interfaces;
using CEEdit.Core.Models.Common;

namespace CEEdit.Core.Services.Implementations
{
    /// <summary>
    /// 项目历史记录服务实现
    /// </summary>
    public class ProjectHistoryService : IProjectHistoryService
    {
        private readonly string _historyFilePath;
        private readonly object _lockObject = new();
        private ProjectHistory? _cachedHistory;

        /// <summary>
        /// 项目历史记录发生变化时触发
        /// </summary>
        public event Action<ProjectHistory>? HistoryChanged;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectHistoryService()
        {
            // 确保data目录存在
            var dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
            Directory.CreateDirectory(dataDirectory);
            
            _historyFilePath = Path.Combine(dataDirectory, "project_history.json");
        }

        /// <summary>
        /// 构造函数（用于测试）
        /// </summary>
        /// <param name="historyFilePath">历史文件路径</param>
        public ProjectHistoryService(string historyFilePath)
        {
            _historyFilePath = historyFilePath;
            
            // 确保目录存在
            var directory = Path.GetDirectoryName(_historyFilePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// 获取项目历史记录
        /// </summary>
        /// <returns>项目历史记录</returns>
        public async Task<ProjectHistory> GetProjectHistoryAsync()
        {
            lock (_lockObject)
            {
                if (_cachedHistory != null)
                    return _cachedHistory;
            }

            try
            {
                if (File.Exists(_historyFilePath))
                {
                    var json = await File.ReadAllTextAsync(_historyFilePath);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var history = JsonConvert.DeserializeObject<ProjectHistory>(json) ?? new ProjectHistory();
                        
                        lock (_lockObject)
                        {
                            _cachedHistory = history;
                        }
                        
                        return history;
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录错误但不抛出异常，返回新的历史记录
                System.Diagnostics.Debug.WriteLine($"加载项目历史记录失败: {ex.Message}");
            }

            var newHistory = new ProjectHistory();
            lock (_lockObject)
            {
                _cachedHistory = newHistory;
            }
            
            // 首次创建时自动保存到文件
            try
            {
                await SaveProjectHistoryAsync(newHistory);
                System.Diagnostics.Debug.WriteLine($"已创建新的项目历史文件: {_historyFilePath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"创建项目历史文件失败: {ex.Message}");
            }
            
            return newHistory;
        }

        /// <summary>
        /// 保存项目历史记录
        /// </summary>
        /// <param name="history">项目历史记录</param>
        public async Task SaveProjectHistoryAsync(ProjectHistory history)
        {
            try
            {
                var json = JsonConvert.SerializeObject(history, Formatting.Indented);
                await File.WriteAllTextAsync(_historyFilePath, json);
                
                lock (_lockObject)
                {
                    _cachedHistory = history;
                }
                
                HistoryChanged?.Invoke(history);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"保存项目历史记录失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 添加最近打开的项目
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        /// <param name="projectType">项目类型</param>
        /// <param name="description">项目描述</param>
        public async Task AddRecentProjectAsync(string projectPath, string? projectType = null, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(projectPath))
                return;

            var history = await GetProjectHistoryAsync();
            
            // 移除已存在的相同项目
            history.RecentProjects.RemoveAll(p => string.Equals(p.ProjectPath, projectPath, 
                StringComparison.OrdinalIgnoreCase));

            // 创建包含pack.yml信息的项目历史项
            var historyItem = await CreateProjectHistoryItemAsync(projectPath);
            
            // 覆盖手动提供的参数
            if (!string.IsNullOrEmpty(projectType))
                historyItem.ProjectType = projectType;
            
            if (!string.IsNullOrEmpty(description))
                historyItem.Description = description;

            // 添加到列表顶部
            history.RecentProjects.Insert(0, historyItem);

            // 保持最大数量限制
            if (history.RecentProjects.Count > ProjectHistory.MaxRecentProjects)
            {
                history.RecentProjects.RemoveRange(ProjectHistory.MaxRecentProjects, 
                    history.RecentProjects.Count - ProjectHistory.MaxRecentProjects);
            }

            await SaveProjectHistoryAsync(history);
        }

        /// <summary>
        /// 移除最近项目
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        public async Task RemoveRecentProjectAsync(string projectPath)
        {
            var history = await GetProjectHistoryAsync();
            history.RemoveRecentProject(projectPath);
            await SaveProjectHistoryAsync(history);
        }

        /// <summary>
        /// 获取最近项目列表
        /// </summary>
        /// <param name="maxCount">最大数量</param>
        /// <returns>最近项目列表</returns>
        public async Task<List<ProjectHistoryItem>> GetRecentProjectsAsync(int maxCount = 10)
        {
            var history = await GetProjectHistoryAsync();
            return history.RecentProjects.Take(maxCount).ToList();
        }

        /// <summary>
        /// 添加收藏项目
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        public async Task AddFavoriteProjectAsync(string projectPath)
        {
            var history = await GetProjectHistoryAsync();
            history.AddFavoriteProject(projectPath);
            await SaveProjectHistoryAsync(history);
        }

        /// <summary>
        /// 移除收藏项目
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        public async Task RemoveFavoriteProjectAsync(string projectPath)
        {
            var history = await GetProjectHistoryAsync();
            history.RemoveFavoriteProject(projectPath);
            await SaveProjectHistoryAsync(history);
        }

        /// <summary>
        /// 获取收藏项目列表
        /// </summary>
        /// <returns>收藏项目列表</returns>
        public async Task<List<ProjectHistoryItem>> GetFavoriteProjectsAsync()
        {
            var history = await GetProjectHistoryAsync();
            var favoriteItems = new List<ProjectHistoryItem>();

            foreach (var favoritePath in history.FavoriteProjects)
            {
                // 先从最近项目中查找
                var recentItem = history.RecentProjects.FirstOrDefault(p => 
                    string.Equals(p.ProjectPath, favoritePath, StringComparison.OrdinalIgnoreCase));

                if (recentItem != null)
                {
                    favoriteItems.Add(recentItem);
                }
                else if (File.Exists(favoritePath))
                {
                    // 创建新的历史项
                    favoriteItems.Add(new ProjectHistoryItem
                    {
                        ProjectPath = favoritePath,
                        ProjectName = Path.GetFileNameWithoutExtension(favoritePath),
                        ProjectType = InferProjectType(favoritePath),
                        LastModifiedTime = File.GetLastWriteTime(favoritePath)
                    });
                }
            }

            return favoriteItems;
        }

        /// <summary>
        /// 检查项目是否已收藏
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        /// <returns>是否已收藏</returns>
        public async Task<bool> IsFavoriteProjectAsync(string projectPath)
        {
            var history = await GetProjectHistoryAsync();
            return history.IsFavoriteProject(projectPath);
        }

        /// <summary>
        /// 清理不存在的项目
        /// </summary>
        public async Task CleanupMissingProjectsAsync()
        {
            var history = await GetProjectHistoryAsync();
            var originalRecentCount = history.RecentProjects.Count;
            var originalFavoriteCount = history.FavoriteProjects.Count;

            history.CleanupMissingProjects();

            // 如果有清理操作，则保存
            if (history.RecentProjects.Count != originalRecentCount || 
                history.FavoriteProjects.Count != originalFavoriteCount)
            {
                await SaveProjectHistoryAsync(history);
            }
        }

        /// <summary>
        /// 搜索项目
        /// </summary>
        /// <param name="searchText">搜索文本</param>
        /// <param name="includeRecent">包含最近项目</param>
        /// <param name="includeFavorites">包含收藏项目</param>
        /// <returns>搜索结果</returns>
        public async Task<List<ProjectHistoryItem>> SearchProjectsAsync(string searchText, bool includeRecent = true, bool includeFavorites = true)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return new List<ProjectHistoryItem>();

            var results = new HashSet<ProjectHistoryItem>();
            var history = await GetProjectHistoryAsync();

            if (includeRecent)
            {
                var recentMatches = history.RecentProjects.Where(p =>
                    p.ProjectName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    p.ProjectPath.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    p.ProjectType.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    p.Tags.Any(tag => tag.Contains(searchText, StringComparison.OrdinalIgnoreCase)));

                foreach (var match in recentMatches)
                {
                    results.Add(match);
                }
            }

            if (includeFavorites)
            {
                var favoriteProjects = await GetFavoriteProjectsAsync();
                var favoriteMatches = favoriteProjects.Where(p =>
                    p.ProjectName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    p.ProjectPath.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    p.ProjectType.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    p.Tags.Any(tag => tag.Contains(searchText, StringComparison.OrdinalIgnoreCase)));

                foreach (var match in favoriteMatches)
                {
                    results.Add(match);
                }
            }

            return results.OrderByDescending(p => p.LastOpenedTime).ToList();
        }

        /// <summary>
        /// 初始化项目历史服务（确保文件和目录存在）
        /// </summary>
        public async Task InitializeAsync()
        {
            try
            {
                // 确保数据目录存在
                var directory = Path.GetDirectoryName(_historyFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    System.Diagnostics.Debug.WriteLine($"已创建数据目录: {directory}");
                }

                // 检查并初始化历史文件
                await GetProjectHistoryAsync();
                System.Diagnostics.Debug.WriteLine("项目历史服务初始化完成");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化项目历史服务失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 从项目目录读取pack.yml文件
        /// </summary>
        /// <param name="projectPath">项目路径（.ceproj文件路径）</param>
        /// <returns>pack.yml数据，如果读取失败则返回null</returns>
        private async Task<PackYml?> ReadPackYmlAsync(string projectPath)
        {
            try
            {
                // 获取项目目录
                var projectDir = Path.GetDirectoryName(projectPath);
                if (string.IsNullOrEmpty(projectDir) || !Directory.Exists(projectDir))
                    return null;

                // 查找pack.yml文件
                var packYmlPath = Path.Combine(projectDir, "pack.yml");
                if (!File.Exists(packYmlPath))
                    return null;

                // 读取并解析YAML文件
                var yamlContent = await File.ReadAllTextAsync(packYmlPath);
                if (string.IsNullOrWhiteSpace(yamlContent))
                    return null;

                var deserializer = new DeserializerBuilder()
                    .IgnoreUnmatchedProperties()
                    .Build();

                var packYml = deserializer.Deserialize<PackYml>(yamlContent);
                return packYml;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"读取pack.yml文件失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 创建包含pack.yml信息的ProjectHistoryItem
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        /// <returns>项目历史项</returns>
        private async Task<ProjectHistoryItem> CreateProjectHistoryItemAsync(string projectPath)
        {
            var historyItem = new ProjectHistoryItem
            {
                ProjectPath = projectPath,
                ProjectName = Path.GetFileNameWithoutExtension(projectPath),
                LastOpenedTime = DateTime.Now,
                LastModifiedTime = File.Exists(projectPath) 
                    ? File.GetLastWriteTime(projectPath) 
                    : DateTime.Now,
                ProjectType = "CEEdit项目"
            };

            // 尝试读取pack.yml信息
            var packYml = await ReadPackYmlAsync(projectPath);
            if (packYml != null && packYml.IsValid)
            {
                // 使用pack.yml中的信息更新项目信息
                if (!string.IsNullOrWhiteSpace(packYml.Author))
                    historyItem.Author = packYml.Author;

                if (!string.IsNullOrWhiteSpace(packYml.Version))
                    historyItem.Version = packYml.Version;

                if (!string.IsNullOrWhiteSpace(packYml.Description))
                    historyItem.Description = packYml.Description;

                if (!string.IsNullOrWhiteSpace(packYml.Namespace))
                {
                    historyItem.Namespace = packYml.Namespace;
                    // 如果有命名空间，使用它作为项目名称
                    historyItem.ProjectName = packYml.Namespace;
                }

                historyItem.Enable = packYml.Enable;

                // 添加一些标签
                var tags = new List<string>();
                if (!string.IsNullOrWhiteSpace(packYml.Author))
                    tags.Add($"作者:{packYml.Author}");
                if (!string.IsNullOrWhiteSpace(packYml.Version))
                    tags.Add($"v{packYml.Version}");
                
                historyItem.Tags = tags;

                Debug.WriteLine($"成功读取pack.yml信息: {packYml.GetFullInfo()}");
            }
            else
            {
                Debug.WriteLine($"未找到有效的pack.yml文件: {projectPath}");
            }

            return historyItem;
        }

        /// <summary>
        /// 更新项目信息
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        /// <param name="projectType">项目类型</param>
        /// <param name="description">描述</param>
        /// <param name="tags">标签</param>
        public async Task UpdateProjectInfoAsync(string projectPath, string? projectType = null, string? description = null, List<string>? tags = null)
        {
            var history = await GetProjectHistoryAsync();
            var project = history.RecentProjects.FirstOrDefault(p => 
                string.Equals(p.ProjectPath, projectPath, StringComparison.OrdinalIgnoreCase));

            if (project != null)
            {
                if (projectType != null)
                    project.ProjectType = projectType;
                
                if (description != null)
                    project.Description = description;
                
                if (tags != null)
                    project.Tags = new List<string>(tags);

                await SaveProjectHistoryAsync(history);
            }
        }

        /// <summary>
        /// 推断项目类型
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        /// <returns>项目类型</returns>
        private string InferProjectType(string projectPath)
        {
            if (string.IsNullOrWhiteSpace(projectPath))
                return "CEEdit项目";

            var extension = Path.GetExtension(projectPath).ToLowerInvariant();
            var fileName = Path.GetFileName(projectPath).ToLowerInvariant();

            return extension switch
            {
                ".ceproj" => "CEEdit项目",
                ".json" when fileName.Contains("plugin") => "插件项目",
                ".json" when fileName.Contains("addon") => "扩展项目",
                ".yml" or ".yaml" => "配置项目",
                _ => "CEEdit项目"
            };
        }
    }
}
