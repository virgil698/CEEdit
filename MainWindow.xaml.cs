using System;
using System.Windows;
using System.Windows.Controls;
using CEEdit.UI.ViewModels;
using CEEdit.Core.Services.Interfaces;

namespace CEEdit
{
    /// <summary>
    /// 主窗口交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoadedProjectData? _loadedProjectData;

        public MainWindow()
        {
            InitializeComponent();
            InitializeViewModel();
        }

        public MainWindow(IProjectService projectService, IProjectHistoryService historyService)
        {
            InitializeComponent();
            InitializeViewModel(projectService, historyService);
        }

        public MainWindow(LoadedProjectData? loadedProjectData)
        {
            InitializeComponent();
            _loadedProjectData = loadedProjectData;
            InitializeViewModel();
            
            // 如果有项目数据，加载项目
            if (_loadedProjectData != null)
            {
                LoadProjectData(_loadedProjectData);
            }
        }

        private void InitializeViewModel(IProjectService? projectService = null, IProjectHistoryService? historyService = null)
        {
            try
            {
                // 创建MainViewModel
                var viewModel = new MainViewModel(projectService);
                DataContext = viewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化视图模型失败: {ex.Message}", "错误", 
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 检查是否有未保存的更改
            if (DataContext is MainViewModel viewModel)
            {
                if (viewModel.IsProjectOpen)
                {
                    var result = MessageBox.Show(
                        "是否保存当前项目的更改？",
                        "CEEdit",
                        MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Question);

                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            // TODO: 保存项目
                            viewModel.SaveProjectCommand?.Execute(null);
                            break;
                        case MessageBoxResult.Cancel:
                            e.Cancel = true;
                            return;
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 窗口加载完成后的初始化工作
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.StatusMessage = "CEEdit 已就绪";
            }
            
            // TODO: 检查Blockbench是否可用
            // TODO: 加载最近项目列表
            // TODO: 检查软件更新
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            // 窗口状态改变处理
            if (WindowState == WindowState.Maximized)
            {
                // 最大化时的处理
            }
        }

        private void ProjectTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // 项目树选中项改变时的处理
            if (DataContext is MainViewModel viewModel && e.NewValue != null)
            {
                viewModel.StatusMessage = $"已选择: {e.NewValue}";
                // TODO: 更新属性面板显示选中项的属性
                viewModel.SelectedItemProperties = e.NewValue;
            }
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 标签页切换时的处理
            if (e.Source == MainTabControl && MainTabControl.SelectedItem is TabItem tab && 
                DataContext is MainViewModel viewModel)
            {
                viewModel.StatusMessage = $"当前编辑器: {tab.Header}";
            }
        }

        /// <summary>
        /// 加载项目数据到编辑器UI
        /// </summary>
        /// <param name="projectData">项目数据</param>
        private void LoadProjectData(LoadedProjectData projectData)
        {
            try
            {
                // 更新窗口标题
                Title = $"CEEdit - {projectData.Project.Name}";

                System.Diagnostics.Debug.WriteLine("开始加载项目数据到编辑器...");
                System.Diagnostics.Debug.WriteLine($"项目名称: {projectData.Project.Name}");
                System.Diagnostics.Debug.WriteLine($"项目版本: {projectData.Project.Version}");
                System.Diagnostics.Debug.WriteLine($"项目作者: {projectData.Project.Author}");
                System.Diagnostics.Debug.WriteLine($"项目目录: {projectData.ProjectDirectory}");
                
                if (projectData.PackYml != null)
                {
                    System.Diagnostics.Debug.WriteLine($"pack.yml信息:");
                    System.Diagnostics.Debug.WriteLine($"  - 作者: {projectData.PackYml.Author}");
                    System.Diagnostics.Debug.WriteLine($"  - 版本: {projectData.PackYml.Version}");
                    System.Diagnostics.Debug.WriteLine($"  - 描述: {projectData.PackYml.Description}");
                    System.Diagnostics.Debug.WriteLine($"  - 命名空间: {projectData.PackYml.Namespace}");
                    System.Diagnostics.Debug.WriteLine($"  - 启用状态: {projectData.PackYml.Enable}");
                }

                System.Diagnostics.Debug.WriteLine($"项目文件结构:");
                System.Diagnostics.Debug.WriteLine($"  - 方块文件: {projectData.FileStructure.Blocks.Count}个");
                System.Diagnostics.Debug.WriteLine($"  - 物品文件: {projectData.FileStructure.Items.Count}个");
                System.Diagnostics.Debug.WriteLine($"  - 配方文件: {projectData.FileStructure.Recipes.Count}个");
                System.Diagnostics.Debug.WriteLine($"  - 纹理文件: {projectData.FileStructure.Textures.Count}个");
                System.Diagnostics.Debug.WriteLine($"  - 模型文件: {projectData.FileStructure.Models.Count}个");
                System.Diagnostics.Debug.WriteLine($"  - 音效文件: {projectData.FileStructure.Sounds.Count}个");

                // 将项目数据传递给MainViewModel
                if (DataContext is MainViewModel mainViewModel)
                {
                    mainViewModel.LoadProject(projectData);
                }

                System.Diagnostics.Debug.WriteLine("项目数据加载完成，编辑器UI已准备就绪");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载项目数据失败: {ex.Message}");
                MessageBox.Show($"加载项目数据失败: {ex.Message}", "错误", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}