using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CEEdit.Core.Models.Recipes;
using CEEdit.Core.Models.Common;
using CEEdit.Core.Services.Interfaces;

namespace CEEdit.UI.ViewModels
{
    public partial class RecipeEditorViewModel : ObservableObject
    {
        private readonly IFileService? _fileService;
        private readonly IValidationService? _validationService;

        [ObservableProperty]
        private Recipe _recipe = new();

        [ObservableProperty]
        private bool _isDirty = false;

        [ObservableProperty]
        private bool _isCraftingTableRecipe = true;

        [ObservableProperty]
        private bool _isSmeltingRecipe = false;

        [ObservableProperty]
        private bool _isStonecuttingRecipe = false;

        // 工作台合成网格 (3x3)
        [ObservableProperty]
        private ObservableCollection<string> _craftingGrid = new(new string[9]);

        // 熔炉配方属性
        [ObservableProperty]
        private string _smeltingInput = string.Empty;

        [ObservableProperty]
        private int _smeltingTime = 200;

        [ObservableProperty]
        private float _smeltingExperience = 0.1f;

        // 切石机配方属性
        [ObservableProperty]
        private string _stonecuttingInput = string.Empty;

        [ObservableProperty]
        private int _stonecuttingInputCount = 1;

        public ObservableCollection<RecipeType> AvailableRecipeTypes { get; } = new()
        {
            RecipeType.Crafting,
            RecipeType.Smelting,
            RecipeType.Blasting,
            RecipeType.Smoking,
            RecipeType.Stonecutting,
            RecipeType.Smithing
        };

