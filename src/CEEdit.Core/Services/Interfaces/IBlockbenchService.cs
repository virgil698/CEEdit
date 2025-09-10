using CEEdit.Core.Models.Resources;

namespace CEEdit.Core.Services.Interfaces
{
    /// <summary>
    /// Blockbench集成服务接口
    /// </summary>
    public interface IBlockbenchService
    {
        /// <summary>
        /// Blockbench启动事件
        /// </summary>
        event EventHandler<BlockbenchLaunchedEventArgs>? BlockbenchLaunched;

        /// <summary>
        /// 模型文件改变事件
        /// </summary>
        event EventHandler<ModelFileChangedEventArgs>? ModelFileChanged;

        /// <summary>
        /// Blockbench关闭事件
        /// </summary>
        event EventHandler<BlockbenchClosedEventArgs>? BlockbenchClosed;

        /// <summary>
        /// 启动Blockbench
        /// </summary>
        /// <param name="modelPath">模型文件路径（可选）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否成功启动</returns>
        Task<bool> LaunchBlockbenchAsync(string? modelPath = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 检查Blockbench是否可用
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否可用</returns>
        Task<bool> IsBlockbenchAvailableAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取Blockbench安装路径
        /// </summary>
        /// <returns>安装路径</returns>
        Task<string?> GetBlockbenchPathAsync();

        /// <summary>
        /// 安装或更新Blockbench
        /// </summary>
        /// <param name="installPath">安装路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>安装结果</returns>
        Task<InstallationResult> InstallBlockbenchAsync(string? installPath = null, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 从Blockbench导入模型
        /// </summary>
        /// <param name="bbmodelPath">bbmodel文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>导入的3D模型</returns>
        Task<Model3D?> ImportModelFromBlockbenchAsync(string bbmodelPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 导出模型到Blockbench格式
        /// </summary>
        /// <param name="model">3D模型</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>导出的文件路径</returns>
        Task<string?> ExportModelToBlockbenchAsync(Model3D model, string outputPath, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 监视模型文件变化
        /// </summary>
        /// <param name="modelPath">模型文件路径</param>
        /// <param name="onChanged">变化回调</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>监视任务</returns>
        Task<bool> WatchModelChangesAsync(string modelPath, Action<string> onChanged, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 停止监视模型文件
        /// </summary>
        /// <param name="modelPath">模型文件路径</param>
        /// <returns>停止任务</returns>
        Task StopWatchingModelAsync(string modelPath);

        /// <summary>
        /// 获取模型动画列表
        /// </summary>
        /// <param name="modelPath">模型文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>动画列表</returns>
        Task<List<ModelAnimation>> GetModelAnimationsAsync(string modelPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 创建新模型
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <param name="templatePath">模板路径（可选）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>新模型路径</returns>
        Task<string?> CreateNewModelAsync(BlockbenchModelType modelType, string? templatePath = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 验证Blockbench模型
        /// </summary>
        /// <param name="modelPath">模型文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        Task<ModelValidationResult> ValidateBlockbenchModelAsync(string modelPath, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 转换模型格式
        /// </summary>
        /// <param name="inputPath">输入文件路径</param>
        /// <param name="outputPath">输出文件路径</param>
        /// <param name="targetFormat">目标格式</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> ConvertModelFormatAsync(string inputPath, string outputPath, 
            ModelFormat targetFormat, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取Blockbench版本信息
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>版本信息</returns>
        Task<BlockbenchVersionInfo?> GetBlockbenchVersionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 安装Blockbench插件
        /// </summary>
        /// <param name="pluginPath">插件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>安装结果</returns>
        Task<bool> InstallBlockbenchPluginAsync(string pluginPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取已安装的Blockbench插件列表
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>插件列表</returns>
        Task<List<BlockbenchPlugin>> GetInstalledPluginsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 与Blockbench通信
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="parameters">参数</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>通信结果</returns>
        Task<BlockbenchResponse?> CommunicateWithBlockbenchAsync(string command, 
            Dictionary<string, object>? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 关闭Blockbench
        /// </summary>
        /// <param name="forceful">是否强制关闭</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>关闭任务</returns>
        Task<bool> CloseBlockbenchAsync(bool forceful = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 检查是否有运行中的Blockbench实例
        /// </summary>
        /// <returns>是否在运行</returns>
        bool IsBlockbenchRunning();

        /// <summary>
        /// 获取Blockbench进程信息
        /// </summary>
        /// <returns>进程信息列表</returns>
        List<BlockbenchProcessInfo> GetBlockbenchProcesses();

        /// <summary>
        /// 设置Blockbench配置
        /// </summary>
        /// <param name="configuration">配置</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>设置任务</returns>
        Task SetBlockbenchConfigurationAsync(BlockbenchConfiguration configuration, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取Blockbench配置
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>配置信息</returns>
        Task<BlockbenchConfiguration?> GetBlockbenchConfigurationAsync(CancellationToken cancellationToken = default);
    }

    #region 事件参数

    /// <summary>
    /// Blockbench启动事件参数
    /// </summary>
    public class BlockbenchLaunchedEventArgs : EventArgs
    {
        /// <summary>
        /// 进程ID
        /// </summary>
        public int ProcessId { get; }

        /// <summary>
        /// 启动的模型路径
        /// </summary>
        public string? ModelPath { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processId">进程ID</param>
        /// <param name="modelPath">模型路径</param>
        public BlockbenchLaunchedEventArgs(int processId, string? modelPath)
        {
            ProcessId = processId;
            ModelPath = modelPath;
        }
    }

    /// <summary>
    /// 模型文件改变事件参数
    /// </summary>
    public class ModelFileChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 模型文件路径
        /// </summary>
        public string ModelPath { get; }

        /// <summary>
        /// 改变类型
        /// </summary>
        public ModelChangeType ChangeType { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="modelPath">模型路径</param>
        /// <param name="changeType">改变类型</param>
        public ModelFileChangedEventArgs(string modelPath, ModelChangeType changeType)
        {
            ModelPath = modelPath;
            ChangeType = changeType;
        }
    }

    /// <summary>
    /// Blockbench关闭事件参数
    /// </summary>
    public class BlockbenchClosedEventArgs : EventArgs
    {
        /// <summary>
        /// 进程ID
        /// </summary>
        public int ProcessId { get; }

        /// <summary>
        /// 退出代码
        /// </summary>
        public int ExitCode { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processId">进程ID</param>
        /// <param name="exitCode">退出代码</param>
        public BlockbenchClosedEventArgs(int processId, int exitCode)
        {
            ProcessId = processId;
            ExitCode = exitCode;
        }
    }

    #endregion

    #region 枚举

    /// <summary>
    /// Blockbench模型类型
    /// </summary>
    public enum BlockbenchModelType
    {
        BlockItem,
        Entity,
        Skin,
        OptiFineEntity,
        OptiFineJpm,
        BedrockBlock,
        BedrockItem,
        BedrockEntity,
        ModdedEntity,
        Generic
    }

    /// <summary>
    /// 模型改变类型
    /// </summary>
    public enum ModelChangeType
    {
        Modified,
        Saved,
        Exported,
        Imported,
        Animated,
        TextureChanged
    }


    #endregion

    #region 辅助类

    /// <summary>
    /// 安装结果
    /// </summary>
    public class InstallationResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 安装路径
        /// </summary>
        public string? InstallationPath { get; set; }

        /// <summary>
        /// 安装的版本
        /// </summary>
        public string? InstalledVersion { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 安装时间
        /// </summary>
        public TimeSpan InstallationTime { get; set; }
    }

    /// <summary>
    /// 模型验证结果
    /// </summary>
    public class ModelValidationResult
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 错误列表
        /// </summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>
        /// 警告列表
        /// </summary>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// 模型统计信息
        /// </summary>
        public ModelStatistics? Statistics { get; set; }
    }

    /// <summary>
    /// 转换结果
    /// </summary>
    public class ConversionResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 输出文件路径
        /// </summary>
        public string? OutputPath { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 转换时间
        /// </summary>
        public TimeSpan ConversionTime { get; set; }

        /// <summary>
        /// 转换日志
        /// </summary>
        public string? ConversionLog { get; set; }
    }

    /// <summary>
    /// Blockbench版本信息
    /// </summary>
    public class BlockbenchVersionInfo
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 构建号
        /// </summary>
        public string Build { get; set; } = string.Empty;

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// 是否为预发布版本
        /// </summary>
        public bool IsPreRelease { get; set; }

        /// <summary>
        /// 更新日志
        /// </summary>
        public string? Changelog { get; set; }
    }

    /// <summary>
    /// Blockbench插件
    /// </summary>
    public class BlockbenchPlugin
    {
        /// <summary>
        /// 插件ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 插件版本
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 插件描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 插件作者
        /// </summary>
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 插件文件路径
        /// </summary>
        public string FilePath { get; set; } = string.Empty;
    }

    /// <summary>
    /// Blockbench响应
    /// </summary>
    public class BlockbenchResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        public Dictionary<string, object> Data { get; set; } = new();

        /// <summary>
        /// 错误消息
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 响应时间
        /// </summary>
        public TimeSpan ResponseTime { get; set; }
    }

    /// <summary>
    /// Blockbench进程信息
    /// </summary>
    public class BlockbenchProcessInfo
    {
        /// <summary>
        /// 进程ID
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// 进程名称
        /// </summary>
        public string ProcessName { get; set; } = string.Empty;

        /// <summary>
        /// 启动时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 内存使用量（字节）
        /// </summary>
        public long MemoryUsage { get; set; }

        /// <summary>
        /// CPU使用率
        /// </summary>
        public double CpuUsage { get; set; }

        /// <summary>
        /// 打开的文件路径
        /// </summary>
        public List<string> OpenFiles { get; set; } = new();
    }

    /// <summary>
    /// Blockbench配置
    /// </summary>
    public class BlockbenchConfiguration
    {
        /// <summary>
        /// 自动保存间隔（秒）
        /// </summary>
        public int AutoSaveInterval { get; set; } = 300;

        /// <summary>
        /// 默认导出格式
        /// </summary>
        public string DefaultExportFormat { get; set; } = "json";

        /// <summary>
        /// 启用备份
        /// </summary>
        public bool EnableBackup { get; set; } = true;

        /// <summary>
        /// 备份数量限制
        /// </summary>
        public int BackupLimit { get; set; } = 10;

        /// <summary>
        /// 界面主题
        /// </summary>
        public string Theme { get; set; } = "dark";

        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; } = "en";

        /// <summary>
        /// 自定义设置
        /// </summary>
        public Dictionary<string, object> CustomSettings { get; set; } = new();
    }

    /// <summary>
    /// 模型统计信息
    /// </summary>
    public class ModelStatistics
    {
        /// <summary>
        /// 顶点数量
        /// </summary>
        public int VertexCount { get; set; }

        /// <summary>
        /// 面数量
        /// </summary>
        public int FaceCount { get; set; }

        /// <summary>
        /// 纹理数量
        /// </summary>
        public int TextureCount { get; set; }

        /// <summary>
        /// 动画数量
        /// </summary>
        public int AnimationCount { get; set; }

        /// <summary>
        /// 骨骼数量
        /// </summary>
        public int BoneCount { get; set; }

        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; set; }
    }

    /// <summary>
    /// 模型动画
    /// </summary>
    public class ModelAnimation
    {
        /// <summary>
        /// 动画名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 动画时长（秒）
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// 是否循环
        /// </summary>
        public bool Loop { get; set; }

        /// <summary>
        /// 关键帧数量
        /// </summary>
        public int KeyframeCount { get; set; }

        /// <summary>
        /// 影响的骨骼
        /// </summary>
        public List<string> AffectedBones { get; set; } = new();

        /// <summary>
        /// 动画类型
        /// </summary>
        public string AnimationType { get; set; } = string.Empty;
    }

    #endregion
}

