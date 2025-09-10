using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CEEdit.Core.Models.Items;
using CEEdit.Core.Models.Common;

namespace CEEdit.Core.Models.Recipes
{
    /// <summary>
    /// 配方模型
    /// </summary>
    public partial class Recipe : ObservableObject
    {
        /// <summary>
        /// 配方ID（唯一标识符）
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 配方名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 配方描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 配方类型
        /// </summary>
        public RecipeType Type { get; set; } = RecipeType.Crafting;

        /// <summary>
        /// 配方分组
        /// </summary>
        public string Group { get; set; } = string.Empty;

        /// <summary>
        /// 配方分类
        /// </summary>
        public RecipeCategory Category { get; set; } = RecipeCategory.Misc;

        /// <summary>
        /// 原料列表
        /// </summary>
        public List<Ingredient> Ingredients { get; set; } = new();

        /// <summary>
        /// 合成结果
        /// </summary>
        public ItemStack Result { get; set; } = new();

        /// <summary>
        /// 是否为有序配方
        /// </summary>
        public bool IsShaped { get; set; } = false;

        /// <summary>
        /// 配方形状（对于有序配方）
        /// </summary>
        public CraftingPattern Pattern { get; set; } = new();

        /// <summary>
        /// 解锁条件
        /// </summary>
        public List<UnlockCondition> UnlockConditions { get; set; } = new();

        /// <summary>
        /// 经验值奖励
        /// </summary>
        public float Experience { get; set; } = 0.0f;

        /// <summary>
        /// 烹饪时间（对于熔炼等配方）
        /// </summary>
        public int CookingTime { get; set; } = 200;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 配方标签
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
        /// 验证配方是否有效
        /// </summary>
        /// <returns>验证结果</returns>
        public RecipeValidationResult Validate()
        {
            var result = new RecipeValidationResult();

            // 验证基本信息
            if (string.IsNullOrEmpty(Id))
                result.Errors.Add("配方ID不能为空");

            if (string.IsNullOrEmpty(Name))
                result.Errors.Add("配方名称不能为空");

            // 验证原料
            if (Ingredients == null || Ingredients.Count == 0)
                result.Errors.Add("配方必须包含至少一个原料");

            // 验证结果
            if (Result == null || Result.IsEmpty)
                result.Errors.Add("配方必须有有效的合成结果");

            // 验证有序配方的模式
            if (IsShaped && (Pattern == null || !Pattern.IsValid()))
                result.Errors.Add("有序配方必须有有效的合成模式");

            // 验证特定类型的配方
            ValidateSpecificType(result);

            result.IsValid = result.Errors.Count == 0;
            return result;
        }

        /// <summary>
        /// 验证特定类型的配方
        /// </summary>
        /// <param name="result">验证结果</param>
        private void ValidateSpecificType(RecipeValidationResult result)
        {
            switch (Type)
            {
                case RecipeType.Smelting:
                case RecipeType.Blasting:
                case RecipeType.Smoking:
                case RecipeType.CampfireCooking:
                    if (Ingredients.Count != 1)
                        result.Errors.Add($"{Type}配方必须只有一个原料");
                    if (CookingTime <= 0)
                        result.Warnings.Add("烹饪时间应该大于0");
                    break;

                case RecipeType.Stonecutting:
                    if (Ingredients.Count != 1)
                        result.Errors.Add("切石配方必须只有一个原料");
                    break;

                case RecipeType.SmithingTable:
                    if (Ingredients.Count < 2)
                        result.Errors.Add("锻造台配方至少需要两个原料");
                    break;
            }
        }
    }

    /// <summary>
    /// 配方验证结果
    /// </summary>
    public class RecipeValidationResult
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; } = false;

        /// <summary>
        /// 错误信息列表
        /// </summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>
        /// 警告信息列表
        /// </summary>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// 建议信息列表
        /// </summary>
        public List<string> Suggestions { get; set; } = new();

    }

    /// <summary>
    /// 解锁条件
    /// </summary>
    public class UnlockCondition
    {
        /// <summary>
        /// 条件类型
        /// </summary>
        public UnlockConditionType Type { get; set; } = UnlockConditionType.HasItem;

        /// <summary>
        /// 条件参数
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();

        /// <summary>
        /// 条件描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// 解锁条件类型枚举
    /// </summary>
    public enum UnlockConditionType
    {
        HasItem,
        HasAdvancement,
        PlayerLevel,
        Custom
    }

    /// <summary>
    /// 配方分类枚举
    /// </summary>
    public enum RecipeCategory
    {
        Building,
        Decoration,
        Redstone,
        Transportation,
        Misc,
        Food,
        Tools,
        Combat,
        Brewing
    }
}

