using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32; // 用于 OpenFolderDialog

namespace CEEdit
{
    public partial class ConversionPage : Page
    {
        public ConversionPage()
        {
            InitializeComponent();
        }

        private void SelectPathButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "请选择配置文件夹"
            };

            if (dialog.ShowDialog() == true)
            {
                string selectedFolder = dialog.FolderName;
                PathTextBox.Text = selectedFolder;
                // 后续可将路径用于转换逻辑
            }
        }
    }
}