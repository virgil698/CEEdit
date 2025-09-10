using System.Collections.Generic;

namespace CEEdit.Core.Models.Project
{
    /// <summary>
    /// 项目设置模型
    /// </summary>
    public class ProjectSettings
    {
        /// <summary>
        /// 输出路径
        /// </summary>
        public string OutputPath { get; set; } = "output";

        /// <summary>
        /// 构建设置
        /// </summary>
        public BuildSettings Build { get; set; } = new();

        /// <summary>
        /// 资源优化设置
        /// </summary>
        public ResourceOptimizationSettings ResourceOptimization { get; set; } = new();

        /// <summary>
        /// 调试设置
        /// </summary>
        public DebugSettings Debug { get; set; } = new();

        /// <summary>
        /// 自定义属性
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; } = new();
    }

    /// <summary>
    /// 构建设置
    /// </summary>
    public class BuildSettings
    {
        /// <summary>
        /// 自动构建
        /// </summary>
        public bool AutoBuild { get; set; } = true;

        /// <summary>
        /// 压缩输出
        /// </summary>
        public bool CompressOutput { get; set; } = true;

        /// <summary>
        /// 包含调试信息
        /// </summary>
        public bool IncludeDebugInfo { get; set; } = false;

        /// <summary>
        /// 验证资源
        /// </summary>
        public bool ValidateResources { get; set; } = true;
    }

    /// <summary>
    /// 资源优化设置
    /// </summary>
    public class ResourceOptimizationSettings
    {
        /// <summary>
        /// 优化纹理
        /// </summary>
        public bool OptimizeTextures { get; set; } = true;

        /// <summary>
        /// 优化模型
        /// </summary>
        public bool OptimizeModels { get; set; } = true;

        /// <summary>
        /// 优化音频
        /// </summary>
        public bool OptimizeAudio { get; set; } = true;

        /// <summary>
        /// 纹理质量
        /// </summary>
        public TextureQuality TextureQuality { get; set; } = TextureQuality.High;
    }

    /// <summary>
    /// 调试设置
    /// </summary>
    public class DebugSettings
    {
        /// <summary>
        /// 启用调试模式
        /// </summary>
        public bool EnableDebugMode { get; set; } = false;

        /// <summary>
        /// 调试服务器地址
        /// </summary>
        public string DebugServerAddress { get; set; } = "localhost";

        /// <summary>
        /// 调试服务器端口
        /// </summary>
        public int DebugServerPort { get; set; } = 25565;

        /// <summary>
        /// 热重载
        /// </summary>
        public bool HotReload { get; set; } = true;
    }

    /// <summary>
    /// 纹理质量枚举
    /// </summary>
    public enum TextureQuality
    {
        Low,
        Medium,
        High,
        Ultra
    }
}

