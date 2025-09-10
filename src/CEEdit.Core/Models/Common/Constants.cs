namespace CEEdit.Core.Models.Common
{
    /// <summary>
    /// 应用程序常量
    /// </summary>
    public static class Constants
    {
        #region 应用程序信息
        
        /// <summary>
        /// 应用程序名称
        /// </summary>
        public const string ApplicationName = "CEEdit";

        /// <summary>
        /// 应用程序版本
        /// </summary>
        public const string ApplicationVersion = "1.0.0";

        /// <summary>
        /// 应用程序作者
        /// </summary>
        public const string ApplicationAuthor = "CEEdit Team";

        /// <summary>
        /// 应用程序描述
        /// </summary>
        public const string ApplicationDescription = "CraftEngine插件可视化创建和编辑器";

        #endregion

        #region 文件扩展名

        /// <summary>
        /// CEEdit项目文件扩展名
        /// </summary>
        public const string ProjectFileExtension = ".ceproj";

        /// <summary>
        /// 方块定义文件扩展名
        /// </summary>
        public const string BlockFileExtension = ".block.yml";

        /// <summary>
        /// 物品定义文件扩展名
        /// </summary>
        public const string ItemFileExtension = ".item.yml";

        /// <summary>
        /// 配方定义文件扩展名
        /// </summary>
        public const string RecipeFileExtension = ".recipe.yml";

        /// <summary>
        /// Blockbench模型文件扩展名
        /// </summary>
        public const string BlockbenchModelExtension = ".bbmodel";

        /// <summary>
        /// Minecraft模型文件扩展名
        /// </summary>
        public const string MinecraftModelExtension = ".json";

        /// <summary>
        /// 纹理文件扩展名
        /// </summary>
        public const string TextureExtension = ".png";

        /// <summary>
        /// 音效文件扩展名
        /// </summary>
        public const string SoundExtension = ".ogg";

        /// <summary>
        /// 语言文件扩展名
        /// </summary>
        public const string LanguageFileExtension = ".json";

        #endregion

        #region 默认路径

        /// <summary>
        /// 默认项目根路径
        /// </summary>
        public const string DefaultProjectsPath = "Documents/CEEdit/Projects";

        /// <summary>
        /// 模板路径
        /// </summary>
        public const string TemplatesPath = "resources/templates";

        /// <summary>
        /// 缓存路径
        /// </summary>
        public const string CachePath = "data/cache";

        /// <summary>
        /// 日志路径
        /// </summary>
        public const string LogPath = "data/logs";

        /// <summary>
        /// 配置文件路径
        /// </summary>
        public const string ConfigPath = "config";

        #endregion

        #region CraftEngine配置

        /// <summary>
        /// 支持的最小CraftEngine版本
        /// </summary>
        public const string MinCraftEngineVersion = "1.20.0";

        /// <summary>
        /// 支持的最大CraftEngine版本
        /// </summary>
        public const string MaxCraftEngineVersion = "1.21.0";

        /// <summary>
        /// 默认CraftEngine版本
        /// </summary>
        public const string DefaultCraftEngineVersion = "1.20.0";

        /// <summary>
        /// CraftEngine命名空间
        /// </summary>
        public const string CraftEngineNamespace = "craftengine";

        #endregion

        #region 界面配置

        /// <summary>
        /// 默认窗口宽度
        /// </summary>
        public const int DefaultWindowWidth = 1200;

        /// <summary>
        /// 默认窗口高度
        /// </summary>
        public const int DefaultWindowHeight = 800;

        /// <summary>
        /// 最小窗口宽度
        /// </summary>
        public const int MinWindowWidth = 800;

        /// <summary>
        /// 最小窗口高度
        /// </summary>
        public const int MinWindowHeight = 600;

        /// <summary>
        /// 默认字体大小
        /// </summary>
        public const int DefaultFontSize = 12;

        /// <summary>
        /// 默认代码编辑器Tab大小
        /// </summary>
        public const int DefaultTabSize = 2;

        #endregion

        #region 性能配置

        /// <summary>
        /// 最大撤销级别
        /// </summary>
        public const int MaxUndoLevels = 100;

        /// <summary>
        /// 自动保存间隔（毫秒）
        /// </summary>
        public const int AutoSaveInterval = 300000; // 5分钟

        /// <summary>
        /// 预览更新延迟（毫秒）
        /// </summary>
        public const int PreviewUpdateDelay = 500;

        /// <summary>
        /// 最大缓存文件大小（MB）
        /// </summary>
        public const int MaxCacheSize = 1024; // 1GB

        /// <summary>
        /// 最大日志文件大小（MB）
        /// </summary>
        public const int MaxLogFileSize = 10;

        /// <summary>
        /// 日志文件保留天数
        /// </summary>
        public const int LogRetentionDays = 30;

        #endregion

        #region 验证规则

        /// <summary>
        /// 项目名称最大长度
        /// </summary>
        public const int MaxProjectNameLength = 50;

        /// <summary>
        /// 方块ID最大长度
        /// </summary>
        public const int MaxBlockIdLength = 64;

        /// <summary>
        /// 物品ID最大长度
        /// </summary>
        public const int MaxItemIdLength = 64;

        /// <summary>
        /// 配方ID最大长度
        /// </summary>
        public const int MaxRecipeIdLength = 64;

        /// <summary>
        /// 描述最大长度
        /// </summary>
        public const int MaxDescriptionLength = 500;

        /// <summary>
        /// ID有效字符正则表达式
        /// </summary>
        public const string IdValidationPattern = @"^[a-z_][a-z0-9_]*$";

        /// <summary>
        /// 项目名称有效字符正则表达式
        /// </summary>
        public const string ProjectNameValidationPattern = @"^[a-zA-Z0-9_\-\s]+$";

        #endregion

        #region 网络配置

        /// <summary>
        /// 默认HTTP请求超时（毫秒）
        /// </summary>
        public const int HttpRequestTimeout = 30000;

        /// <summary>
        /// 最大并发下载数
        /// </summary>
        public const int MaxConcurrentDownloads = 3;

        /// <summary>
        /// 下载重试次数
        /// </summary>
        public const int DownloadRetryCount = 3;

        #endregion

        #region Blockbench集成

        /// <summary>
        /// Blockbench下载URL模板
        /// </summary>
        public const string BlockbenchDownloadUrl = "https://github.com/JannisX11/blockbench/releases/latest";

        /// <summary>
        /// Blockbench可执行文件名
        /// </summary>
        public const string BlockbenchExecutableName = "Blockbench.exe";

        /// <summary>
        /// Blockbench启动参数模板
        /// </summary>
        public const string BlockbenchLaunchArgs = "--open \"{0}\"";

        /// <summary>
        /// 支持的Blockbench最小版本
        /// </summary>
        public const string MinBlockbenchVersion = "4.0.0";

        #endregion

        #region 文件筛选器

        /// <summary>
        /// 项目文件筛选器
        /// </summary>
        public const string ProjectFileFilter = "CEEdit项目文件 (*.ceproj)|*.ceproj|所有文件 (*.*)|*.*";

        /// <summary>
        /// 图片文件筛选器
        /// </summary>
        public const string ImageFileFilter = "图片文件 (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|所有文件 (*.*)|*.*";

        /// <summary>
        /// 音效文件筛选器
        /// </summary>
        public const string AudioFileFilter = "音效文件 (*.ogg;*.wav;*.mp3)|*.ogg;*.wav;*.mp3|所有文件 (*.*)|*.*";

        /// <summary>
        /// 模型文件筛选器
        /// </summary>
        public const string ModelFileFilter = "模型文件 (*.bbmodel;*.json;*.obj)|*.bbmodel;*.json;*.obj|所有文件 (*.*)|*.*";

        /// <summary>
        /// YAML文件筛选器
        /// </summary>
        public const string YamlFileFilter = "YAML文件 (*.yml;*.yaml)|*.yml;*.yaml|所有文件 (*.*)|*.*";

        /// <summary>
        /// JSON文件筛选器
        /// </summary>
        public const string JsonFileFilter = "JSON文件 (*.json)|*.json|所有文件 (*.*)|*.*";

        #endregion

        #region 快捷键

        /// <summary>
        /// 新建项目快捷键
        /// </summary>
        public const string NewProjectShortcut = "Ctrl+N";

        /// <summary>
        /// 打开项目快捷键
        /// </summary>
        public const string OpenProjectShortcut = "Ctrl+O";

        /// <summary>
        /// 保存项目快捷键
        /// </summary>
        public const string SaveProjectShortcut = "Ctrl+S";

        /// <summary>
        /// 另存为项目快捷键
        /// </summary>
        public const string SaveAsProjectShortcut = "Ctrl+Shift+S";

        /// <summary>
        /// 构建项目快捷键
        /// </summary>
        public const string BuildProjectShortcut = "F7";

        /// <summary>
        /// 预览快捷键
        /// </summary>
        public const string PreviewShortcut = "F5";

        /// <summary>
        /// 撤销快捷键
        /// </summary>
        public const string UndoShortcut = "Ctrl+Z";

        /// <summary>
        /// 重做快捷键
        /// </summary>
        public const string RedoShortcut = "Ctrl+Y";

        #endregion

        #region 默认值

        /// <summary>
        /// 默认方块硬度
        /// </summary>
        public const float DefaultBlockHardness = 1.0f;

        /// <summary>
        /// 默认方块爆炸抗性
        /// </summary>
        public const float DefaultBlockBlastResistance = 1.0f;

        /// <summary>
        /// 默认物品最大堆叠数量
        /// </summary>
        public const int DefaultItemMaxStackSize = 64;

        /// <summary>
        /// 默认合成时间（tick）
        /// </summary>
        public const int DefaultCookingTime = 200;

        /// <summary>
        /// 默认纹理尺寸
        /// </summary>
        public const int DefaultTextureSize = 16;

        #endregion
    }
}

