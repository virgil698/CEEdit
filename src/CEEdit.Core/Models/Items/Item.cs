using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CEEdit.Core.Models.Common;

namespace CEEdit.Core.Models.Items
{
    /// <summary>
    /// 物品模型
    /// </summary>
    public partial class Item : ObservableObject
    {
        /// <summary>
        /// 物品ID（唯一标识符）
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// 物品描述（Lore）
        /// </summary>
        public List<string> Lore { get; set; } = new();

        /// <summary>
        /// 最大堆叠数量
        /// </summary>
        public int MaxStackSize { get; set; } = 64;

        /// <summary>
        /// 耐久度
        /// </summary>
        public int Durability { get; set; } = 0;

        /// <summary>
        /// 稀有度
        /// </summary>
        public Rarity Rarity { get; set; } = Rarity.Common;

        /// <summary>
        /// 物品类型
        /// </summary>
        public ItemType Type { get; set; } = ItemType.Miscellaneous;

        /// <summary>
        /// 纹理路径
        /// </summary>
        public string Texture { get; set; } = string.Empty;

        /// <summary>
        /// 自定义模型数据
        /// </summary>
        public int CustomModelData { get; set; } = 0;

        /// <summary>
        /// 物品行为
        /// </summary>
        public ItemBehavior Behavior { get; set; } = new();

        /// <summary>
        /// 物品属性
        /// </summary>
        public ItemProperties Properties { get; set; } = new();

        /// <summary>
        /// 附魔列表
        /// </summary>
        public List<Enchantment> Enchantments { get; set; } = new();

        /// <summary>
        /// 属性修改器
        /// </summary>
        public List<AttributeModifier> AttributeModifiers { get; set; } = new();

        /// <summary>
        /// 自定义NBT数据
        /// </summary>
        public Dictionary<string, object> CustomNBT { get; set; } = new();

        /// <summary>
        /// 标签列表
        /// </summary>
        public List<string> Tags { get; set; } = new();

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
    /// 附魔配置
    /// </summary>
    public class Enchantment
    {
        /// <summary>
        /// 附魔类型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 附魔等级
        /// </summary>
        public int Level { get; set; } = 1;
    }

    /// <summary>
    /// 属性修改器
    /// </summary>
    public class AttributeModifier
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Attribute { get; set; } = string.Empty;

        /// <summary>
        /// 修改器名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 修改值
        /// </summary>
        public double Amount { get; set; } = 0.0;

        /// <summary>
        /// 操作类型
        /// </summary>
        public AttributeOperation Operation { get; set; } = AttributeOperation.Addition;

        /// <summary>
        /// 应用部位
        /// </summary>
        public EquipmentSlot Slot { get; set; } = EquipmentSlot.MainHand;

    }

    /// <summary>
    /// 属性操作类型
    /// </summary>
    public enum AttributeOperation
    {
        Addition,
        MultiplyBase,
        MultiplyTotal
    }

    /// <summary>
    /// 装备部位
    /// </summary>
    public enum EquipmentSlot
    {
        MainHand,
        OffHand,
        Head,
        Chest,
        Legs,
        Feet
    }
}

