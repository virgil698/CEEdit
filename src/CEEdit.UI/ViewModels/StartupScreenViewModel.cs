using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using CEEdit.Core.Models.Project;
using CEEdit.Core.Services.Implementations;

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

        public StartupScreenViewModel()
        {
            _projectHistoryService = new ProjectHistoryService();
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
                    var projectName = newProjectWindow.ProjectName;
                    var projectLocation = newProjectWindow.ProjectLocation;
                    var fullProjectPath = Path.Combine(projectLocation, projectName);
                    
                    // 创建项目文件路径
                    var projectFilePath = Path.Combine(fullProjectPath, $"{projectName}.ceproj");
                    CreateNewProject(projectFilePath);
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

        private void CreateNewProject(string projectPath)
        {
            try
            {
                // 创建项目文件结构
                var projectDir = Path.GetDirectoryName(projectPath);
                if (projectDir != null && !Directory.Exists(projectDir))
                {
                    Directory.CreateDirectory(projectDir);
                }

                // 创建基本的项目文件内容
                var projectContent = GenerateProjectTemplate();
                File.WriteAllText(projectPath, projectContent);

                // 创建项目子目录
                CreateProjectDirectories(projectDir!);

                // 设置为已选择项目
                SelectedProjectPath = projectPath;
                IsProjectSelected = true;

                // 添加到最近项目列表
                _ = AddToRecentProjectsAsync(projectPath);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"创建项目失败: {ex.Message}", "错误", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public void OpenProject(string projectPath)
        {
            try
            {
                if (!File.Exists(projectPath))
                {
                    System.Windows.MessageBox.Show("项目文件不存在！", "错误", 
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }

                // 设置为已选择项目
                SelectedProjectPath = projectPath;
                IsProjectSelected = true;

                // 添加到最近项目列表
                _ = AddToRecentProjectsAsync(projectPath);
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

        private string GenerateProjectTemplate()
        {
            return """
                   {
                     "name": "CEEdit Project",
                     "version": "1.0.0",
                     "description": "A CraftEngine plugin project",
                     "author": "Your Name",
                     "minecraft_version": "1.20.4",
                     "api_version": "1.20",
                     "main": "plugin.yml",
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