using CEEdit.Core.Models.Blocks;
using CEEdit.Core.Models.Items;
using CEEdit.Core.Models.Resources;

namespace CEEdit.Core.Services.Interfaces
{
    /// <summary>
    /// 预览服务接口
    /// </summary>
    public interface IPreviewService
    {
        /// <summary>
        /// 预览更新事件
        /// </summary>
        event EventHandler<PreviewUpdatedEventArgs>? PreviewUpdated;

        /// <summary>
        /// 渲染方块预览
        /// </summary>
        /// <param name="block">方块</param>
        /// <param name="renderOptions">渲染选项</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>渲染结果</returns>
        Task<RenderResult?> RenderBlockAsync(Block block, RenderOptions? renderOptions = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 渲染物品预览
        /// </summary>
        /// <param name="item">物品</param>
        /// <param name="renderOptions">渲染选项</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>渲染结果</returns>
        Task<RenderResult?> RenderItemAsync(Item item, RenderOptions? renderOptions = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 渲染3D模型预览
        /// </summary>
        /// <param name="model">3D模型</param>
        /// <param name="renderOptions">渲染选项</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>渲染结果</returns>
        Task<RenderResult?> RenderModelAsync(Model3D model, RenderOptions? renderOptions = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioPath">音效路径</param>
        /// <param name="playbackOptions">播放选项</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>播放任务</returns>
        Task PlayAudioAsync(string audioPath, AudioPlaybackOptions? playbackOptions = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 停止音效播放
        /// </summary>
        /// <returns>停止任务</returns>
        Task StopAudioAsync();

        /// <summary>
        /// 显示粒子效果
        /// </summary>
        /// <param name="effect">粒子效果</param>
        /// <param name="duration">持续时间（毫秒）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>显示任务</returns>
        Task ShowParticleEffectAsync(ParticleEffect effect, int duration = 5000,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 预览纹理
        /// </summary>
        /// <param name="texture">纹理</param>
        /// <param name="previewOptions">预览选项</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>预览结果</returns>
        Task<PreviewResult?> PreviewTextureAsync(Texture texture, TexturePreviewOptions? previewOptions = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 预览动画
        /// </summary>
        /// <param name="animationPath">动画路径</param>
        /// <param name="model">关联的模型</param>
        /// <param name="animationOptions">动画选项</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>预览任务</returns>
        Task PreviewAnimationAsync(string animationPath, Model3D? model = null,
            AnimationPreviewOptions? animationOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 停止动画预览
        /// </summary>
        /// <returns>停止任务</returns>
        Task StopAnimationPreviewAsync();

        /// <summary>
        /// 创建预览快照
        /// </summary>
        /// <param name="target">预览目标</param>
        /// <param name="snapshotOptions">快照选项</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>快照数据</returns>
        Task<byte[]?> CreateSnapshotAsync(object target, SnapshotOptions? snapshotOptions = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 设置预览环境
        /// </summary>
        /// <param name="environment">环境设置</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>设置任务</returns>
        Task SetPreviewEnvironmentAsync(PreviewEnvironment environment, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取当前预览环境
        /// </summary>
        /// <returns>当前环境设置</returns>
        PreviewEnvironment GetCurrentEnvironment();

        /// <summary>
        /// 清理预览资源
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>清理任务</returns>
        Task CleanupPreviewResourcesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取预览统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        PreviewStatistics GetPreviewStatistics();
    }

    #region 事件参数

    /// <summary>
    /// 预览更新事件参数
    /// </summary>
    public class PreviewUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// 预览类型
        /// </summary>
        public PreviewType PreviewType { get; }

        /// <summary>
        /// 预览目标
        /// </summary>
        public object Target { get; }

        /// <summary>
        /// 更新原因
        /// </summary>
        public string UpdateReason { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="previewType">预览类型</param>
        /// <param name="target">预览目标</param>
        /// <param name="updateReason">更新原因</param>
        public PreviewUpdatedEventArgs(PreviewType previewType, object target, string updateReason)
        {
            PreviewType = previewType;
            Target = target;
            UpdateReason = updateReason;
        }
    }

    #endregion

    #region 枚举

    /// <summary>
    /// 预览类型
    /// </summary>
    public enum PreviewType
    {
        Block,
        Item,
        Model,
        Texture,
        Animation,
        Audio,
        Particle
    }

    /// <summary>
    /// 渲染质量
    /// </summary>
    public enum RenderQuality
    {
        Low,
        Medium,
        High,
        Ultra
    }

    /// <summary>
    /// 背景类型
    /// </summary>
    public enum BackgroundType
    {
        Transparent,
        Solid,
        Gradient,
        Grid,
        Environment
    }

    #endregion

    #region 辅助类

    /// <summary>
    /// 渲染选项
    /// </summary>
    public class RenderOptions
    {
        /// <summary>
        /// 输出尺寸
        /// </summary>
        public Size OutputSize { get; set; } = new(256, 256);

        /// <summary>
        /// 渲染质量
        /// </summary>
        public RenderQuality Quality { get; set; } = RenderQuality.High;

        /// <summary>
        /// 背景类型
        /// </summary>
        public BackgroundType Background { get; set; } = BackgroundType.Transparent;

        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color BackgroundColor { get; set; } = Color.Transparent;

        /// <summary>
        /// 是否显示网格
        /// </summary>
        public bool ShowGrid { get; set; } = false;

        /// <summary>
        /// 是否启用光照
        /// </summary>
        public bool EnableLighting { get; set; } = true;

        /// <summary>
        /// 是否启用阴影
        /// </summary>
        public bool EnableShadows { get; set; } = true;

        /// <summary>
        /// 相机位置
        /// </summary>
        public Vector3 CameraPosition { get; set; } = new(3, 3, 3);

        /// <summary>
        /// 相机目标
        /// </summary>
        public Vector3 CameraTarget { get; set; } = new(0, 0, 0);
    }

    /// <summary>
    /// 渲染结果
    /// </summary>
    public class RenderResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 渲染的图像数据
        /// </summary>
        public byte[]? ImageData { get; set; }

        /// <summary>
        /// 图像格式
        /// </summary>
        public string ImageFormat { get; set; } = "PNG";

        /// <summary>
        /// 渲染时间
        /// </summary>
        public TimeSpan RenderTime { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 渲染统计
        /// </summary>
        public RenderStatistics Statistics { get; set; } = new();
    }

    /// <summary>
    /// 音频播放选项
    /// </summary>
    public class AudioPlaybackOptions
    {
        /// <summary>
        /// 音量（0.0-1.0）
        /// </summary>
        public float Volume { get; set; } = 1.0f;

        /// <summary>
        /// 是否循环播放
        /// </summary>
        public bool Loop { get; set; } = false;

        /// <summary>
        /// 播放速度
        /// </summary>
        public float PlaybackRate { get; set; } = 1.0f;

        /// <summary>
        /// 开始位置（秒）
        /// </summary>
        public double StartPosition { get; set; } = 0.0;
    }

    /// <summary>
    /// 纹理预览选项
    /// </summary>
    public class TexturePreviewOptions
    {
        /// <summary>
        /// 预览尺寸
        /// </summary>
        public Size PreviewSize { get; set; } = new(512, 512);

        /// <summary>
        /// 显示Alpha通道
        /// </summary>
        public bool ShowAlpha { get; set; } = true;

        /// <summary>
        /// 背景棋盘格
        /// </summary>
        public bool ShowCheckerboard { get; set; } = true;

        /// <summary>
        /// 缩放模式
        /// </summary>
        public ScaleMode ScaleMode { get; set; } = ScaleMode.Fit;
    }

    /// <summary>
    /// 动画预览选项
    /// </summary>
    public class AnimationPreviewOptions
    {
        /// <summary>
        /// 播放速度
        /// </summary>
        public float PlaybackSpeed { get; set; } = 1.0f;

        /// <summary>
        /// 是否循环播放
        /// </summary>
        public bool Loop { get; set; } = true;

        /// <summary>
        /// 自动播放
        /// </summary>
        public bool AutoPlay { get; set; } = true;

        /// <summary>
        /// 显示骨骼
        /// </summary>
        public bool ShowBones { get; set; } = false;

        /// <summary>
        /// 显示网格
        /// </summary>
        public bool ShowWireframe { get; set; } = false;
    }

    /// <summary>
    /// 快照选项
    /// </summary>
    public class SnapshotOptions
    {
        /// <summary>
        /// 输出格式
        /// </summary>
        public string Format { get; set; } = "PNG";

        /// <summary>
        /// 图像质量（0-100）
        /// </summary>
        public int Quality { get; set; } = 95;

        /// <summary>
        /// 输出尺寸
        /// </summary>
        public Size Size { get; set; } = new(512, 512);

        /// <summary>
        /// 透明背景
        /// </summary>
        public bool TransparentBackground { get; set; } = true;
    }

    /// <summary>
    /// 预览环境
    /// </summary>
    public class PreviewEnvironment
    {
        /// <summary>
        /// 环境光颜色
        /// </summary>
        public Color AmbientLight { get; set; } = new(128, 128, 128);

        /// <summary>
        /// 主光源颜色
        /// </summary>
        public Color MainLight { get; set; } = Color.White;

        /// <summary>
        /// 主光源方向
        /// </summary>
        public Vector3 MainLightDirection { get; set; } = new(-1, -1, -1);

        /// <summary>
        /// 天空盒路径
        /// </summary>
        public string? SkyboxPath { get; set; }

        /// <summary>
        /// 雾效设置
        /// </summary>
        public FogSettings Fog { get; set; } = new();

        /// <summary>
        /// 后处理效果
        /// </summary>
        public List<string> PostProcessingEffects { get; set; } = new();
    }

    /// <summary>
    /// 雾效设置
    /// </summary>
    public class FogSettings
    {
        /// <summary>
        /// 是否启用雾效
        /// </summary>
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// 雾效颜色
        /// </summary>
        public Color Color { get; set; } = new(128, 128, 128);

        /// <summary>
        /// 雾效密度
        /// </summary>
        public float Density { get; set; } = 0.01f;

        /// <summary>
        /// 开始距离
        /// </summary>
        public float StartDistance { get; set; } = 10.0f;

        /// <summary>
        /// 结束距离
        /// </summary>
        public float EndDistance { get; set; } = 100.0f;
    }

    /// <summary>
    /// 预览结果
    /// </summary>
    public class PreviewResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 预览数据
        /// </summary>
        public byte[]? PreviewData { get; set; }

        /// <summary>
        /// 预览格式
        /// </summary>
        public string Format { get; set; } = "PNG";

        /// <summary>
        /// 错误消息
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 元数据
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    /// <summary>
    /// 渲染统计
    /// </summary>
    public class RenderStatistics
    {
        /// <summary>
        /// 顶点数
        /// </summary>
        public int VertexCount { get; set; }

        /// <summary>
        /// 三角形数
        /// </summary>
        public int TriangleCount { get; set; }

        /// <summary>
        /// 绘制调用数
        /// </summary>
        public int DrawCalls { get; set; }

        /// <summary>
        /// 纹理数
        /// </summary>
        public int TextureCount { get; set; }

        /// <summary>
        /// 显存使用量（字节）
        /// </summary>
        public long MemoryUsage { get; set; }
    }

    /// <summary>
    /// 预览统计信息
    /// </summary>
    public class PreviewStatistics
    {
        /// <summary>
        /// 总渲染次数
        /// </summary>
        public int TotalRenderCount { get; set; }

        /// <summary>
        /// 平均渲染时间
        /// </summary>
        public TimeSpan AverageRenderTime { get; set; }

        /// <summary>
        /// 缓存命中率
        /// </summary>
        public double CacheHitRate { get; set; }

        /// <summary>
        /// 内存使用量
        /// </summary>
        public long MemoryUsage { get; set; }
    }

    /// <summary>
    /// 尺寸结构
    /// </summary>
    public struct Size
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    /// <summary>
    /// 3D向量
    /// </summary>
    public class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3() { }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    /// <summary>
    /// 颜色结构
    /// </summary>
    public struct Color
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public Color(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static Color White => new(255, 255, 255);
        public static Color Black => new(0, 0, 0);
        public static Color Transparent => new(0, 0, 0, 0);
    }

    /// <summary>
    /// 缩放模式
    /// </summary>
    public enum ScaleMode
    {
        Fit,
        Fill,
        Stretch,
        Center
    }

    #endregion
}

