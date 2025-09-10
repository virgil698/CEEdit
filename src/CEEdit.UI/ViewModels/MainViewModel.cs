using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CEEdit.Core.Models.Project;
using CEEdit.Core.Services.Interfaces;
using CEEdit.UI.Models;

namespace CEEdit.UI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IProjectService? _projectService;
        private readonly IFileService? _fileService;
        private readonly IResourceService? _resourceService;
        private readonly IValidationService? _validationService;
        private readonly IBlockbenchService? _blockbenchService;
        private readonly IProjectHistoryService? _projectHistoryService;

        [ObservableProperty]
        private string _statusMessage = "就绪";

        [ObservableProperty]
        private string _currentProjectName = "无项目";

        [ObservableProperty]
        private bool _isProjectOpen = false;

        [ObservableProperty]
        private ObservableCollection<object> _projectItems = new ObservableCollection<object>();

        [ObservableProperty]
        private object? _selectedItemProperties;

        [ObservableProperty]
        private bool _isProjectExplorerVisible = true;

        [ObservableProperty]
        private bool _isPropertyPanelVisible = true;

        [ObservableProperty]
        private bool _isPreviewPanelVisible = true;

        [ObservableProperty]
        private GridLength _projectExplorerWidth = new GridLength(250);

        [ObservableProperty]
        private GridLength _propertyPanelWidth = new GridLength(300);

        [ObservableProperty]
        private ObservableCollection<ProjectHistoryItem> _recentProjects = new ObservableCollection<ProjectHistoryItem>();

        [ObservableProperty]
        private bool _hasRecentProjects = false;

        public string WindowTitle => IsProjectOpen 
            ? $"{CurrentProjectName} - CEEdit" 
            : "CEEdit - CraftEngine插件可视化编辑器";

        public string StatusText => StatusMessage;
        public string ProjectStatus => CurrentProjectName;

        public MainViewModel(IProjectService? projectService = null, IFileService? fileService = null,
                            IResourceService? resourceService = null, IValidationService? validationService = null,
                            IBlockbenchService? blockbenchService = null, IProjectHistoryService? projectHistoryService = null)
        {
            _projectService = projectService;
            _fileService = fileService;
            _resourceService = resourceService;
            _validationService = validationService;
            _blockbenchService = blockbenchService;
            _projectHistoryService = projectHistoryService ?? new Core.Services.Implementations.ProjectHistoryService();
            
            InitializeProjectTree();
            _ = LoadRecentProjectsAsync(); // 异步加载最近项目，不等待结果
        }

        private void InitializeProjectTree()
        {
            // 添加一些示例项目结构
            ProjectItems.Clear();
            // 这里暂时不添加内容，等实际项目加载时再填充
        }

        /// <summary>
        /// 异步加载最近项目列表
        /// </summary>
        private async Task LoadRecentProjectsAsync()
        {
            try
            {
                if (_projectHistoryService != null)
                {
                    var recentProjectItems = await _projectHistoryService.GetRecentProjectsAsync(10);
                    
                    // 清空现有列表并添加新项目
                    RecentProjects.Clear();
                    foreach (var project in recentProjectItems)
                    {
                        RecentProjects.Add(project);
                    }
                    
                    // 更新是否有最近项目的状态
                    HasRecentProjects = RecentProjects.Count > 0;
                    
                    if (HasRecentProjects)
                    {
                        StatusMessage = $"已加载 {RecentProjects.Count} 个最近项目";
                    }
                    else
                    {
                        StatusMessage = "暂无任何项目";
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"加载最近项目失败: {ex.Message}";
                HasRecentProjects = false;
            }
        }

        private void LoadProjectStructure(CraftEngineProject project)
        {
            try
            {
                ProjectItems.Clear();
                
                // 创建项目树结构（符合CraftEngine addon结构）
                var projectRoot = new ProjectTreeItem
                {
                    Name = project.Name,
                    Type = ProjectItemType.Project,
                    Icon = "📦",
                    IsExpanded = true
                };

                // 添加配置目录节点
                var configurationNode = new ProjectTreeItem
                {
                    Name = "configuration",
                    Type = ProjectItemType.Folder,
                    Icon = "⚙️",
                    IsExpanded = true
                };

                // 扫描配置目录中的文件
                var configPath = Path.Combine(project.ProjectPath, "configuration");
                if (Directory.Exists(configPath))
                {
                    var configFiles = Directory.GetFiles(configPath, "*.yml", SearchOption.AllDirectories);
                    foreach (var filePath in configFiles)
                    {
                        var fileName = Path.GetFileName(filePath);
                        var relativePath = Path.GetRelativePath(configPath, filePath);
                        
                        var fileNode = new ProjectTreeItem
                        {
                            Name = fileName,
                            Type = DetermineFileType(fileName),
                            Icon = GetFileIcon(fileName),
                            Data = filePath
                        };
                        
                        configurationNode.Children.Add(fileNode);
                    }
                }

                projectRoot.Children.Add(configurationNode);

                // 添加pack.yml（在项目根目录）
                var packYmlPath = Path.Combine(project.ProjectPath, "pack.yml");
                if (File.Exists(packYmlPath))
                {
                    projectRoot.Children.Add(new ProjectTreeItem
                    {
                        Name = "pack.yml",
                        Type = ProjectItemType.Other,
                        Icon = "📄",
                        Data = packYmlPath
                    });
                }

                // 添加资源包节点
                var resourcepackNode = new ProjectTreeItem
                {
                    Name = "resourcepack",
                    Type = ProjectItemType.Folder,
                    Icon = "📁",
                    IsExpanded = true
                };

                // 添加assets子节点
                var assetsNode = new ProjectTreeItem
                {
                    Name = "assets",
                    Type = ProjectItemType.Folder,
                    Icon = "📁",
                    IsExpanded = false
                };

                // 扫描Minecraft资源包结构 (按命名空间组织)
                var assetsPath = Path.Combine(project.ProjectPath, "resourcepack", "assets");
                if (Directory.Exists(assetsPath))
                {
                    // 扫描命名空间目录
                    var namespaceDirs = Directory.GetDirectories(assetsPath);
                    
                    foreach (var namespaceDir in namespaceDirs)
                    {
                        var namespaceName = Path.GetFileName(namespaceDir);
                        
                        var namespaceNode = new ProjectTreeItem
                        {
                            Name = namespaceName,
                            Type = ProjectItemType.Folder,
                            Icon = "📦",
                            IsExpanded = false
                        };

                        // 扫描命名空间下的资源类型
                        ScanNamespaceForUI(namespaceDir, namespaceNode);
                        
                        if (namespaceNode.Children.Any())
                        {
                            assetsNode.Children.Add(namespaceNode);
                        }
                    }
                }

                resourcepackNode.Children.Add(assetsNode);
                projectRoot.Children.Add(resourcepackNode);

                ProjectItems.Add(projectRoot);

                // 统计配置文件数量
                var configFileCount = configurationNode.Children.Count;
                StatusMessage = $"项目 '{project.Name}' 已加载: {configFileCount} 个配置文件";
            }
            catch (Exception ex)
            {
                StatusMessage = $"加载项目 '{project?.Name ?? "未知"}' 结构失败: {ex.Message}";
            }
        }

        private ProjectItemType DetermineFileType(string fileName)
        {
            var lowerName = fileName.ToLower();
            if (lowerName.Contains("block")) return ProjectItemType.Block;
            if (lowerName.Contains("item")) return ProjectItemType.Item;
            if (lowerName.Contains("recipe")) return ProjectItemType.Recipe;
            return ProjectItemType.Other;
        }

        private string GetFileIcon(string fileName)
        {
            var lowerName = fileName.ToLower();
            var extension = Path.GetExtension(lowerName);

            return extension switch
            {
                ".yml" or ".yaml" => lowerName.Contains("block") ? "🧱" :
                                    lowerName.Contains("item") ? "🎒" :
                                    lowerName.Contains("recipe") ? "⚗️" : "📄",
                ".json" => "📋",
                ".png" or ".jpg" or ".jpeg" => "🖼️",
                ".wav" or ".ogg" or ".mp3" => "🔊",
                ".fsh" or ".vsh" => "🎨",
                _ => "📄"
            };
        }

        /// <summary>
        /// 扫描命名空间下的资源用于UI显示
        /// </summary>
        private void ScanNamespaceForUI(string namespacePath, ProjectTreeItem namespaceNode)
        {
            try
            {
                var resourceTypes = new Dictionary<string, (string Icon, string DisplayName)>
                {
                    { "textures", ("🖼️", "纹理") },
                    { "models", ("📐", "模型") },
                    { "sounds", ("🔊", "音效") },
                    { "lang", ("🌐", "语言") },
                    { "blockstates", ("🔧", "方块状态") },
                    { "shaders", ("🎨", "着色器") },
                    { "font", ("🔤", "字体") }
                };

                foreach (var (resourceType, (icon, displayName)) in resourceTypes)
                {
                    var resourceTypePath = Path.Combine(namespacePath, resourceType);
                    if (Directory.Exists(resourceTypePath))
                    {
                        var allFiles = Directory.GetFiles(resourceTypePath, "*.*", SearchOption.AllDirectories);
                        if (allFiles.Length > 0)
                        {
                            var resourceTypeNode = new ProjectTreeItem
                            {
                                Name = $"{displayName} ({allFiles.Length})",
                                Type = ProjectItemType.Folder,
                                Icon = icon,
                                IsExpanded = false
                            };

                            // 对于纹理和模型，按子目录分组
                            if (resourceType == "textures" || resourceType == "models")
                            {
                                AddResourceSubdirectories(resourceTypePath, resourceTypeNode, resourceType);
                            }
                            else
                            {
                                // 直接添加文件（限制显示数量）
                                foreach (var file in allFiles.Take(50))
                                {
                                    resourceTypeNode.Children.Add(new ProjectTreeItem
                                    {
                                        Name = Path.GetFileName(file),
                                        Type = ProjectItemType.Other,
                                        Icon = GetFileIcon(file),
                                        Data = file
                                    });
                                }
                            }

                            namespaceNode.Children.Add(resourceTypeNode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UI扫描命名空间失败 {namespacePath}: {ex.Message}");
            }
        }

        /// <summary>
        /// 添加资源子目录到UI树
        /// </summary>
        private void AddResourceSubdirectories(string resourcePath, ProjectTreeItem parentNode, string resourceType)
        {
            try
            {
                var subdirs = Directory.GetDirectories(resourcePath);
                
                if (subdirs.Length > 0)
                {
                    // 有子目录，按子目录分组
                    foreach (var subdir in subdirs)
                    {
                        var subdirName = Path.GetFileName(subdir);
                        var files = Directory.GetFiles(subdir, "*.*", SearchOption.AllDirectories);
                        
                        if (files.Length > 0)
                        {
                            var subdirIcon = subdirName switch
                            {
                                "block" => "🧱",
                                "item" => "🎒",
                                "entity" => "👾",
                                "gui" => "🖼️",
                                "particle" => "✨",
                                _ => "📁"
                            };

                            var subdirNode = new ProjectTreeItem
                            {
                                Name = $"{subdirName} ({files.Length})",
                                Type = ProjectItemType.Folder,
                                Icon = subdirIcon,
                                IsExpanded = false
                            };

                            // 添加文件（限制显示数量）
                            foreach (var file in files.Take(30))
                            {
                                subdirNode.Children.Add(new ProjectTreeItem
                                {
                                    Name = Path.GetFileName(file),
                                    Type = ProjectItemType.Other,
                                    Icon = GetFileIcon(file),
                                    Data = file
                                });
                            }

                            parentNode.Children.Add(subdirNode);
                        }
                    }
                }
                else
                {
                    // 没有子目录，直接添加文件
                    var files = Directory.GetFiles(resourcePath, "*.*");
                    foreach (var file in files.Take(50))
                    {
                        parentNode.Children.Add(new ProjectTreeItem
                        {
                            Name = Path.GetFileName(file),
                            Type = ProjectItemType.Other,
                            Icon = GetFileIcon(file),
                            Data = file
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"添加资源子目录失败 {resourcePath}: {ex.Message}");
            }
        }

        [RelayCommand]
        private void NewProject()
        {
            StatusMessage = "创建新项目...";
            try
            {
                var newProjectWindow = new UI.Views.Windows.NewProjectWindow();
                newProjectWindow.ShowDialog();
                
                if (newProjectWindow.DialogResult)
                {
                    StatusMessage = "正在创建项目...";
                    
                    var projectName = newProjectWindow.ProjectName;
                    var projectLocation = newProjectWindow.ProjectLocation;
                    var fullProjectPath = Path.Combine(projectLocation, projectName);
                    
                    // TODO: 使用ProjectService创建项目
                    // var template = new ProjectTemplate { Name = newProjectWindow.SelectedTemplate };
                    // var project = await _projectService.CreateProjectAsync(template, projectName, fullProjectPath);
                    
                    // 临时实现：直接设置项目状态
                    IsProjectOpen = true;
                    CurrentProjectName = projectName;
                    StatusMessage = $"项目 '{projectName}' 创建完成";
                    
                    OnPropertyChanged(nameof(WindowTitle));
                }
                else
                {
                    StatusMessage = "取消创建项目";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"创建项目失败: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task OpenProject()
        {
            StatusMessage = "选择要打开的项目...";
            try
            {
                var historyService = new Core.Services.Implementations.ProjectHistoryService();
                var openDialog = new UI.Views.Windows.OpenProjectWindow(historyService);
                openDialog.ShowDialog();
                
                if (openDialog.DialogResult && !string.IsNullOrEmpty(openDialog.SelectedProjectPath))
                {
                    StatusMessage = "正在加载项目...";
                    
                    // 使用ProjectService打开项目
                    if (_projectService != null)
                    {
                        var project = await _projectService.OpenProjectAsync(openDialog.SelectedProjectPath);
                        
                        // 更新UI状态
                        IsProjectOpen = true;
                        CurrentProjectName = project.Name;
                        StatusMessage = $"项目 '{project.Name}' 已成功打开";
                        
                        // 加载项目结构到UI
                        LoadProjectStructure(project);
                        
                        // 添加到最近项目历史
                        if (_projectHistoryService != null)
                        {
                            await _projectHistoryService.AddRecentProjectAsync(openDialog.SelectedProjectPath, "CraftEngine项目");
                            // 重新加载最近项目列表
                            await LoadRecentProjectsAsync();
                        }
                        
                        OnPropertyChanged(nameof(WindowTitle));
                    }
                    else
                    {
                        StatusMessage = "项目服务不可用，无法打开项目";
                    }
                }
                else
                {
                    StatusMessage = "已取消打开项目";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"打开项目失败: {ex.Message}";
                MessageBox.Show($"打开项目时发生错误:\n{ex.Message}", "错误", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task SaveProject()
        {
            if (!IsProjectOpen) return;
            
            StatusMessage = $"保存项目 '{CurrentProjectName}'...";
            try
            {
                if (_projectService != null)
                {
                    // TODO: 获取当前项目实例
                    // await _projectService.SaveProjectAsync(currentProject);
                }
                await Task.Delay(100); // 模拟异步操作
                StatusMessage = $"项目 '{CurrentProjectName}' 已保存";
            }
            catch (Exception ex)
            {
                StatusMessage = $"保存项目 '{CurrentProjectName}' 失败: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task SaveProjectAs()
        {
            if (!IsProjectOpen) return;
            
            StatusMessage = $"项目 '{CurrentProjectName}' 另存为...";
            try
            {
                await Task.Delay(100); // 模拟异步操作
                StatusMessage = $"项目 '{CurrentProjectName}' 已另存为";
            }
            catch (Exception ex)
            {
                StatusMessage = $"另存为项目 '{CurrentProjectName}' 失败: {ex.Message}";
            }
        }

        [RelayCommand]
        private void CloseProject()
        {
            var closingProjectName = CurrentProjectName;
            StatusMessage = $"关闭项目 '{closingProjectName}'";
            IsProjectOpen = false;
            CurrentProjectName = "无项目";
            ProjectItems.Clear();
            OnPropertyChanged(nameof(WindowTitle));
        }

        [RelayCommand]
        private void Exit()
        {
            Application.Current.Shutdown();
        }

        [RelayCommand]
        private void Undo()
        {
            StatusMessage = "执行撤销操作。";
        }

        [RelayCommand]
        private void Redo()
        {
            StatusMessage = "执行重做操作。";
        }

        [RelayCommand]
        private void Find()
        {
            StatusMessage = "打开查找对话框。";
        }

        [RelayCommand]
        private void Replace()
        {
            StatusMessage = "打开替换对话框。";
        }

        [RelayCommand]
        private void ProjectSettings()
        {
            StatusMessage = "打开项目设置。";
        }

        [RelayCommand]
        private async Task BuildProject()
        {
            if (!IsProjectOpen) return;
            
            StatusMessage = "构建项目...";
            try
            {
                await Task.Delay(1000); // 模拟构建过程
                StatusMessage = "项目构建完成。";
            }
            catch (Exception ex)
            {
                StatusMessage = $"构建失败: {ex.Message}";
            }
        }

        [RelayCommand]
        private void CleanProject()
        {
            if (!IsProjectOpen) return;
            
            StatusMessage = "清理项目完成。";
        }

        [RelayCommand]
        private void AddBlock()
        {
            if (!IsProjectOpen) return;
            
            StatusMessage = "添加新方块。";
        }

        [RelayCommand]
        private void AddItem()
        {
            if (!IsProjectOpen) return;
            
            StatusMessage = "添加新物品。";
        }

        [RelayCommand]
        private void AddRecipe()
        {
            if (!IsProjectOpen) return;
            
            StatusMessage = "添加新配方。";
        }

        [RelayCommand]
        private async Task LaunchBlockbench()
        {
            StatusMessage = "启动 Blockbench...";
            try
            {
                if (_blockbenchService != null)
                {
                    bool result = await _blockbenchService.LaunchBlockbenchAsync();
                    StatusMessage = result ? "Blockbench 已启动。" : "无法启动 Blockbench。";
                }
                else
                {
                    StatusMessage = "Blockbench 服务不可用。";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"启动 Blockbench 失败: {ex.Message}";
            }
        }

        [RelayCommand]
        private void ShowPreview()
        {
            StatusMessage = "打开预览窗口。";
        }

        [RelayCommand]
        private void ShowResourceManager()
        {
            StatusMessage = "打开资源管理器。";
        }

        [RelayCommand]
        private void PackagePlugin()
        {
            if (!IsProjectOpen) return;
            
            StatusMessage = "打包插件。";
        }

        [RelayCommand]
        private void ImportPlugin()
        {
            StatusMessage = "导入插件。";
        }

        [RelayCommand]
        private void ExportProject()
        {
            if (!IsProjectOpen) return;
            
            StatusMessage = "导出项目。";
        }

        [RelayCommand]
        private async Task ImportTexture()
        {
            if (!IsProjectOpen) return;
            
            StatusMessage = "导入纹理...";
            try
            {
                await Task.Delay(100); // 模拟异步操作
                StatusMessage = "纹理导入完成。";
            }
            catch (Exception ex)
            {
                StatusMessage = $"导入纹理失败: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task ImportModel()
        {
            if (!IsProjectOpen) return;
            
            StatusMessage = "导入模型...";
            try
            {
                await Task.Delay(100); // 模拟异步操作
                StatusMessage = "模型导入完成。";
            }
            catch (Exception ex)
            {
                StatusMessage = $"导入模型失败: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task ImportAudio()
        {
            if (!IsProjectOpen) return;
            
            StatusMessage = "导入音效...";
            try
            {
                await Task.Delay(100); // 模拟异步操作
                StatusMessage = "音效导入完成。";
            }
            catch (Exception ex)
            {
                StatusMessage = $"导入音效失败: {ex.Message}";
            }
        }

        [RelayCommand]
        private void OpenSettings()
        {
            StatusMessage = "打开设置。";
        }

        [RelayCommand]
        private void ResetLayout()
        {
            StatusMessage = "重置布局。";
            ProjectExplorerWidth = new GridLength(250);
            PropertyPanelWidth = new GridLength(300);
            IsProjectExplorerVisible = true;
            IsPropertyPanelVisible = true;
            IsPreviewPanelVisible = true;
        }

        [RelayCommand]
        private void ToggleFullScreen()
        {
            StatusMessage = "切换全屏模式。";
        }

        [RelayCommand]
        private void ShowManual()
        {
            StatusMessage = "打开用户手册。";
        }

        [RelayCommand]
        private void ShowSamples()
        {
            StatusMessage = "显示示例项目。";
        }

        [RelayCommand]
        private async Task CheckForUpdates()
        {
            StatusMessage = "检查更新...";
            try
            {
                await Task.Delay(1000); // 模拟检查更新
                StatusMessage = "已是最新版本。";
            }
            catch (Exception ex)
            {
                StatusMessage = $"检查更新失败: {ex.Message}";
            }
        }

        [RelayCommand]
        private void OpenAbout()
        {
            StatusMessage = "关于 CEEdit。";
            MessageBox.Show("CEEdit v1.0.0\nCraftEngine插件可视化编辑器\n\n专业的Minecraft插件开发工具", 
                          "关于 CEEdit", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        [RelayCommand]
        private async Task OpenRecentProject(ProjectHistoryItem? projectItem)
        {
            if (projectItem == null)
                return;

            try
            {
                StatusMessage = $"正在打开最近项目: {projectItem.ProjectName}...";

                // 检查文件是否存在
                if (!File.Exists(projectItem.ProjectPath))
                {
                    MessageBox.Show($"项目文件不存在: {projectItem.ProjectPath}\n可能已被移动或删除。", "项目不存在", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    
                    // 从最近项目中移除不存在的项目
                    if (_projectHistoryService != null)
                    {
                        await _projectHistoryService.RemoveRecentProjectAsync(projectItem.ProjectPath);
                        await LoadRecentProjectsAsync();
                    }
                    return;
                }

                // 使用ProjectService打开项目
                if (_projectService != null)
                {
                    var project = await _projectService.OpenProjectAsync(projectItem.ProjectPath);
                    
                    // 更新UI状态
                    IsProjectOpen = true;
                    CurrentProjectName = project.Name;
                    StatusMessage = $"项目 '{project.Name}' 已成功打开";
                    
                    // 加载项目结构到UI
                    LoadProjectStructure(project);
                    
                    // 更新最近项目历史（移到最前面）
                    if (_projectHistoryService != null)
                    {
                        await _projectHistoryService.AddRecentProjectAsync(projectItem.ProjectPath, projectItem.ProjectType, projectItem.Description);
                        await LoadRecentProjectsAsync();
                    }
                    
                    OnPropertyChanged(nameof(WindowTitle));
                }
                else
                {
                    StatusMessage = "项目服务不可用，无法打开项目";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"打开最近项目失败: {ex.Message}";
                MessageBox.Show($"打开项目时发生错误:\n{ex.Message}", "错误", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
