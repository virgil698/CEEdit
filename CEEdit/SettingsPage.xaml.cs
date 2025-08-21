using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace CEEdit
{
    public partial class SettingsPage : Page, INotifyPropertyChanged
    {
        public SettingsPage()
        {
            InitializeComponent();
            DataContext = this;
            
            // 订阅语言变更事件
            LanguageManager.Instance.LanguageChanged += OnLanguageChanged;
            
            // 延迟设置当前选中的语言，等待控件加载完成
            Loaded += (s, e) =>
            {
                // 直接从INI配置文件读取语言设置，不受LanguageManager影响
                var currentLang = IniConfigManager.Instance.GetCurrentLanguage();
                string[] languageCodes = { "zh_cn", "zh_tw", "en_us" };
                var selectedIndex = Array.IndexOf(languageCodes, currentLang);
                if (selectedIndex >= 0 && LanguageComboBox != null)
                {
                    LanguageComboBox.SelectedIndex = selectedIndex;
                }
                
                // 加载路径设置
                LoadPathSettings();
            };
        }

        public string LabelLanguage => LanguageManager.Instance.GetString("SettingsPage.Label.Language");
        public string LabelProjectPath => LanguageManager.Instance.GetString("SettingsPage.Label.ProjectPath");
        public string LabelConversionPath => LanguageManager.Instance.GetString("SettingsPage.Label.ConversionPath");
        public string ButtonChangeLocation => LanguageManager.Instance.GetString("SettingsPage.Button.ChangeLocation");

        private void LoadPathSettings()
        {
            // 从INI配置加载路径设置
            ProjectPathTextBox.Text = IniConfigManager.Instance.GetValue("Paths", "ProjectPath", @"C:\CEEdit\project");
            ConversionPathTextBox.Text = IniConfigManager.Instance.GetValue("Paths", "ConversionPath", @"C:\CEEdit\conversion");
        }

        private void OnLanguageChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelLanguage)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelProjectPath)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelConversionPath)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonChangeLocation)));
            
            // 语言变更后更新ComboBox选中项
            UpdateLanguageSelection();
        }

        private void UpdateLanguageSelection()
        {
            if (LanguageComboBox != null)
            {
                // 直接从INI配置文件读取语言设置，确保显示正确的选中项
                var currentLang = IniConfigManager.Instance.GetCurrentLanguage();
                string[] languageCodes = { "zh_cn", "zh_tw", "en_us" };
                var selectedIndex = Array.IndexOf(languageCodes, currentLang);
                if (selectedIndex >= 0)
                {
                    LanguageComboBox.SelectedIndex = selectedIndex;
                }
            }
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedIndex >= 0)
            {
                string[] languageCodes = { "zh_cn", "zh_tw", "en_us" };
                if (comboBox.SelectedIndex < languageCodes.Length)
                {
                    string selectedLanguageCode = languageCodes[comboBox.SelectedIndex];
                    LanguageManager.Instance.LoadLanguage(selectedLanguageCode);
                }
            }
        }

        private void ChangeProjectPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog 
            { 
                Title = LanguageManager.Instance.GetString("SettingsPage.Dialog.SelectProjectFolder") 
            };
            if (dialog.ShowDialog() == true)
            {
                ProjectPathTextBox.Text = dialog.FolderName;
                // 保存到INI配置
                IniConfigManager.Instance.SetValue("Paths", "ProjectPath", dialog.FolderName);
            }
        }

        private void ChangeConversionPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog 
            { 
                Title = LanguageManager.Instance.GetString("SettingsPage.Dialog.SelectConversionFolder") 
            };
            if (dialog.ShowDialog() == true)
            {
                ConversionPathTextBox.Text = dialog.FolderName;
                // 保存到INI配置
                IniConfigManager.Instance.SetValue("Paths", "ConversionPath", dialog.FolderName);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
