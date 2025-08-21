using System;
using System.IO;
using System.Text.Json;

namespace CEEdit
{
    public class ConfigManager
    {
        private static ConfigManager? _instance;
        private AppConfig _config = new();
        private readonly string _configPath;

        public static ConfigManager Instance => _instance ??= new ConfigManager();

        private ConfigManager()
        {
            _configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CEEdit", "config.json");
            LoadConfig();
        }

        public AppConfig Config => _config;

        public void LoadConfig()
        {
            try
            {
                if (File.Exists(_configPath))
                {
                    string jsonContent = File.ReadAllText(_configPath);
                    _config = JsonSerializer.Deserialize<AppConfig>(jsonContent) ?? new AppConfig();
                }
                else
                {
                    // 创建默认配置
                    _config = new AppConfig();
                    SaveConfig();
                }
            }
            catch (Exception)
            {
                // 如果加载失败，使用默认配置
                _config = new AppConfig();
            }
        }

        public void SaveConfig()
        {
            try
            {
                // 确保目录存在
                string? directory = Path.GetDirectoryName(_configPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string jsonContent = JsonSerializer.Serialize(_config, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(_configPath, jsonContent);
            }
            catch (Exception)
            {
                // 保存失败时静默处理
            }
        }

        public void SetLanguage(string languageCode)
        {
            _config.Language = languageCode;
            SaveConfig();
        }

        public void SetProjectPath(string path)
        {
            _config.ProjectPath = path;
            SaveConfig();
        }

        public void SetConversionPath(string path)
        {
            _config.ConversionPath = path;
            SaveConfig();
        }
    }

    public class AppConfig
    {
        public string Language { get; set; } = "zh_cn";
        public string ProjectPath { get; set; } = @"C:\CEEdit\project";
        public string ConversionPath { get; set; } = @"C:\CEEdit\conversion";
    }
}