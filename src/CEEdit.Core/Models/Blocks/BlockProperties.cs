using System.Collections.Generic;

namespace CEEdit.Core.Models.Blocks
{
    /// <summary>
    /// 方块属性配置
    /// </summary>
    public class BlockProperties
    {
        /// <summary>
        /// 碰撞箱设置
        /// </summary>
        public CollisionBox CollisionBox { get; set; } = new();

        /// <summary>
        /// 选择框设置
        /// </summary>
        public SelectionBox SelectionBox { get; set; } = new();

        /// <summary>
        /// 物理属性
        /// </summary>
        public PhysicsProperties Physics { get; set; } = new();

        /// <summary>
        /// 环境属性
        /// </summary>
        public EnvironmentProperties Environment { get; set; } = new();

        /// <summary>
        /// 玩家交互属性
        /// </summary>
        public InteractionProperties Interaction { get; set; } = new();

        /// <summary>
        /// 方块状态
        /// </summary>
        public Dictionary<string, BlockState> States { get; set; } = new();

        /// <summary>
        /// 自定义NBT数据
        /// </summary>
        public Dictionary<string, object> CustomNBT { get; set; } = new();
    }

    /// <summary>
    /// 碰撞箱配置
    /// </summary>
    public class CollisionBox
    {
        /// <summary>
        /// 是否有碰撞
        /// </summary>
        public bool HasCollision { get; set; } = true;

        /// <summary>
        /// 碰撞箱列表（支持多个碰撞箱）
        /// </summary>
        public List<BoundingBox> Boxes { get; set; } = new() { new BoundingBox() };
    }

    /// <summary>
    /// 选择框配置
    /// </summary>
    public class SelectionBox
    {
        /// <summary>
        /// 是否有选择框
        /// </summary>
        public bool HasSelection { get; set; } = true;

        /// <summary>
        /// 选择框
        /// </summary>
        public BoundingBox Box { get; set; } = new();
    }

    /// <summary>
    /// 边界框
    /// </summary>
    public class BoundingBox
    {
        /// <summary>
        /// 最小X坐标
        /// </summary>
        public float MinX { get; set; } = 0.0f;

        /// <summary>
        /// 最小Y坐标
        /// </summary>
        public float MinY { get; set; } = 0.0f;

        /// <summary>
        /// 最小Z坐标
        /// </summary>
        public float MinZ { get; set; } = 0.0f;

        /// <summary>
        /// 最大X坐标
        /// </summary>
        public float MaxX { get; set; } = 1.0f;

        /// <summary>
        /// 最大Y坐标
        /// </summary>
        public float MaxY { get; set; } = 1.0f;

        /// <summary>
        /// 最大Z坐标
        /// </summary>
        public float MaxZ { get; set; } = 1.0f;
    }

    /// <summary>
    /// 物理属性
    /// </summary>
    public class PhysicsProperties
    {
        /// <summary>
        /// 摩擦系数
        /// </summary>
        public float Friction { get; set; } = 0.6f;

        /// <summary>
        /// 弹跳系数
        /// </summary>
        public float Bounciness { get; set; } = 0.0f;

        /// <summary>
        /// 是否受重力影响
        /// </summary>
        public bool AffectedByGravity { get; set; } = false;

        /// <summary>
        /// 是否能被推动
        /// </summary>
        public bool CanBePushed { get; set; } = false;
    }

    /// <summary>
    /// 环境属性
    /// </summary>
    public class EnvironmentProperties
    {
        /// <summary>
        /// 阻挡光线
        /// </summary>
        public bool BlocksLight { get; set; } = true;

        /// <summary>
        /// 导热性
        /// </summary>
        public bool ConductsHeat { get; set; } = false;

        /// <summary>
        /// 可燃性
        /// </summary>
        public bool Flammable { get; set; } = false;

        /// <summary>
        /// 燃烧速度
        /// </summary>
        public int BurnRate { get; set; } = 0;

        /// <summary>
        /// 防火等级
        /// </summary>
        public int FireResistance { get; set; } = 0;

        /// <summary>
        /// 生长属性
        /// </summary>
        public GrowthProperties Growth { get; set; } = new();
    }

    /// <summary>
    /// 生长属性
    /// </summary>
    public class GrowthProperties
    {
        /// <summary>
        /// 是否可生长
        /// </summary>
        public bool CanGrow { get; set; } = false;

        /// <summary>
        /// 生长速度
        /// </summary>
        public float GrowthRate { get; set; } = 1.0f;

        /// <summary>
        /// 生长条件
        /// </summary>
        public List<GrowthCondition> Conditions { get; set; } = new();

        /// <summary>
        /// 最大生长阶段
        /// </summary>
        public int MaxGrowthStage { get; set; } = 1;
    }

    /// <summary>
    /// 生长条件
    /// </summary>
    public class GrowthCondition
    {
        /// <summary>
        /// 条件类型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 条件值
        /// </summary>
        public object Value { get; set; } = new();
    }

    /// <summary>
    /// 交互属性
    /// </summary>
    public class InteractionProperties
    {
        /// <summary>
        /// 是否可以通过
        /// </summary>
        public bool CanWalkThrough { get; set; } = false;

        /// <summary>
        /// 是否可以站立
        /// </summary>
        public bool CanStandOn { get; set; } = true;

        /// <summary>
        /// 行走速度修改器
        /// </summary>
        public float WalkSpeedModifier { get; set; } = 1.0f;

        /// <summary>
        /// 跳跃高度修改器
        /// </summary>
        public float JumpHeightModifier { get; set; } = 1.0f;
    }

    /// <summary>
    /// 方块状态
    /// </summary>
    public class BlockState
    {
        /// <summary>
        /// 状态名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 状态类型
        /// </summary>
        public StateType Type { get; set; } = StateType.Boolean;

        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; } = false;

        /// <summary>
        /// 可能的值
        /// </summary>
        public List<object> PossibleValues { get; set; } = new();
    }

    /// <summary>
    /// 状态类型枚举
    /// </summary>
    public enum StateType
    {
        Boolean,
        Integer,
        String,
        Enum
    }
}

