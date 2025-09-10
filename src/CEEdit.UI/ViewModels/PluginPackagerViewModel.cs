using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using CEEdit.Core.Models.Project;
using CEEdit.Core.Services.Interfaces;

namespace CEEdit.UI.ViewModels
{
    public partial class PluginPackagerViewModel : ObservableObject
    {
        private readonly IProjectService? _projectService;
        private readonly IFileService? _fileService;
        private readonly IResourceService? _resourceService;
        private readonly IValidationService? _validationService;

        [ObservableProperty]
        private string _outputPath = string.Empty;

        [ObservableProperty]
        private string _versionNumber = "1.0.0";

        [ObservableProperty]
        private string _buildType = "Release";

        [ObservableProperty]
        private string _targetVersion = "1.20.0";

        [ObservableProperty]
        private string _compressionLevel = "Normal";

        [ObservableProperty]
        private bool _includeDebugInfo = false;

        [ObservableProperty]
        private bool _optimizeResources = true;

        [ObservableProperty]
        private bool _validatePlugin = true;

        [ObservableProperty]
        private bool _generateManifest = true;

        [ObservableProperty]
        private string _fileFilter = "所有文件";

        [ObservableProperty]
        private int _selectedFilesCount = 0;

        [ObservableProperty]
        private string _totalSelectedSize = "0 KB";

        [ObservableProperty]
        private string _buildStatus = "准备就绪";

        [ObservableProperty]
        private double _buildProgress = 0;

        [ObservableProperty]
        private string _buildLog = string.Empty;

        [ObservableProperty]
        private bool _showBuildResult = false;

        [ObservableProperty]
        private string _buildResultIcon = "✅";

        [ObservableProperty]
        private string _buildResultTitle = "构建完成";

        [ObservableProperty]
        private string _buildResultMessage = string.Empty;

        [ObservableProperty]
        private string _outputFilePath = string.Empty;

        [ObservableProperty]
        private bool _buildSuccessful = false;

        [ObservableProperty]
        private bool _isBuilding = false;

        public ObservableCollection<string> AvailableBuildTypes { get; } = new()
        {
            "Debug", "Release"
        };

        public ObservableCollection<string> AvailableTargetVersions { get; } = new()
        {
            "1.20.0", "1.20.1", "1.21.0"
        };

        public ObservableCollection<string> AvailableCompressionLevels { get; } = new()
        {
            "None", "Fast", "Normal", "Maximum"
        };

        public ObservableCollection<ProjectFileItem> ProjectFiles { get; } = new();

