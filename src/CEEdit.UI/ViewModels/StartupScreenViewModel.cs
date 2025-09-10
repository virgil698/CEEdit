using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using YamlDotNet.Serialization;
using CEEdit.Core.Models.Project;
using CEEdit.Core.Services.Implementations;
using CEEdit.Core.Services.Interfaces;

namespace CEEdit.UI.ViewModels
{
    /// <summary>
    /// 启动屏幕视图模型
    /// </summary>
    public class StartupScreenViewModel : INotifyPropertyChanged
    {
        private bool _isProjectSelected;
        private string _selectedProjectPath = string.Empty;
        private readonly ProjectHistoryService _projectHistoryService;
        private readonly IProjectLoaderService _projectLoaderService;
        private LoadedProjectData? _loadedProjectData;

        public StartupScreenViewModel()
        {
            _projectHistoryService = new ProjectHistoryService();
            _projectLoaderService = new ProjectLoaderService();
            // 初始化命令
            NewProjectCommand = new RelayCommand(ExecuteNewProject);
            OpenProjectCommand = new RelayCommand(ExecuteOpenProject);
            OpenRecentProjectCommand = new RelayCommand<ProjectHistoryItem>(ExecuteOpenRecentProject);
            CloneRepositoryCommand = new RelayCommand(ExecuteCloneRepository);
            ShowSamplesCommand = new RelayCommand(ExecuteShowSamples);
            ShowDocumentationCommand = new RelayCommand(ExecuteShowDocumentation);
            ShowSettingsCommand = new RelayCommand(ExecuteShowSettings);
            ShowRecentProjectsCommand = new RelayCommand(ExecuteShowRecentProjects);
            ShowTemplatesCommand = new RelayCommand(ExecuteShowTemplates);

            // 加载最近项目
            _ = LoadRecentProjectsAsync();
        }

        #region 属性

