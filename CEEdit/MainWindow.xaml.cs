using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace CEEdit;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        
        // 订阅语言变更事件
        LanguageManager.Instance.LanguageChanged += OnLanguageChanged;
        
        // 默认显示项目页面
        MainFrame.Navigate(new ProjectPage());
    }

    public string WindowTitle => LanguageManager.Instance.GetString("MainWindow.Title");
    public string Version => LanguageManager.Instance.GetString("MainWindow.Version");
    public string MenuProject => LanguageManager.Instance.GetString("MainWindow.Menu.Project");
    public string MenuConversion => LanguageManager.Instance.GetString("MainWindow.Menu.Conversion");
    public string MenuSettings => LanguageManager.Instance.GetString("MainWindow.Menu.Settings");

    private void OnLanguageChanged()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowTitle)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Version)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuProject)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuConversion)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuSettings)));
    }

    private void ProjectButton_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new ProjectPage());
    }

    private void ConversionButton_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new ConversionPage());
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new SettingsPage());
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
