using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace CEEdit
{
    public partial class ConversionPage : Page, INotifyPropertyChanged
    {
        public ConversionPage()
        {
            InitializeComponent();
            DataContext = this;
            
            // 订阅语言变更事件
            LanguageManager.Instance.LanguageChanged += OnLanguageChanged;
            
            // 页面加载时触发一次属性更新
            Loaded += (s, e) => {
                // 确保语言管理器已经初始化
                if (LanguageManager.Instance != null)
                {
                    OnLanguageChanged();
                }
            };
            
            // 页面卸载时取消订阅事件，避免内存泄漏
            Unloaded += ConversionPage_Unloaded;
            
            // 立即触发一次属性更新，确保初始显示正确
            OnLanguageChanged();
        }

        private void ConversionPage_Unloaded(object sender, RoutedEventArgs e)
        {
            LanguageManager.Instance.LanguageChanged -= OnLanguageChanged;
        }

        public string LabelConfigFolder => LanguageManager.Instance?.GetString("ConversionPage.Label.ConfigFolder") ?? "要转换的配置文件夹：";
        public string LabelPluginConfig => LanguageManager.Instance?.GetString("ConversionPage.Label.PluginConfig") ?? "当前配置指定插件配置：";
        public string ButtonSelectPath => LanguageManager.Instance?.GetString("ConversionPage.Button.SelectPath") ?? "选择路径";
        public string ButtonStartConversion => LanguageManager.Instance?.GetString("ConversionPage.Button.StartConversion") ?? "开始转换";
        public string TextBoxPlaceholder => LanguageManager.Instance?.GetString("ConversionPage.TextBox.Placeholder") ?? "待选择";
        public string PluginItemsAdder => LanguageManager.Instance?.GetString("ConversionPage.Plugin.ItemsAdder") ?? "ItemsAdder";
        public string PluginNexo => LanguageManager.Instance?.GetString("ConversionPage.Plugin.Nexo") ?? "Nexo";

        private void OnLanguageChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelConfigFolder)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelPluginConfig)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonSelectPath)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonStartConversion)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextBoxPlaceholder)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PluginItemsAdder)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PluginNexo)));
        }

        private void SelectPathButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = LanguageManager.Instance.GetString("ConversionPage.Dialog.SelectConfigFolder")
            };

            if (dialog.ShowDialog() == true)
            {
                string selectedFolder = dialog.FolderName;
                PathTextBox.Text = selectedFolder;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}