using System.Configuration;
using System.Data;
using System.Windows;

namespace CEEdit;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // 程序启动时先初始化配置管理器
        InitializeConfiguration();
        
        // 调用基类的OnStartup
        base.OnStartup(e);
    }

    private void InitializeConfiguration()
    {
        try
        {
            // 初始化INI配置管理器（这会创建默认配置文件如果不存在）
            var iniManager = IniConfigManager.Instance;
            
            // 获取配置文件中的语言设置
            string configuredLanguage = iniManager.GetCurrentLanguage();
            
            // 初始化语言管理器并加载对应的语言文件
            var languageManager = LanguageManager.Instance;
            languageManager.LoadLanguage(configuredLanguage);
            
            // 可以在这里添加其他配置项的初始化
            // 例如：主题设置、窗口大小等
        }
        catch (System.Exception ex)
        {
            // 如果配置初始化失败，使用默认设置
            MessageBox.Show($"配置初始化失败，将使用默认设置: {ex.Message}", "警告", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            
            // 确保至少有基本的语言设置
            LanguageManager.Instance.LoadLanguage("zh_cn");
        }
    }
}
