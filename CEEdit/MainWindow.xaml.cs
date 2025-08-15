using System.Windows;
using System.Windows.Controls;

namespace CEEdit;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // 默认显示项目页面
        MainFrame.Navigate(new ProjectPage());
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
}