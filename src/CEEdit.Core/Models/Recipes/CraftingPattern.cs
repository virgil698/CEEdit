using System.Collections.Generic;
using System.Text;

namespace CEEdit.Core.Models.Recipes
{
    /// <summary>
    /// 合成模式（用于有序配方）
    /// </summary>
    public class CraftingPattern
    {
        /// <summary>
        /// 模式宽度
        /// </summary>
        public int Width { get; set; } = 3;

        /// <summary>
        /// 模式高度
        /// </summary>
        public int Height { get; set; } = 3;

        /// <summary>
        /// 模式数据（字符表示）
        /// </summary>
        public List<string> Pattern { get; set; } = new();

        /// <summary>
        /// 字符到原料的映射
        /// </summary>
        public Dictionary<char, Ingredient> Key { get; set; } = new();

        /// <summary>
        /// 是否为有序配方（形状敏感）
        /// </summary>
        public bool IsShaped { get; set; } = true;

        /// <summary>
        /// 是否允许镜像
        /// </summary>
        public bool AllowMirror { get; set; } = true;

        /// <summary>
        /// 是否允许旋转
        /// </summary>
        public bool AllowRotation { get; set; } = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CraftingPattern()
        {
            InitializeEmptyPattern();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public CraftingPattern(int width, int height)
        {
            Width = Math.Max(1, Math.Min(width, 3));
            Height = Math.Max(1, Math.Min(height, 3));
            InitializeEmptyPattern();
        }

        /// <summary>
        /// 初始化空模式
        /// </summary>
        private void InitializeEmptyPattern()
        {
            Pattern.Clear();
            for (int i = 0; i < Height; i++)
            {
                Pattern.Add(new string(' ', Width));
            }
        }

        /// <summary>
        /// 设置模式的某个位置
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="key">字符键</param>
        public void SetPattern(int x, int y, char key)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return;

            var row = Pattern[y].ToCharArray();
            row[x] = key;
            Pattern[y] = new string(row);
        }

        /// <summary>
        /// 获取模式的某个位置
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns>字符键</returns>
        public char GetPattern(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return ' ';

            return Pattern[y][x];
        }

        /// <summary>
        /// 设置原料映射
        /// </summary>
        /// <param name="key">字符键</param>
        /// <param name="ingredient">原料</param>
        public void SetKey(char key, Ingredient ingredient)
        {
            if (key == ' ')
                return;

            Key[key] = ingredient;
        }

        /// <summary>
        /// 获取原料映射
        /// </summary>
        /// <param name="key">字符键</param>
        /// <returns>原料</returns>
        public Ingredient? GetKey(char key)
        {
            return Key.TryGetValue(key, out var ingredient) ? ingredient : null;
        }

        /// <summary>
        /// 从字符串数组设置模式
        /// </summary>
        /// <param name="pattern">模式字符串数组</param>
        public void SetPatternFromStrings(params string[] pattern)
        {
            if (pattern == null || pattern.Length == 0)
                return;

            Height = Math.Min(pattern.Length, 3);
            Width = Math.Min(pattern[0].Length, 3);
            
            Pattern.Clear();
            for (int i = 0; i < Height; i++)
            {
                var row = pattern[i];
                if (row.Length > Width)
                    row = row.Substring(0, Width);
                else if (row.Length < Width)
                    row = row.PadRight(Width);
                
                Pattern.Add(row);
            }

            // 补齐行数
            while (Pattern.Count < 3)
            {
                Pattern.Add(new string(' ', Width));
            }
        }

        /// <summary>
        /// 验证模式是否有效
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            // 检查是否有至少一个非空格字符
            bool hasIngredient = false;
            foreach (var row in Pattern)
            {
                foreach (var c in row)
                {
                    if (c != ' ')
                    {
                        hasIngredient = true;
                        // 检查字符是否有对应的原料映射
                        if (!Key.ContainsKey(c))
                            return false;
                    }
                }
            }

            return hasIngredient;
        }

        /// <summary>
        /// 获取所有使用的原料
        /// </summary>
        /// <returns>原料列表</returns>
        public List<Ingredient> GetUsedIngredients()
        {
            var usedKeys = new HashSet<char>();
            foreach (var row in Pattern)
            {
                foreach (var c in row)
                {
                    if (c != ' ')
                        usedKeys.Add(c);
                }
            }

            var ingredients = new List<Ingredient>();
            foreach (var key in usedKeys)
            {
                if (Key.TryGetValue(key, out var ingredient))
                    ingredients.Add(ingredient);
            }

            return ingredients;
        }

        /// <summary>
        /// 获取紧凑的模式（移除空行和空列）
        /// </summary>
        /// <returns>紧凑的模式</returns>
        public CraftingPattern GetCompactPattern()
        {
            var compactPattern = new CraftingPattern();
            var nonEmptyRows = new List<string>();

            // 找到非空行
            foreach (var row in Pattern)
            {
                if (row.Trim().Length > 0)
                    nonEmptyRows.Add(row);
            }

            if (nonEmptyRows.Count == 0)
                return compactPattern;

            // 找到非空列的范围
            int minCol = int.MaxValue;
            int maxCol = -1;

            foreach (var row in nonEmptyRows)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i] != ' ')
                    {
                        minCol = Math.Min(minCol, i);
                        maxCol = Math.Max(maxCol, i);
                    }
                }
            }

            if (minCol > maxCol)
                return compactPattern;

            // 构建紧凑模式
            var compactRows = new List<string>();
            foreach (var row in nonEmptyRows)
            {
                var compactRow = row.Substring(minCol, maxCol - minCol + 1);
                compactRows.Add(compactRow);
            }

            compactPattern.Width = maxCol - minCol + 1;
            compactPattern.Height = compactRows.Count;
            compactPattern.Pattern = compactRows;
            compactPattern.Key = new Dictionary<char, Ingredient>(Key);
            compactPattern.AllowMirror = AllowMirror;
            compactPattern.AllowRotation = AllowRotation;

            return compactPattern;
        }

        /// <summary>
        /// 克隆模式
        /// </summary>
        /// <returns>克隆的模式</returns>
        public CraftingPattern Clone()
        {
            var clone = new CraftingPattern(Width, Height)
            {
                Pattern = new List<string>(Pattern),
                Key = new Dictionary<char, Ingredient>(),
                AllowMirror = AllowMirror,
                AllowRotation = AllowRotation
            };

            foreach (var kvp in Key)
            {
                clone.Key[kvp.Key] = kvp.Value.Clone();
            }

            return clone;
        }

        /// <summary>
        /// 获取模式的字符串表示
        /// </summary>
        /// <returns>字符串表示</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Pattern ({Width}x{Height}):");
            
            foreach (var row in Pattern)
            {
                sb.AppendLine($"  '{row}'");
            }

            if (Key.Count > 0)
            {
                sb.AppendLine("Key mappings:");
                foreach (var kvp in Key)
                {
                    sb.AppendLine($"  '{kvp.Key}' = {kvp.Value.GetDisplayName()}");
                }
            }

            return sb.ToString();
        }
    }
}

