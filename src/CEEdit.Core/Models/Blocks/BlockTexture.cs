using System.Collections.Generic;

namespace CEEdit.Core.Models.Blocks
{
    /// <summary>
    /// 方块纹理配置
    /// </summary>
    public class BlockTexture
    {
        /// <summary>
        /// 所有面使用的纹理（简化配置）
        /// </summary>
        public string AllFaces { get; set; } = string.Empty;

        /// <summary>
        /// 顶部纹理
        /// </summary>
        public string Top { get; set; } = string.Empty;

        /// <summary>
        /// 底部纹理
        /// </summary>
        public string Bottom { get; set; } = string.Empty;

        /// <summary>
        /// 北面纹理
        /// </summary>
        public string North { get; set; } = string.Empty;

        /// <summary>
        /// 南面纹理
        /// </summary>
        public string South { get; set; } = string.Empty;

        /// <summary>
        /// 东面纹理
        /// </summary>
        public string East { get; set; } = string.Empty;

        /// <summary>
        /// 西面纹理
        /// </summary>
        public string West { get; set; } = string.Empty;

        /// <summary>
        /// 侧面纹理（简化配置，用于所有侧面）
        /// </summary>
        public string Side { get; set; } = string.Empty;

        /// <summary>
        /// 纹理动画配置
        /// </summary>
        public List<TextureAnimation> Animations { get; set; } = new();

        /// <summary>
        /// 覆盖纹理（如草地的草色）
        /// </summary>
        public Dictionary<string, string> OverlayTextures { get; set; } = new();

        /// <summary>
        /// 纹理着色配置
        /// </summary>
        public TextureTinting Tinting { get; set; } = new();

        /// <summary>
        /// 是否使用自定义模型
        /// </summary>
        public bool UseCustomModel { get; set; } = false;

        /// <summary>
        /// 自定义模型路径
        /// </summary>
        public string CustomModelPath { get; set; } = string.Empty;

        /// <summary>
        /// 获取指定面的纹理
        /// </summary>
        /// <param name="face">面方向</param>
        /// <returns>纹理路径</returns>
        public string GetTexture(BlockFace face)
        {
            if (!string.IsNullOrEmpty(AllFaces))
                return AllFaces;

            return face switch
            {
                BlockFace.Top => Top,
                BlockFace.Bottom => Bottom,
                BlockFace.North => North,
                BlockFace.South => South,
                BlockFace.East => East,
                BlockFace.West => West,
                _ => AllFaces
            };
        }
    }

    /// <summary>
    /// 纹理动画配置
    /// </summary>
    public class TextureAnimation
    {
        /// <summary>
        /// 动画名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 动画帧列表
        /// </summary>
        public List<string> Frames { get; set; } = new();

        /// <summary>
        /// 每帧持续时间（毫秒）
        /// </summary>
        public int FrameDuration { get; set; } = 100;

        /// <summary>
        /// 是否循环播放
        /// </summary>
        public bool Loop { get; set; } = true;

        /// <summary>
        /// 应用到的面
        /// </summary>
        public List<BlockFace> ApplyToFaces { get; set; } = new();
    }

    /// <summary>
    /// 纹理着色配置
    /// </summary>
    public class TextureTinting
    {
        /// <summary>
        /// 是否启用着色
        /// </summary>
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// 着色类型
        /// </summary>
        public TintType Type { get; set; } = TintType.Fixed;

        /// <summary>
        /// 固定颜色（RGB）
        /// </summary>
        public Color FixedColor { get; set; } = new();

        /// <summary>
        /// 生物群系着色索引
        /// </summary>
        public int BiomeTintIndex { get; set; } = 0;
    }

    /// <summary>
    /// 方块面枚举
    /// </summary>
    public enum BlockFace
    {
        Top,
        Bottom,
        North,
        South,
        East,
        West
    }

    /// <summary>
    /// 着色类型枚举
    /// </summary>
    public enum TintType
    {
        None,
        Fixed,
        Biome,
        Custom
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
}

