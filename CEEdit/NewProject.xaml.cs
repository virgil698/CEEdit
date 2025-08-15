using System.Windows;

namespace CEEdit
{
    public partial class NewProject : Window
    {
        public NewProject()
        {
            InitializeComponent();
        }

        // 点击“取消”按钮时关闭窗口
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}