        public RecipeEditorViewModel(IFileService? fileService = null, 
                                    IValidationService? validationService = null)
        {
            _fileService = fileService;
            _validationService = validationService;
            
            // 监听Recipe属性变化
            Recipe.PropertyChanged += (s, e) => IsDirty = true;
            
            // 监听配方类型变化
            PropertyChanged += OnPropertyChanged;
            
            // 初始化网格
            InitializeCraftingGrid();
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IsCraftingTableRecipe):
                    if (IsCraftingTableRecipe)
                    {
                        IsSmeltingRecipe = false;
                        IsStonecuttingRecipe = false;
                        Recipe.Type = RecipeType.Crafting;
                    }
                    break;
                case nameof(IsSmeltingRecipe):
                    if (IsSmeltingRecipe)
                    {
                        IsCraftingTableRecipe = false;
                        IsStonecuttingRecipe = false;
                        Recipe.Type = RecipeType.Smelting;
                    }
                    break;
                case nameof(IsStonecuttingRecipe):
                    if (IsStonecuttingRecipe)
                    {
                        IsCraftingTableRecipe = false;
                        IsSmeltingRecipe = false;
                        Recipe.Type = RecipeType.Stonecutting;
                    }
                    break;
            }
        }

        private void InitializeCraftingGrid()
        {
            for (int i = 0; i < 9; i++)
            {
                CraftingGrid[i] = string.Empty;
            }
            
            // 监听网格变化
            for (int i = 0; i < CraftingGrid.Count; i++)
            {
                int index = i; // 避免闭包问题
                CraftingGrid.CollectionChanged += (s, e) => 
                {
                    UpdateRecipeFromGrid();
                    IsDirty = true;
                };
            }
        }

        private void UpdateRecipeFromGrid()
        {
            if (Recipe.Pattern == null)
                Recipe.Pattern = new CraftingPattern();

            // 更新配方模式
            Recipe.Pattern.Pattern.Clear();
            Recipe.Ingredients.Clear();

            if (Recipe.Pattern.IsShaped)
            {
                // 有序配方 - 保持3x3格式
                for (int row = 0; row < 3; row++)
                {
                    string patternRow = "";
                    for (int col = 0; col < 3; col++)
                    {
                        int index = row * 3 + col;
                        string item = CraftingGrid[index];
                        
                        if (!string.IsNullOrEmpty(item))
                        {
                            char key = (char)('A' + Recipe.Ingredients.Count);
                            patternRow += key;
                            
                            Recipe.Ingredients.Add(new Ingredient
                            {
                                Key = key,
                                Item = item,
                                Count = 1
                            });
                        }
                        else
                        {
                            patternRow += " ";
                        }
                    }
                    Recipe.Pattern.Pattern.Add(patternRow);
                }
            }
            else
            {
                // 无序配方 - 只收集不同的物品
                var uniqueItems = CraftingGrid.Where(item => !string.IsNullOrEmpty(item))
                                             .GroupBy(item => item)
                                             .ToList();

                foreach (var group in uniqueItems)
                {
                    Recipe.Ingredients.Add(new Ingredient
                    {
                        Key = (char)('A' + Recipe.Ingredients.Count),
                        Item = group.Key,
                        Count = group.Count()
                    });
                }
            }
        }

        [RelayCommand]
        private void ClearGrid()
        {
            for (int i = 0; i < CraftingGrid.Count; i++)
            {
                CraftingGrid[i] = string.Empty;
            }
            
            Recipe.Ingredients.Clear();
            if (Recipe.Pattern != null)
            {
                Recipe.Pattern.Pattern.Clear();
            }
            
            IsDirty = true;
        }

        [RelayCommand]
        private async Task LoadTemplate()
        {
            await Task.CompletedTask;
            try
            {
                // TODO: 实现从模板加载配方
                MessageBox.Show("模板加载功能正在开发中...", 
                              "信息", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载模板失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task SaveTemplate()
        {
            await Task.CompletedTask;
            try
            {
                // TODO: 实现保存配方为模板
                MessageBox.Show("模板保存功能正在开发中...", 
                              "信息", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存模板失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void Reset()
        {
            var result = MessageBox.Show("确定要重置所有设置吗？这将清除所有未保存的更改。", 
                                       "确认重置", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                Recipe = new Recipe();
                IsCraftingTableRecipe = true;
                IsSmeltingRecipe = false;
                IsStonecuttingRecipe = false;
                
                ClearGrid();
                
                SmeltingInput = string.Empty;
                SmeltingTime = 200;
                SmeltingExperience = 0.1f;
                
                StonecuttingInput = string.Empty;
                StonecuttingInputCount = 1;
                
                IsDirty = false;
            }
        }

        [RelayCommand]
        private async Task TestRecipe()
        {
            await Task.CompletedTask;
            try
            {
                // 验证配方
                if (string.IsNullOrWhiteSpace(Recipe.Id))
                {
                    MessageBox.Show("配方ID不能为空。", 
                                  "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Recipe.Result.ItemId))
                {
                    MessageBox.Show("输出物品ID不能为空。", 
                                  "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Recipe.Result.Count <= 0)
                {
                    MessageBox.Show("输出物品数量必须大于0。", 
                                  "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 根据配方类型验证
                switch (Recipe.Type)
                {
                    case RecipeType.Crafting:
                        if (Recipe.Ingredients.Count == 0)
                        {
                            MessageBox.Show("合成配方至少需要一个材料。", 
                                          "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        break;
                    case RecipeType.Smelting:
                        if (string.IsNullOrWhiteSpace(SmeltingInput))
                        {
                            MessageBox.Show("熔炉配方需要指定输入材料。", 
                                          "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        break;
                    case RecipeType.Stonecutting:
                        if (string.IsNullOrWhiteSpace(StonecuttingInput))
                        {
                            MessageBox.Show("切石机配方需要指定输入材料。", 
                                          "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        break;
                }

                // TODO: 实际的配方测试逻辑
                MessageBox.Show("配方验证通过！", 
                              "测试成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"测试配方失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            await Task.CompletedTask;
            try
            {
                // 更新配方数据
                UpdateRecipeFromCurrentInputs();

                // 验证数据
                if (string.IsNullOrWhiteSpace(Recipe.Id))
                {
                    MessageBox.Show("配方ID不能为空。", 
                                  "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Recipe.Result.ItemId))
                {
                    MessageBox.Show("输出物品ID不能为空。", 
                                  "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // TODO: 保存配方数据
                // 这里可以调用ProjectService来保存配方
                
                IsDirty = false;
                
                MessageBox.Show("配方保存成功！", 
                              "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateRecipeFromCurrentInputs()
        {
            // 根据当前选择的配方类型更新Recipe对象
            if (IsSmeltingRecipe)
            {
                Recipe.Ingredients.Clear();
                Recipe.Ingredients.Add(new Ingredient
                {
                    Key = 'I',
                    Item = SmeltingInput,
                    Count = 1
                });
                
                // TODO: 设置烹饪时间和经验值
            }
            else if (IsStonecuttingRecipe)
            {
                Recipe.Ingredients.Clear();
                Recipe.Ingredients.Add(new Ingredient
                {
                    Key = 'S',
                    Item = StonecuttingInput,
                    Count = StonecuttingInputCount
                });
            }
            else if (IsCraftingTableRecipe)
            {
                UpdateRecipeFromGrid();
            }
        }

        /// <summary>
        /// 加载配方数据
        /// </summary>
        /// <param name="recipe">要编辑的配方</param>
        public void LoadRecipe(Recipe recipe)
        {
            Recipe = recipe ?? new Recipe();
            
            // 根据配方类型设置界面状态
            IsCraftingTableRecipe = Recipe.Type == RecipeType.Crafting;
            IsSmeltingRecipe = Recipe.Type == RecipeType.Smelting || 
                               Recipe.Type == RecipeType.Blasting || 
                               Recipe.Type == RecipeType.Smoking;
            IsStonecuttingRecipe = Recipe.Type == RecipeType.Stonecutting;
            
            // 加载配方数据到界面
            LoadRecipeToInterface();
            
            IsDirty = false;
        }

        private void LoadRecipeToInterface()
        {
            if (IsCraftingTableRecipe && Recipe.Pattern != null)
            {
                // 加载工作台配方到网格
                LoadCraftingPatternToGrid();
            }
            else if (IsSmeltingRecipe && Recipe.Ingredients.Count > 0)
            {
                // 加载熔炉配方
                SmeltingInput = Recipe.Ingredients.FirstOrDefault()?.Item ?? string.Empty;
                // TODO: 从Recipe中获取烹饪时间和经验值
            }
            else if (IsStonecuttingRecipe && Recipe.Ingredients.Count > 0)
            {
                // 加载切石机配方
                var ingredient = Recipe.Ingredients.FirstOrDefault();
                StonecuttingInput = ingredient?.Item ?? string.Empty;
                StonecuttingInputCount = ingredient?.Count ?? 1;
            }
        }

        private void LoadCraftingPatternToGrid()
        {
            ClearGrid();
            
            if (Recipe.Pattern?.Pattern != null && Recipe.Pattern.Pattern.Count > 0)
            {
                for (int row = 0; row < Math.Min(3, Recipe.Pattern.Pattern.Count); row++)
                {
                    string patternRow = Recipe.Pattern.Pattern[row];
                    for (int col = 0; col < Math.Min(3, patternRow.Length); col++)
                    {
                        char key = patternRow[col];
                        if (key != ' ')
                        {
                            var ingredient = Recipe.Ingredients.FirstOrDefault(i => i.Key == key);
                            if (ingredient != null)
                            {
                                int index = row * 3 + col;
                                CraftingGrid[index] = ingredient.Item;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 检查是否有未保存的更改
        /// </summary>
        /// <returns>如果有未保存的更改返回true</returns>
        public bool HasUnsavedChanges()
        {
            return IsDirty;
        }
    }
}
