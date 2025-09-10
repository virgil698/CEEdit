using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CEEdit.Core.Models.Project;
using CEEdit.Core.Services.Interfaces;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Newtonsoft.Json;

namespace CEEdit.Core.Services.Implementations
{
    /// <summary>
    /// 项目管理服务实现
    /// </summary>
    public class ProjectService : IProjectService
    {
        private readonly IProjectHistoryService _historyService;
        private CraftEngineProject? _currentProject;
        private readonly object _lockObject = new object();

        /// <summary>
        /// 当前项目改变事件
        /// </summary>
        public event EventHandler<ProjectChangedEventArgs>? CurrentProjectChanged;

        /// <summary>
        /// 项目保存事件
        /// </summary>
        public event EventHandler<ProjectSavedEventArgs>? ProjectSaved;

        /// <summary>
        /// 触发项目保存事件
        /// </summary>
        /// <param name="project">项目实例</param>
        /// <param name="savePath">保存路径</param>
        protected virtual void OnProjectSaved(CraftEngineProject project, string savePath)
        {
            ProjectSaved?.Invoke(this, new ProjectSavedEventArgs(project, savePath));
        }

        /// <summary>
        /// 获取当前项目
        /// </summary>
        public CraftEngineProject? CurrentProject
        {
            get
            {
                lock (_lockObject)
                {
                    return _currentProject;
                }
            }
        }

        public ProjectService(IProjectHistoryService historyService)
        {
            _historyService = historyService;
        }

        /// <summary>
        /// 打开项目
        /// </summary>
        public async Task<CraftEngineProject> OpenProjectAsync(string projectPath, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(projectPath))
                throw new ArgumentException("项目路径不能为空", nameof(projectPath));

            if (!Directory.Exists(projectPath))
                throw new DirectoryNotFoundException($"项目目录不存在: {projectPath}");

