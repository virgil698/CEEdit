using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CEEdit.Core.Services.Interfaces;
using CEEdit.Core.Services.Implementations;

namespace CEEdit
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private IHost? _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // 全局异常处理
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            
            // 初始化依赖注入容器
            _host = CreateHostBuilder().Build();
            
            // 启动项目启动窗口
            var startupWindow = new CEEdit.UI.Views.Windows.StartupWindow();
            startupWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host?.Dispose();
            base.OnExit(e);
        }

        private IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // 注册服务
                    services.AddSingleton<IProjectHistoryService, ProjectHistoryService>();
                    // TODO: 暂时注释掉复杂的服务，等待接口定义完善
                    // services.AddTransient<IProjectService, ProjectService>();
                    // services.AddTransient<IFileService, FileService>();
                    
                    // 注册窗口
                    services.AddTransient<MainWindow>();
                });
        }

        private void App_DispatcherUnhandledException(object sender, 
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"发生未处理的异常：{e.Exception.Message}", 
                          "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            
            // TODO: 记录错误日志
            e.Handled = true;
        }
    }
}