using System.Collections.Generic;
using CEEdit.Core.Models.Common;

namespace CEEdit.Core.Models.Resources
{
    /// <summary>
    /// 资源基类
    /// </summary>
    public abstract class ResourceBase
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 资源描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 文件哈希值
        /// </summary>
        public string FileHash { get; set; } = string.Empty;

        /// <summary>
        /// 资源类型
        /// </summary>
        public abstract ResourceType Type { get; }

        /// <summary>
        /// 资源标签
        /// </summary>
        public List<string> Tags { get; set; } = new();

        /// <summary>
        /// 自定义属性
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; } = new();

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否为内置资源
        /// </summary>
        public bool IsBuiltIn { get; set; } = false;

        /// <summary>
        /// 验证资源
        /// </summary>
        /// <returns>验证结果</returns>
        public virtual ValidationResult Validate()
        {
            var result = new ValidationResult();

            if (string.IsNullOrEmpty(Id))
                result.AddError("资源ID不能为空");

            if (string.IsNullOrEmpty(Name))
                result.AddError("资源名称不能为空");

            if (string.IsNullOrEmpty(FilePath))
                result.AddError("文件路径不能为空");

            return result;
        }
    }

    /// <summary>
    /// 纹理资源
    /// </summary>
    public class Texture : ResourceBase
    {
        public override ResourceType Type => ResourceType.Texture;

        /// <summary>
        /// 图片宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 图片高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 颜色深度
        /// </summary>
        public int BitDepth { get; set; } = 32;

        /// <summary>
        /// 图片格式
        /// </summary>
        public string Format { get; set; } = "PNG";

        /// <summary>
        /// 是否有透明通道
        /// </summary>
        public bool HasAlpha { get; set; } = true;

        /// <summary>
        /// 是否为动画纹理
        /// </summary>
        public bool IsAnimated { get; set; } = false;

        /// <summary>
        /// 动画帧数
        /// </summary>
        public int FrameCount { get; set; } = 1;

        /// <summary>
        /// 动画配置文件路径
        /// </summary>
        public string AnimationConfigPath { get; set; } = string.Empty;

        public override ValidationResult Validate()
        {
            var result = base.Validate();

            if (Width <= 0)
                result.AddError("纹理宽度必须大于0");

            if (Height <= 0)
                result.AddError("纹理高度必须大于0");

            // 检查尺寸是否为2的幂次方
            if (!IsPowerOfTwo(Width) || !IsPowerOfTwo(Height))
                result.AddWarning("建议纹理尺寸为2的幂次方以优化性能");

            return result;
        }

        private static bool IsPowerOfTwo(int x)
        {
            return x > 0 && (x & (x - 1)) == 0;
        }
    }

    /// <summary>
    /// 3D模型资源
    /// </summary>
    public class Model3D : ResourceBase
    {
        public override ResourceType Type => ResourceType.Model;

        /// <summary>
        /// 模型类型
        /// </summary>
        public ModelType ModelType { get; set; } = ModelType.Block;

        /// <summary>
        /// 顶点数量
        /// </summary>
        public int VertexCount { get; set; }

        /// <summary>
        /// 面数量
        /// </summary>
        public int FaceCount { get; set; }

        /// <summary>
        /// 纹理映射
        /// </summary>
        public Dictionary<string, string> TextureMapping { get; set; } = new();

        /// <summary>
        /// 动画列表
        /// </summary>
        public List<ModelAnimation> Animations { get; set; } = new();

        /// <summary>
        /// 边界框
        /// </summary>
        public BoundingBox BoundingBox { get; set; } = new();

        /// <summary>
        /// 材质列表
        /// </summary>
        public List<string> Materials { get; set; } = new();

        public override ValidationResult Validate()
        {
            var result = base.Validate();

            if (FaceCount > 10000)
                result.AddWarning("模型面数过多，可能影响性能");

            if (TextureMapping.Count == 0)
                result.AddWarning("模型没有纹理映射");

            return result;
        }
    }

    /// <summary>
    /// 音频资源
    /// </summary>
    public class AudioClip : ResourceBase
    {
        public override ResourceType Type => ResourceType.Sound;

        /// <summary>
        /// 音效类型
        /// </summary>
        public SoundType SoundType { get; set; } = SoundType.Ambient;

        /// <summary>
        /// 持续时间（秒）
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// 采样率
        /// </summary>
        public int SampleRate { get; set; } = 44100;

        /// <summary>
        /// 声道数
        /// </summary>
        public int Channels { get; set; } = 1;

        /// <summary>
        /// 位深度
        /// </summary>
        public int BitRate { get; set; } = 16;

        /// <summary>
        /// 音频格式
        /// </summary>
        public string Format { get; set; } = "OGG";

        /// <summary>
        /// 音量
        /// </summary>
        public float Volume { get; set; } = 1.0f;

        /// <summary>
        /// 是否循环播放
        /// </summary>
        public bool Loop { get; set; } = false;

        public override ValidationResult Validate()
        {
            var result = base.Validate();

            if (Duration <= 0)
                result.AddError("音频持续时间必须大于0");

            if (Volume < 0 || Volume > 2)
                result.AddWarning("音量建议在0-2之间");

            if (Duration > 60 && SoundType != SoundType.Music)
                result.AddWarning("非音乐类音效过长，可能影响性能");

            return result;
        }
    }

    /// <summary>
    /// 语言文件资源
    /// </summary>
    public class LanguageFile : ResourceBase
    {
        public override ResourceType Type => ResourceType.Language;

        /// <summary>
        /// 语言代码
        /// </summary>
        public string LanguageCode { get; set; } = string.Empty;

        /// <summary>
        /// 语言名称
        /// </summary>
        public string LanguageName { get; set; } = string.Empty;

        /// <summary>
        /// 地区代码
        /// </summary>
        public string RegionCode { get; set; } = string.Empty;

        /// <summary>
        /// 翻译项数量
        /// </summary>
        public int EntryCount { get; set; }

        /// <summary>
        /// 翻译完成度
        /// </summary>
        public float CompletionPercentage { get; set; }

        /// <summary>
        /// 翻译条目
        /// </summary>
        public Dictionary<string, string> Translations { get; set; } = new();

        public override ValidationResult Validate()
        {
            var result = base.Validate();

            if (string.IsNullOrEmpty(LanguageCode))
                result.AddError("语言代码不能为空");

            if (string.IsNullOrEmpty(LanguageName))
                result.AddError("语言名称不能为空");

            if (Translations.Count == 0)
                result.AddWarning("语言文件没有翻译条目");

            return result;
        }
    }

    /// <summary>
    /// 字体资源
    /// </summary>
    public class FontResource : ResourceBase
    {
        public override ResourceType Type => ResourceType.Font;

        /// <summary>
        /// 字体家族
        /// </summary>
        public string FontFamily { get; set; } = string.Empty;

        /// <summary>
        /// 字体样式
        /// </summary>
        public string FontStyle { get; set; } = "Regular";

        /// <summary>
        /// 字体大小
        /// </summary>
        public int FontSize { get; set; } = 12;

        /// <summary>
        /// 字符集
        /// </summary>
        public string CharacterSet { get; set; } = "UTF-8";

        /// <summary>
        /// 支持的字符数量
        /// </summary>
        public int CharacterCount { get; set; }
    }

    /// <summary>
    /// 着色器资源
    /// </summary>
    public class ShaderResource : ResourceBase
    {
        public override ResourceType Type => ResourceType.Shader;

        /// <summary>
        /// 着色器类型
        /// </summary>
        public ShaderType ShaderType { get; set; } = ShaderType.Fragment;

        /// <summary>
        /// GLSL版本
        /// </summary>
        public string GLSLVersion { get; set; } = "150";

        /// <summary>
        /// 统一变量
        /// </summary>
        public List<string> Uniforms { get; set; } = new();

        /// <summary>
        /// 属性变量
        /// </summary>
        public List<string> Attributes { get; set; } = new();
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
        /// 动画时长
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// 是否循环
        /// </summary>
        public bool Loop { get; set; } = false;

        /// <summary>
        /// 关键帧数量
        /// </summary>
        public int KeyframeCount { get; set; }
    }

    /// <summary>
    /// 边界框
    /// </summary>
    public class BoundingBox
    {
        public float MinX { get; set; } = 0.0f;
        public float MinY { get; set; } = 0.0f;
        public float MinZ { get; set; } = 0.0f;
        public float MaxX { get; set; } = 1.0f;
        public float MaxY { get; set; } = 1.0f;
        public float MaxZ { get; set; } = 1.0f;

        public float Width => MaxX - MinX;
        public float Height => MaxY - MinY;
        public float Depth => MaxZ - MinZ;
    }

    /// <summary>
    /// 着色器类型枚举
    /// </summary>
    public enum ShaderType
    {
        Vertex,
        Fragment,
        Geometry,
        Compute
    }
}

