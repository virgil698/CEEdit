using System.ComponentModel;
using System.Windows;

namespace CEEdit
{
    public partial class NewProject : Window, INotifyPropertyChanged
    {
        public NewProject()
        {
            InitializeComponent();
            DataContext = this;
            
            // 订阅语言变更事件
            LanguageManager.Instance.LanguageChanged += OnLanguageChanged;
        }

        public string WindowTitle => LanguageManager.Instance.GetString("NewProject.Title");
        public string LabelProjectName => LanguageManager.Instance.GetString("NewProject.Label.ProjectName");
        public string LabelProjectDescription => LanguageManager.Instance.GetString("NewProject.Label.ProjectDescription");
        public string LabelProjectAuthor => LanguageManager.Instance.GetString("NewProject.Label.ProjectAuthor");
        public string LabelIsEnabled => LanguageManager.Instance.GetString("NewProject.Label.IsEnabled");
        public string ButtonCancel => LanguageManager.Instance.GetString("NewProject.Button.Cancel");
        public string ButtonCreate => LanguageManager.Instance.GetString("NewProject.Button.Create");
        public string ValueTrue => LanguageManager.Instance.GetString("NewProject.Value.True");
        public string ValueFalse => LanguageManager.Instance.GetString("NewProject.Value.False");

        private void OnLanguageChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowTitle)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelProjectName)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelProjectDescription)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelProjectAuthor)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelIsEnabled)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonCancel)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonCreate)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValueTrue)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValueFalse)));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}