using System;
using System.Windows;
using CEEdit.UI.ViewModels;

namespace CEEdit.UI.Views.Windows
{
    /// <summary>
    /// StartupWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
            DataContext = new StartupScreenViewModel();
            
            // 设置窗口属性
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ShowInTaskbar = true;
            
            // 添加事件处理
            this.Loaded += StartupWindow_Loaded;
        }

        private void StartupWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 窗口加载完成后的初始化逻辑
            if (DataContext is StartupScreenViewModel viewModel)
            {
                viewModel.PropertyChanged += ViewModel_PropertyChanged;
            }
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is StartupScreenViewModel viewModel)
            {
                // 监听项目打开事件
                if (e.PropertyName == nameof(StartupScreenViewModel.IsProjectSelected) && 
                    viewModel.IsProjectSelected)
                {
                    OpenMainWindow();
                }
            }
        }

        private void OpenMainWindow()
        {
            try
            {
                // 创建并显示主窗口
                var mainWindow = new MainWindow();
                mainWindow.Show();

                // 关闭启动窗口
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开主窗口时发生错误: {ex.Message}", 
                               "错误", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Error);
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // 如果用户关闭启动窗口但没有选择项目，退出应用程序
            if (DataContext is StartupScreenViewModel viewModel && !viewModel.IsProjectSelected)
            {
                Application.Current.Shutdown();
            }
            
            base.OnClosing(e);
        }
    }
}
