using System.Collections.Generic;

namespace CEEdit.Core.Models.Items
{
    /// <summary>
    /// 物品堆栈模型
    /// </summary>
    public class ItemStack
    {
        /// <summary>
        /// 物品ID
        /// </summary>
        public string ItemId { get; set; } = string.Empty;

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; } = 1;

        /// <summary>
        /// 当前耐久度
        /// </summary>
        public int CurrentDurability { get; set; } = 0;

        /// <summary>
        /// 自定义显示名称
        /// </summary>
        public string CustomDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// 自定义Lore
        /// </summary>
        public List<string> CustomLore { get; set; } = new();

        /// <summary>
        /// 附魔列表
        /// </summary>
        public List<Enchantment> Enchantments { get; set; } = new();

        /// <summary>
        /// NBT数据
        /// </summary>
        public Dictionary<string, object> NBTData { get; set; } = new();

        /// <summary>
        /// 物品标签
        /// </summary>
        public List<string> Tags { get; set; } = new();

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsEmpty => Count <= 0 || string.IsNullOrEmpty(ItemId);

        /// <summary>
        /// 获取总价值（用于排序和比较）
        /// </summary>
        public int TotalValue => Count * GetItemValue();

        /// <summary>
        /// 是否可以与另一个物品堆栈合并
        /// </summary>
        /// <param name="other">另一个物品堆栈</param>
        /// <returns>是否可合并</returns>
        public bool CanStackWith(ItemStack other)
        {
            if (other == null || IsEmpty || other.IsEmpty)
                return false;

            return ItemId == other.ItemId &&
                   CurrentDurability == other.CurrentDurability &&
                   CustomDisplayName == other.CustomDisplayName &&
                   AreListsEqual(CustomLore, other.CustomLore) &&
                   AreListsEqual(Enchantments, other.Enchantments) &&
                   AreDictionariesEqual(NBTData, other.NBTData);
        }

        /// <summary>
        /// 尝试添加数量
        /// </summary>
        /// <param name="amount">要添加的数量</param>
        /// <param name="maxStackSize">最大堆叠数量</param>
        /// <returns>实际添加的数量</returns>
        public int TryAddAmount(int amount, int maxStackSize = 64)
        {
            if (IsEmpty)
                return 0;

            var canAdd = Math.Min(amount, maxStackSize - Count);
            Count += canAdd;
            return canAdd;
        }

        /// <summary>
        /// 尝试移除数量
        /// </summary>
        /// <param name="amount">要移除的数量</param>
        /// <returns>实际移除的数量</returns>
        public int TryRemoveAmount(int amount)
        {
            var canRemove = Math.Min(amount, Count);
            Count -= canRemove;
            return canRemove;
        }

        /// <summary>
        /// 分割物品堆栈
        /// </summary>
        /// <param name="amount">要分割的数量</param>
        /// <returns>新的物品堆栈</returns>
        public ItemStack Split(int amount)
        {
            if (amount >= Count)
            {
                var result = Clone();
                Clear();
                return result;
            }

            var splitStack = Clone();
            splitStack.Count = amount;
            Count -= amount;
            return splitStack;
        }

        /// <summary>
        /// 清空物品堆栈
        /// </summary>
        public void Clear()
        {
            ItemId = string.Empty;
            Count = 0;
            CurrentDurability = 0;
            CustomDisplayName = string.Empty;
            CustomLore.Clear();
            Enchantments.Clear();
            NBTData.Clear();
            Tags.Clear();
        }

        /// <summary>
        /// 克隆物品堆栈
        /// </summary>
        /// <returns>克隆的物品堆栈</returns>
        public ItemStack Clone()
        {
            return new ItemStack
            {
                ItemId = ItemId,
                Count = Count,
                CurrentDurability = CurrentDurability,
                CustomDisplayName = CustomDisplayName,
                CustomLore = new List<string>(CustomLore),
                Enchantments = new List<Enchantment>(Enchantments),
                NBTData = new Dictionary<string, object>(NBTData),
                Tags = new List<string>(Tags)
            };
        }

        /// <summary>
        /// 获取物品单价（用于价值计算）
        /// </summary>
        /// <returns>物品单价</returns>
        private int GetItemValue()
        {
            // 这里可以根据物品稀有度、附魔等计算价值
            // 暂时返回固定值1
            return 1;
        }

        /// <summary>
        /// 比较两个列表是否相等
        /// </summary>
        private static bool AreListsEqual<T>(List<T> list1, List<T> list2)
        {
            if (list1.Count != list2.Count)
                return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(list1[i], list2[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 比较两个字典是否相等
        /// </summary>
        private static bool AreDictionariesEqual<TKey, TValue>(Dictionary<TKey, TValue> dict1, Dictionary<TKey, TValue> dict2) where TKey : notnull
        {
            if (dict1.Count != dict2.Count)
                return false;

            foreach (var kvp in dict1)
            {
                if (!dict2.TryGetValue(kvp.Key, out var value) || 
                    !EqualityComparer<TValue>.Default.Equals(kvp.Value, value))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        public override string ToString()
        {
            if (IsEmpty)
                return "Empty";

            var displayName = !string.IsNullOrEmpty(CustomDisplayName) ? CustomDisplayName : ItemId;
            return $"{displayName} x{Count}";
        }

        /// <summary>
        /// 重写Equals方法
        /// </summary>
        public override bool Equals(object? obj)
        {
            return obj is ItemStack other && CanStackWith(other) && Count == other.Count;
        }

        /// <summary>
        /// 重写GetHashCode方法
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(ItemId, Count, CurrentDurability, CustomDisplayName);
        }
    }
}

