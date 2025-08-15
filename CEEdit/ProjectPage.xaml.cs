using System.Windows;
using System.Windows.Controls;

namespace CEEdit;

public partial class ProjectPage : Page
{
    public ProjectPage()
    {
        InitializeComponent();
    }

    // 添加按钮点击事件
    private void CreateProjectButton_Click(object sender, RoutedEventArgs e)
    {
        // 创建并显示 NewProject 窗口
        NewProject window = new NewProject();
        window.ShowDialog(); // 使用 ShowDialog 使新窗口模态显示
    }
}