using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CEEdit.Core.Models.Project;
using CEEdit.Core.Services.Interfaces;

namespace CEEdit.UI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IProjectService? _projectService;
        private readonly IFileService? _fileService;
        private readonly IResourceService? _resourceService;
        private readonly IValidationService? _validationService;
        private readonly IBlockbenchService? _blockbenchService;

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

        public string WindowTitle => IsProjectOpen 
            ? $"{CurrentProjectName} - CEEdit" 
            : "CEEdit - CraftEngine插件可视化编辑器";

        public string StatusText => StatusMessage;
        public string ProjectStatus => CurrentProjectName;

        public MainViewModel(IProjectService? projectService = null, IFileService? fileService = null,
                            IResourceService? resourceService = null, IValidationService? validationService = null,
                            IBlockbenchService? blockbenchService = null)
        {
            _projectService = projectService;
            _fileService = fileService;
            _resourceService = resourceService;
            _validationService = validationService;
            _blockbenchService = blockbenchService;
            
            InitializeProjectTree();
        }

        private void InitializeProjectTree()
        {
            // 添加一些示例项目结构
            ProjectItems.Clear();
            // 这里暂时不添加内容，等实际项目加载时再填充
        }

        [RelayCommand]
        private async Task NewProject()
        {
            StatusMessage = "创建新项目...";
            try
            {
                await Task.Delay(100); // 模拟异步操作
                StatusMessage = "新项目创建完成。";
                IsProjectOpen = true;
                CurrentProjectName = "新项目";
                OnPropertyChanged(nameof(WindowTitle));
            }
            catch (Exception ex)
            {
                StatusMessage = $"创建项目失败: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task OpenProject()
        {
            StatusMessage = "打开项目...";
            try
            {
                // TODO: 实现项目打开逻辑
                await Task.Delay(100); // 模拟异步操作
                StatusMessage = "项目已打开。";
                IsProjectOpen = true;
                CurrentProjectName = "示例项目";
                OnPropertyChanged(nameof(WindowTitle));
            }
            catch (Exception ex)
            {
                StatusMessage = $"打开项目失败: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task SaveProject()
        {
            if (!IsProjectOpen) return;
            
            StatusMessage = "保存项目...";
            try
            {
                if (_projectService != null)
                {
                    // TODO: 获取当前项目实例
                    // await _projectService.SaveProjectAsync(currentProject);
                }
                await Task.Delay(100); // 模拟异步操作
                StatusMessage = "项目已保存。";
            }
            catch (Exception ex)
            {
                StatusMessage = $"保存项目失败: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task SaveProjectAs()
        {
            if (!IsProjectOpen) return;
            
            StatusMessage = "项目另存为...";
            try
            {
                await Task.Delay(100); // 模拟异步操作
                StatusMessage = "项目已另存为。";
            }
            catch (Exception ex)
            {
                StatusMessage = $"另存为失败: {ex.Message}";
            }
        }

        [RelayCommand]
        private void CloseProject()
        {
            StatusMessage = "关闭项目。";
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
    }
}
