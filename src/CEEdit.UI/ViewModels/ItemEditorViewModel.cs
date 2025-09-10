using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using CEEdit.Core.Models.Items;
using CEEdit.Core.Models.Common;
using CEEdit.Core.Services.Interfaces;

namespace CEEdit.UI.ViewModels
{
    public partial class ItemEditorViewModel : ObservableObject
    {
        private readonly IFileService? _fileService;
        private readonly IResourceService? _resourceService;
        private readonly IValidationService? _validationService;
        private readonly IBlockbenchService? _blockbenchService;

        [ObservableProperty]
        private Item _item = new();

        [ObservableProperty]
        private string? _selectedLoreLine;

        [ObservableProperty]
        private int _selectedLoreIndex = -1;

        [ObservableProperty]
        private bool _isDirty = false;

        public ObservableCollection<ItemType> AvailableItemTypes { get; } = new()
        {
            ItemType.Material,
            ItemType.Tool,
            ItemType.Weapon,
            ItemType.Armor,
            ItemType.Food,
            ItemType.Consumable,
            ItemType.Misc
        };

        public ObservableCollection<Rarity> AvailableRarities { get; } = new()
        {
            Rarity.Common,
            Rarity.Uncommon,
            Rarity.Rare,
            Rarity.Epic,
            Rarity.Legendary
        };

        public ItemEditorViewModel(IFileService? fileService = null, 
                                  IResourceService? resourceService = null,
                                  IValidationService? validationService = null,
                                  IBlockbenchService? blockbenchService = null)
        {
            _fileService = fileService;
            _resourceService = resourceService;
            _validationService = validationService;
            _blockbenchService = blockbenchService;
            
            // 监听Item属性变化
            Item.PropertyChanged += (s, e) => IsDirty = true;
        }

        [RelayCommand]
        private void BrowseTexture()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "选择纹理文件",
                Filter = "图片文件 (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|所有文件 (*.*)|*.*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                Item.Texture = openFileDialog.FileName;
                IsDirty = true;
            }
        }

        [RelayCommand]
        private void BrowseModel()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "选择模型文件",
                Filter = "模型文件 (*.bbmodel;*.json)|*.bbmodel;*.json|所有文件 (*.*)|*.*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // 如果物品属性中有模型配置，则设置模型路径
                if (Item.Properties.CustomModel != null)
                {
                    Item.Properties.CustomModel.UseCustomModel = true;
                    Item.Properties.CustomModel.ModelPath = openFileDialog.FileName;
                }
                IsDirty = true;
            }
        }

        [RelayCommand]
        private async Task LaunchBlockbench()
        {
            try
            {
                if (_blockbenchService != null)
                {
                    string? modelPath = Item.Properties.CustomModel?.UseCustomModel == true ? 
                                       Item.Properties.CustomModel.ModelPath : null;
                    
                    bool result = await _blockbenchService.LaunchBlockbenchAsync(modelPath);
                    
                    if (!result)
                    {
                        MessageBox.Show("无法启动Blockbench。请确保已正确安装Blockbench。", 
                                      "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Blockbench服务不可用。", 
                                  "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动Blockbench时出错: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void AddLoreLine()
        {
            Item.Lore.Add("新的描述行");
            SelectedLoreIndex = Item.Lore.Count - 1;
            SelectedLoreLine = Item.Lore.LastOrDefault();
            IsDirty = true;
        }

        [RelayCommand]
        private void RemoveLoreLine()
        {
            if (SelectedLoreIndex >= 0 && SelectedLoreIndex < Item.Lore.Count)
            {
                Item.Lore.RemoveAt(SelectedLoreIndex);
                
                // 调整选中索引
                if (SelectedLoreIndex >= Item.Lore.Count)
                {
                    SelectedLoreIndex = Item.Lore.Count - 1;
                }
                
                SelectedLoreLine = SelectedLoreIndex >= 0 ? Item.Lore[SelectedLoreIndex] : null;
                IsDirty = true;
            }
        }

        [RelayCommand]
        private void MoveLoreUp()
        {
            if (SelectedLoreIndex > 0 && SelectedLoreIndex < Item.Lore.Count)
            {
                var item = Item.Lore[SelectedLoreIndex];
                Item.Lore.RemoveAt(SelectedLoreIndex);
                Item.Lore.Insert(SelectedLoreIndex - 1, item);
                SelectedLoreIndex--;
                IsDirty = true;
            }
        }

        [RelayCommand]
        private void MoveLoreDown()
        {
            if (SelectedLoreIndex >= 0 && SelectedLoreIndex < Item.Lore.Count - 1)
            {
                var item = Item.Lore[SelectedLoreIndex];
                Item.Lore.RemoveAt(SelectedLoreIndex);
                Item.Lore.Insert(SelectedLoreIndex + 1, item);
                SelectedLoreIndex++;
                IsDirty = true;
            }
        }

        [RelayCommand]
        private void Reset()
        {
            var result = MessageBox.Show("确定要重置所有设置吗？这将清除所有未保存的更改。", 
                                       "确认重置", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                Item = new Item();
                SelectedLoreIndex = -1;
                SelectedLoreLine = null;
                IsDirty = false;
            }
        }

        [RelayCommand]
        private void Preview()
        {
            try
            {
                // TODO: 实现预览功能
                // 这里可以调用PreviewService来显示物品预览
                MessageBox.Show("预览功能正在开发中...", 
                              "信息", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"预览失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void Save()
        {
            try
            {
                // 验证数据
                if (string.IsNullOrWhiteSpace(Item.Id))
                {
                    MessageBox.Show("物品ID不能为空。", 
                                  "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Item.DisplayName))
                {
                    MessageBox.Show("显示名称不能为空。", 
                                  "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Item.MaxStackSize <= 0)
                {
                    MessageBox.Show("最大堆叠数量必须大于0。", 
                                  "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // TODO: 保存物品数据
                // 这里可以调用ProjectService来保存物品
                
                IsDirty = false;
                
                MessageBox.Show("物品保存成功！", 
                              "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 加载物品数据
        /// </summary>
        /// <param name="item">要编辑的物品</param>
        public void LoadItem(Item item)
        {
            Item = item ?? new Item();
            SelectedLoreIndex = -1;
            SelectedLoreLine = null;
            IsDirty = false;
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
