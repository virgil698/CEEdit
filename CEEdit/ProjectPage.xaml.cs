using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32; // 添加命名空间

namespace CEEdit
{
    public partial class ProjectPage : Page
    {
        public ProjectPage()
        {
            InitializeComponent();
        }

        private void CreateProjectButton_Click(object sender, RoutedEventArgs e)
        {
            NewProject window = new NewProject();
            window.ShowDialog();
        }

        // 新增：打开项目按钮事件
        private void OpenProjectButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "选择项目文件夹"
            };

            if (dialog.ShowDialog() == true)
            {
                string selectedPath = dialog.FolderName;
                MessageBox.Show($"已选择项目路径：{selectedPath}", "项目路径");
                // 后续逻辑：加载项目内容
            }
        }
    }
}