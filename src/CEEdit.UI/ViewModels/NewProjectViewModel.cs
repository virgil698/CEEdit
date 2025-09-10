using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CEEdit.UI.ViewModels
{
    public class NewProjectViewModel : INotifyPropertyChanged
    {
        #region Private Fields
        private string _projectName = "MyPlugin";
        private string _projectLocation = "";
        private string _projectVersion = "1.0.0";
        private string _projectAuthor = "";
        private string _projectDescription = "";
        private bool _isProjectEnabled = true;
        private string _selectedTemplate = "basic";
        private bool _initGit = false;
        private bool _addSampleCode = true;
        private bool _createReadme = true;
        private string _statusText = "准备创建新项目...";
        private bool _canCreateProject = false;
        #endregion

        #region Public Properties
        public string ProjectName
        {
            get => _projectName;
            set
            {
                if (SetProperty(ref _projectName, value))
                {
                    ValidateAndUpdateStatus();
                }
            }
        }

        public string ProjectLocation
        {
            get => _projectLocation;
            set
            {
                if (SetProperty(ref _projectLocation, value))
                {
                    ValidateAndUpdateStatus();
                }
            }
        }

        public string ProjectVersion
        {
            get => _projectVersion;
            set => SetProperty(ref _projectVersion, value);
        }

        public string ProjectAuthor
        {
            get => _projectAuthor;
            set => SetProperty(ref _projectAuthor, value);
        }

        public string ProjectDescription
        {
            get => _projectDescription;
            set => SetProperty(ref _projectDescription, value);
        }

        public bool IsProjectEnabled
        {
            get => _isProjectEnabled;
            set => SetProperty(ref _isProjectEnabled, value);
        }

        public string SelectedTemplate
        {
            get => _selectedTemplate;
            set
            {
                if (SetProperty(ref _selectedTemplate, value))
                {
                    OnTemplateChanged();
                }
            }
        }

        public bool InitGit
        {
            get => _initGit;
            set => SetProperty(ref _initGit, value);
        }

        public bool AddSampleCode
        {
            get => _addSampleCode;
            set => SetProperty(ref _addSampleCode, value);
        }

        public bool CreateReadme
        {
            get => _createReadme;
            set => SetProperty(ref _createReadme, value);
        }

        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        public bool CanCreateProject
        {
            get => _canCreateProject;
            set => SetProperty(ref _canCreateProject, value);
        }

        public ObservableCollection<ProjectTemplate> Templates { get; private set; }

        #endregion

        #region Commands
        public ICommand BrowseLocationCommand { get; private set; }
        public ICommand CreateProjectCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        #endregion

        #region Constructor
        public NewProjectViewModel()
        {
            Templates = new ObservableCollection<ProjectTemplate>();
            BrowseLocationCommand = new RelayCommand(BrowseLocation);
            CreateProjectCommand = new RelayCommand(CreateProject, () => CanCreateProject);
            CancelCommand = new RelayCommand(Cancel);
            
            InitializeDefaults();
            InitializeTemplates();
            ValidateAndUpdateStatus();
        }
        #endregion

        #region Private Methods
        private void InitializeDefaults()
        {
            // 设置默认项目位置
            ProjectLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CEEdit Projects");
            
            // 设置默认作者为系统用户名
            ProjectAuthor = Environment.UserName;
        }

        private void InitializeTemplates()
        {
            Templates = new ObservableCollection<ProjectTemplate>
            {
                new ProjectTemplate
                {
                    Id = "basic",
                    Name = "基础插件",
                    Icon = "📄",
                    Description = "创建一个基础的CraftEngine插件项目",
                    Category = "基础模板",
                    Preview = @"项目结构预览:
📁 项目根目录/
  📄 plugin.yml - 插件配置文件
  📁 src/ - 源代码目录
  📁 resources/ - 资源文件目录
  📄 README.md - 项目说明"
                },
                new ProjectTemplate
                {
                    Id = "advanced",
                    Name = "高级插件",
                    Icon = "🚀",
                    Description = "包含示例方块、物品和配方的完整项目",
                    Category = "基础模板",
                    Preview = @"项目结构预览:
📁 项目根目录/
  📄 plugin.yml - 插件配置文件
  📁 blocks/ - 方块定义目录
    📄 example_block.yml
  📁 items/ - 物品定义目录
    📄 example_item.yml
  📁 recipes/ - 配方定义目录
    📄 example_recipe.yml
  📁 resources/textures/ - 纹理资源
  📄 README.md - 项目说明"
                },
                new ProjectTemplate
                {
                    Id = "blocks",
                    Name = "方块包",
                    Icon = "🧱",
                    Description = "专注于自定义方块的插件项目",
                    Category = "专项模板",
                    Preview = @"项目结构预览:
📁 项目根目录/
  📄 plugin.yml - 插件配置文件
  📁 blocks/ - 方块定义目录
    📄 stone_block.yml
    📄 wood_block.yml
  📁 resources/textures/blocks/ - 方块纹理
  📁 resources/models/blocks/ - 方块模型
  📄 README.md - 项目说明"
                },
                new ProjectTemplate
                {
                    Id = "items",
                    Name = "物品包",
                    Icon = "🎒",
                    Description = "专注于自定义物品的插件项目",
                    Category = "专项模板",
                    Preview = @"项目结构预览:
📁 项目根目录/
  📄 plugin.yml - 插件配置文件
  📁 items/ - 物品定义目录
    📄 magic_sword.yml
    📄 health_potion.yml
  📁 resources/textures/items/ - 物品纹理
  📁 resources/models/items/ - 物品模型
  📄 README.md - 项目说明"
                },
                new ProjectTemplate
                {
                    Id = "recipes",
                    Name = "配方包",
                    Icon = "⚗️",
                    Description = "专注于合成配方的插件项目",
                    Category = "专项模板",
                    Preview = @"项目结构预览:
📁 项目根目录/
  📄 plugin.yml - 插件配置文件
  📁 recipes/ - 配方定义目录
    📄 crafting_recipes.yml
    📄 smelting_recipes.yml
  📁 blocks/ - 相关方块定义
  📁 items/ - 相关物品定义
  📄 README.md - 项目说明"
                }
            };
        }

        private void OnTemplateChanged()
        {
            // 当模板变更时的额外逻辑
            ValidateAndUpdateStatus();
        }

        private void ValidateAndUpdateStatus()
        {
            var projectName = ProjectName?.Trim() ?? "";
            var projectLocation = ProjectLocation?.Trim() ?? "";
            
            if (string.IsNullOrEmpty(projectName))
            {
                StatusText = "⚠️ 请输入项目名称";
                CanCreateProject = false;
                return;
            }
            
            if (string.IsNullOrEmpty(projectLocation))
            {
                StatusText = "⚠️ 请选择项目位置";
                CanCreateProject = false;
                return;
            }
            
            // 检查项目名称是否包含非法字符
            var invalidChars = Path.GetInvalidFileNameChars();
            if (projectName.IndexOfAny(invalidChars) >= 0)
            {
                StatusText = "⚠️ 项目名称包含非法字符";
                CanCreateProject = false;
                return;
            }
            
            var fullPath = Path.Combine(projectLocation, projectName);
            if (Directory.Exists(fullPath))
            {
                StatusText = "⚠️ 项目目录已存在";
                CanCreateProject = false;
                return;
            }
            
            StatusText = $"✅ 准备在 {fullPath} 创建项目";
            CanCreateProject = true;
        }

        private void BrowseLocation()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "选择项目位置",
                UseDescriptionForTitle = true,
                SelectedPath = ProjectLocation
            };
            
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ProjectLocation = dialog.SelectedPath;
            }
        }

        private void CreateProject()
        {
            // 这里将由调用者处理项目创建逻辑
            ProjectCreated?.Invoke(this, EventArgs.Empty);
        }

        private void Cancel()
        {
            ProjectCancelled?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Events
        public event EventHandler? ProjectCreated;
        public event EventHandler? ProjectCancelled;
        #endregion

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    #region Helper Classes
    public class ProjectTemplate
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Icon { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public string Preview { get; set; } = "";
    }

    #endregion
}
