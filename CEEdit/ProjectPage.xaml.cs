using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace CEEdit
{
    public partial class ProjectPage : Page, INotifyPropertyChanged
    {
        public ProjectPage()
        {
            InitializeComponent();
            DataContext = this;
            
            // 订阅语言变更事件
            LanguageManager.Instance.LanguageChanged += OnLanguageChanged;
        }

        public string SearchPlaceholder => LanguageManager.Instance.GetString("ProjectPage.Search.Placeholder");
        public string ButtonCreateProject => LanguageManager.Instance.GetString("ProjectPage.Button.CreateProject");
        public string ButtonOpen => LanguageManager.Instance.GetString("ProjectPage.Button.Open");

        private void OnLanguageChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchPlaceholder)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonCreateProject)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonOpen)));
        }

        private void CreateProjectButton_Click(object sender, RoutedEventArgs e)
        {
            NewProject window = new NewProject();
            window.ShowDialog();
        }

        private void OpenProjectButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = LanguageManager.Instance.GetString("ProjectPage.Dialog.SelectProject")
            };

            if (dialog.ShowDialog() == true)
            {
                string selectedPath = dialog.FolderName;
                string message = string.Format(LanguageManager.Instance.GetString("ProjectPage.Message.ProjectSelected"), selectedPath);
                string title = LanguageManager.Instance.GetString("ProjectPage.Message.ProjectPath");
                MessageBox.Show(message, title);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}