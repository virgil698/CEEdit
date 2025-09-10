using System.Collections.Generic;
using CEEdit.Core.Models.Common;

namespace CEEdit.Core.Models.Items
{
    /// <summary>
    /// 物品行为配置
    /// </summary>
    public class ItemBehavior
    {
        /// <summary>
        /// 使用行为
        /// </summary>
        public UseBehavior Use { get; set; } = new();

        /// <summary>
        /// 攻击行为
        /// </summary>
        public AttackBehavior Attack { get; set; } = new();

        /// <summary>
        /// 防御行为
        /// </summary>
        public DefenseBehavior Defense { get; set; } = new();

        /// <summary>
        /// 工具行为
        /// </summary>
        public ToolBehavior Tool { get; set; } = new();

        /// <summary>
        /// 食物行为
        /// </summary>
        public FoodBehavior Food { get; set; } = new();

        /// <summary>
        /// 燃料行为
        /// </summary>
        public FuelBehavior Fuel { get; set; } = new();

        /// <summary>
        /// 投射物行为
        /// </summary>
        public ProjectileBehavior Projectile { get; set; } = new();

        /// <summary>
        /// 音效配置
        /// </summary>
        public ItemSounds Sounds { get; set; } = new();

        /// <summary>
        /// 粒子效果
        /// </summary>
        public List<ItemParticleEffect> ParticleEffects { get; set; } = new();

        /// <summary>
        /// 自定义事件处理器
        /// </summary>
        public Dictionary<string, string> EventHandlers { get; set; } = new();
    }

    /// <summary>
    /// 使用行为配置
    /// </summary>
    public class UseBehavior
    {
        /// <summary>
        /// 是否可使用
        /// </summary>
        public bool CanUse { get; set; } = false;

        /// <summary>
        /// 使用时间（毫秒）
        /// </summary>
        public int UseTime { get; set; } = 0;

        /// <summary>
        /// 使用动画
        /// </summary>
        public UseAnimation Animation { get; set; } = UseAnimation.None;

        /// <summary>
        /// 使用脚本
        /// </summary>
        public string UseScript { get; set; } = string.Empty;

        /// <summary>
        /// 冷却时间（毫秒）
        /// </summary>
        public int Cooldown { get; set; } = 0;

        /// <summary>
        /// 消耗耐久度
        /// </summary>
        public int DurabilityConsumption { get; set; } = 0;

        /// <summary>
        /// 使用条件
        /// </summary>
        public List<UseCondition> Conditions { get; set; } = new();
    }

    /// <summary>
    /// 攻击行为配置
    /// </summary>
    public class AttackBehavior
    {
        /// <summary>
        /// 攻击伤害
        /// </summary>
        public float AttackDamage { get; set; } = 1.0f;

        /// <summary>
        /// 攻击速度
        /// </summary>
        public float AttackSpeed { get; set; } = 4.0f;

        /// <summary>
        /// 攻击范围
        /// </summary>
        public float AttackRange { get; set; } = 1.0f;

        /// <summary>
        /// 击退强度
        /// </summary>
        public float KnockbackStrength { get; set; } = 0.0f;

        /// <summary>
        /// 攻击脚本
        /// </summary>
        public string AttackScript { get; set; } = string.Empty;

        /// <summary>
        /// 特殊攻击效果
        /// </summary>
        public List<AttackEffect> SpecialEffects { get; set; } = new();
    }

    /// <summary>
    /// 防御行为配置
    /// </summary>
    public class DefenseBehavior
    {
        /// <summary>
        /// 护甲值
        /// </summary>
        public float ArmorValue { get; set; } = 0.0f;

        /// <summary>
        /// 护甲韧性
        /// </summary>
        public float ArmorToughness { get; set; } = 0.0f;

        /// <summary>
        /// 击退抗性
        /// </summary>
        public float KnockbackResistance { get; set; } = 0.0f;

        /// <summary>
        /// 格挡效率
        /// </summary>
        public float BlockingEfficiency { get; set; } = 0.0f;

        /// <summary>
        /// 防御脚本
        /// </summary>
        public string DefenseScript { get; set; } = string.Empty;
    }

    /// <summary>
    /// 工具行为配置
    /// </summary>
    public class ToolBehavior
    {
        /// <summary>
        /// 是否为工具
        /// </summary>
        public bool IsTool { get; set; } = false;

        /// <summary>
        /// 工具类型
        /// </summary>
        public ToolType ToolType { get; set; } = ToolType.None;

        /// <summary>
        /// 工具等级
        /// </summary>
        public int ToolLevel { get; set; } = 0;

        /// <summary>
        /// 挖掘速度
        /// </summary>
        public float MiningSpeed { get; set; } = 1.0f;

        /// <summary>
        /// 有效材料
        /// </summary>
        public List<string> EffectiveMaterials { get; set; } = new();

