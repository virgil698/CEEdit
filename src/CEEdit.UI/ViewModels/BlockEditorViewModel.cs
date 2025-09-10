using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using CEEdit.Core.Models.Blocks;
using CEEdit.Core.Models.Common;
using CEEdit.Core.Services.Interfaces;

namespace CEEdit.UI.ViewModels
{
    public partial class BlockEditorViewModel : ObservableObject
    {
        private readonly IFileService? _fileService;
        private readonly IResourceService? _resourceService;
        private readonly IValidationService? _validationService;
        private readonly IBlockbenchService? _blockbenchService;

        [ObservableProperty]
        private Block _block = new();

        [ObservableProperty]
        private BlockDrop? _selectedDrop;

        [ObservableProperty]
        private bool _isDirty = false;

        public ObservableCollection<BlockMaterial> AvailableMaterials { get; } = new()
        {
            BlockMaterial.Stone,
            BlockMaterial.Wood,
            BlockMaterial.Metal,
            BlockMaterial.Dirt,
            BlockMaterial.Sand,
            BlockMaterial.Glass,
            BlockMaterial.Wool,
            BlockMaterial.Plant,
            BlockMaterial.Ice,
            BlockMaterial.Water
        };

        public ObservableCollection<ToolType> AvailableTools { get; } = new()
        {
            ToolType.None,
            ToolType.Pickaxe,
            ToolType.Axe,
            ToolType.Shovel,
            ToolType.Hoe,
            ToolType.Shears,
            ToolType.Sword
        };

        public BlockEditorViewModel(IFileService? fileService = null, 
                                   IResourceService? resourceService = null,
                                   IValidationService? validationService = null,
                                   IBlockbenchService? blockbenchService = null)
        {
            _fileService = fileService;
            _resourceService = resourceService;
            _validationService = validationService;
            _blockbenchService = blockbenchService;
            
            // 监听Block属性变化
            Block.PropertyChanged += (s, e) => IsDirty = true;
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
                Block.ModelPath = openFileDialog.FileName;
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
                    string? modelPath = !string.IsNullOrEmpty(Block.ModelPath) ? Block.ModelPath : null;
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
        private void SelectTopTexture()
        {
            SelectTexture("top");
        }

        [RelayCommand]
        private void SelectSideTexture()
        {
            SelectTexture("side");
        }

        [RelayCommand]
        private void SelectBottomTexture()
        {
            SelectTexture("bottom");
        }

        private void SelectTexture(string face)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "选择纹理文件",
                Filter = "图片文件 (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|所有文件 (*.*)|*.*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string texturePath = openFileDialog.FileName;
                    
                    // 根据面向设置纹理
                    switch (face.ToLower())
                    {
                        case "top":
                            Block.Textures.Top = texturePath;
                            break;
                        case "side":
                            Block.Textures.Side = texturePath;
                            break;
                        case "bottom":
                            Block.Textures.Bottom = texturePath;
                            break;
                    }
                    
                    IsDirty = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加载纹理失败: {ex.Message}", 
                                  "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        [RelayCommand]
        private void ImportTexture()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "导入纹理文件",
                Filter = "图片文件 (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|所有文件 (*.*)|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // TODO: 实现纹理导入逻辑
                    // 这里可以调用ResourceService来处理纹理导入
                    if (_resourceService != null && openFileDialog.FileNames.Length > 0)
                    {
                        // 示例：将第一个文件设置为侧面纹理
                        Block.Textures.Side = openFileDialog.FileNames[0];
                        IsDirty = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"导入纹理失败: {ex.Message}", 
                                  "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        [RelayCommand]
        private void UnifyTextures()
        {
            if (!string.IsNullOrEmpty(Block.Textures.Side))
            {
                Block.Textures.Top = Block.Textures.Side;
                Block.Textures.Bottom = Block.Textures.Side;
                IsDirty = true;
            }
        }

        [RelayCommand]
        private void ResetTextures()
        {
            Block.Textures.Top = string.Empty;
            Block.Textures.Side = string.Empty;
            Block.Textures.Bottom = string.Empty;
            IsDirty = true;
        }

        [RelayCommand]
        private void AddDrop()
        {
            var newDrop = new BlockDrop
            {
                ItemId = "minecraft:cobblestone",
                Count = 1,
                Chance = 1.0f,
                RequiredTool = ToolType.None
            };
            
            Block.Drops.Add(newDrop);
            SelectedDrop = newDrop;
            IsDirty = true;
        }

        [RelayCommand]
        private void RemoveDrop()
        {
            if (SelectedDrop != null)
            {
                Block.Drops.Remove(SelectedDrop);
                SelectedDrop = null;
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
                Block = new Block();
                IsDirty = false;
            }
        }

        [RelayCommand]
        private void Preview()
        {
            try
            {
                // TODO: 实现预览功能
                // 这里可以调用PreviewService来显示3D预览
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
                if (string.IsNullOrWhiteSpace(Block.Id))
                {
                    MessageBox.Show("方块ID不能为空。", 
                                  "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Block.DisplayName))
                {
                    MessageBox.Show("显示名称不能为空。", 
                                  "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // TODO: 保存方块数据
                // 这里可以调用ProjectService来保存方块
                
                IsDirty = false;
                
                MessageBox.Show("方块保存成功！", 
                              "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 加载方块数据
        /// </summary>
        /// <param name="block">要编辑的方块</param>
        public void LoadBlock(Block block)
        {
            Block = block ?? new Block();
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
