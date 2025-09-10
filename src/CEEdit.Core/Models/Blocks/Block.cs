using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CEEdit.Core.Models.Common;

namespace CEEdit.Core.Models.Blocks
{
    /// <summary>
    /// 方块模型
    /// </summary>
    public partial class Block : ObservableObject
    {
        /// <summary>
        /// 方块ID（唯一标识符）
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// 方块描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 硬度（挖掘时间相关）
        /// </summary>
        public float Hardness { get; set; } = 1.0f;

        /// <summary>
        /// 爆炸抗性
        /// </summary>
        public float BlastResistance { get; set; } = 1.0f;

        /// <summary>
        /// 材质类型
        /// </summary>
        public BlockMaterial Material { get; set; } = BlockMaterial.Stone;

        /// <summary>
        /// 挖掘工具要求
        /// </summary>
        public ToolType RequiredTool { get; set; } = ToolType.None;

        /// <summary>
        /// 挖掘等级要求
        /// </summary>
        public int RequiredToolLevel { get; set; } = 0;

        /// <summary>
        /// 光照等级（0-15）
        /// </summary>
        public int LightLevel { get; set; } = 0;

        /// <summary>
        /// 是否透明
        /// </summary>
        public bool IsTransparent { get; set; } = false;

        /// <summary>
        /// 是否为完整方块
        /// </summary>
        public bool IsFullBlock { get; set; } = true;

        /// <summary>
        /// 纹理配置
        /// </summary>
        public BlockTexture Textures { get; set; } = new();

        /// <summary>
        /// 方块行为
        /// </summary>
        public BlockBehavior Behavior { get; set; } = new();

        /// <summary>
        /// 方块属性
        /// </summary>
        public BlockProperties Properties { get; set; } = new();

        /// <summary>
        /// 掉落物配置
        /// </summary>
        public List<BlockDrop> Drops { get; set; } = new();

        /// <summary>
        /// 自定义属性
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; } = new();

        /// <summary>
        /// 模型路径
        /// </summary>
        public string ModelPath { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 方块掉落物
    /// </summary>
    public class BlockDrop
    {
        /// <summary>
        /// 掉落物品ID
        /// </summary>
        public string ItemId { get; set; } = string.Empty;

        /// <summary>
        /// 掉落数量
        /// </summary>
        public int Count { get; set; } = 1;

        /// <summary>
        /// 掉落几率（0.0-1.0）
        /// </summary>
        public float Chance { get; set; } = 1.0f;

        /// <summary>
        /// 所需工具
        /// </summary>
        public ToolType RequiredTool { get; set; } = ToolType.None;

        /// <summary>
        /// 所需工具等级
        /// </summary>
        public int RequiredToolLevel { get; set; } = 0;

        /// <summary>
        /// 是否受时运影响
        /// </summary>
        public bool AffectedByFortune { get; set; } = false;

    }
}