        /// <summary>
        /// 工具脚本
        /// </summary>
        public string ToolScript { get; set; } = string.Empty;
    }

    /// <summary>
    /// 食物行为配置
    /// </summary>
    public class FoodBehavior
    {
        /// <summary>
        /// 是否可食用
        /// </summary>
        public bool IsEdible { get; set; } = false;

        /// <summary>
        /// 恢复饥饿值
        /// </summary>
        public int HungerRestore { get; set; } = 0;

        /// <summary>
        /// 饱和度修饰符
        /// </summary>
        public float SaturationModifier { get; set; } = 0.0f;

        /// <summary>
        /// 是否为肉类
        /// </summary>
        public bool IsMeat { get; set; } = false;

        /// <summary>
        /// 食用时间（毫秒）
        /// </summary>
        public int EatingTime { get; set; } = 1600;

        /// <summary>
        /// 食用后效果
        /// </summary>
        public List<FoodEffect> Effects { get; set; } = new();
    }

    /// <summary>
    /// 燃料行为配置
    /// </summary>
    public class FuelBehavior
    {
        /// <summary>
        /// 是否为燃料
        /// </summary>
        public bool IsFuel { get; set; } = false;

        /// <summary>
        /// 燃烧时间（tick）
        /// </summary>
        public int BurnTime { get; set; } = 0;

        /// <summary>
        /// 燃烧效率
        /// </summary>
        public float BurnEfficiency { get; set; } = 1.0f;
    }

    /// <summary>
    /// 投射物行为配置
    /// </summary>
    public class ProjectileBehavior
    {
        /// <summary>
        /// 是否可投射
        /// </summary>
        public bool IsProjectile { get; set; } = false;

        /// <summary>
        /// 投射物类型
        /// </summary>
        public string ProjectileType { get; set; } = string.Empty;

        /// <summary>
        /// 投射速度
        /// </summary>
        public float ProjectileSpeed { get; set; } = 1.0f;

        /// <summary>
        /// 重力影响
        /// </summary>
        public float GravityAffection { get; set; } = 1.0f;

        /// <summary>
        /// 投射脚本
        /// </summary>
        public string ProjectileScript { get; set; } = string.Empty;
    }

    /// <summary>
    /// 物品音效配置
    /// </summary>
    public class ItemSounds
    {
        /// <summary>
        /// 使用音效
        /// </summary>
        public string UseSound { get; set; } = string.Empty;

        /// <summary>
        /// 攻击音效
        /// </summary>
        public string AttackSound { get; set; } = string.Empty;

        /// <summary>
        /// 破损音效
        /// </summary>
        public string BreakSound { get; set; } = string.Empty;

        /// <summary>
        /// 拾取音效
        /// </summary>
        public string PickupSound { get; set; } = string.Empty;

        /// <summary>
        /// 丢弃音效
        /// </summary>
        public string DropSound { get; set; } = string.Empty;
    }

    /// <summary>
    /// 物品粒子效果
    /// </summary>
    public class ItemParticleEffect
    {
        /// <summary>
        /// 粒子类型
        /// </summary>
        public string ParticleType { get; set; } = string.Empty;

        /// <summary>
        /// 触发事件
        /// </summary>
        public string TriggerEvent { get; set; } = string.Empty;

        /// <summary>
        /// 粒子数量
        /// </summary>
        public int Count { get; set; } = 1;

        /// <summary>
        /// 偏移量
        /// </summary>
        public Vector3 Offset { get; set; } = new();

        /// <summary>
        /// 速度
        /// </summary>
        public Vector3 Velocity { get; set; } = new();

        /// <summary>
        /// 持续时间
        /// </summary>
        public int Duration { get; set; } = 0;
    }

    /// <summary>
    /// 使用条件
    /// </summary>
    public class UseCondition
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
    /// 攻击效果
    /// </summary>
    public class AttackEffect
    {
        /// <summary>
        /// 效果类型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 效果强度
        /// </summary>
        public float Strength { get; set; } = 1.0f;

        /// <summary>
        /// 持续时间
        /// </summary>
        public int Duration { get; set; } = 0;
    }

    /// <summary>
    /// 食物效果
    /// </summary>
    public class FoodEffect
    {
        /// <summary>
        /// 效果类型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 效果等级
        /// </summary>
        public int Level { get; set; } = 0;

        /// <summary>
        /// 持续时间
        /// </summary>
        public int Duration { get; set; } = 0;

        /// <summary>
        /// 触发概率
        /// </summary>
        public float Chance { get; set; } = 1.0f;
    }

    /// <summary>
    /// 使用动画枚举
    /// </summary>
    public enum UseAnimation
    {
        None,
        Eat,
        Drink,
        Block,
        Bow,
        Spear,
        Crossbow,
        Spyglass
    }

}

