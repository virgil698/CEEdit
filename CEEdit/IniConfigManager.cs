using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CEEdit
{
    public class IniConfigManager
    {
        private static IniConfigManager? _instance;
        private readonly string _configPath;
        private readonly string _languageFolderPath;
        private Dictionary<string, Dictionary<string, string>> _iniData = new();

        public static IniConfigManager Instance => _instance ??= new IniConfigManager();

        private IniConfigManager()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _configPath = Path.Combine(appDirectory, "CEEdit.ini");
            _languageFolderPath = Path.Combine(appDirectory, "language");
            
            InitializeLanguageFolder();
            LoadConfig();
        }

        /// <summary>
        /// 初始化语言文件夹，检查是否存在，不存在则复制项目内的language文件夹
        /// </summary>
        private void InitializeLanguageFolder()
        {
            if (!Directory.Exists(_languageFolderPath))
            {
                CopyLanguageFolderFromProject();
            }
            else
            {
                // 检查语言文件是否完整，缺失则重新复制
                EnsureLanguageFilesExist();
            }
        }

        /// <summary>
        /// 从项目内复制language文件夹
        /// </summary>
        private void CopyLanguageFolderFromProject()
        {
            try
            {
                // 获取项目根目录的language文件夹路径
                string projectRoot = GetProjectRootDirectory();
                string sourceLanguageFolder = Path.Combine(projectRoot, "language");
                
                if (Directory.Exists(sourceLanguageFolder))
                {
                    // 创建目标文件夹
                    Directory.CreateDirectory(_languageFolderPath);
                    
                    // 复制所有语言文件
                    string[] languageFiles = Directory.GetFiles(sourceLanguageFolder, "*.json");
                    foreach (string sourceFile in languageFiles)
                    {
                        string fileName = Path.GetFileName(sourceFile);
                        string destFile = Path.Combine(_languageFolderPath, fileName);
                        File.Copy(sourceFile, destFile, true);
                    }
                }
                else
                {
                    // 如果项目内没有language文件夹，则生成默认文件
                    Directory.CreateDirectory(_languageFolderPath);
                    GenerateDefaultLanguageFiles();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"复制语言文件夹失败: {ex.Message}");
                // 失败时生成默认语言文件
                Directory.CreateDirectory(_languageFolderPath);
                GenerateDefaultLanguageFiles();
            }
        }

        /// <summary>
        /// 获取项目根目录
        /// </summary>
        private string GetProjectRootDirectory()
        {
            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
            
            // 从当前目录向上查找，直到找到包含CEEdit.csproj的目录
            DirectoryInfo? dir = new DirectoryInfo(currentDir);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "CEEdit.csproj")) ||
                    Directory.Exists(Path.Combine(dir.FullName, "CEEdit")) ||
                    Directory.Exists(Path.Combine(dir.FullName, "language")))
                {
                    return dir.FullName;
                }
                dir = dir.Parent;
            }
            
            // 如果找不到，返回当前目录的上级目录
            return Path.GetDirectoryName(currentDir) ?? currentDir;
        }

        /// <summary>
        /// 生成默认语言文件（备用方案）
        /// </summary>
        private void GenerateDefaultLanguageFiles()
        {
            // 简体中文
            var zhCnContent = new Dictionary<string, string>
            {
                ["MainWindow.Title"] = "欢迎访问 CEEditor",
                ["MainWindow.Version"] = "v0.0.1",
                ["MainWindow.Menu.Project"] = "项目",
                ["MainWindow.Menu.Conversion"] = "转换",
                ["MainWindow.Menu.Settings"] = "设置",
                ["ProjectPage.Search.Placeholder"] = "搜索项目...",
                ["ProjectPage.Button.CreateProject"] = "创建项目",
                ["ProjectPage.Button.Open"] = "打开",
                ["SettingsPage.Label.Language"] = "选择语言：",
                ["SettingsPage.Label.ProjectPath"] = "项目默认文件夹：",
                ["SettingsPage.Label.ConversionPath"] = "转换默认文件夹：",
                ["SettingsPage.Button.ChangeLocation"] = "更改位置",
                ["ConversionPage.Label.ConfigFolder"] = "要转换的配置文件夹：",
                ["ConversionPage.Button.SelectPath"] = "选择路径",
                ["ConversionPage.Button.StartConversion"] = "开始转换",
                ["NewProject.Title"] = "创建新项目",
                ["NewProject.Label.ProjectName"] = "项目名：",
                ["NewProject.Button.Cancel"] = "取消",
                ["NewProject.Button.Create"] = "创建项目"
            };
            WriteLanguageFile("zh_cn.json", zhCnContent);

            // 繁体中文
            var zhTwContent = new Dictionary<string, string>
            {
                ["MainWindow.Title"] = "歡迎使用 CEEditor",
                ["MainWindow.Version"] = "v0.0.1",
                ["MainWindow.Menu.Project"] = "專案",
                ["MainWindow.Menu.Conversion"] = "轉換",
                ["MainWindow.Menu.Settings"] = "設定",
                ["ProjectPage.Search.Placeholder"] = "搜尋專案...",
                ["ProjectPage.Button.CreateProject"] = "建立專案",
                ["ProjectPage.Button.Open"] = "開啟",
                ["SettingsPage.Label.Language"] = "選擇語言：",
                ["SettingsPage.Label.ProjectPath"] = "專案預設資料夾：",
                ["SettingsPage.Label.ConversionPath"] = "轉換預設資料夾：",
                ["SettingsPage.Button.ChangeLocation"] = "更改位置",
                ["ConversionPage.Label.ConfigFolder"] = "要轉換的設定資料夾：",
                ["ConversionPage.Button.SelectPath"] = "選擇路徑",
                ["ConversionPage.Button.StartConversion"] = "開始轉換",
                ["NewProject.Title"] = "建立新專案",
                ["NewProject.Label.ProjectName"] = "專案名稱：",
                ["NewProject.Button.Cancel"] = "取消",
                ["NewProject.Button.Create"] = "建立專案"
            };
            WriteLanguageFile("zh_tw.json", zhTwContent);

            // 英文
            var enUsContent = new Dictionary<string, string>
            {
                ["MainWindow.Title"] = "Welcome to CEEditor",
                ["MainWindow.Version"] = "v0.0.1",
                ["MainWindow.Menu.Project"] = "Project",
                ["MainWindow.Menu.Conversion"] = "Conversion",
                ["MainWindow.Menu.Settings"] = "Settings",
                ["ProjectPage.Search.Placeholder"] = "Search projects...",
                ["ProjectPage.Button.CreateProject"] = "Create Project",
                ["ProjectPage.Button.Open"] = "Open",
                ["SettingsPage.Label.Language"] = "Select Language:",
                ["SettingsPage.Label.ProjectPath"] = "Default Project Folder:",
                ["SettingsPage.Label.ConversionPath"] = "Default Conversion Folder:",
                ["SettingsPage.Button.ChangeLocation"] = "Change Location",
                ["ConversionPage.Label.ConfigFolder"] = "Configuration folder to convert:",
                ["ConversionPage.Button.SelectPath"] = "Select Path",
                ["ConversionPage.Button.StartConversion"] = "Start Conversion",
                ["NewProject.Title"] = "Create New Project",
                ["NewProject.Label.ProjectName"] = "Project Name:",
                ["NewProject.Button.Cancel"] = "Cancel",
                ["NewProject.Button.Create"] = "Create Project"
            };
            WriteLanguageFile("en_us.json", enUsContent);
        }

        /// <summary>
        /// 确保语言文件存在
        /// </summary>
        private void EnsureLanguageFilesExist()
        {
            string[] requiredFiles = { "zh_cn.json", "zh_tw.json", "en_us.json" };
            
            foreach (string file in requiredFiles)
            {
                string filePath = Path.Combine(_languageFolderPath, file);
                if (!File.Exists(filePath))
                {
                    CopyLanguageFolderFromProject();
                    break;
                }
            }
        }

        /// <summary>
        /// 写入语言文件
        /// </summary>
        private void WriteLanguageFile(string fileName, Dictionary<string, string> content)
        {
            try
            {
                string filePath = Path.Combine(_languageFolderPath, fileName);
                string jsonContent = System.Text.Json.JsonSerializer.Serialize(content, new System.Text.Json.JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
                File.WriteAllText(filePath, jsonContent, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"写入语言文件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取可用的语言列表
        /// </summary>
        public List<(string Code, string DisplayName)> GetAvailableLanguages()
        {
            var languages = new List<(string Code, string DisplayName)>();
            
            if (Directory.Exists(_languageFolderPath))
            {
                var jsonFiles = Directory.GetFiles(_languageFolderPath, "*.json");
                
                foreach (string file in jsonFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    string displayName = GetLanguageDisplayName(fileName);
                    languages.Add((fileName, displayName));
                }
            }
            
            // 如果没有找到语言文件，返回默认语言
            if (languages.Count == 0)
            {
                languages.Add(("zh_cn", "简体中文"));
            }
            
            return languages;
        }

        /// <summary>
        /// 获取语言显示名称
        /// </summary>
        private string GetLanguageDisplayName(string languageCode)
        {
            return languageCode switch
            {
                "zh_cn" => "简体中文",
                "zh_tw" => "繁體中文",
                "en_us" => "English",
                _ => languageCode
            };
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        private void LoadConfig()
        {
            if (File.Exists(_configPath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(_configPath);
                    string currentSection = "";
                    
                    foreach (string line in lines)
                    {
                        string trimmedLine = line.Trim();
                        
                        if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                            continue;
                            
                        if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                        {
                            currentSection = trimmedLine[1..^1];
                            if (!_iniData.ContainsKey(currentSection))
                                _iniData[currentSection] = new Dictionary<string, string>();
                        }
                        else if (trimmedLine.Contains("=") && !string.IsNullOrEmpty(currentSection))
                        {
                            int equalIndex = trimmedLine.IndexOf('=');
                            string key = trimmedLine[..equalIndex].Trim();
                            string value = trimmedLine[(equalIndex + 1)..].Trim();
                            _iniData[currentSection][key] = value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"加载配置文件失败: {ex.Message}");
                }
            }
            else
            {
                // 创建默认配置文件
                CreateDefaultConfig();
            }
        }

        /// <summary>
        /// 创建默认配置文件
        /// </summary>
        private void CreateDefaultConfig()
        {
            _iniData["General"] = new Dictionary<string, string>
            {
                ["Language"] = "zh_cn"
            };
            
            _iniData["Paths"] = new Dictionary<string, string>
            {
                ["ProjectPath"] = @"C:\CEEdit\project",
                ["ConversionPath"] = @"C:\CEEdit\conversion"
            };
            
            SaveConfig();
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public void SaveConfig()
        {
            try
            {
                var lines = new List<string>();
                
                foreach (var section in _iniData)
                {
                    lines.Add($"[{section.Key}]");
                    
                    foreach (var kvp in section.Value)
                    {
                        lines.Add($"{kvp.Key}={kvp.Value}");
                    }
                    
                    lines.Add(""); // 空行分隔
                }
                
                File.WriteAllLines(_configPath, lines);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"保存配置文件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        public string GetValue(string section, string key, string defaultValue = "")
        {
            if (_iniData.TryGetValue(section, out var sectionData) && 
                sectionData.TryGetValue(key, out var value))
            {
                return value;
            }
            return defaultValue;
        }

        /// <summary>
        /// 设置配置值
        /// </summary>
        public void SetValue(string section, string key, string value)
        {
            if (!_iniData.ContainsKey(section))
                _iniData[section] = new Dictionary<string, string>();
                
            _iniData[section][key] = value;
            SaveConfig();
        }

        /// <summary>
        /// 获取当前语言设置
        /// </summary>
        public string GetCurrentLanguage()
        {
            string language = GetValue("General", "Language", "zh_cn");
            // 确保返回的语言代码是有效的
            if (string.IsNullOrEmpty(language) || !IsValidLanguageCode(language))
            {
                language = "zh_cn";
                SetCurrentLanguage(language); // 保存默认语言到配置文件
            }
            return language;
        }

        /// <summary>
        /// 验证语言代码是否有效
        /// </summary>
        private bool IsValidLanguageCode(string languageCode)
        {
            string[] validCodes = { "zh_cn", "zh_tw", "en_us" };
            return Array.Exists(validCodes, code => code == languageCode);
        }

        /// <summary>
        /// 设置当前语言
        /// </summary>
        public void SetCurrentLanguage(string languageCode)
        {
            SetValue("General", "Language", languageCode);
        }
    }
}
