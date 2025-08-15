using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace CEEdit
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void ChangeProjectPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog { Title = "选择项目文件夹" };
            if (dialog.ShowDialog() == true)
            {
                ProjectPathTextBox.Text = dialog.FolderName;
            }
        }

        private void ChangeConversionPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog { Title = "选择转换文件夹" };
            if (dialog.ShowDialog() == true)
            {
                ConversionPathTextBox.Text = dialog.FolderName;
            }
        }
    }
}