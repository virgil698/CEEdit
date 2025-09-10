using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YamlDotNet.Serialization;
using CEEdit.Core.Models.Project;
using CEEdit.Core.Services.Interfaces;

namespace CEEdit.Core.Services.Implementations
{
    /// <summary>
    /// 项目加载服务实现
    /// </summary>
    public class ProjectLoaderService : IProjectLoaderService
    {
        /// <summary>
        /// 加载项目数据
        /// </summary>
        /// <param name="projectPath">项目文件路径（.ceproj文件）</param>
        /// <returns>加载的项目数据</returns>
        public async Task<LoadedProjectData?> LoadProjectAsync(string projectPath)
        {
            try
            {
                var loadedData = new LoadedProjectData
                {
                    ProjectPath = projectPath,
                    ProjectDirectory = Path.GetDirectoryName(projectPath) ?? string.Empty
                };

                // 1. 验证项目文件是否存在
                if (!File.Exists(projectPath))
                {
                    loadedData.LoadError = "项目文件不存在";
                    return loadedData;
                }

                // 2. 读取.ceproj文件
                var ceprojContent = await File.ReadAllTextAsync(projectPath);
                if (string.IsNullOrWhiteSpace(ceprojContent))
                {
                    loadedData.LoadError = "项目文件为空";
                    return loadedData;
                }

                // 尝试解析项目文件
                CraftEngineProject project;
                try
                {
                    // 如果是JSON格式
                    if (ceprojContent.TrimStart().StartsWith("{"))
                    {
                        var projectData = JsonConvert.DeserializeObject<dynamic>(ceprojContent);
                        project = new CraftEngineProject
                        {
                            Name = projectData?.name ?? Path.GetFileNameWithoutExtension(projectPath),
                            Version = projectData?.version ?? "1.0.0",
                            Description = projectData?.description ?? string.Empty,
                            Author = projectData?.author ?? string.Empty,
                            ProjectPath = projectPath
                        };
                    }
                    else
                    {
                        // 创建默认项目
                        project = new CraftEngineProject
                        {
                            Name = Path.GetFileNameWithoutExtension(projectPath),
                            Version = "1.0.0",
                            ProjectPath = projectPath
                        };
                    }
                }
                catch (JsonException ex)
                {
                    loadedData.LoadError = $"解析项目文件失败: {ex.Message}";
                    return loadedData;
                }

                loadedData.Project = project;

                // 3. 读取pack.yml文件
                var packYmlPath = Path.Combine(loadedData.ProjectDirectory, "pack.yml");
                if (File.Exists(packYmlPath))
                {
                    try
                    {
                        var yamlContent = await File.ReadAllTextAsync(packYmlPath);
                        if (!string.IsNullOrWhiteSpace(yamlContent))
                        {
                            var deserializer = new DeserializerBuilder()
                                .IgnoreUnmatchedProperties()
                                .Build();

                            var packYml = deserializer.Deserialize<PackYml>(yamlContent);
                            loadedData.PackYml = packYml;

                            // 使用pack.yml的信息更新项目信息
                            if (packYml != null && packYml.IsValid)
                            {
                                if (!string.IsNullOrWhiteSpace(packYml.Author))
                                    project.Author = packYml.Author;
                                
                                if (!string.IsNullOrWhiteSpace(packYml.Version))
                                    project.Version = packYml.Version;
                                
                                if (!string.IsNullOrWhiteSpace(packYml.Description))
                                    project.Description = packYml.Description;
                                
                                System.Diagnostics.Debug.WriteLine($"已加载pack.yml: {packYml.GetFullInfo()}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"读取pack.yml失败: {ex.Message}");
                        // pack.yml读取失败不阻止项目加载
                    }
                }

                // 4. 扫描项目文件结构
                await ScanProjectFilesAsync(loadedData);

                // 5. 标记为成功加载
                loadedData.IsLoaded = true;
                
                System.Diagnostics.Debug.WriteLine($"项目加载成功: {project.Name} v{project.Version}");
                return loadedData;
            }
            catch (Exception ex)
            {
                var errorData = new LoadedProjectData
                {
                    ProjectPath = projectPath,
                    ProjectDirectory = Path.GetDirectoryName(projectPath) ?? string.Empty,
                    LoadError = $"加载项目时发生错误: {ex.Message}",
                    IsLoaded = false
                };
                
                System.Diagnostics.Debug.WriteLine($"项目加载失败: {ex.Message}");
                return errorData;
            }
        }

        /// <summary>
        /// 验证项目文件是否有效
        /// </summary>
        /// <param name="projectPath">项目文件路径</param>
        /// <returns>是否为有效的项目文件</returns>
        public async Task<bool> ValidateProjectAsync(string projectPath)
        {
            try
            {
                if (!File.Exists(projectPath))
                    return false;

                if (!projectPath.EndsWith(".ceproj", StringComparison.OrdinalIgnoreCase))
                    return false;

                var content = await File.ReadAllTextAsync(projectPath);
                if (string.IsNullOrWhiteSpace(content))
                    return false;

                // 基本验证：项目目录应该存在
                var projectDir = Path.GetDirectoryName(projectPath);
                if (string.IsNullOrEmpty(projectDir) || !Directory.Exists(projectDir))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取项目基本信息（不加载完整数据）
        /// </summary>
        /// <param name="projectPath">项目文件路径</param>
        /// <returns>项目基本信息</returns>
        public async Task<ProjectInfo?> GetProjectInfoAsync(string projectPath)
        {
            try
            {
                if (!await ValidateProjectAsync(projectPath))
                    return null;

                var projectDir = Path.GetDirectoryName(projectPath);
                var projectInfo = new ProjectInfo
                {
                    Name = Path.GetFileNameWithoutExtension(projectPath),
                    Version = "1.0.0"
                };

                // 尝试从.ceproj文件读取基本信息
                try
                {
                    var ceprojContent = await File.ReadAllTextAsync(projectPath);
                    if (!string.IsNullOrWhiteSpace(ceprojContent) && ceprojContent.TrimStart().StartsWith("{"))
                    {
                        var data = JsonConvert.DeserializeObject<dynamic>(ceprojContent);
                        if (data != null)
                        {
                            projectInfo.Name = data.name ?? projectInfo.Name;
                            projectInfo.Version = data.version ?? projectInfo.Version;
                            projectInfo.Description = data.description ?? string.Empty;
                            projectInfo.Author = data.author ?? string.Empty;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"读取.ceproj文件基本信息失败: {ex.Message}");
                }

                // 尝试从pack.yml文件读取信息
                var packYmlPath = Path.Combine(projectDir!, "pack.yml");
                if (File.Exists(packYmlPath))
                {
                    try
                    {
                        var yamlContent = await File.ReadAllTextAsync(packYmlPath);
                        if (!string.IsNullOrWhiteSpace(yamlContent))
                        {
                            var deserializer = new DeserializerBuilder()
                                .IgnoreUnmatchedProperties()
                                .Build();

                            var packYml = deserializer.Deserialize<PackYml>(yamlContent);
                            if (packYml != null && packYml.IsValid)
                            {
                                if (!string.IsNullOrWhiteSpace(packYml.Author))
                                    projectInfo.Author = packYml.Author;
                                
                                if (!string.IsNullOrWhiteSpace(packYml.Version))
                                    projectInfo.Version = packYml.Version;
                                
                                if (!string.IsNullOrWhiteSpace(packYml.Description))
                                    projectInfo.Description = packYml.Description;
                                
                                if (!string.IsNullOrWhiteSpace(packYml.Namespace))
                                    projectInfo.Namespace = packYml.Namespace;
                                
                                projectInfo.Enable = packYml.Enable;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"读取pack.yml基本信息失败: {ex.Message}");
                    }
                }

                return projectInfo;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取项目基本信息失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 扫描项目文件结构
        /// </summary>
        /// <param name="loadedData">已加载的项目数据</param>
        private async Task ScanProjectFilesAsync(LoadedProjectData loadedData)
        {
            await Task.Run(() =>
            {
                try
                {
                    var projectDir = loadedData.ProjectDirectory;
                    var fileStructure = loadedData.FileStructure;

                    // 扫描方块文件
                    var blocksDir = Path.Combine(projectDir, "blocks");
                    if (Directory.Exists(blocksDir))
                    {
                        fileStructure.Blocks.AddRange(
                            Directory.GetFiles(blocksDir, "*.json", SearchOption.AllDirectories));
                    }

                    // 扫描物品文件
                    var itemsDir = Path.Combine(projectDir, "items");
                    if (Directory.Exists(itemsDir))
                    {
                        fileStructure.Items.AddRange(
                            Directory.GetFiles(itemsDir, "*.json", SearchOption.AllDirectories));
                    }

                    // 扫描配方文件
                    var recipesDir = Path.Combine(projectDir, "recipes");
                    if (Directory.Exists(recipesDir))
                    {
                        fileStructure.Recipes.AddRange(
                            Directory.GetFiles(recipesDir, "*.json", SearchOption.AllDirectories));
                    }

                    // 扫描资源文件
                    var resourcesDir = Path.Combine(projectDir, "resources");
                    if (Directory.Exists(resourcesDir))
                    {
                        var texturesDir = Path.Combine(resourcesDir, "textures");
                        if (Directory.Exists(texturesDir))
                        {
                            fileStructure.Textures.AddRange(
                                Directory.GetFiles(texturesDir, "*.png", SearchOption.AllDirectories));
                        }

                        var modelsDir = Path.Combine(resourcesDir, "models");
                        if (Directory.Exists(modelsDir))
                        {
                            fileStructure.Models.AddRange(
                                Directory.GetFiles(modelsDir, "*.json", SearchOption.AllDirectories));
                        }

                        var soundsDir = Path.Combine(resourcesDir, "sounds");
                        if (Directory.Exists(soundsDir))
                        {
                            fileStructure.Sounds.AddRange(
                                Directory.GetFiles(soundsDir, "*.ogg", SearchOption.AllDirectories));
                        }
                    }

                    System.Diagnostics.Debug.WriteLine(
                        $"项目文件扫描完成: {fileStructure.Blocks.Count}个方块, " +
                        $"{fileStructure.Items.Count}个物品, " +
                        $"{fileStructure.Recipes.Count}个配方, " +
                        $"{fileStructure.Textures.Count}个纹理");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"扫描项目文件失败: {ex.Message}");
                }
            });
        }
    }
}
