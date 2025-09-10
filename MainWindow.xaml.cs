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
    }
}