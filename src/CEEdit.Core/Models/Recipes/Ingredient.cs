using System.Collections.Generic;
using CEEdit.Core.Models.Items;

namespace CEEdit.Core.Models.Recipes
{
    /// <summary>
    /// 配方原料模型
    /// </summary>
    public class Ingredient
    {
        /// <summary>
        /// 原料类型
        /// </summary>
        public IngredientType Type { get; set; } = IngredientType.Item;

        /// <summary>
        /// 物品ID（当类型为Item时）
        /// </summary>
        public string ItemId { get; set; } = string.Empty;

        /// <summary>
        /// 物品标签（当类型为Tag时）
        /// </summary>
        public string Tag { get; set; } = string.Empty;

        /// <summary>
        /// 可接受的物品列表（当类型为ItemList时）
        /// </summary>
        public List<string> AcceptedItems { get; set; } = new();

        /// <summary>
        /// 配方模式中的字符键
        /// </summary>
        public char Key { get; set; } = ' ';

        /// <summary>
        /// 物品实例（简化访问）
        /// </summary>
        public string Item { get; set; } = string.Empty;

        /// <summary>
        /// 所需数量
        /// </summary>
        public int Count { get; set; } = 1;

        /// <summary>
        /// 是否消耗耐久度而不是数量
        /// </summary>
        public bool ConsumeDurability { get; set; } = false;

        /// <summary>
        /// 消耗的耐久度值
        /// </summary>
        public int DurabilityConsumption { get; set; } = 1;

        /// <summary>
        /// NBT匹配要求
        /// </summary>
        public Dictionary<string, object> RequiredNBT { get; set; } = new();

        /// <summary>
        /// 是否保留NBT数据到结果
        /// </summary>
        public bool PreserveNBT { get; set; } = false;

        /// <summary>
        /// 在合成网格中的位置（用于有序配方）
        /// </summary>
        public GridPosition Position { get; set; } = new();

        /// <summary>
        /// 替代原料（可选）
        /// </summary>
        public List<Ingredient> Alternatives { get; set; } = new();

        /// <summary>
        /// 检查物品是否匹配此原料
        /// </summary>
        /// <param name="itemStack">要检查的物品堆栈</param>
        /// <returns>是否匹配</returns>
        public bool Matches(ItemStack itemStack)
        {
            if (itemStack == null || itemStack.IsEmpty)
                return false;

            if (itemStack.Count < Count)
                return false;

            switch (Type)
            {
                case IngredientType.Item:
                    if (itemStack.ItemId != ItemId)
                        return false;
                    break;

                case IngredientType.Tag:
                    if (!itemStack.Tags.Contains(Tag))
                        return false;
                    break;

                case IngredientType.ItemList:
                    if (!AcceptedItems.Contains(itemStack.ItemId))
                        return false;
                    break;

                case IngredientType.Any:
                    // 任何物品都匹配
                    break;

                default:
                    return false;
            }

            // 检查NBT要求
            if (RequiredNBT.Count > 0)
            {
                foreach (var requirement in RequiredNBT)
                {
                    if (!itemStack.NBTData.TryGetValue(requirement.Key, out var value) ||
                        !value.Equals(requirement.Value))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 检查替代原料是否匹配
        /// </summary>
        /// <param name="itemStack">要检查的物品堆栈</param>
        /// <returns>匹配的原料（包括自身）</returns>
        public Ingredient? FindMatchingIngredient(ItemStack itemStack)
        {
            if (Matches(itemStack))
                return this;

            foreach (var alternative in Alternatives)
            {
                if (alternative.Matches(itemStack))
                    return alternative;
            }

            return null;
        }

        /// <summary>
        /// 获取显示名称
        /// </summary>
        /// <returns>显示名称</returns>
        public string GetDisplayName()
        {
            return Type switch
            {
                IngredientType.Item => ItemId,
                IngredientType.Tag => $"#{Tag}",
                IngredientType.ItemList => string.Join("|", AcceptedItems),
                IngredientType.Any => "Any",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// 克隆原料
        /// </summary>
        /// <returns>克隆的原料</returns>
        public Ingredient Clone()
        {
            return new Ingredient
            {
                Type = Type,
                ItemId = ItemId,
                Tag = Tag,
                AcceptedItems = new List<string>(AcceptedItems),
                Count = Count,
                ConsumeDurability = ConsumeDurability,
                DurabilityConsumption = DurabilityConsumption,
                RequiredNBT = new Dictionary<string, object>(RequiredNBT),
                PreserveNBT = PreserveNBT,
                Position = Position.Clone(),
                Alternatives = Alternatives.ConvertAll(alt => alt.Clone())
            };
        }
    }

    /// <summary>
    /// 原料类型枚举
    /// </summary>
    public enum IngredientType
    {
        /// <summary>
        /// 特定物品
        /// </summary>
        Item,

        /// <summary>
        /// 物品标签
        /// </summary>
        Tag,

        /// <summary>
        /// 物品列表（任选其一）
        /// </summary>
        ItemList,

        /// <summary>
        /// 任何物品
        /// </summary>
        Any
    }

    /// <summary>
    /// 网格位置
    /// </summary>
    public class GridPosition
    {
        /// <summary>
        /// X坐标（列）
        /// </summary>
        public int X { get; set; } = 0;

        /// <summary>
        /// Y坐标（行）
        /// </summary>
        public int Y { get; set; } = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        public GridPosition() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        public GridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// 克隆位置
        /// </summary>
        /// <returns>克隆的位置</returns>
        public GridPosition Clone()
        {
            return new GridPosition(X, Y);
        }

        /// <summary>
        /// 重写Equals方法
        /// </summary>
        public override bool Equals(object? obj)
        {
            return obj is GridPosition other && X == other.X && Y == other.Y;
        }

        /// <summary>
        /// 重写GetHashCode方法
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}