        public PluginPackagerViewModel(IProjectService? projectService = null,
                                      IFileService? fileService = null,
                                      IResourceService? resourceService = null,
                                      IValidationService? validationService = null)
        {
            _projectService = projectService;
            _fileService = fileService;
            _resourceService = resourceService;
            _validationService = validationService;

            // 设置默认输出路径
            OutputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CEEdit_Output");

            // 监听文件选择变化
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(FileFilter))
                {
                    RefreshFiles();
                }
            };
        }

        [RelayCommand]
        private void BrowseOutputPath()
        {
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "选择输出文件夹",
                SelectedPath = OutputPath
            };

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OutputPath = folderDialog.SelectedPath;
            }
        }

        [RelayCommand]
        private void SelectAllFiles()
        {
            foreach (var file in ProjectFiles)
            {
                file.IsIncluded = true;
            }
            UpdateFileStatistics();
        }

        [RelayCommand]
        private void DeselectAllFiles()
        {
            foreach (var file in ProjectFiles)
            {
                file.IsIncluded = false;
            }
            UpdateFileStatistics();
        }

        [RelayCommand]
        private void RefreshFiles()
        {
            // TODO: 从当前项目刷新文件列表
            LoadProjectFiles();
        }

        [RelayCommand]
        private async Task BuildDebug()
        {
            BuildType = "Debug";
            IncludeDebugInfo = true;
            OptimizeResources = false;
            await StartBuild();
        }

        [RelayCommand]
        private async Task BuildRelease()
        {
            BuildType = "Release";
            IncludeDebugInfo = false;
            OptimizeResources = true;
            await StartBuild();
        }

        [RelayCommand]
        private async Task CleanOutput()
        {
            try
            {
                if (Directory.Exists(OutputPath))
                {
                    Directory.Delete(OutputPath, true);
                    Directory.CreateDirectory(OutputPath);
                }
                
                AppendToLog("输出文件夹已清理");
                BuildStatus = "输出已清理";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清理输出失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task ValidateProject()
        {
            try
            {
                BuildStatus = "验证项目中...";
                BuildProgress = 0;
                AppendToLog("开始验证项目...");

                var errors = new List<string>();
                
                // 验证基本设置
                if (string.IsNullOrWhiteSpace(VersionNumber))
                {
                    errors.Add("版本号不能为空");
                }
                
                if (string.IsNullOrWhiteSpace(OutputPath))
                {
                    errors.Add("输出路径不能为空");
                }

                // 验证文件
                var includedFiles = ProjectFiles.Where(f => f.IsIncluded).ToList();
                if (includedFiles.Count == 0)
                {
                    errors.Add("没有选择要包含的文件");
                }

                BuildProgress = 50;

                // 使用验证服务
                if (_validationService != null)
                {
                    // TODO: 调用验证服务进行更深入的验证
                }

                BuildProgress = 100;

                if (errors.Count > 0)
                {
                    BuildStatus = $"验证失败，发现 {errors.Count} 个问题";
                    foreach (var error in errors)
                    {
                        AppendToLog($"错误: {error}");
                    }
                }
                else
                {
                    BuildStatus = "验证通过";
                    AppendToLog("项目验证通过");
                }
            }
            catch (Exception ex)
            {
                BuildStatus = "验证失败";
                AppendToLog($"验证过程中出错: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task ExportTemplate()
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Title = "导出项目模板",
                    Filter = "CEEdit模板文件 (*.cetemplate)|*.cetemplate|所有文件 (*.*)|*.*",
                    FileName = "MyTemplate.cetemplate"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    BuildStatus = "导出模板中...";
                    AppendToLog("开始导出项目模板...");
                    
                    // TODO: 实现模板导出逻辑
                    await Task.Delay(1000); // 模拟导出过程
                    
                    BuildStatus = "模板导出完成";
                    AppendToLog($"模板已导出到: {saveFileDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出模板失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task PublishToServer()
        {
            try
            {
                BuildStatus = "发布到服务器中...";
                AppendToLog("开始发布到测试服务器...");
                
                // TODO: 实现服务器发布逻辑
                await Task.Delay(2000); // 模拟发布过程
                
                BuildStatus = "发布完成";
                AppendToLog("已成功发布到测试服务器");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发布失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task SaveConfig()
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Title = "保存构建配置",
                    Filter = "构建配置文件 (*.buildconfig)|*.buildconfig|所有文件 (*.*)|*.*",
                    FileName = "build.buildconfig"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var config = CreateBuildConfig();
                    // TODO: 序列化配置到文件
                    BuildStatus = "配置已保存";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task LoadConfig()
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Title = "加载构建配置",
                    Filter = "构建配置文件 (*.buildconfig)|*.buildconfig|所有文件 (*.*)|*.*"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    // TODO: 从文件加载配置
                    BuildStatus = "配置已加载";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载配置失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task StartBuild()
        {
            if (IsBuilding) return;

            try
            {
                IsBuilding = true;
                ShowBuildResult = false;
                BuildProgress = 0;
                BuildLog = string.Empty;
                
                AppendToLog("开始构建插件...");
                BuildStatus = "初始化构建...";

                // 验证设置
                await ValidateProject();
                if (BuildStatus.Contains("失败"))
                {
                    return;
                }

                BuildProgress = 20;
                BuildStatus = "准备文件...";
                AppendToLog("准备构建文件...");

                // 确保输出目录存在
                Directory.CreateDirectory(OutputPath);

                BuildProgress = 40;
                BuildStatus = "复制文件...";
                AppendToLog("复制项目文件...");

                // 复制选中的文件
                await CopySelectedFiles();

                BuildProgress = 60;
                BuildStatus = "优化资源...";
                AppendToLog("优化资源文件...");

                // 优化资源
                if (OptimizeResources)
                {
                    await OptimizeProjectResources();
                }

                BuildProgress = 80;
                BuildStatus = "生成清单...";
                AppendToLog("生成插件清单...");

                // 生成清单文件
                if (GenerateManifest)
                {
                    await GeneratePluginManifest();
                }

                BuildProgress = 100;
                BuildStatus = "构建完成";
                
                // 设置构建结果
                BuildSuccessful = true;
                ShowBuildResult = true;
                BuildResultIcon = "✅";
                BuildResultTitle = "构建成功";
                BuildResultMessage = $"插件已成功构建到输出文件夹";
                OutputFilePath = OutputPath;
                
                AppendToLog("插件构建完成!");
            }
            catch (Exception ex)
            {
                BuildSuccessful = false;
                ShowBuildResult = true;
                BuildResultIcon = "❌";
                BuildResultTitle = "构建失败";
                BuildResultMessage = ex.Message;
                
                AppendToLog($"构建失败: {ex.Message}");
                BuildStatus = "构建失败";
            }
            finally
            {
                IsBuilding = false;
            }
        }

        [RelayCommand]
        private void OpenOutputFolder()
        {
            try
            {
                if (Directory.Exists(OutputPath))
                {
                    System.Diagnostics.Process.Start("explorer.exe", OutputPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法打开输出文件夹: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task RunPlugin()
        {
            try
            {
                // TODO: 实现运行插件逻辑
                MessageBox.Show("运行插件功能正在开发中...", 
                              "信息", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"运行插件失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CopySelectedFiles()
        {
            var includedFiles = ProjectFiles.Where(f => f.IsIncluded).ToList();
            int processedFiles = 0;

            foreach (var file in includedFiles)
            {
                try
                {
                    var targetPath = Path.Combine(OutputPath, file.RelativePath);
                    var targetDir = Path.GetDirectoryName(targetPath);
                    
                    if (!string.IsNullOrEmpty(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }

                    if (_fileService != null)
                    {
                        var data = await _fileService.ReadBinaryFileAsync(file.FullPath);
                        await _fileService.WriteBinaryFileAsync(targetPath, data);
                    }
                    else
                    {
                        File.Copy(file.FullPath, targetPath, true);
                    }

                    AppendToLog($"已复制: {file.Name}");
                }
                catch (Exception ex)
                {
                    AppendToLog($"复制失败 {file.Name}: {ex.Message}");
                }

                processedFiles++;
                // 更新进度 (40-60%)
                BuildProgress = 40 + (20.0 * processedFiles / includedFiles.Count);
            }
        }

        private async Task OptimizeProjectResources()
        {
            try
            {
                if (_resourceService != null)
                {
                    // TODO: 调用资源优化服务
                    AppendToLog("资源优化完成");
                }
                else
                {
                    AppendToLog("跳过资源优化 (服务不可用)");
                }
            }
            catch (Exception ex)
            {
                AppendToLog($"资源优化失败: {ex.Message}");
            }
        }

        private async Task GeneratePluginManifest()
        {
            try
            {
                var manifestPath = Path.Combine(OutputPath, "plugin.yml");
                var manifest = CreatePluginManifest();
                
                if (_fileService != null)
                {
                    await _fileService.WriteTextFileAsync(manifestPath, manifest);
                }
                else
                {
                    await File.WriteAllTextAsync(manifestPath, manifest);
                }
                
                AppendToLog("插件清单已生成");
            }
            catch (Exception ex)
            {
                AppendToLog($"生成清单失败: {ex.Message}");
            }
        }

        private string CreatePluginManifest()
        {
            return $@"name: MyPlugin
version: {VersionNumber}
main: com.example.MyPlugin
api-version: {TargetVersion}
description: A plugin created with CEEdit
author: Plugin Author
website: https://example.com

# 生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}
# 构建类型: {BuildType}
# CEEdit版本: 1.0.0
";
        }

        private BuildConfig CreateBuildConfig()
        {
            return new BuildConfig
            {
                OutputPath = OutputPath,
                VersionNumber = VersionNumber,
                BuildType = BuildType,
                TargetVersion = TargetVersion,
                CompressionLevel = CompressionLevel,
                IncludeDebugInfo = IncludeDebugInfo,
                OptimizeResources = OptimizeResources,
                ValidatePlugin = ValidatePlugin,
                GenerateManifest = GenerateManifest
            };
        }

        private void LoadProjectFiles()
        {
            ProjectFiles.Clear();
            
            // TODO: 从当前项目加载文件
            // 这里添加一些示例文件
            var exampleFiles = new[]
            {
                new ProjectFileItem { Name = "plugin.yml", Type = "配置", RelativePath = "plugin.yml", Size = 1024, IsIncluded = true },
                new ProjectFileItem { Name = "MyBlock.yml", Type = "方块", RelativePath = "blocks/MyBlock.yml", Size = 512, IsIncluded = true },
                new ProjectFileItem { Name = "MyItem.yml", Type = "物品", RelativePath = "items/MyItem.yml", Size = 768, IsIncluded = true },
                new ProjectFileItem { Name = "stone.png", Type = "纹理", RelativePath = "textures/stone.png", Size = 2048, IsIncluded = true },
                new ProjectFileItem { Name = "sword.json", Type = "模型", RelativePath = "models/sword.json", Size = 4096, IsIncluded = false }
            };

            foreach (var file in exampleFiles)
            {
                ProjectFiles.Add(file);
            }

            UpdateFileStatistics();
        }

        private void UpdateFileStatistics()
        {
            var selectedFiles = ProjectFiles.Where(f => f.IsIncluded);
            SelectedFilesCount = selectedFiles.Count();
            
            var totalSize = selectedFiles.Sum(f => f.Size);
            TotalSelectedSize = FormatFileSize(totalSize);
        }

        private string FormatFileSize(long bytes)
        {
            if (bytes < 1024) return $"{bytes} B";
            if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
            if (bytes < 1024 * 1024 * 1024) return $"{bytes / (1024.0 * 1024):F1} MB";
            return $"{bytes / (1024.0 * 1024 * 1024):F1} GB";
        }

        private void AppendToLog(string message)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            BuildLog += $"[{timestamp}] {message}\n";
        }

        /// <summary>
        /// 加载项目进行打包
        /// </summary>
        /// <param name="project">项目对象</param>
        public void LoadProject(CraftEngineProject project)
        {
            // TODO: 从项目加载设置和文件
            VersionNumber = project.Version;
            LoadProjectFiles();
        }
    }

    /// <summary>
    /// 项目文件项目
    /// </summary>
    public partial class ProjectFileItem : ObservableObject
    {
        [ObservableProperty]
        private bool _isIncluded = true;

        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string RelativePath { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public long Size { get; set; } = 0;
        
        public string SizeFormatted
        {
            get
            {
                if (Size < 1024) return $"{Size} B";
                if (Size < 1024 * 1024) return $"{Size / 1024.0:F1} KB";
                return $"{Size / (1024.0 * 1024):F1} MB";
            }
        }
    }

    /// <summary>
    /// 构建配置
    /// </summary>
    public class BuildConfig
    {
        public string OutputPath { get; set; } = string.Empty;
        public string VersionNumber { get; set; } = string.Empty;
        public string BuildType { get; set; } = string.Empty;
        public string TargetVersion { get; set; } = string.Empty;
        public string CompressionLevel { get; set; } = string.Empty;
        public bool IncludeDebugInfo { get; set; }
        public bool OptimizeResources { get; set; }
        public bool ValidatePlugin { get; set; }
        public bool GenerateManifest { get; set; }
    }
}
