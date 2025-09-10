using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace CEEdit.UI.Views.Windows
{
    /// <summary>
    /// NewProjectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewProjectWindow : Window
    {
        private string _selectedTemplate = "basic";
        
        public string ProjectName { get; private set; } = "";
        public string ProjectLocation { get; private set; } = "";
        public string ProjectVersion { get; private set; } = "";
        public string ProjectAuthor { get; private set; } = "";
        public string ProjectDescription { get; private set; } = "";
        public bool IsProjectEnabled { get; private set; }
        public string SelectedTemplate { get; private set; } = "";
        public bool InitGit { get; private set; }
        public bool AddSampleCode { get; private set; }
        public bool CreateReadme { get; private set; }

        public new bool DialogResult { get; private set; } = false;

        public NewProjectWindow()
        {
            InitializeComponent();
            InitializeDefaults();
            UpdateTemplateInfo("basic");
        }

        private void InitializeDefaults()
        {
            // 确保控件已经初始化
            if (ProjectLocationTextBox == null)
            {
                Loaded += (s, e) => SetDefaultValues();
                return;
            }
            
            SetDefaultValues();
        }
        
        private void SetDefaultValues()
        {
            // 设置默认项目位置
            var defaultLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CEEdit Projects");
            if (ProjectLocationTextBox != null)
                ProjectLocationTextBox.Text = defaultLocation;

            // 设置默认作者为系统用户名
            if (ProjectAuthorTextBox != null)
                ProjectAuthorTextBox.Text = Environment.UserName;

            // 选择默认模板
            if (BasicTemplateButton != null)
                BasicTemplateButton.Background = System.Windows.Media.Brushes.LightBlue;
        }

        private void TemplateButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string templateType)
            {
                // 重置所有模板按钮的样式
                ResetTemplateButtonStyles();
                
                // 高亮选中的模板
                button.Background = System.Windows.Media.Brushes.LightBlue;
                
                _selectedTemplate = templateType;
                UpdateTemplateInfo(templateType);
                UpdateStatusText();
            }
        }

        private void ResetTemplateButtonStyles()
        {
            if (BasicTemplateButton != null)
                BasicTemplateButton.Background = System.Windows.Media.Brushes.Transparent;
            if (AdvancedTemplateButton != null)
                AdvancedTemplateButton.Background = System.Windows.Media.Brushes.Transparent;
            if (BlockTemplateButton != null)
                BlockTemplateButton.Background = System.Windows.Media.Brushes.Transparent;
            if (ItemTemplateButton != null)
                ItemTemplateButton.Background = System.Windows.Media.Brushes.Transparent;
            if (RecipeTemplateButton != null)
                RecipeTemplateButton.Background = System.Windows.Media.Brushes.Transparent;
        }

        private void UpdateTemplateInfo(string templateType)
        {
            string description, preview;
            
            switch (templateType)
            {
                case "basic":
                    description = "创建一个基础的CraftEngine addon项目，包含标准的Minecraft资源包结构";
                    preview = @"项目结构预览:
📁 项目根目录/
  📁 configuration/ - 配置文件目录
  📁 resourcepack/ - 资源包目录
    📁 assets/ - 资源文件
      📁 minecraft/ - Minecraft命名空间
        📁 textures/
        📁 models/
        📁 lang/
  📄 pack.yml - 资源包配置
  📄 .ceproject - CEEdit项目文件
  📄 README.md - 项目说明";
                    break;
                
                case "advanced":
                    description = "创建一个包含示例内容的完整CraftEngine addon项目";
                    preview = @"项目结构预览:
📁 项目根目录/
  📁 configuration/ - 配置文件目录
    📄 example_item.yml - 示例物品配置
    📄 example_block.yml - 示例方块配置
  📁 resourcepack/ - 资源包目录
    📁 assets/ - 资源文件
      📁 minecraft/ - Minecraft命名空间
        📁 textures/
          📁 block/ - 方块纹理
          📁 item/ - 物品纹理
        📁 models/
          📁 block/ - 方块模型
          📁 item/ - 物品模型
        📁 blockstates/ - 方块状态
        📁 lang/ - 语言文件
  📄 pack.yml - 资源包配置
  📄 .ceproject - CEEdit项目文件
  📄 README.md - 项目说明";
                    break;
                
                case "blocks":
                    description = "专注于自定义方块的addon项目，包含方块相关的示例配置";
                    preview = @"项目结构预览:
📁 项目根目录/
  📁 configuration/ - 配置文件目录
    📄 stone_block.yml - 石头方块
    📄 wood_block.yml - 木头方块
  📁 resourcepack/ - 资源包目录
    📁 assets/ - 资源文件
      📁 minecraft/ - Minecraft命名空间
        📁 textures/
          📁 block/ - 方块纹理
        📁 models/
          📁 block/ - 方块模型
        📁 blockstates/ - 方块状态
  📄 pack.yml - 资源包配置
  📄 .ceproject - CEEdit项目文件
  📄 README.md - 项目说明";
                    break;
                
                case "items":
                    description = "专注于自定义物品的addon项目，包含物品相关的示例配置";
                    preview = @"项目结构预览:
📁 项目根目录/
  📁 configuration/ - 配置文件目录
    📄 magic_sword.yml - 魔法剑
    📄 health_potion.yml - 生命药水
  📁 resourcepack/ - 资源包目录
    📁 assets/ - 资源文件
      📁 minecraft/ - Minecraft命名空间
        📁 textures/
          📁 item/ - 物品纹理
        📁 models/
          📁 item/ - 物品模型
  📄 pack.yml - 资源包配置
  📄 .ceproject - CEEdit项目文件
  📄 README.md - 项目说明";
                    break;
                
                case "recipes":
                    description = "专注于合成配方的addon项目，包含配方相关的示例配置";
                    preview = @"项目结构预览:
📁 项目根目录/
  📁 configuration/ - 配置文件目录
    📄 crafting_recipes.yml - 合成配方
    📄 smelting_recipes.yml - 熔炼配方
    📄 related_items.yml - 相关物品
  📁 resourcepack/ - 资源包目录
    📁 assets/ - 资源文件
      📁 minecraft/ - Minecraft命名空间
        📁 textures/
        📁 models/
        📁 lang/
  📄 pack.yml - 资源包配置
  📄 .ceproject - CEEdit项目文件
  📄 README.md - 项目说明";
                    break;
                
                default:
                    description = "请选择一个项目模板以开始配置";
                    preview = "选择项目模板以查看预览和文件结构信息...";
                    break;
            }
            
            if (TemplateDescriptionText != null)
                TemplateDescriptionText.Text = description;
            if (TemplatePreviewText != null)
                TemplatePreviewText.Text = preview;
        }

        private void ProjectNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateStatusText();
        }

        private void BrowseLocationButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectLocationTextBox == null)
                return;
                
            var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "选择项目位置",
                UseDescriptionForTitle = true,
                SelectedPath = ProjectLocationTextBox.Text
            };
            
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ProjectLocationTextBox.Text = dialog.SelectedPath;
                UpdateStatusText();
            }
        }

        private void UpdateStatusText()
        {
            // 检查控件是否已初始化
            if (ProjectNameTextBox == null || ProjectLocationTextBox == null || 
                StatusText == null || CreateButton == null)
                return;
                
            var projectName = ProjectNameTextBox.Text.Trim();
            var projectLocation = ProjectLocationTextBox.Text.Trim();
            
            if (string.IsNullOrEmpty(projectName))
            {
                StatusText.Text = "⚠️ 请输入项目名称";
                CreateButton.IsEnabled = false;
                return;
            }
            
            if (string.IsNullOrEmpty(projectLocation))
            {
                StatusText.Text = "⚠️ 请选择项目位置";
                CreateButton.IsEnabled = false;
                return;
            }
            
            var fullPath = Path.Combine(projectLocation, projectName);
            if (Directory.Exists(fullPath))
            {
                StatusText.Text = "⚠️ 项目目录已存在";
                CreateButton.IsEnabled = false;
                return;
            }
            
            StatusText.Text = $"✅ 准备在 {fullPath} 创建项目";
            CreateButton.IsEnabled = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;
            
            // 检查所有控件是否已初始化
            if (ProjectNameTextBox == null || ProjectLocationTextBox == null ||
                ProjectVersionTextBox == null || ProjectAuthorTextBox == null ||
                ProjectDescriptionTextBox == null || ProjectEnabledCheckBox == null ||
                InitGitCheckBox == null || AddSampleCodeCheckBox == null ||
                CreateReadmeCheckBox == null)
            {
                MessageBox.Show("窗口控件未完全初始化，请稍后重试。", "系统错误", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            // 收集表单数据
            ProjectName = ProjectNameTextBox.Text.Trim();
            ProjectLocation = ProjectLocationTextBox.Text.Trim();
            ProjectVersion = ProjectVersionTextBox.Text.Trim();
            ProjectAuthor = ProjectAuthorTextBox.Text.Trim();
            ProjectDescription = ProjectDescriptionTextBox.Text.Trim();
            IsProjectEnabled = ProjectEnabledCheckBox.IsChecked == true;
            SelectedTemplate = _selectedTemplate;
            InitGit = InitGitCheckBox.IsChecked == true;
            AddSampleCode = AddSampleCodeCheckBox.IsChecked == true;
            CreateReadme = CreateReadmeCheckBox.IsChecked == true;
            
            DialogResult = true;
            Close();
        }

        private bool ValidateInput()
        {
            if (ProjectNameTextBox == null || ProjectLocationTextBox == null)
                return false;
                
            var projectName = ProjectNameTextBox.Text.Trim();
            var projectLocation = ProjectLocationTextBox.Text.Trim();
            
            if (string.IsNullOrEmpty(projectName))
            {
                MessageBox.Show("请输入项目名称。", "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                ProjectNameTextBox.Focus();
                return false;
            }
            
            if (string.IsNullOrEmpty(projectLocation))
            {
                MessageBox.Show("请选择项目位置。", "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            
            // 检查项目名称是否包含非法字符
            var invalidChars = Path.GetInvalidFileNameChars();
            if (projectName.IndexOfAny(invalidChars) >= 0)
            {
                MessageBox.Show("项目名称包含非法字符，请重新输入。", "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (ProjectNameTextBox != null)
                    ProjectNameTextBox.Focus();
                return false;
            }
            
            // 检查项目目录是否已存在
            var fullPath = Path.Combine(projectLocation, projectName);
            if (Directory.Exists(fullPath))
            {
                var result = MessageBox.Show(
                    $"目录 '{fullPath}' 已存在。\n\n是否要继续创建项目？这可能会覆盖现有文件。", 
                    "目录已存在", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Question);
                    
                if (result != MessageBoxResult.Yes)
                    return false;
            }
            
            return true;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            UpdateStatusText();
        }
    }
}
