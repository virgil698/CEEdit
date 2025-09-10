using System.Collections.Generic;

namespace CEEdit.Core.Models.Blocks
{
    /// <summary>
    /// 方块行为配置
    /// </summary>
    public class BlockBehavior
    {
        /// <summary>
        /// 是否可以点击
        /// </summary>
        public bool IsClickable { get; set; } = false;

        /// <summary>
        /// 右键交互行为
        /// </summary>
        public InteractionBehavior RightClickBehavior { get; set; } = new();

        /// <summary>
        /// 左键交互行为
        /// </summary>
        public InteractionBehavior LeftClickBehavior { get; set; } = new();

        /// <summary>
        /// 红石信号配置
        /// </summary>
        public RedstoneBehavior Redstone { get; set; } = new();

        /// <summary>
        /// 流体交互配置
        /// </summary>
        public FluidBehavior Fluid { get; set; } = new();

        /// <summary>
        /// 方块状态变化规则
        /// </summary>
        public List<StateChangeRule> StateChangeRules { get; set; } = new();

        /// <summary>
        /// 粒子效果配置
        /// </summary>
        public List<ParticleEffect> ParticleEffects { get; set; } = new();

        /// <summary>
        /// 音效配置
        /// </summary>
        public SoundEffects Sounds { get; set; } = new();

        /// <summary>
        /// 自定义事件处理器
        /// </summary>
        public Dictionary<string, string> EventHandlers { get; set; } = new();
    }

    /// <summary>
    /// 交互行为配置
    /// </summary>
    public class InteractionBehavior
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// 执行的命令
        /// </summary>
        public string Command { get; set; } = string.Empty;

        /// <summary>
        /// 脚本路径
        /// </summary>
        public string ScriptPath { get; set; } = string.Empty;

        /// <summary>
        /// 冷却时间（毫秒）
        /// </summary>
        public int Cooldown { get; set; } = 0;

        /// <summary>
        /// 所需权限
        /// </summary>
        public string RequiredPermission { get; set; } = string.Empty;
    }

    /// <summary>
    /// 红石行为配置
    /// </summary>
    public class RedstoneBehavior
    {
        /// <summary>
        /// 是否能输出红石信号
        /// </summary>
        public bool CanOutputSignal { get; set; } = false;

        /// <summary>
        /// 是否能接收红石信号
        /// </summary>
        public bool CanReceiveSignal { get; set; } = false;

        /// <summary>
        /// 输出信号强度
        /// </summary>
        public int OutputStrength { get; set; } = 0;

        /// <summary>
        /// 信号变化时的行为
        /// </summary>
        public string OnSignalChangeScript { get; set; } = string.Empty;
    }

    /// <summary>
    /// 流体行为配置
    /// </summary>
    public class FluidBehavior
    {
        /// <summary>
        /// 是否能被流体冲毁
        /// </summary>
        public bool CanBeDestroyed { get; set; } = true;

        /// <summary>
        /// 是否能阻挡流体
        /// </summary>
        public bool BlocksFluid { get; set; } = true;

        /// <summary>
        /// 流体交互脚本
        /// </summary>
        public string FluidInteractionScript { get; set; } = string.Empty;
    }

    /// <summary>
    /// 状态变化规则
    /// </summary>
    public class StateChangeRule
    {
        /// <summary>
        /// 规则名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 触发条件
        /// </summary>
        public string Condition { get; set; } = string.Empty;

        /// <summary>
        /// 目标状态
        /// </summary>
        public string TargetState { get; set; } = string.Empty;

        /// <summary>
        /// 延迟时间（毫秒）
        /// </summary>
        public int Delay { get; set; } = 0;

        /// <summary>
        /// 变化几率
        /// </summary>
        public float Chance { get; set; } = 1.0f;
    }

    /// <summary>
    /// 粒子效果配置
    /// </summary>
    public class ParticleEffect
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
    }

    /// <summary>
    /// 音效配置
    /// </summary>
    public class SoundEffects
    {
        /// <summary>
        /// 破坏音效
        /// </summary>
        public string BreakSound { get; set; } = string.Empty;

        /// <summary>
        /// 放置音效
        /// </summary>
        public string PlaceSound { get; set; } = string.Empty;

        /// <summary>
        /// 踩踏音效
        /// </summary>
        public string StepSound { get; set; } = string.Empty;

        /// <summary>
        /// 点击音效
        /// </summary>
        public string HitSound { get; set; } = string.Empty;

        /// <summary>
        /// 自定义音效
        /// </summary>
        public Dictionary<string, string> CustomSounds { get; set; } = new();
    }

    /// <summary>
    /// 3D向量
    /// </summary>
    public class Vector3
    {
        public float X { get; set; } = 0.0f;
        public float Y { get; set; } = 0.0f;
        public float Z { get; set; } = 0.0f;

        public Vector3() { }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}