            // 验证项目
            var validationResult = await ValidateProjectPathAsync(projectPath, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.Message));
                throw new InvalidOperationException($"无效的项目目录: {errorMessages}");
            }

            // 加载项目
            var project = await LoadProjectAsync(projectPath, cancellationToken);

            // 设置为当前项目
            var oldProject = CurrentProject;
            lock (_lockObject)
            {
                _currentProject = project;
            }

            // 添加到最近项目
            await _historyService.AddRecentProjectAsync(projectPath, project.Name);

            // 触发事件
            CurrentProjectChanged?.Invoke(this, new ProjectChangedEventArgs(oldProject, project));

            return project;
        }

        /// <summary>
        /// 验证项目路径
        /// </summary>
        private async Task<ValidationResult> ValidateProjectPathAsync(string projectPath, CancellationToken cancellationToken)
        {
            var result = new ValidationResult { IsValid = true };

            try
            {
                // 检查CraftEngine addon必要的目录结构
                var requiredDirectories = new[]
                {
                    "configuration",
                    "resourcepack"
                };

                foreach (var requiredDir in requiredDirectories)
                {
                    var dirPath = Path.Combine(projectPath, requiredDir);
                    if (!Directory.Exists(dirPath))
                    {
                        result.IsValid = false;
                        result.Errors.Add(new ValidationError
                        {
                            Message = $"缺少必需目录: {requiredDir}",
                            Code = "MISSING_DIRECTORY",
                            FilePath = dirPath
                        });
                    }
                }

                // 检查资源包配置文件（在项目根目录）
                var packYmlPath = Path.Combine(projectPath, "pack.yml");
                if (!File.Exists(packYmlPath))
                {
                    result.Warnings.Add(new ValidationWarning
                    {
                        Message = "未找到资源包配置文件 pack.yml",
                        Code = "MISSING_PACK_YML",
                        FilePath = packYmlPath
                    });
                }

                // 检查CEEdit项目标识文件
                var ceProjectPath = Path.Combine(projectPath, ".ceproject");
                if (File.Exists(ceProjectPath))
                {
                    try
                    {
                        var configContent = await File.ReadAllTextAsync(ceProjectPath, cancellationToken);
                        var config = JsonConvert.DeserializeObject<Dictionary<string, object>>(configContent);
                        
                        if (config == null || !config.ContainsKey("projectType"))
                        {
                            result.Warnings.Add(new ValidationWarning
                            {
                                Message = "项目配置文件格式可能不正确",
                                Code = "INVALID_CONFIG",
                                FilePath = ceProjectPath
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Errors.Add(new ValidationError
                        {
                            Message = $"无法读取项目配置: {ex.Message}",
                            Code = "CONFIG_READ_ERROR",
                            FilePath = ceProjectPath
                        });
                        result.IsValid = false;
                    }
                }
                else
                {
                    // 对于没有.ceproject文件的项目，只要有基本结构就认为有效
                    result.Warnings.Add(new ValidationWarning
                    {
                        Message = "这是一个标准的CraftEngine addon项目，但不是CEEdit项目",
                        Code = "NOT_CEDIT_PROJECT",
                        FilePath = ceProjectPath
                    });
                }
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    Message = $"项目验证失败: {ex.Message}",
                    Code = "VALIDATION_ERROR"
                });
            }

            return result;
        }

        /// <summary>
        /// 加载项目
        /// </summary>
        private async Task<CraftEngineProject> LoadProjectAsync(string projectPath, CancellationToken cancellationToken)
        {
            var project = new CraftEngineProject
            {
                ProjectPath = projectPath,
                Name = Path.GetFileName(projectPath) // 默认使用目录名作为项目名称
            };

            // 加载资源包配置 pack.yml
            await LoadPackYmlAsync(project, cancellationToken);

            // 加载CEEdit项目配置
            await LoadProjectConfigAsync(project, cancellationToken);

            // 扫描项目文件
            await ScanProjectFilesAsync(project, cancellationToken);

            return project;
        }

        /// <summary>
        /// 加载资源包配置 pack.yml
        /// </summary>
        private async Task LoadPackYmlAsync(CraftEngineProject project, CancellationToken cancellationToken)
        {
            var packYmlPath = Path.Combine(project.ProjectPath, "pack.yml");
            
            if (!File.Exists(packYmlPath))
                return;

            try
            {
                var yamlContent = await File.ReadAllTextAsync(packYmlPath, cancellationToken);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                var packData = deserializer.Deserialize<Dictionary<string, object>>(yamlContent);

                if (packData != null)
                {
                    if (packData.TryGetValue("name", out var name))
                        project.Name = name?.ToString() ?? project.Name;

                    if (packData.TryGetValue("version", out var version))
                        project.Version = version?.ToString() ?? "1.0.0";

                    if (packData.TryGetValue("author", out var author))
                        project.Author = author?.ToString() ?? "";

                    if (packData.TryGetValue("description", out var description))
                        project.Description = description?.ToString() ?? "";

                    // CraftEngine addon默认启用
                    project.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"加载pack.yml失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 加载项目配置
        /// </summary>
        private async Task LoadProjectConfigAsync(CraftEngineProject project, CancellationToken cancellationToken)
        {
            var configPath = Path.Combine(project.ProjectPath, ".ceproject");
            
            if (!File.Exists(configPath))
                return;

            try
            {
                var configContent = await File.ReadAllTextAsync(configPath, cancellationToken);
                var config = JsonConvert.DeserializeObject<Dictionary<string, object>>(configContent);

                if (config != null)
                {
                    if (config.TryGetValue("targetCraftEngineVersion", out var targetVersion))
                        project.TargetCraftEngineVersion = targetVersion?.ToString() ?? "";

                    if (config.TryGetValue("license", out var license))
                        project.License = license?.ToString() ?? "";

                    if (config.TryGetValue("dependencies", out var deps) && deps is Array depsArray)
                    {
                        project.Dependencies.Clear();
                        foreach (var dep in depsArray)
                        {
                            if (dep?.ToString() is string depStr)
                                project.Dependencies.Add(depStr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"加载项目配置失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 扫描项目文件
        /// </summary>
        private async Task ScanProjectFilesAsync(CraftEngineProject project, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                // 扫描configuration目录中的配置文件
                ScanConfigurationDirectory(project);

                // 扫描资源文件
                ScanResourceDirectory(project);

            }, cancellationToken);
        }

        /// <summary>
        /// 扫描配置目录
        /// </summary>
        private void ScanConfigurationDirectory(CraftEngineProject project)
        {
            var configurationPath = Path.Combine(project.ProjectPath, "configuration");
            if (!Directory.Exists(configurationPath))
                return;

            try
            {
                var files = Directory.GetFiles(configurationPath, "*.yml", SearchOption.AllDirectories);
                foreach (var filePath in files)
                {
                    try
                    {
                        var fileName = Path.GetFileNameWithoutExtension(filePath);
                        var fileContent = File.ReadAllText(filePath);
                        
                        // 通过文件名或内容判断配置类型
                        var configType = DetermineConfigurationType(fileName, fileContent);
                        
                        // 根据配置类型添加到相应集合
                        // TODO: 实际的解析和添加逻辑
                        switch (configType)
                        {
                            case ConfigurationType.Block:
                                // var block = ParseBlockConfiguration(filePath, fileContent);
                                // project.Blocks.Add(block);
                                break;
                            case ConfigurationType.Item:
                                // var item = ParseItemConfiguration(filePath, fileContent);
                                // project.Items.Add(item);
                                break;
                            case ConfigurationType.Recipe:
                                // var recipe = ParseRecipeConfiguration(filePath, fileContent);
                                // project.Recipes.Add(recipe);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"解析配置文件失败 {filePath}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"扫描配置目录失败 {configurationPath}: {ex.Message}");
            }
        }

        /// <summary>
        /// 确定配置文件类型
        /// </summary>
        private ConfigurationType DetermineConfigurationType(string fileName, string content)
        {
            // 通过文件名判断
            if (fileName.Contains("block", StringComparison.OrdinalIgnoreCase))
                return ConfigurationType.Block;
            if (fileName.Contains("item", StringComparison.OrdinalIgnoreCase))
                return ConfigurationType.Item;
            if (fileName.Contains("recipe", StringComparison.OrdinalIgnoreCase))
                return ConfigurationType.Recipe;

            // 通过内容判断（简单的关键词检测）
            if (content.Contains("blockType") || content.Contains("hardness") || content.Contains("material"))
                return ConfigurationType.Block;
            if (content.Contains("itemType") || content.Contains("stackSize") || content.Contains("durability"))
                return ConfigurationType.Item;
            if (content.Contains("recipe") || content.Contains("ingredients") || content.Contains("result"))
                return ConfigurationType.Recipe;

            // 默认返回物品类型
            return ConfigurationType.Item;
        }

        /// <summary>
        /// 配置文件类型枚举
        /// </summary>
        private enum ConfigurationType
        {
            Block,
            Item,
            Recipe,
            Unknown
        }

        /// <summary>
        /// 扫描目录
        /// </summary>
        private void ScanDirectory(string directoryPath, string searchPattern, Action<string> fileProcessor)
        {
            if (!Directory.Exists(directoryPath))
                return;

            try
            {
                var files = Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    fileProcessor(file);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"扫描目录失败 {directoryPath}: {ex.Message}");
            }
        }

        /// <summary>
        /// 扫描资源目录 (遵循Minecraft资源包结构)
        /// </summary>
        private void ScanResourceDirectory(CraftEngineProject project)
        {
            var assetsPath = Path.Combine(project.ProjectPath, "resourcepack", "assets");
            if (!Directory.Exists(assetsPath))
                return;

            try
            {
                // 扫描命名空间目录
                var namespaceDirs = Directory.GetDirectories(assetsPath);
                
                foreach (var namespaceDir in namespaceDirs)
                {
                    var namespaceName = Path.GetFileName(namespaceDir);
                    System.Diagnostics.Debug.WriteLine($"扫描命名空间: {namespaceName}");
                    
                    // 扫描各种资源类型
                    ScanNamespaceResources(namespaceDir, namespaceName);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"扫描资源目录失败 {assetsPath}: {ex.Message}");
            }
        }

        /// <summary>
        /// 扫描命名空间下的资源
        /// </summary>
        private void ScanNamespaceResources(string namespacePath, string namespaceName)
        {
            try
            {
                var resourceTypes = new Dictionary<string, string[]>
                {
                    { "textures", new[] { "*.png", "*.jpg", "*.jpeg" } },
                    { "models", new[] { "*.json" } },
                    { "sounds", new[] { "*.ogg", "*.wav", "*.mp3" } },
                    { "lang", new[] { "*.json" } },
                    { "blockstates", new[] { "*.json" } },
                    { "shaders", new[] { "*.fsh", "*.vsh", "*.json" } },
                    { "font", new[] { "*.json" } }
                };

                foreach (var (resourceType, patterns) in resourceTypes)
                {
                    var resourceTypePath = Path.Combine(namespacePath, resourceType);
                    if (Directory.Exists(resourceTypePath))
                    {
                        var totalFiles = 0;
                        foreach (var pattern in patterns)
                        {
                            var files = Directory.GetFiles(resourceTypePath, pattern, SearchOption.AllDirectories);
                            totalFiles += files.Length;
                        }
                        
                        if (totalFiles > 0)
                        {
                            System.Diagnostics.Debug.WriteLine($"  {resourceType}: 找到 {totalFiles} 个文件");
                            
                            // 对于纹理和模型，还要扫描子目录
                            if (resourceType == "textures" || resourceType == "models")
                            {
                                ScanResourceSubdirectories(resourceTypePath, resourceType);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"扫描命名空间资源失败 {namespacePath}: {ex.Message}");
            }
        }

        /// <summary>
        /// 扫描资源子目录 (如textures/block, models/item等)
        /// </summary>
        private void ScanResourceSubdirectories(string resourcePath, string resourceType)
        {
            try
            {
                var subdirs = Directory.GetDirectories(resourcePath);
                foreach (var subdir in subdirs)
                {
                    var subdirName = Path.GetFileName(subdir);
                    var patterns = resourceType == "textures" 
                        ? new[] { "*.png", "*.jpg", "*.jpeg" }
                        : new[] { "*.json" };
                    
                    var totalFiles = 0;
                    foreach (var pattern in patterns)
                    {
                        var files = Directory.GetFiles(subdir, pattern, SearchOption.AllDirectories);
                        totalFiles += files.Length;
                    }
                    
                    if (totalFiles > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"    {subdirName}: {totalFiles} 个文件");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"扫描资源子目录失败 {resourcePath}: {ex.Message}");
            }
        }

        #region 未实现的接口方法 (占位符)

        public Task<CraftEngineProject> CreateProjectAsync(ProjectTemplate template, string projectName, string projectPath, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("创建项目功能尚未实现");
        }

        public Task<CraftEngineProject> ImportProjectFromPluginAsync(string pluginPath, string projectPath, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("导入项目功能尚未实现");
        }

        public Task SaveProjectAsync(CraftEngineProject? project = null, CancellationToken cancellationToken = default)
        {
            // TODO: 实现保存项目功能
            var targetProject = project ?? CurrentProject;
            if (targetProject != null)
            {
                // 触发保存事件（当实现具体保存逻辑时）
                OnProjectSaved(targetProject, targetProject.ProjectPath);
            }
            throw new NotImplementedException("保存项目功能尚未实现");
        }

        public Task SaveProjectAsAsync(CraftEngineProject project, string newPath, CancellationToken cancellationToken = default)
        {
            // TODO: 实现另存为项目功能
            // 触发保存事件（当实现具体保存逻辑时）
            OnProjectSaved(project, newPath);
            throw new NotImplementedException("另存为项目功能尚未实现");
        }

        public Task<bool> CloseProjectAsync(bool saveChanges = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("关闭项目功能尚未实现");
        }

        public async Task<ValidationResult> ValidateProjectAsync(CraftEngineProject? project = null, CancellationToken cancellationToken = default)
        {
            var targetProject = project ?? CurrentProject;
            if (targetProject == null)
                throw new InvalidOperationException("没有项目可以验证");

            return await ValidateProjectPathAsync(targetProject.ProjectPath, cancellationToken);
        }

        public Task<BuildResult> BuildProjectAsync(CraftEngineProject? project = null, 
            BuildMode buildMode = BuildMode.Release, string? outputPath = null, 
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("构建项目功能尚未实现");
        }

        public async Task<List<RecentProject>> GetRecentProjectsAsync()
        {
            var history = await _historyService.GetProjectHistoryAsync();
            return history.RecentProjects.Select(item => new RecentProject
            {
                Path = item.ProjectPath,
                Name = item.ProjectName,
                LastAccessed = item.LastOpenedTime
            }).ToList();
        }

        public async Task AddToRecentProjectsAsync(string projectPath, string projectName)
        {
            await _historyService.AddRecentProjectAsync(projectPath, projectName);
        }

        public async Task RemoveFromRecentProjectsAsync(string projectPath)
        {
            await _historyService.RemoveRecentProjectAsync(projectPath);
        }

        public async Task ClearRecentProjectsAsync()
        {
            // 由于IProjectHistoryService没有ClearRecentProjectsAsync方法，
            // 我们通过获取历史记录并清空RecentProjects来实现
            var history = await _historyService.GetProjectHistoryAsync();
            history.RecentProjects.Clear();
            await _historyService.SaveProjectHistoryAsync(history);
        }

        public ProjectStatistics GetProjectStatistics(CraftEngineProject? project = null)
        {
            var targetProject = project ?? CurrentProject;
            if (targetProject == null)
                return new ProjectStatistics();

            return new ProjectStatistics
            {
                BlockCount = targetProject.Blocks.Count,
                ItemCount = targetProject.Items.Count,
                RecipeCount = targetProject.Recipes.Count,
                // TODO: 计算其他统计信息
            };
        }

        public bool HasUnsavedChanges(CraftEngineProject? project = null)
        {
            // TODO: 实现未保存更改检测
            return false;
        }

        public void MarkAsModified(CraftEngineProject? project = null)
        {
            // TODO: 实现修改标记
        }

        public void MarkAsUnmodified(CraftEngineProject? project = null)
        {
            // TODO: 实现未修改标记
        }

        #endregion
    }
}
