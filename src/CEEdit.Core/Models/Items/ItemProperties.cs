using System.Collections.Generic;
using CEEdit.Core.Models.Common;

namespace CEEdit.Core.Models.Items
{
    /// <summary>
    /// 物品属性配置
    /// </summary>
    public class ItemProperties
    {
        /// <summary>
        /// 是否无法破坏
        /// </summary>
        public bool Unbreakable { get; set; } = false;

        /// <summary>
        /// 是否隐藏附魔
        /// </summary>
        public bool HideEnchants { get; set; } = false;

        /// <summary>
        /// 是否隐藏属性修改器
        /// </summary>
        public bool HideAttributes { get; set; } = false;

        /// <summary>
        /// 是否隐藏其他信息
        /// </summary>
        public bool HideAdditionalTooltip { get; set; } = false;

        /// <summary>
        /// 自定义名称颜色
        /// </summary>
        public string NameColor { get; set; } = "#FFFFFF";

        /// <summary>
        /// Lore颜色列表
        /// </summary>
        public List<string> LoreColors { get; set; } = new();

        /// <summary>
        /// 物品发光
        /// </summary>
        public bool HasGlint { get; set; } = false;

        /// <summary>
        /// 自定义模型配置
        /// </summary>
        public CustomModelConfig CustomModel { get; set; } = new();

        /// <summary>
        /// 动画配置
        /// </summary>
        public AnimationConfig Animation { get; set; } = new();

        /// <summary>
        /// 特殊属性
        /// </summary>
        public Dictionary<string, object> SpecialProperties { get; set; } = new();
    }

    /// <summary>
    /// 自定义模型配置
    /// </summary>
    public class CustomModelConfig
    {
        /// <summary>
        /// 是否使用自定义模型
        /// </summary>
        public bool UseCustomModel { get; set; } = false;

        /// <summary>
        /// 模型文件路径
        /// </summary>
        public string ModelPath { get; set; } = string.Empty;

        /// <summary>
        /// 纹理映射
        /// </summary>
        public Dictionary<string, string> TextureMapping { get; set; } = new();

        /// <summary>
        /// 模型缩放
        /// </summary>
        public ModelScale Scale { get; set; } = new();

        /// <summary>
        /// 模型旋转
        /// </summary>
        public ModelRotation Rotation { get; set; } = new();

        /// <summary>
        /// 模型偏移
        /// </summary>
        public ModelOffset Offset { get; set; } = new();
    }

    /// <summary>
    /// 动画配置
    /// </summary>
    public class AnimationConfig
    {
        /// <summary>
        /// 是否启用动画
        /// </summary>
        public bool EnableAnimation { get; set; } = false;

        /// <summary>
        /// 动画列表
        /// </summary>
        public List<ItemAnimation> Animations { get; set; } = new();

        /// <summary>
        /// 默认动画
        /// </summary>
        public string DefaultAnimation { get; set; } = string.Empty;
    }

    /// <summary>
    /// 物品动画
    /// </summary>
    public class ItemAnimation
    {
        /// <summary>
        /// 动画名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 动画类型
        /// </summary>
        public AnimationType Type { get; set; } = AnimationType.Idle;

        /// <summary>
        /// 动画时长（毫秒）
        /// </summary>
        public int Duration { get; set; } = 1000;

        /// <summary>
        /// 是否循环
        /// </summary>
        public bool Loop { get; set; } = false;

        /// <summary>
        /// 关键帧列表
        /// </summary>
        public List<AnimationKeyframe> Keyframes { get; set; } = new();

        /// <summary>
        /// 触发条件
        /// </summary>
        public List<AnimationTrigger> Triggers { get; set; } = new();
    }

    /// <summary>
    /// 动画关键帧
    /// </summary>
    public class AnimationKeyframe
    {
        /// <summary>
        /// 时间点（毫秒）
        /// </summary>
        public int Time { get; set; } = 0;

        /// <summary>
        /// 变换数据
        /// </summary>
        public Transform Transform { get; set; } = new();

        /// <summary>
        /// 缓动类型
        /// </summary>
        public EasingType Easing { get; set; } = EasingType.Linear;
    }

    /// <summary>
    /// 变换数据
    /// </summary>
    public class Transform
    {
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; } = new();

        /// <summary>
        /// 旋转（欧拉角）
        /// </summary>
        public Vector3 Rotation { get; set; } = new();

        /// <summary>
        /// 缩放
        /// </summary>
        public Vector3 Scale { get; set; } = new(1, 1, 1);
    }

    /// <summary>
    /// 动画触发器
    /// </summary>
    public class AnimationTrigger
    {
        /// <summary>
        /// 触发事件
        /// </summary>
        public string Event { get; set; } = string.Empty;

        /// <summary>
        /// 触发条件
        /// </summary>
        public string Condition { get; set; } = string.Empty;
    }

    /// <summary>
    /// 模型缩放
    /// </summary>
    public class ModelScale
    {
        public float X { get; set; } = 1.0f;
        public float Y { get; set; } = 1.0f;
        public float Z { get; set; } = 1.0f;
    }

    /// <summary>
    /// 模型旋转
    /// </summary>
    public class ModelRotation
    {
        public float X { get; set; } = 0.0f;
        public float Y { get; set; } = 0.0f;
        public float Z { get; set; } = 0.0f;
    }

    /// <summary>
    /// 模型偏移
    /// </summary>
    public class ModelOffset
    {
        public float X { get; set; } = 0.0f;
        public float Y { get; set; } = 0.0f;
        public float Z { get; set; } = 0.0f;
    }

    /// <summary>
    /// 动画类型枚举
    /// </summary>
    public enum AnimationType
    {
        Idle,
        Attack,
        Use,
        Block,
        Throw,
        Custom
    }

    /// <summary>
    /// 缓动类型枚举
    /// </summary>
    public enum EasingType
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
        Bounce,
        Elastic,
        Back
    }

}