        /// <summary>
        /// 是否已选择项目
        /// </summary>
        public bool IsProjectSelected
        {
            get => _isProjectSelected;
            private set
            {
                if (_isProjectSelected != value)
                {
                    _isProjectSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 选择的项目路径
        /// </summary>
        public string SelectedProjectPath
        {
            get => _selectedProjectPath;
            private set
            {
                if (_selectedProjectPath != value)
                {
                    _selectedProjectPath = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 最近项目列表
        /// </summary>
        public ObservableCollection<ProjectHistoryItem> RecentProjects { get; } = new();

        /// <summary>
        /// 加载的项目数据
        /// </summary>
        public LoadedProjectData? LoadedProjectData => _loadedProjectData;

        #endregion

        #region 命令

        public ICommand NewProjectCommand { get; }
        public ICommand OpenProjectCommand { get; }
        public ICommand OpenRecentProjectCommand { get; }
        public ICommand CloneRepositoryCommand { get; }
        public ICommand ShowSamplesCommand { get; }
        public ICommand ShowDocumentationCommand { get; }
        public ICommand ShowSettingsCommand { get; }
        public ICommand ShowRecentProjectsCommand { get; }
        public ICommand ShowTemplatesCommand { get; }

        #endregion

        #region 命令实现

        private void ExecuteNewProject()
        {
            try
            {
                // 直接打开新建项目窗口
                var newProjectWindow = new CEEdit.UI.Views.Windows.NewProjectWindow();
                newProjectWindow.ShowDialog();
                
                if (newProjectWindow.DialogResult)
                {
                    // 收集用户输入的所有信息
                    var projectInfo = new ProjectCreationInfo
                    {
                        ProjectName = newProjectWindow.ProjectName,
                        ProjectLocation = newProjectWindow.ProjectLocation,
                        ProjectVersion = newProjectWindow.ProjectVersion,
                        ProjectAuthor = newProjectWindow.ProjectAuthor,
                        ProjectDescription = newProjectWindow.ProjectDescription,
                        IsProjectEnabled = newProjectWindow.IsProjectEnabled,
                        SelectedTemplate = newProjectWindow.SelectedTemplate,
                        InitGit = newProjectWindow.InitGit,
                        AddSampleCode = newProjectWindow.AddSampleCode,
                        CreateReadme = newProjectWindow.CreateReadme
                    };
                    
                    var fullProjectPath = Path.Combine(projectInfo.ProjectLocation, projectInfo.ProjectName);
                    var projectFilePath = Path.Combine(fullProjectPath, $"{projectInfo.ProjectName}.ceproj");
                    
                    CreateNewProject(projectFilePath, projectInfo);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"创建项目失败: {ex.Message}", "错误", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void ExecuteOpenProject()
        {
            try
            {
                // 打开项目选择窗口，传递项目历史服务
                var openProjectWindow = new CEEdit.UI.Views.Windows.OpenProjectWindow(_projectHistoryService);
                var result = openProjectWindow.ShowDialog();
                
                if (result == true && !string.IsNullOrEmpty(openProjectWindow.SelectedProjectPath))
                {
                    OpenProject(openProjectWindow.SelectedProjectPath);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"打开项目窗口失败: {ex.Message}", "错误", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void ExecuteOpenRecentProject(ProjectHistoryItem? project)
        {
            if (project != null)
            {
                OpenProject(project.ProjectPath);
            }
        }

        private void ExecuteCloneRepository()
        {
            // TODO: 实现仓库克隆功能
            System.Windows.MessageBox.Show("仓库克隆功能即将推出！", "提示", 
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        private void ExecuteShowSamples()
        {
            // TODO: 显示示例项目
            System.Windows.MessageBox.Show("示例项目功能即将推出！", "提示", 
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        private void ExecuteShowDocumentation()
        {
            // TODO: 打开用户手册
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://github.com/BlockBridge/CEEdit/wiki",
                    UseShellExecute = true
                });
            }
            catch
            {
                System.Windows.MessageBox.Show("无法打开用户手册。请检查网络连接。", "错误", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }

        private void ExecuteShowSettings()
        {
            // TODO: 显示设置页面
            System.Windows.MessageBox.Show("设置功能即将推出！", "提示", 
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        private void ExecuteShowRecentProjects()
        {
            // 切换到最近项目视图
            // TODO: 实现视图切换逻辑
        }

        private void ExecuteShowTemplates()
        {
            // 切换到项目模板视图
            // TODO: 实现模板选择逻辑
        }

        #endregion

        #region 私有方法

        private void CreateNewProject(string projectPath, ProjectCreationInfo projectInfo)
        {
            try
            {
                // 创建项目文件结构
                var projectDir = Path.GetDirectoryName(projectPath);
                if (projectDir != null && !Directory.Exists(projectDir))
                {
                    Directory.CreateDirectory(projectDir);
                }

                // 创建项目子目录
                CreateProjectDirectories(projectDir!);

                // 生成pack.yml文件
                GeneratePackYmlFile(projectDir!, projectInfo);

                // 创建项目文件内容
                var projectContent = GenerateProjectTemplate(projectInfo);
                File.WriteAllText(projectPath, projectContent);

                // 如果需要创建README
                if (projectInfo.CreateReadme)
                {
                    GenerateReadmeFile(projectDir!, projectInfo);
                }

                // 如果需要初始化Git
                if (projectInfo.InitGit)
                {
                    InitializeGitRepository(projectDir!);
                }

                // 如果需要添加示例代码
                if (projectInfo.AddSampleCode)
                {
                    GenerateSampleCode(projectDir!, projectInfo);
                }

                // 设置为已选择项目
                SelectedProjectPath = projectPath;
                IsProjectSelected = true;

                // 添加到最近项目列表
                _ = AddToRecentProjectsAsync(projectPath);
                
                System.Diagnostics.Debug.WriteLine($"项目创建成功: {projectPath}");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"创建项目失败: {ex.Message}", "错误", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public async void OpenProject(string projectPath)
        {
            try
            {
                if (!File.Exists(projectPath))
                {
                    System.Windows.MessageBox.Show("项目文件不存在！", "错误", 
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }

                // 显示加载状态
                System.Diagnostics.Debug.WriteLine($"开始加载项目: {projectPath}");

                // 使用ProjectLoaderService加载项目数据
                _loadedProjectData = await _projectLoaderService.LoadProjectAsync(projectPath);

                if (_loadedProjectData == null || !_loadedProjectData.IsLoaded)
                {
                    var errorMsg = _loadedProjectData?.LoadError ?? "未知错误";
                    System.Windows.MessageBox.Show($"加载项目失败: {errorMsg}", "错误", 
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }

                // 设置为已选择项目
                SelectedProjectPath = projectPath;
                IsProjectSelected = true;

                // 添加到最近项目列表
                _ = AddToRecentProjectsAsync(projectPath);

                System.Diagnostics.Debug.WriteLine($"项目加载完成: {_loadedProjectData.Project.Name}");
                System.Diagnostics.Debug.WriteLine($"项目目录: {_loadedProjectData.ProjectDirectory}");
                System.Diagnostics.Debug.WriteLine($"pack.yml: {(_loadedProjectData.PackYml != null ? "已加载" : "未找到")}");
                System.Diagnostics.Debug.WriteLine($"文件统计: {_loadedProjectData.FileStructure.Blocks.Count}个方块, " +
                    $"{_loadedProjectData.FileStructure.Items.Count}个物品, " +
                    $"{_loadedProjectData.FileStructure.Recipes.Count}个配方");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"打开项目失败: {ex.Message}", "错误", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private async Task LoadRecentProjectsAsync()
        {
            try
            {
                var recentProjectItems = await _projectHistoryService.GetRecentProjectsAsync(10);
                
                // 清空现有列表并添加新项目
                RecentProjects.Clear();
                foreach (var project in recentProjectItems)
                {
                    RecentProjects.Add(project);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载最近项目失败: {ex.Message}");
                // 加载失败时清空列表
                RecentProjects.Clear();
            }
        }

        private async Task AddToRecentProjectsAsync(string projectPath)
        {
            try
            {
                await _projectHistoryService.AddRecentProjectAsync(projectPath, "CraftEngine项目");
                // 重新加载最近项目列表
                await LoadRecentProjectsAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"添加最近项目失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 生成pack.yml文件
        /// </summary>
        private void GeneratePackYmlFile(string projectDir, ProjectCreationInfo projectInfo)
        {
            var packYml = new PackYml
            {
                Author = projectInfo.ProjectAuthor,
                Version = projectInfo.ProjectVersion,
                Description = projectInfo.ProjectDescription,
                Namespace = projectInfo.GetNamespace(),
                Enable = projectInfo.IsProjectEnabled
            };

            var serializer = new SerializerBuilder()
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                .Build();

            var yamlContent = serializer.Serialize(packYml);
            var packYmlPath = Path.Combine(projectDir, "pack.yml");
            
            File.WriteAllText(packYmlPath, yamlContent);
            System.Diagnostics.Debug.WriteLine($"已创建pack.yml文件: {packYmlPath}");
        }

        /// <summary>
        /// 生成README.md文件
        /// </summary>
        private void GenerateReadmeFile(string projectDir, ProjectCreationInfo projectInfo)
        {
            var readmeContent = $"""
                # {projectInfo.ProjectName}

                {projectInfo.ProjectDescription}

                ## 项目信息

                - **作者**: {projectInfo.ProjectAuthor}
                - **版本**: {projectInfo.ProjectVersion}
                - **命名空间**: {projectInfo.GetNamespace()}

                ## 项目结构

                ```
                {projectInfo.ProjectName}/
                ├── pack.yml              # 项目配置文件
                ├── {projectInfo.ProjectName}.ceproj    # CEEdit项目文件
                ├── blocks/               # 方块定义
                ├── items/                # 物品定义
                ├── recipes/              # 配方定义
                └── resources/            # 资源文件
                    ├── textures/         # 纹理文件
                    ├── models/           # 模型文件
                    └── sounds/           # 音效文件
                ```

                ## 开始使用

                1. 使用CEEdit打开此项目
                2. 在相应目录中添加你的方块、物品和配方
                3. 在resources目录中添加纹理和其他资源文件
                4. 构建并导出你的插件

                ---
                
                *由CEEdit自动生成于 {DateTime.Now:yyyy-MM-dd HH:mm:ss}*
                """;

            var readmePath = Path.Combine(projectDir, "README.md");
            File.WriteAllText(readmePath, readmeContent);
            System.Diagnostics.Debug.WriteLine($"已创建README.md文件: {readmePath}");
        }

        /// <summary>
        /// 初始化Git仓库
        /// </summary>
        private void InitializeGitRepository(string projectDir)
        {
            try
            {
                // 创建.gitignore文件
                var gitignoreContent = """
                    # CEEdit临时文件
                    *.tmp
                    *.cache
                    .ceedit/

                    # 构建输出
                    build/
                    dist/
                    output/

                    # 系统文件
                    .DS_Store
                    Thumbs.db
                    desktop.ini

                    # IDE文件
                    .vscode/
                    .idea/
                    """;

                var gitignorePath = Path.Combine(projectDir, ".gitignore");
                File.WriteAllText(gitignorePath, gitignoreContent);

                // 尝试初始化Git仓库
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "init",
                    WorkingDirectory = projectDir,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(processStartInfo);
                process?.WaitForExit();

                System.Diagnostics.Debug.WriteLine($"已初始化Git仓库: {projectDir}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化Git仓库失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 生成示例代码
        /// </summary>
        private void GenerateSampleCode(string projectDir, ProjectCreationInfo projectInfo)
        {
            var namespaceName = projectInfo.GetNamespace();
            
            // 创建示例方块
            var sampleBlockContent = $$"""
                {
                  "id": "{{namespaceName}}_example_block",
                  "name": "示例方块",
                  "material": "STONE",
                  "hardness": 3.0,
                  "resistance": 6.0,
                  "tool": "pickaxe",
                  "texture": {
                    "all": "{{namespaceName}}/blocks/example_block"
                  },
                  "drops": [
                    {
                      "item": "{{namespaceName}}_example_block",
                      "count": 1
                    }
                  ]
                }
                """;

            var blocksDir = Path.Combine(projectDir, "blocks");
            File.WriteAllText(Path.Combine(blocksDir, "example_block.json"), sampleBlockContent);

            // 创建示例物品
            var sampleItemContent = $$"""
                {
                  "id": "{{namespaceName}}_example_item",
                  "name": "示例物品",
                  "material": "STICK",
                  "texture": "{{namespaceName}}/items/example_item",
                  "stackable": true,
                  "maxStackSize": 64,
                  "description": "这是一个示例物品"
                }
                """;

            var itemsDir = Path.Combine(projectDir, "items");
            File.WriteAllText(Path.Combine(itemsDir, "example_item.json"), sampleItemContent);

            // 创建示例配方
            var sampleRecipeContent = $$"""
                {
                  "type": "shaped",
                  "result": {
                    "item": "{{namespaceName}}_example_block",
                    "count": 1
                  },
                  "pattern": [
                    "SSS",
                    "SSS",
                    "SSS"
                  ],
                  "ingredients": {
                    "S": "STONE"
                  }
                }
                """;

            var recipesDir = Path.Combine(projectDir, "recipes");
            File.WriteAllText(Path.Combine(recipesDir, "example_block_recipe.json"), sampleRecipeContent);

            System.Diagnostics.Debug.WriteLine($"已生成示例代码文件");
        }

        private string GenerateProjectTemplate(ProjectCreationInfo projectInfo)
        {
            return $$"""
                   {
                     "name": "{{projectInfo.ProjectName}}",
                     "version": "{{projectInfo.ProjectVersion}}",
                     "description": "{{projectInfo.ProjectDescription}}",
                     "author": "{{projectInfo.ProjectAuthor}}",
                     "namespace": "{{projectInfo.GetNamespace()}}",
                     "enabled": {{projectInfo.IsProjectEnabled.ToString().ToLower()}},
                     "template": "{{projectInfo.SelectedTemplate}}",
                     "minecraft_version": "1.20.4",
                     "api_version": "1.20",
                     "created": "{{DateTime.Now:yyyy-MM-dd HH:mm:ss}}",
                     "blocks": [],
                     "items": [],
                     "recipes": [],
                     "resources": {
                       "textures": [],
                       "models": [],
                       "sounds": []
                     }
                   }
                   """;
        }

        private void CreateProjectDirectories(string projectDir)
        {
            // 创建项目标准目录结构
            Directory.CreateDirectory(Path.Combine(projectDir, "blocks"));
            Directory.CreateDirectory(Path.Combine(projectDir, "items"));
            Directory.CreateDirectory(Path.Combine(projectDir, "recipes"));
            Directory.CreateDirectory(Path.Combine(projectDir, "resources", "textures"));
            Directory.CreateDirectory(Path.Combine(projectDir, "resources", "models"));
            Directory.CreateDirectory(Path.Combine(projectDir, "resources", "sounds"));
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }


    /// <summary>
    /// 简单的RelayCommand实现
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;
        public void Execute(object? parameter) => _execute();
    }

    /// <summary>
    /// 带参数的RelayCommand实现
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T?> _execute;
        private readonly Func<T?, bool>? _canExecute;

        public RelayCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke((T?)parameter) ?? true;
        public void Execute(object? parameter) => _execute((T?)parameter);
    }
}