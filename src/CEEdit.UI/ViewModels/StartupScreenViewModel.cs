using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Win32;

namespace CEEdit.UI.ViewModels
{
    /// <summary>
    /// 启动屏幕视图模型
    /// </summary>
    public class StartupScreenViewModel : INotifyPropertyChanged
    {
        private bool _isProjectSelected;
        private string _selectedProjectPath = string.Empty;

        public StartupScreenViewModel()
        {
            // 初始化命令
            NewProjectCommand = new RelayCommand(ExecuteNewProject);
            OpenProjectCommand = new RelayCommand(ExecuteOpenProject);
            OpenRecentProjectCommand = new RelayCommand<RecentProject>(ExecuteOpenRecentProject);
            CloneRepositoryCommand = new RelayCommand(ExecuteCloneRepository);
            ShowSamplesCommand = new RelayCommand(ExecuteShowSamples);
            ShowDocumentationCommand = new RelayCommand(ExecuteShowDocumentation);
            ShowSettingsCommand = new RelayCommand(ExecuteShowSettings);
            ShowRecentProjectsCommand = new RelayCommand(ExecuteShowRecentProjects);
            ShowTemplatesCommand = new RelayCommand(ExecuteShowTemplates);

            // 加载最近项目
            LoadRecentProjects();
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
        public ObservableCollection<RecentProject> RecentProjects { get; } = new();

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
            // 显示新建项目对话框
            var saveDialog = new SaveFileDialog
            {
                Title = "新建 CEEdit 项目",
                Filter = "CEEdit项目文件 (*.ceproj)|*.ceproj",
                DefaultExt = "ceproj",
                AddExtension = true
            };

            if (saveDialog.ShowDialog() == true)
            {
                CreateNewProject(saveDialog.FileName);
            }
        }

        private void ExecuteOpenProject()
        {
            // 显示打开项目对话框
            var openDialog = new OpenFileDialog
            {
                Title = "打开 CEEdit 项目",
                Filter = "CEEdit项目文件 (*.ceproj)|*.ceproj|所有文件 (*.*)|*.*",
                DefaultExt = "ceproj"
            };

            if (openDialog.ShowDialog() == true)
            {
                OpenProject(openDialog.FileName);
            }
        }

        private void ExecuteOpenRecentProject(RecentProject? project)
        {
            if (project != null)
            {
                OpenProject(project.Path);
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
                AddToRecentProjects(projectPath);
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
                AddToRecentProjects(projectPath);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"打开项目失败: {ex.Message}", "错误", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void LoadRecentProjects()
        {
            // TODO: 从配置文件加载最近项目
            // 暂时添加一些示例数据
            RecentProjects.Clear();
            RecentProjects.Add(new RecentProject 
            { 
                Name = "我的第一个插件", 
                Path = @"C:\Projects\MyFirstPlugin\MyFirstPlugin.ceproj" 
            });
            RecentProjects.Add(new RecentProject 
            { 
                Name = "魔法物品模组", 
                Path = @"C:\Projects\MagicItems\MagicItems.ceproj" 
            });
        }

        private void AddToRecentProjects(string projectPath)
        {
            var projectName = Path.GetFileNameWithoutExtension(projectPath);
            var existingProject = RecentProjects.FirstOrDefault(p => p.Path == projectPath);

            if (existingProject != null)
            {
                // 移到最前面
                RecentProjects.Remove(existingProject);
                RecentProjects.Insert(0, existingProject);
            }
            else
            {
                // 添加新项目
                RecentProjects.Insert(0, new RecentProject 
                { 
                    Name = projectName, 
                    Path = projectPath 
                });
            }

            // 保持最多10个最近项目
            while (RecentProjects.Count > 10)
            {
                RecentProjects.RemoveAt(RecentProjects.Count - 1);
            }

            // TODO: 保存到配置文件
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
    /// 最近项目信息
    /// </summary>
    public class RecentProject
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
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