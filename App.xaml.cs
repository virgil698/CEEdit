using System.IO;
using System.Reflection;
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

        /// <summary>
        /// 静态构造函数，设置程序集解析器
        /// </summary>
        static App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        /// <summary>
        /// 程序集解析事件处理器，从libraries文件夹加载DLL
        /// </summary>
        private static Assembly? OnAssemblyResolve(object? sender, ResolveEventArgs args)
        {
            try
            {
                // 获取程序集名称
                var assemblyName = new AssemblyName(args.Name);
                var fileName = assemblyName.Name + ".dll";

                // 构造libraries文件夹路径
                var appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var librariesPath = Path.Combine(appPath ?? "", "libraries", fileName);

                // 如果在libraries文件夹中找到DLL，则加载它
                if (File.Exists(librariesPath))
                {
                    System.Diagnostics.Debug.WriteLine($"从libraries文件夹加载程序集: {fileName}");
                    return Assembly.LoadFrom(librariesPath);
                }

                // 也检查当前目录
                var currentPath = Path.Combine(appPath ?? "", fileName);
                if (File.Exists(currentPath))
                {
                    return Assembly.LoadFrom(currentPath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载程序集失败 {args.Name}: {ex.Message}");
            }

            return null;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // 全局异常处理
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            
            // 初始化依赖注入容器
            _host = CreateHostBuilder().Build();
            
            // 初始化项目历史服务
            await InitializeServicesAsync();
            
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
                    services.AddSingleton<IProjectService, CEEdit.Core.Services.Implementations.ProjectService>();
                    // TODO: 其他服务待实现
                    // services.AddTransient<IFileService, FileService>();
                    
                    // 注册窗口
                    services.AddTransient<MainWindow>();
                });
        }

        /// <summary>
        /// 初始化应用程序服务
        /// </summary>
        private async Task InitializeServicesAsync()
        {
            try
            {
                // 获取项目历史服务并初始化
                var projectHistoryService = _host?.Services.GetService<IProjectHistoryService>();
                if (projectHistoryService != null)
                {
                    await projectHistoryService.InitializeAsync();
                }
                
                // TODO: 初始化其他服务
                
                System.Diagnostics.Debug.WriteLine("应用程序服务初始化完成");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"服务初始化失败: {ex.Message}");
                MessageBox.Show($"应用程序初始化失败: {ex.Message}", 
                              "启动错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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