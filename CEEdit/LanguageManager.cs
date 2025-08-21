using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Linq;

namespace CEEdit
{
    public class LanguageManager
    {
        private static LanguageManager? _instance;
        private Dictionary<string, string> _currentLanguage = new();
        private string _currentLanguageCode = "zh_cn";

        public static LanguageManager Instance => _instance ??= new LanguageManager();

        public event Action? LanguageChanged;

        private LanguageManager()
        {
            // 构造函数中不自动加载语言，由App.xaml.cs在启动时统一初始化
            // 这样可以确保配置文件先被读取，然后再加载对应的语言
            _currentLanguageCode = "zh_cn"; // 设置默认值，但不加载语言文件
        }

        public void LoadLanguage(string languageCode)
        {
            try
            {
                string languageFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "language", $"{languageCode}.json");
                
                if (!File.Exists(languageFile))
                {
                    // 如果文件不存在，使用默认中文
                    languageCode = "zh_cn";
                    languageFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "language", $"{languageCode}.json");
                }

                if (File.Exists(languageFile))
                {
                    string jsonContent = File.ReadAllText(languageFile, System.Text.Encoding.UTF8);
                    _currentLanguage = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent) ?? new Dictionary<string, string>();
                    _currentLanguageCode = languageCode;
                    
                    // 保存语言设置到INI配置
                    IniConfigManager.Instance.SetCurrentLanguage(languageCode);
                    
                    LanguageChanged?.Invoke();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载语言文件失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string GetString(string key, string defaultValue = "")
        {
            // 如果语言字典为空，尝试重新加载默认语言
            if (_currentLanguage.Count == 0)
            {
                LoadLanguage(_currentLanguageCode);
            }
            
            if (_currentLanguage.TryGetValue(key, out string? value) && !string.IsNullOrEmpty(value))
            {
                return value;
            }
            
            // 如果没有找到翻译或翻译为空，返回默认值或键名
            return string.IsNullOrEmpty(defaultValue) ? key : defaultValue;
        }

        public string CurrentLanguageCode => _currentLanguageCode;

        public List<LanguageInfo> GetAvailableLanguages()
        {
            var availableLanguages = IniConfigManager.Instance.GetAvailableLanguages();
            return availableLanguages.Select(lang => new LanguageInfo(lang.Code, lang.DisplayName)).ToList();
        }
    }

    public record LanguageInfo(string Code, string DisplayName);
}