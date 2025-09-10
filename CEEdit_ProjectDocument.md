# CEEdit编辑器 - 完整开发大纲

## 项目概述

### 项目名称
CEEdit - CraftEngine插件可视化创建和编辑器

### 项目目标
为CraftEngine插件开发者提供一个功能完整、界面友好的可视化开发工具，简化插件创建、编辑、测试和发布流程，提高开发效率。

### 目标用户
- CraftEngine插件开发者
- Minecraft服务器管理员
- 游戏内容创作者
- 模组开发新手

---

## 1. 功能需求分析

### 1.1 核心功能模块

#### 1.1.1 项目管理模块
**功能描述**: 提供完整的CraftEngine项目生命周期管理

**详细功能**:
- **项目创建**
  - 新建空白项目
  - 从模板创建项目
  - 从现有插件导入项目
  - 项目向导（引导式创建）
  
- **项目配置**
  - 项目基本信息编辑（名称、版本、作者、描述）
  - 依赖管理（CraftEngine版本、其他插件依赖）
  - 构建配置（输出路径、打包设置）
  - 项目权限和许可证设置

- **文件管理**
  - 项目文件树展示
  - 文件创建、删除、重命名
  - 文件夹组织和管理
  - 文件搜索和过滤
  - 文件关联和快速打开

#### 1.1.2 可视化编辑器模块

##### 方块编辑器
**功能描述**: 可视化创建和编辑CraftEngine方块

**详细功能**:
- **基础属性编辑**
  - 方块ID和显示名称
  - 硬度、爆炸抗性设置
  - 挖掘工具要求
  - 掉落物配置
  - 光照等级设置

- **视觉属性**
  - 纹理选择和预览
  - **内嵌Blockbench 3D模型编辑器**
    - 直接在编辑器内启动Blockbench
    - 支持方块、物品、实体模型编辑
    - 实时模型预览和导出
    - 与项目文件自动同步
  - 方块形状定义
  - 透明度和发光效果
  - 粒子效果绑定

- **行为属性**
  - 交互行为定义
  - 红石信号处理
  - 流体交互设置
  - 生长和衰减规则
  - 自定义事件触发器

- **高级功能**
  - 多方块结构支持
  - 方块实体数据
  - 库存界面集成
  - 方块状态管理

##### 物品编辑器
**功能描述**: 创建和编辑游戏物品

**详细功能**:
- **基础属性**
  - 物品ID和显示名称
  - 物品描述和Lore
  - 堆叠数量限制
  - 稀有度等级设置

- **功能属性**
  - 耐久度系统
  - 附魔支持
  - 使用效果定义
  - 食物属性配置
  - 燃料价值设置

- **外观定制**
  - 物品图标/纹理
  - **内嵌Blockbench 3D武器/工具建模**
    - 武器、工具、装备3D模型编辑
    - 动画序列编辑（攻击、使用动画）
    - 材质贴图和UV展开
    - 导出为CraftEngine兼容格式
  - 自定义模型数据
  - 动态纹理支持
  - 颜色变体系统

##### 配方编辑器
**功能描述**: 可视化配置合成和处理配方

**详细功能**:
- **合成配方**
  - 工作台配方编辑器
  - 有序/无序配方支持
  - 配方形状定义
  - 输出物品配置

- **特殊配方**
  - 熔炼配方编辑
  - 切石配方支持
  - 锻造台配方
  - 酿造配方编辑

- **高级配方**
  - 条件配方系统
  - 动态配方生成
  - 配方解锁机制
  - 经验值奖励设置

#### 1.1.3 代码编辑模块

##### YAML配置编辑器
**功能特性**:
- 语法高亮显示
- 自动缩进和格式化
- 实时语法检查
- 智能代码补全
- 错误标记和提示
- 代码折叠功能
- 查找替换功能

##### JavaScript行为脚本编辑器
**功能特性**:
- ECMAScript语法支持
- CraftEngine API智能提示
- 调试断点支持
- 变量监视功能
- 调用栈追踪
- 性能分析工具

#### 1.1.4 资源管理模块

**详细功能**:
- **纹理管理**
  - 纹理文件导入导出
  - 格式转换（PNG, JPG等）
  - 纹理尺寸标准化
  - 纹理打包优化
  - 动画纹理支持

- **模型管理**
  - .json模型文件编辑
  - 3D模型预览
  - 模型验证检查
  - 模型优化建议

- **音效管理**
  - 音效文件管理
  - 音效预览播放
  - 音效格式转换
  - 音量标准化

- **语言文件管理**
  - 多语言文本编辑
  - 翻译状态跟踪
  - 文本导入导出
  - 占位符检查

### 1.2 辅助功能模块

#### 1.2.1 预览和测试系统

**实时预览功能**:
- **3D模型预览**
  - 实时3D渲染
  - 材质贴图显示
  - 光照效果模拟
  - 动画播放支持
  - 多角度查看

- **游戏内效果预览**
  - 方块放置效果
  - 物品使用动画
  - 粒子效果展示
  - 音效播放测试

**测试集成**:
- 本地测试服务器连接
- 热重载支持
- 错误日志实时显示
- 性能监控

#### 1.2.2 版本控制系统

**Git集成功能**:
- 仓库初始化和克隆
- 提交历史可视化
- 分支管理和合并
- 冲突解决辅助
- 远程仓库同步
- 标签管理

**版本对比**:
- 文件差异对比
- 配置变更高亮
- 版本回退功能
- 变更日志生成

#### 1.2.3 发布和分发系统

**插件打包**:
- 自动依赖检查
- 资源优化压缩
- 版本号自动递增
- 构建脚本执行
- 输出验证

**平台集成**:
- Modrinth API集成
- Polymart上传支持
- 自动发布流程
- 版本更新通知

---

## 2. 技术架构设计

### 2.1 总体架构

#### 架构模式
采用MVVM（Model-View-ViewModel）架构模式，确保关注点分离和代码可维护性。

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│      View       │◄──►│   ViewModel     │◄──►│      Model      │
│   (XAML + UI)   │    │ (Business Logic)│    │  (Data & API)   │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         ▼                       ▼                       ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│    Controls     │    │    Services     │    │   Repositories  │
│ (Custom UI)     │    │ (Core Logic)    │    │ (Data Access)   │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

#### 核心层次结构

**表示层 (Presentation Layer)**
- Views: XAML界面文件
- ViewModels: 界面逻辑和数据绑定
- Controls: 自定义用户控件
- Converters: 数据转换器

**业务逻辑层 (Business Logic Layer)**
- Services: 核心业务服务
- Managers: 功能管理器
- Validators: 数据验证器
- Handlers: 事件处理器

**数据访问层 (Data Access Layer)**
- Repositories: 数据访问接口
- Models: 数据模型定义
- DTOs: 数据传输对象
- Mappers: 对象映射器

### 2.2 技术选型

#### 主要框架和库

| 组件 | 技术选型 | 版本 | 选择理由 |
|------|----------|------|----------|
| UI框架 | WPF | .NET 9.0 | 成熟的桌面应用框架，丰富的控件库 |
| MVVM框架 | CommunityToolkit.Mvvm | 8.0+ | 轻量级MVVM实现，代码生成器支持 |
| 代码编辑器 | AvalonEdit | 6.3+ | 强大的文本编辑器，语法高亮支持 |
| 3D渲染 | HelixToolkit.Wpf | 2.25+ | WPF 3D渲染库，模型预览功能 |
| 3D建模集成 | Blockbench API | Latest | 内嵌Blockbench进行3D建模和动画 |
| YAML处理 | YamlDotNet | 13.0+ | .NET标准YAML库，性能优秀 |
| JSON处理 | System.Text.Json | Built-in | 官方JSON库，性能最佳 |
| 压缩解压 | System.IO.Compression | Built-in | 内置压缩库，插件打包使用 |
| 版本控制 | LibGit2Sharp | 0.29+ | Git操作库，版本控制集成 |
| 图像处理 | System.Drawing.Common | Built-in | 纹理处理和格式转换 |
| 音频播放 | NAudio | 2.2+ | 音效预览功能 |
| 网络请求 | HttpClient | Built-in | API调用和文件下载 |

#### 开发工具和环境

- **IDE**: Visual Studio 2022 / JetBrains Rider
- **版本控制**: Git
- **包管理**: NuGet
- **构建工具**: MSBuild
- **测试框架**: xUnit + Moq
- **文档工具**: DocFX

### 2.3 数据模型设计

#### 核心数据结构

```csharp
// 项目模型
public class CraftEngineProject
{
    public string Name { get; set; }
    public string Version { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public List<string> Dependencies { get; set; }
    public ProjectSettings Settings { get; set; }
    public List<Block> Blocks { get; set; }
    public List<Item> Items { get; set; }
    public List<Recipe> Recipes { get; set; }
    public ResourcePack Resources { get; set; }
}

// 方块模型
public class Block
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public float Hardness { get; set; }
    public float BlastResistance { get; set; }
    public string Material { get; set; }
    public List<string> Textures { get; set; }
    public BlockBehavior Behavior { get; set; }
    public Dictionary<string, object> Properties { get; set; }
}

// 物品模型
public class Item
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public int MaxStackSize { get; set; }
    public int Durability { get; set; }
    public Rarity Rarity { get; set; }
    public string Texture { get; set; }
    public List<string> Lore { get; set; }
    public ItemBehavior Behavior { get; set; }
}

// 配方模型
public class Recipe
{
    public string Id { get; set; }
    public RecipeType Type { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public ItemStack Result { get; set; }
    public Dictionary<string, object> Conditions { get; set; }
}
```

### 2.4 服务架构设计

#### 服务接口定义

```csharp
// 项目管理服务
public interface IProjectService
{
    Task<CraftEngineProject> CreateProjectAsync(ProjectTemplate template);
    Task<CraftEngineProject> OpenProjectAsync(string projectPath);
    Task SaveProjectAsync(CraftEngineProject project);
    Task<bool> ValidateProjectAsync(CraftEngineProject project);
}

// 文件管理服务
public interface IFileService
{
    Task<string> ReadTextFileAsync(string filePath);
    Task WriteTextFileAsync(string filePath, string content);
    Task<byte[]> ReadBinaryFileAsync(string filePath);
    Task WriteBinaryFileAsync(string filePath, byte[] data);
    Task<bool> FileExistsAsync(string filePath);
}

// 资源管理服务
public interface IResourceService
{
    Task<Texture> LoadTextureAsync(string texturePath);
    Task<Model3D> LoadModelAsync(string modelPath);
    Task<AudioClip> LoadAudioAsync(string audioPath);
    Task OptimizeResourcesAsync(ResourcePack resourcePack);
}

// 预览服务
public interface IPreviewService
{
    Task<RenderResult> RenderBlockAsync(Block block);
    Task<RenderResult> RenderItemAsync(Item item);
    Task PlayAudioAsync(string audioPath);
    Task ShowParticleEffectAsync(ParticleEffect effect);
}
```

```csharp
// Blockbench集成服务
public interface IBlockbenchService
{
    Task<bool> LaunchBlockbenchAsync(string modelPath = null);
    Task<bool> IsBlockbenchAvailableAsync();
    Task<Model3D> ImportModelFromBlockbenchAsync(string bbmodelPath);
    Task<string> ExportModelToBlockbenchAsync(Model3D model);
    Task<bool> WatchModelChangesAsync(string modelPath, Action<string> onChanged);
    Task<List<Animation>> GetModelAnimationsAsync(string modelPath);
}
```


---

## 3. 详细文件结构

### 3.1 项目目录结构

```
CEEdit/
├── 📁 src/                           # 源代码目录
│   ├── 📁 CEEdit.Core/              # 核心库项目
│   │   ├── 📁 Models/               # 数据模型
│   │   │   ├── 📄 Project/
│   │   │   │   ├── CraftEngineProject.cs
│   │   │   │   ├── ProjectSettings.cs
│   │   │   │   └── ProjectTemplate.cs
│   │   │   ├── 📄 Blocks/
│   │   │   │   ├── Block.cs
│   │   │   │   ├── BlockBehavior.cs
│   │   │   │   ├── BlockProperties.cs
│   │   │   │   └── BlockTexture.cs
│   │   │   ├── 📄 Items/
│   │   │   │   ├── Item.cs
│   │   │   │   ├── ItemBehavior.cs
│   │   │   │   ├── ItemProperties.cs
│   │   │   │   └── ItemStack.cs
│   │   │   ├── 📄 Recipes/
│   │   │   │   ├── Recipe.cs
│   │   │   │   ├── Ingredient.cs
│   │   │   │   ├── RecipeType.cs
│   │   │   │   └── CraftingPattern.cs
│   │   │   ├── 📄 Resources/
│   │   │   │   ├── ResourcePack.cs
│   │   │   │   ├── Texture.cs
│   │   │   │   ├── Model3D.cs
│   │   │   │   ├── AudioClip.cs
│   │   │   │   └── LanguageFile.cs
│   │   │   └── 📄 Common/
│   │   │       ├── Enums.cs
│   │   │       ├── Constants.cs
│   │   │       └── Extensions.cs
│   │   ├── 📁 Services/              # 服务接口和实现
│   │   │   ├── 📄 Interfaces/
│   │   │   │   ├── IProjectService.cs
│   │   │   │   ├── IFileService.cs
│   │   │   │   ├── IResourceService.cs
│   │   │   │   ├── IPreviewService.cs
│   │   │   │   ├── IValidationService.cs
│   │   │   │   ├── ITemplateService.cs
│   │   │   │   ├── IPublishService.cs
│   │   │   │   └── IBlockbenchService.cs
│   │   │   └── 📄 Implementations/
│   │   │       ├── ProjectService.cs
│   │   │       ├── FileService.cs
│   │   │       ├── ResourceService.cs
│   │   │       ├── PreviewService.cs
│   │   │       ├── ValidationService.cs
│   │   │       ├── TemplateService.cs
│   │   │       ├── PublishService.cs
│   │   │       └── BlockbenchService.cs
│   │   ├── 📁 Utils/                 # 工具类
│   │   │   ├── FileHelper.cs
│   │   │   ├── YamlHelper.cs
│   │   │   ├── JsonHelper.cs
│   │   │   ├── ValidationHelper.cs
│   │   │   ├── ColorHelper.cs
│   │   │   ├── ImageHelper.cs
│   │   │   └── ZipHelper.cs
│   │   └── 📁 Exceptions/            # 自定义异常
│   │       ├── ProjectException.cs
│   │       ├── ValidationException.cs
│   │       └── ResourceException.cs
│   │
│   ├── 📁 CEEdit.UI/                # UI项目
│   │   ├── 📄 App.xaml
│   │   ├── 📄 App.xaml.cs
│   │   ├── 📁 Views/                # 视图文件
│   │   │   ├── 📄 Windows/
│   │   │   │   ├── MainWindow.xaml
│   │   │   │   ├── MainWindow.xaml.cs
│   │   │   │   ├── SettingsWindow.xaml
│   │   │   │   ├── AboutWindow.xaml
│   │   │   │   └── PublishDialog.xaml
│   │   │   ├── 📄 Pages/
│   │   │   │   ├── WelcomePage.xaml
│   │   │   │   ├── ProjectPage.xaml
│   │   │   │   ├── EditorPage.xaml
│   │   │   │   └── PreviewPage.xaml
│   │   │   └── 📄 UserControls/
│   │   │       ├── ProjectExplorer.xaml
│   │   │       ├── PropertyPanel.xaml
│   │   │       ├── CodeEditor.xaml
│   │   │       ├── BlockEditor.xaml
│   │   │       ├── ItemEditor.xaml
│   │   │       ├── RecipeEditor.xaml
│   │   │       ├── ResourceManager.xaml
│   │   │       ├── Preview3D.xaml
│   │   │       └── LogViewer.xaml
│   │   ├── 📁 ViewModels/           # 视图模型
│   │   │   ├── MainViewModel.cs
│   │   │   ├── ProjectViewModel.cs
│   │   │   ├── EditorViewModel.cs
│   │   │   ├── BlockEditorViewModel.cs
│   │   │   ├── ItemEditorViewModel.cs
│   │   │   ├── RecipeEditorViewModel.cs
│   │   │   ├── ResourceManagerViewModel.cs
│   │   │   ├── PreviewViewModel.cs
│   │   │   └── SettingsViewModel.cs
│   │   ├── 📁 Controls/             # 自定义控件
│   │   │   ├── CodeEditorControl.cs
│   │   │   ├── PropertyGridControl.cs
│   │   │   ├── TreeViewEx.cs
│   │   │   ├── Preview3DControl.cs
│   │   │   ├── BlockbenchIntegrationControl.cs
│   │   │   ├── ModelViewerControl.cs
│   │   │   ├── AnimationPreviewControl.cs
│   │   │   ├── ColorPicker.cs
│   │   │   ├── FileDropTarget.cs
│   │   │   └── NumericUpDown.cs
│   │   ├── 📁 Converters/           # 数据转换器
│   │   │   ├── BoolToVisibilityConverter.cs
│   │   │   ├── ColorToBrushConverter.cs
│   │   │   ├── EnumToStringConverter.cs
│   │   │   ├── PathToImageConverter.cs
│   │   │   └── ValidationResultConverter.cs
│   │   ├── 📁 Behaviors/            # 行为类
│   │   │   ├── DragDropBehavior.cs
│   │   │   ├── KeyBindingBehavior.cs
│   │   │   └── ValidationBehavior.cs
│   │   └── 📁 Themes/               # 主题样式
│   │       ├── Generic.xaml
│   │       ├── DarkTheme.xaml
│   │       ├── LightTheme.xaml
│   │       └── Colors.xaml
│   │
│   └── 📁 CEEdit.Tests/             # 测试项目
│       ├── 📁 Unit/                 # 单元测试
│       ├── 📁 Integration/          # 集成测试
│       └── 📁 UI/                   # UI测试
│
├── 📁 resources/                    # 资源文件
│   ├── 📁 icons/                    # 图标资源
│   │   ├── app.ico
│   │   ├── block.png
│   │   ├── item.png
│   │   ├── recipe.png
│   │   └── ...
│   ├── 📁 templates/                # 项目模板
│   │   ├── 📁 basic/               # 基础模板
│   │   ├── 📁 advanced/            # 高级模板
│   │   ├── 📁 blocks/              # 方块模板
│   │   ├── 📁 items/               # 物品模板
│   │   └── 📁 recipes/             # 配方模板
│   ├── 📁 samples/                  # 示例项目
│   │   ├── 📁 getting-started/
│   │   ├── 📁 custom-blocks/
│   │   └── 📁 advanced-recipes/
│   └── 📁 localization/            # 多语言文件
│       ├── en-US.json
│       ├── zh-CN.json
│       └── zh-TW.json
│
├── 📁 docs/                        # 文档目录
│   ├── 📄 README.md               # 项目说明
│   ├── 📄 CHANGELOG.md            # 更新日志
│   ├── 📄 CONTRIBUTING.md         # 贡献指南
│   ├── 📁 user-manual/            # 用户手册
│   ├── 📁 developer-guide/        # 开发者指南
│   └── 📁 api-reference/          # API参考文档
│
├── 📁 scripts/                     # 构建脚本
│   ├── build.ps1                  # Windows构建脚本
│   ├── build.sh                   # Linux构建脚本
│   ├── deploy.ps1                 # 部署脚本
│   └── package.ps1               # 打包脚本
│
├── 📁 config/                      # 配置文件
│   ├── app.config                 # 应用配置
│   ├── settings.json              # 默认设置
│   ├── keybindings.json           # 快捷键配置
│   └── templates.json             # 模板配置
│
├── 📄 CEEdit.sln                  # 解决方案文件
├── 📄 .gitignore                  # Git忽略文件
├── 📄 .editorconfig               # 编辑器配置
├── 📄 LICENSE                     # 许可证文件
└── 📄 版本信息.txt                # 版本信息
```

### 3.2 配置文件详细说明

#### 应用程序配置 (app.config)
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>
    <!-- 应用程序基本设置 -->
    <add key="ApplicationName" value="CEEdit" />
    <add key="Version" value="1.0.0" />
    <add key="MinCraftEngineVersion" value="1.20.0" />
    <add key="MaxCraftEngineVersion" value="1.21.0" />
    
    <!-- 文件路径设置 -->
    <add key="DefaultProjectPath" value="%USERPROFILE%\Documents\CEEdit\Projects" />
    <add key="TemplatesPath" value="resources\templates" />
    <add key="CachePath" value="%TEMP%\CEEdit" />
    
    <!-- 功能开关 -->
    <add key="EnableAutoSave" value="true" />
    <add key="EnableCrashReporting" value="true" />
    <add key="EnableUsageAnalytics" value="false" />
    
    <!-- 性能设置 -->
    <add key="MaxUndoLevels" value="100" />
    <add key="AutoSaveInterval" value="300" />
    <add key="PreviewUpdateDelay" value="500" />
  </appSettings>
</configuration>
```

#### 用户设置 (settings.json)
```json
{
  "ui": {
    "theme": "dark",
    "language": "zh-CN",
    "fontSize": 12,
    "fontFamily": "Consolas",
    "windowState": {
      "maximized": true,
      "width": 1200,
      "height": 800,
      "left": 100,
      "top": 100
    },
    "panels": {
      "projectExplorer": {
        "visible": true,
        "width": 250
      },
      "propertyPanel": {
        "visible": true,
        "width": 300
      },
      "preview": {
        "visible": true,
        "height": 200
      }
    }
  },
  "editor": {
    "showLineNumbers": true,
    "showWhitespace": false,
    "enableCodeFolding": true,
    "autoComplete": true,
    "tabSize": 2,
    "insertSpaces": true
  },
  "project": {
    "defaultAuthor": "Your Name",
    "defaultLicense": "MIT",
    "autoBackup": true,
    "backupInterval": 1800
  },
  "preview": {
    "autoRefresh": true,
    "renderQuality": "high",
    "showGrid": true,
    "backgroundType": "transparent"
  }
}
```

---

## 4. 软件打包部署结构

### 4.1 打包文件夹组织

根据实际部署和用户使用需求，软件打包后采用以下标准化目录结构：

```
CEEdit/                          # 软件根目录
├── 📄 CEEdit.exe               # 主程序可执行文件
├── 📁 libraries/               # 所有软件依赖DLL文件
│   ├── System.Text.Json.dll    # JSON处理库
│   ├── YamlDotNet.dll          # YAML解析库
│   ├── HelixToolkit.Wpf.dll    # 3D渲染库
│   ├── AvalonEdit.dll          # 代码编辑器库
│   ├── LibGit2Sharp.dll        # Git操作库
│   ├── NAudio.dll              # 音频处理库
│   ├── CommunityToolkit.Mvvm.dll # MVVM框架
│   ├── Microsoft.*.dll         # 微软运行时库
│   └── ...                     # 其他第三方依赖库
│
├── 📁 config/                  # 用户个性化配置文件
│   ├── 📄 settings.json        # 用户个人设置
│   │   # 包含：主题、语言、字体、窗口状态等
│   ├── 📄 keybindings.json     # 自定义快捷键配置
│   ├── 📄 ui-layout.json       # 界面布局配置
│   │   # 包含：面板位置、大小、停靠状态等
│   ├── 📄 recent-projects.json # 最近打开的项目列表
│   ├── 📄 user-preferences.json # 用户编辑偏好
│   │   # 包含：自动保存、代码风格、验证规则等
│   └── 📄 workspace-state.json # 工作区状态保存
│
├── 📁 data/                    # 软件缓存数据和系统文件
│   ├── 📁 logs/                # 日志文件夹
│   │   ├── 📄 app-{date}.log   # 应用程序日志（按日期分割）
│   │   ├── 📄 error-{date}.log # 错误日志（按日期分割）
│   │   ├── 📄 debug.log        # 调试日志
│   │   └── 📄 performance.log  # 性能监控日志
│   ├── 📁 localization/        # 多语言文件
│   │   ├── 📄 en-US.json       # 英语语言包
│   │   ├── 📄 zh-CN.json       # 简体中文语言包
│   │   ├── 📄 zh-TW.json       # 繁体中文语言包
│   │   ├── 📄 ja-JP.json       # 日语语言包
│   │   └── 📄 ko-KR.json       # 韩语语言包
│   ├── 📁 cache/               # 临时缓存文件
│   │   ├── 📁 thumbnails/      # 模型和纹理缩略图缓存
│   │   ├── 📁 models/          # 3D模型文件缓存
│   │   ├── 📁 textures/        # 纹理文件缓存
│   │   ├── 📁 previews/        # 预览图像缓存
│   │   └── 📁 temp/            # 临时工作文件
│   ├── 📁 databases/           # 内置数据库文件
│   │   ├── 📄 minecraft-data.db # Minecraft原版数据库
│   │   ├── 📄 craftengine-api.db # CraftEngine API数据库
│   │   ├── 📄 templates.db     # 项目模板数据库
│   │   └── 📄 user-history.db  # 用户操作历史数据库
│   └── 📄 app-crash-dumps/     # 崩溃转储文件夹
│
├── 📁 cache/                   # 外部引用软件
│   ├── 📁 blockbench/          # Blockbench软件缓存
│   │   ├── 📄 blockbench.exe   # Blockbench主程序
│   │   ├── 📄 version.json     # 版本信息文件
│   │   ├── 📁 plugins/         # Blockbench插件
│   │   │   ├── ceedit-sync.js  # CEEdit同步插件
│   │   │   └── ...             # 其他插件
│   │   ├── 📁 models/          # Blockbench示例模型
│   │   └── 📁 themes/          # Blockbench主题文件
│   ├── 📁 minecraft-assets/    # Minecraft资源文件缓存
│   │   ├── 📁 textures/        # 原版纹理文件
│   │   │   ├── 📁 block/       # 方块纹理
│   │   │   ├── 📁 item/        # 物品纹理
│   │   │   └── 📁 entity/      # 实体纹理
│   │   ├── 📁 models/          # 原版模型文件
│   │   │   ├── 📁 block/       # 方块模型
│   │   │   └── 📁 item/        # 物品模型
│   │   ├── 📁 sounds/          # 原版音效文件
│   │   └── 📄 assets-index.json # 资源索引文件
│   └── 📁 git-portable/        # 便携版Git工具（可选）
│       ├── 📄 git.exe          # Git主程序
│       ├── 📁 bin/             # Git二进制文件
│       └── 📁 libexec/         # Git库文件
│
├── 📁 project/                 # 工程文件夹
│   ├── 📁 templates/           # 项目模板库
│   │   ├── 📁 basic/           # 基础插件模板
│   │   │   ├── 📄 project.json # 模板配置文件
│   │   │   ├── 📁 blocks/      # 方块模板
│   │   │   ├── 📁 items/       # 物品模板
│   │   │   └── 📁 resources/   # 资源模板
│   │   ├── 📁 advanced/        # 高级插件模板
│   │   ├── 📁 blocks-only/     # 纯方块模板
│   │   ├── 📁 items-only/      # 纯物品模板
│   │   ├── 📁 recipes-focused/ # 配方主题模板
│   │   └── 📁 custom/          # 用户自定义模板
│   ├── 📁 samples/             # 示例项目集合
│   │   ├── 📁 getting-started/ # 新手入门项目
│   │   │   ├── 📄 README.md    # 项目说明
│   │   │   ├── 📄 project.ceproj # CEEdit项目文件
│   │   │   └── ...             # 项目文件
│   │   ├── 📁 custom-blocks/   # 自定义方块示例
│   │   ├── 📁 advanced-recipes/ # 高级配方示例
│   │   ├── 📁 complex-items/   # 复杂物品示例
│   │   └── 📁 full-mod/        # 完整模组示例
│   ├── 📁 workspace/           # 用户工作区
│   │   ├── 📁 MyFirstProject/  # 用户项目示例
│   │   │   ├── 📄 project.ceproj # 项目配置文件
│   │   │   ├── 📁 blocks/      # 方块定义文件
│   │   │   ├── 📁 items/       # 物品定义文件
│   │   │   ├── 📁 recipes/     # 配方定义文件
│   │   │   ├── 📁 models/      # 3D模型文件
│   │   │   ├── 📁 textures/    # 纹理文件
│   │   │   └── 📁 scripts/     # 行为脚本文件
│   │   └── ...                 # 其他用户项目
│   └── 📄 workspace.json       # 工作区配置文件
│
├── 📁 versions/                # CraftEngine语法配置解析文件
│   ├── 📁 1.20.0/              # CraftEngine 1.20.0版本支持
│   │   ├── 📄 syntax.json      # 语法定义文件
│   │   ├── 📄 api-schema.json  # API架构定义
│   │   ├── 📄 validation.json  # 数据验证规则
│   │   ├── 📄 blocks.json      # 方块定义规范
│   │   ├── 📄 items.json       # 物品定义规范
│   │   ├── 📄 recipes.json     # 配方定义规范
│   │   ├── 📄 behaviors.json   # 行为脚本规范
│   │   ├── 📄 events.json      # 事件系统规范
│   │   └── 📄 compatibility.json # 兼容性配置
│   ├── 📁 1.20.1/              # CraftEngine 1.20.1版本支持
│   │   └── ...                 # 同上结构文件
│   ├── 📁 1.21.0/              # CraftEngine 1.21.0版本支持
│   │   └── ...                 # 同上结构文件
│   ├── 📄 versions.json        # 支持版本索引文件
│   └── 📄 latest.json          # 最新版本配置
│
├── 📁 docs/                    # 帮助文档文件夹
│   ├── 📄 README.md            # 软件使用说明
│   ├── 📄 CHANGELOG.md         # 版本更新日志
│   ├── 📄 user-manual.pdf      # 用户操作手册
│   ├── 📄 api-reference.pdf    # API参考文档
│   ├── 📄 troubleshooting.md   # 故障排除指南
│   └── 📁 tutorials/           # 教程文档
│       ├── 📄 getting-started.md
│       ├── 📄 advanced-features.md
│       └── 📄 best-practices.md
│
├── 📄 app.config               # 应用程序配置文件
├── 📄 LICENSE                  # 软件许可证文件
├── 📄 VERSION                  # 当前版本信息
└── 📄 INSTALL.md               # 安装说明文档
```

### 4.2 文件夹功能详解

#### 4.2.1 libraries/ - 依赖库管理
**目的**: 集中管理所有.NET依赖库文件，避免DLL冲突
**特性**:
- 自动依赖解析和版本管理
- 支持延迟加载减少启动时间
- 包含数字签名验证防止篡改
- 支持增量更新减少下载量

#### 4.2.2 config/ - 配置文件管理
**目的**: 存储用户个性化设置，支持多用户配置
**特性**:
- JSON格式便于编辑和迁移
- 支持配置备份和恢复
- 热重载配置变更
- 配置版本兼容性检查

#### 4.2.3 data/ - 数据和缓存管理
**目的**: 管理应用数据、日志和缓存文件
**特性**:
- 日志按日期和大小自动滚动
- 多语言文件支持动态加载
- 智能缓存清理机制
- 数据库文件自动备份

#### 4.2.4 cache/ - 外部软件缓存
**目的**: 管理Blockbench等外部工具的本地缓存
**特性**:
- 自动下载和更新外部工具
- 版本管理和回滚支持
- 离线工作模式支持
- 资源文件增量同步

#### 4.2.5 project/ - 项目和模板管理
**目的**: 组织用户项目、模板和示例文件
**特性**:
- 项目模板快速应用
- 示例项目学习参考
- 工作区隔离和备份
- 项目导入导出功能

#### 4.2.6 versions/ - 版本兼容性管理
**目的**: 支持多个CraftEngine版本的语法和API
**特性**:
- 多版本并存支持
- 语法验证和智能提示
- 版本迁移工具
- 向后兼容性检查

### 4.3 打包配置实现

#### 4.3.1 目录初始化代码
```csharp
public class DirectoryManager
{
    private static readonly string[] RequiredDirectories = {
        "libraries", "config", "data", "cache", 
        "project", "versions", "docs"
    };

    public static void EnsureDirectoriesExist()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        
        foreach (var dir in RequiredDirectories)
        {
            var fullPath = Path.Combine(baseDir, dir);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                CreateDirectoryReadme(fullPath, dir);
            }
        }
    }

    private static void CreateDirectoryReadme(string dirPath, string dirName)
    {
        var readmePath = Path.Combine(dirPath, "README.md");
        var content = GetDirectoryDescription(dirName);
        File.WriteAllText(readmePath, content);
    }
}
```

#### 4.3.2 配置路径管理
```csharp
public static class PathManager
{
    public static string LibrariesPath => GetPath("libraries");
    public static string ConfigPath => GetPath("config");
    public static string DataPath => GetPath("data");
    public static string CachePath => GetPath("cache");
    public static string ProjectPath => GetPath("project");
    public static string VersionsPath => GetPath("versions");
    public static string DocsPath => GetPath("docs");

    private static string GetPath(string folder)
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folder);
    }
}
```

---

## 5. Blockbench集成技术方案

### 5.1 Blockbench集成架构

#### 5.1.1 集成方式选择
**嵌入式集成（推荐方案）**
- 将Blockbench作为子进程启动
- 通过IPC（进程间通信）进行数据交换
- 实时监控模型文件变更
- 支持热重载和自动同步

**技术实现方案**:
```csharp
public class BlockbenchService : IBlockbenchService
{
    private Process _blockbenchProcess;
    private FileSystemWatcher _modelWatcher;
    
    public async Task<bool> LaunchBlockbenchAsync(string modelPath = null)
    {
        var blockbenchPath = await GetBlockbenchExecutableAsync();
        var startInfo = new ProcessStartInfo
        {
            FileName = blockbenchPath,
            Arguments = modelPath != null ? $"--open \"{modelPath}\"" : "",
            UseShellExecute = true
        };
        
        _blockbenchProcess = Process.Start(startInfo);
        
        // 设置文件监控
        if (modelPath != null)
        {
            SetupFileWatcher(Path.GetDirectoryName(modelPath));
        }
        
        return _blockbenchProcess != null;
    }
    
    private void SetupFileWatcher(string directory)
    {
        _modelWatcher = new FileSystemWatcher(directory, "*.bbmodel");
        _modelWatcher.Changed += OnModelFileChanged;
        _modelWatcher.EnableRaisingEvents = true;
    }
    
    private async void OnModelFileChanged(object sender, FileSystemEventArgs e)
    {
        // 延迟执行避免多次触发
        await Task.Delay(500);
        
        // 通知UI更新模型预览
        ModelChanged?.Invoke(e.FullPath);
    }
}
```

#### 5.1.2 文件格式支持

**支持的模型格式**:
- `.bbmodel` - Blockbench原生格式
- `.json` - Minecraft模型格式
- `.obj` - 通用3D模型格式（导入）
- `.fbx` - 动画模型格式（导入）

**格式转换器**:
```csharp
public class ModelFormatConverter
{
    public async Task<MinecraftModel> ConvertBbmodelToMinecraftAsync(string bbmodelPath)
    {
        var bbmodel = await LoadBbmodelAsync(bbmodelPath);
        var mcModel = new MinecraftModel();
        
        // 转换几何体数据
        mcModel.Elements = ConvertElements(bbmodel.Elements);
        
        // 转换纹理映射
        mcModel.Textures = ConvertTextures(bbmodel.Textures);
        
        // 转换显示设置
        mcModel.Display = ConvertDisplaySettings(bbmodel.Display);
        
        return mcModel;
    }
}
```

### 5.2 3D模型编辑工作流

#### 5.2.1 工作流程设计
```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   CEEdit界面    │───►│   Blockbench    │───►│   模型预览      │
│  (选择编辑模型)  │    │   (3D建模)      │    │  (实时更新)     │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         │                       ▼                       │
         │               ┌─────────────────┐              │
         │               │   文件监控      │              │
         │               │  (自动同步)     │              │
         │               └─────────────────┘              │
         │                       │                       │
         ▼                       ▼                       ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  项目文件更新   │    │   模型文件保存  │    │   预览刷新      │
│ (.ceproj更新)   │    │ (.bbmodel保存)  │    │ (3D渲染更新)    │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

#### 5.2.2 用户界面设计
**模型编辑面板布局**:
```xml
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="2*"/>
        <ColumnDefinition Width="3*"/>
        <ColumnDefinition Width="2*"/>
    </Grid.ColumnDefinitions>
    
    <!-- 左侧：模型列表和属性 -->
    <StackPanel Grid.Column="0">
        <TextBlock Text="模型列表" Style="{StaticResource HeaderStyle}"/>
        <TreeView x:Name="ModelTreeView"/>
        
        <TextBlock Text="模型属性" Style="{StaticResource HeaderStyle}"/>
        <controls:PropertyGridControl x:Name="ModelProperties"/>
    </StackPanel>
    
    <!-- 中央：Blockbench集成区域 -->
    <Grid Grid.Column="1">
        <controls:BlockbenchIntegrationControl x:Name="BlockbenchPanel"/>
        <controls:ModelViewerControl x:Name="ModelPreview" 
                                   Visibility="{Binding ShowPreview, Converter={StaticResource BoolToVisibility}}"/>
    </Grid>
    
    <!-- 右侧：动画和材质 -->
    <StackPanel Grid.Column="2">
        <TextBlock Text="动画序列" Style="{StaticResource HeaderStyle}"/>
        <controls:AnimationPreviewControl x:Name="AnimationPanel"/>
        
        <TextBlock Text="材质管理" Style="{StaticResource HeaderStyle}"/>
        <controls:TextureManagerControl x:Name="TexturePanel"/>
    </StackPanel>
</Grid>
```

### 5.3 动画系统集成

#### 5.3.1 动画数据结构
```csharp
public class Animation
{
    public string Name { get; set; }
    public float Duration { get; set; }
    public bool Loop { get; set; }
    public List<Keyframe> Keyframes { get; set; }
    public AnimationType Type { get; set; } // Attack, Use, Idle, etc.
}

public class Keyframe
{
    public float Time { get; set; }
    public Transform Transform { get; set; }
    public EasingType Easing { get; set; }
}

public enum AnimationType
{
    Attack,
    Use,
    Block,
    Idle,
    Walk,
    Run,
    Custom
}
```

#### 5.3.2 动画预览系统
```csharp
public class AnimationPreviewControl : UserControl
{
    private readonly Timer _animationTimer;
    private Animation _currentAnimation;
    private float _currentTime;
    
    public void PlayAnimation(Animation animation)
    {
        _currentAnimation = animation;
        _currentTime = 0;
        _animationTimer.Start();
    }
    
    private void OnAnimationTick(object sender, EventArgs e)
    {
        if (_currentAnimation == null) return;
        
        _currentTime += 0.016f; // 60 FPS
        
        if (_currentTime >= _currentAnimation.Duration)
        {
            if (_currentAnimation.Loop)
                _currentTime = 0;
            else
                _animationTimer.Stop();
        }
        
        // 更新模型变换
        UpdateModelTransform(_currentTime);
    }
}
```

### 5.4 模型优化和验证

#### 5.4.1 模型验证规则
```csharp
public class ModelValidator
{
    public ValidationResult ValidateModel(Model3D model)
    {
        var result = new ValidationResult();
        
        // 检查面数限制
        if (model.FaceCount > 1000)
        {
            result.Warnings.Add("模型面数过多，可能影响性能");
        }
        
        // 检查纹理尺寸
        foreach (var texture in model.Textures)
        {
            if (!IsPowerOfTwo(texture.Width) || !IsPowerOfTwo(texture.Height))
            {
                result.Warnings.Add($"纹理 {texture.Name} 尺寸不是2的幂次方");
            }
        }
        
        // 检查动画时长
        foreach (var animation in model.Animations)
        {
            if (animation.Duration > 10.0f)
            {
                result.Warnings.Add($"动画 {animation.Name} 时长过长");
            }
        }
        
        return result;
    }
}
```

#### 5.4.2 自动优化功能
- **模型简化**: 自动减少不必要的顶点和面
- **纹理压缩**: 智能压缩纹理文件大小
- **动画优化**: 移除冗余关键帧
- **文件大小优化**: 压缩模型文件

---

## 6. 开发注意事项详解

### 6.1 性能优化策略

#### 6.1.1 UI性能优化
**虚拟化技术**
- 大数据集合使用`VirtualizingPanel`
- 实现自定义虚拟化控件
- 延迟加载非可见UI元素

**异步操作**
```csharp
// 示例：异步加载项目文件
public async Task<CraftEngineProject> LoadProjectAsync(string projectPath)
{
    return await Task.Run(() =>
    {
        // 在后台线程执行耗时操作
        var project = DeserializeProject(projectPath);
        
        // 切换回UI线程更新界面
        Dispatcher.Invoke(() =>
        {
            UpdateProjectExplorer(project);
        });
        
        return project;
    });
}
```

**内存管理**
- 及时释放大对象引用
- 使用弱引用避免内存泄漏
- 实现资源缓存池
- 定期执行垃圾回收

#### 6.1.2 3D渲染优化
**LOD (Level of Detail) 系统**
- 根据距离调整模型细节
- 实现多级细节模型
- 动态加载/卸载模型资源

**批处理渲染**
- 合并相似模型减少绘制调用
- 使用实例化渲染技术
- 优化材质和纹理使用

#### 6.1.3 文件IO优化
**异步文件操作**
```csharp
public async Task<string> ReadFileAsync(string filePath)
{
    using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
    using var reader = new StreamReader(fileStream);
    return await reader.ReadToEndAsync();
}
```

**缓存策略**
- 实现多级缓存系统
- 使用内存缓存频繁访问的文件
- 实现文件变更监听自动刷新缓存

### 6.2 用户体验设计

#### 6.2.1 响应式设计
**布局适配**
- 支持不同屏幕分辨率
- 实现响应式面板布局
- 自适应字体和图标大小

**主题系统**
```csharp
public class ThemeManager
{
    public void ApplyTheme(ThemeType themeType)
    {
        var themeDict = LoadThemeResources(themeType);
        Application.Current.Resources.MergedDictionaries.Clear();
        Application.Current.Resources.MergedDictionaries.Add(themeDict);
    }
}
```

#### 6.2.2 交互设计
**键盘快捷键**
```xml
<Window.InputBindings>
    <KeyBinding Key="N" Modifiers="Ctrl" Command="{Binding NewProjectCommand}"/>
    <KeyBinding Key="O" Modifiers="Ctrl" Command="{Binding OpenProjectCommand}"/>
    <KeyBinding Key="S" Modifiers="Ctrl" Command="{Binding SaveProjectCommand}"/>
    <KeyBinding Key="F5" Command="{Binding PreviewCommand}"/>
</Window.InputBindings>
```

**拖拽支持**
- 文件拖拽导入
- 组件拖拽排序
- 可视化编辑器拖拽操作

### 6.3 数据验证和错误处理

#### 6.3.1 输入验证
**数据注解验证**
```csharp
public class Block : INotifyDataErrorInfo
{
    [Required(ErrorMessage = "方块ID不能为空")]
    [RegularExpression(@"^[a-z_][a-z0-9_]*$", ErrorMessage = "方块ID格式不正确")]
    public string Id { get; set; }
    
    [Range(0.0, 1000.0, ErrorMessage = "硬度值必须在0-1000之间")]
    public float Hardness { get; set; }
}
```

**实时验证**
- 输入时即时验证
- 可视化错误提示
- 批量验证项目文件

#### 6.3.2 异常处理
**全局异常处理**
```csharp
private void Application_DispatcherUnhandledException(object sender, 
    DispatcherUnhandledExceptionEventArgs e)
{
    LogManager.LogError(e.Exception);
    ShowErrorDialog(e.Exception);
    e.Handled = true;
}
```

**错误恢复机制**
- 自动保存机制
- 项目备份和恢复
- 崩溃后状态恢复

### 6.4 安全性考虑

#### 6.4.1 文件系统安全
**路径验证**
```csharp
public bool IsValidPath(string path)
{
    // 检查路径注入攻击
    if (path.Contains("..") || path.Contains("//"))
        return false;
        
    // 验证文件扩展名白名单
    var allowedExtensions = new[] { ".yml", ".json", ".png", ".obj" };
    return allowedExtensions.Any(ext => path.EndsWith(ext));
}
```

**权限控制**
- 限制文件访问范围
- 验证用户操作权限
- 沙箱化外部脚本执行

#### 6.4.2 网络安全
**HTTPS通信**
- 所有网络请求使用HTTPS
- 证书验证
- API访问令牌管理

### 6.5 国际化和本地化

#### 6.5.1 多语言支持
**资源文件结构**
```
localization/
├── en-US.json      # 英语
├── zh-CN.json      # 简体中文
├── zh-TW.json      # 繁体中文
└── ja-JP.json      # 日语
```

**动态语言切换**
```csharp
public class LocalizationService
{
    public void ChangeLanguage(string cultureName)
    {
        var culture = new CultureInfo(cultureName);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
        
        LoadResourceDictionary(cultureName);
    }
}
```

#### 6.5.2 文化适配
- 日期时间格式本地化
- 数字和货币格式适配
- 文本方向支持（RTL/LTR）

---

## 7. 测试策略

### 7.1 测试金字塔

```
        🔺 E2E Tests (10%)
       /               \
      /   Integration     \
     /    Tests (20%)      \
    /                       \
   /     Unit Tests (70%)    \
  /                           \
 /___________________________ \
```

### 7.2 单元测试策略

#### 测试覆盖率目标
- 核心业务逻辑: 90%+
- UI逻辑: 70%+
- 工具类: 95%+
- 整体代码覆盖率: 80%+

#### 测试示例
```csharp
[Fact]
public async Task CreateProject_ShouldGenerateValidProject()
{
    // Arrange
    var projectService = new ProjectService();
    var template = new ProjectTemplate { Name = "Basic" };
    
    // Act
    var project = await projectService.CreateProjectAsync(template);
    
    // Assert
    Assert.NotNull(project);
    Assert.NotEmpty(project.Name);
    Assert.NotNull(project.Settings);
}

[Theory]
[InlineData("test_block", true)]
[InlineData("123invalid", false)]
[InlineData("", false)]
public void ValidateBlockId_ShouldReturnExpectedResult(string blockId, bool expected)
{
    // Arrange & Act
    var result = ValidationHelper.IsValidBlockId(blockId);
    
    // Assert
    Assert.Equal(expected, result);
}
```

### 7.3 集成测试

**数据库测试**
```csharp
public class ProjectRepositoryTests : IClassFixture<DatabaseFixture>
{
    [Fact]
    public async Task SaveProject_ShouldPersistToDatabase()
    {
        // 测试项目保存功能
    }
}
```

**文件系统测试**
```csharp
[Fact]
public async Task LoadProject_ShouldReadFromFileSystem()
{
    // 测试项目文件加载
}
```

### 7.4 UI测试

**自动化UI测试**
使用WinAppDriver进行UI自动化测试：

```csharp
[TestMethod]
public void CreateNewProject_ShouldOpenProjectWizard()
{
    var newProjectButton = session.FindElementByName("New Project");
    newProjectButton.Click();
    
    var wizardWindow = session.FindElementByName("Project Wizard");
    Assert.IsNotNull(wizardWindow);
}
```

### 7.5 性能测试

**负载测试**
```csharp
[Fact]
public async Task LoadLargeProject_ShouldCompleteWithin5Seconds()
{
    var stopwatch = Stopwatch.StartNew();
    
    await projectService.LoadProjectAsync("large-project.ceproj");
    
    stopwatch.Stop();
    Assert.True(stopwatch.ElapsedMilliseconds < 5000);
}
```

**内存泄漏测试**
- 使用内存分析器检测泄漏
- 长时间运行稳定性测试
- 资源释放验证

---

## 8. 部署和发布

### 8.1 构建流水线

#### GitHub Actions 工作流
```yaml
name: Build and Release

on:
  push:
    tags: ['v*']

jobs:
  build:
    runs-on: windows-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '9.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --configuration Release --no-restore
    
    - name: Test
      run: dotnet test --configuration Release --no-build --verbosity normal
    
    - name: Publish
      run: dotnet publish -c Release -r win-x64 --self-contained true
    
    - name: Create installer
      run: |
        # 使用WiX或Inno Setup创建安装包
        
    - name: Upload artifacts
      uses: actions/upload-artifact@v3
      with:
        name: CEEdit-${{ github.ref_name }}
        path: publish/
```

### 8.2 安装包制作

#### 使用WiX Toolset
```xml
<?xml version="1.0" encoding="UTF-8"?>
<Wix xmlns="http://schemas.microsoft.com/wix/2006/wi">
  <Product Id="*" Name="CEEdit" Language="1033" Version="1.0.0.0" 
           Manufacturer="YourCompany" UpgradeCode="...">
    
    <Package InstallerVersion="200" Compressed="yes" InstallScope="perMachine"/>
    
    <MediaTemplate EmbedCab="yes"/>
    
    <Feature Id="ProductFeature" Title="CEEdit" Level="1">
      <ComponentGroupRef Id="ProductComponents"/>
    </Feature>
    
    <Directory Id="TARGETDIR" Name="SourceDir">
      <Directory Id="ProgramFilesFolder">
        <Directory Id="INSTALLFOLDER" Name="CEEdit"/>
      </Directory>
    </Directory>
    
    <ComponentGroup Id="ProductComponents" Directory="INSTALLFOLDER">
      <Component Id="MainExecutable">
        <File Source="CEEdit.exe" KeyPath="yes"/>
      </Component>
    </ComponentGroup>
  </Product>
</Wix>
```

### 8.3 自动更新系统

#### 更新检查服务
```csharp
public class UpdateService
{
    private readonly HttpClient _httpClient;
    
    public async Task<UpdateInfo> CheckForUpdatesAsync()
    {
        var response = await _httpClient.GetStringAsync("https://api.ceedit.com/updates/latest");
        return JsonSerializer.Deserialize<UpdateInfo>(response);
    }
    
    public async Task<bool> DownloadAndInstallUpdateAsync(UpdateInfo updateInfo)
    {
        // 下载更新文件
        var updateFile = await DownloadFileAsync(updateInfo.DownloadUrl);
        
        // 启动更新程序
        Process.Start("Updater.exe", $"--update \"{updateFile}\"");
        
        // 关闭当前应用
        Application.Current.Shutdown();
        
        return true;
    }
}
```

### 8.4 版本管理策略

#### 语义化版本控制
- **主版本号(Major)**: 不兼容的API修改
- **次版本号(Minor)**: 向后兼容的功能性新增
- **修订号(Patch)**: 向后兼容的问题修正
- **预发布标识**: alpha, beta, rc

#### 版本号示例
- `1.0.0` - 首个正式版本
- `1.1.0` - 新增功能
- `1.1.1` - 错误修复
- `2.0.0-beta.1` - 下一个主版本的测试版

---

## 9. 项目管理

### 9.1 开发流程

#### 敏捷开发方法
**迭代计划**
- Sprint周期: 2周
- 每日站会: 同步进度和问题
- Sprint评审: 展示完成功能
- 回顾会议: 持续改进

**用户故事管理**
```
作为一个CraftEngine插件开发者，
我希望能够可视化编辑方块属性，
以便更快速地创建自定义方块。

验收标准：
- [ ] 可以设置方块基础属性
- [ ] 可以选择和预览纹理
- [ ] 可以配置方块行为
- [ ] 可以实时预览效果
```

### 9.2 代码质量管理

#### 代码规范
- 遵循C#编码规范
- 使用EditorConfig统一格式
- 强制代码审查制度
- SonarQube代码质量检查

#### Git工作流
```
main (主分支)
├── develop (开发分支)
│   ├── feature/block-editor (功能分支)
│   ├── feature/item-editor
│   └── feature/recipe-editor
├── release/v1.0.0 (发布分支)
└── hotfix/critical-bug (热修复分支)
```

### 9.3 团队协作

#### 角色分工
- **项目经理**: 项目规划和进度管理
- **技术负责人**: 架构设计和技术决策
- **前端开发**: UI界面和用户交互
- **后端开发**: 核心逻辑和服务
- **测试工程师**: 质量保证和测试
- **UX设计师**: 用户体验设计

#### 沟通工具
- **项目管理**: Jira/Azure DevOps
- **即时沟通**: Slack/Teams
- **文档协作**: Confluence/Notion
- **代码协作**: GitHub/GitLab

---

## 10. 风险评估和应对

### 10.1 技术风险

| 风险项目 | 概率 | 影响 | 应对策略 |
|----------|------|------|----------|
| CraftEngine API变更 | 中 | 高 | 版本兼容性设计，API抽象层 |
| 性能问题 | 低 | 中 | 性能测试，代码优化 |
| 第三方库依赖 | 中 | 中 | 依赖版本锁定，备选方案 |
| 跨平台兼容性 | 低 | 低 | 目标平台测试验证 |

### 10.2 业务风险

| 风险项目 | 概率 | 影响 | 应对策略 |
|----------|------|------|----------|
| 用户需求变更 | 高 | 中 | 敏捷开发，快速迭代 |
| 竞品威胁 | 中 | 中 | 差异化功能，持续创新 |
| 市场接受度 | 中 | 高 | 用户调研，MVP验证 |
| 技术人员流失 | 低 | 高 | 知识文档，技能备份 |

### 10.3 应急预案

#### 数据备份策略
- 自动备份用户项目
- 云端同步功能
- 本地备份恢复

#### 故障恢复计划
- 应用崩溃自动重启
- 项目文件损坏修复
- 紧急补丁发布流程

---

## 11. 成本估算

### 11.1 人力成本

| 角色 | 人数 | 工期(月) | 月薪(万) | 小计(万) |
|------|------|----------|----------|----------|
| 项目经理 | 1 | 6 | 2.5 | 15 |
| 技术负责人 | 1 | 6 | 3.0 | 18 |
| 高级开发 | 2 | 6 | 2.0 | 24 |
| 中级开发 | 2 | 6 | 1.5 | 18 |
| UI设计师 | 1 | 4 | 1.8 | 7.2 |
| 测试工程师 | 1 | 4 | 1.2 | 4.8 |
| **总计** | **8** | **-** | **-** | **87** |

### 11.2 其他成本

| 成本项目 | 金额(万) | 说明 |
|----------|----------|------|
| 开发工具和授权 | 5 | Visual Studio, JetBrains等 |
| 云服务费用 | 3 | GitHub, CI/CD服务 |
| 第三方库授权 | 2 | 商业控件库 |
| Blockbench集成授权 | 1 | Blockbench API使用授权 |
| 硬件设备 | 8 | 开发机器和测试设备 |
| **总计** | **19** | - |

### 11.3 总体预算
- **人力成本**: 87万
- **其他成本**: 19万
- **风险缓冲(15%)**: 15.9万
- **项目总预算**: 121.9万

---

## 12. 里程碑规划

### Phase 1: 基础架构 (Month 1-2)
- [ ] 项目架构设计完成
- [ ] MVVM基础框架搭建
- [ ] 核心服务接口定义
- [ ] 基础UI布局实现
- [ ] 项目管理功能开发

### Phase 2: 核心编辑器 (Month 3-4)
- [ ] YAML/JSON编辑器集成
- [ ] 方块编辑器开发
- [ ] 物品编辑器开发
- [ ] **Blockbench集成开发**
  - [ ] Blockbench服务实现
  - [ ] 文件监控系统
  - [ ] 模型格式转换器
- [ ] 资源管理器实现
- [ ] 数据验证系统

### Phase 3: 高级功能 (Month 4-5)
- [ ] 3D预览功能
- [ ] **Blockbench高级功能集成**
  - [ ] 动画系统集成
  - [ ] 实时协同编辑
  - [ ] 模型优化和验证
- [ ] 配方编辑器
- [ ] 版本控制集成
- [ ] 插件打包功能
- [ ] 模板系统

### Phase 4: 优化完善 (Month 5-6)
- [ ] 性能优化
- [ ] UI/UX优化
- [ ] 多语言支持
- [ ] 完整测试覆盖
- [ ] 文档编写

### Phase 5: 发布准备 (Month 6)
- [ ] Beta版本发布
- [ ] 用户反馈收集
- [ ] 问题修复
- [ ] 正式版本发布
- [ ] 用户培训和支持

---

## 结语

这份开发大纲为CEEdit项目提供了全面的规划和指导。通过系统化的方法，我们可以确保项目按时保质完成，并为CraftEngine插件开发社区提供强大的工具支持。

项目的成功依赖于：
1. **清晰的架构设计**
2. **高质量的代码实现**
3. **完善的测试覆盖**
4. **优秀的用户体验**
5. **持续的社区支持**

建议按照里程碑逐步推进，定期回顾和调整计划，确保项目目标的达成。

---

## 💡 Blockbench集成亮点

通过集成Blockbench软件，CEEdit在3D建模方面具备了以下独特优势：

### 🎯 核心优势
- **专业级建模**: 直接使用业界领先的Minecraft建模工具
- **无缝集成**: 一键启动Blockbench，自动同步模型文件
- **实时预览**: 模型修改后立即在CEEdit中更新预览
- **格式兼容**: 完美支持CraftEngine所需的各种模型格式

### 🚀 创新功能
- **智能监控**: 自动检测Blockbench中的模型变更
- **动画集成**: 支持武器攻击、物品使用等动画效果
- **批量优化**: 自动优化模型性能和文件大小
- **验证系统**: 实时检查模型规范和性能指标

### 💼 用户价值
- **降低门槛**: 新手也能快速创建专业级3D模型
- **提升效率**: 专业建模师可以使用熟悉的工具
- **保证质量**: 自动验证确保模型符合游戏要求
- **节省时间**: 一站式解决方案，无需在多个工具间切换

这一集成方案将CEEdit打造成了真正意义上的**一站式CraftEngine插件开发平台**，为开发者提供了从概念到成品的完整工具链。

---

## 📦 软件打包部署说明

### 新增打包要求

根据你的最新要求，软件打包结构已按照以下6个标准文件夹进行组织：

#### 🔧 核心文件夹结构
1. **`libraries/`** - 存放所有软件依赖DLL文件
   - 包含.NET框架库、第三方组件、插件依赖等
   - 支持自动依赖解析和版本管理
   - 数字签名验证确保安全性

2. **`config/`** - 存放用户个性化配置文件
   - 用户设置、快捷键、界面布局等配置
   - 支持配置备份、恢复和热重载
   - JSON格式便于编辑和迁移

3. **`data/`** - 存放软件缓存数据、日志文件夹及多语言文件
   - 应用日志、错误日志、性能监控日志
   - 多语言包（中英文等）
   - 缓存文件和临时数据
   - 内置数据库文件

4. **`cache/`** - 存放外部引用软件（如Blockbench）
   - Blockbench软件缓存和插件
   - Minecraft原版资源文件缓存
   - Git便携版工具（可选）
   - 自动下载和版本管理

5. **`project/`** - 存放工程文件夹
   - 项目模板库（基础、高级、专项模板）
   - 示例项目集合（新手教程、高级案例）
   - 用户工作区和项目文件
   - 工作区配置和管理

6. **`versions/`** - 存放CraftEngine语法配置解析文件
   - 多版本CraftEngine支持（1.20.0, 1.20.1, 1.21.0等）
   - 语法定义、API架构、验证规则
   - 兼容性配置和版本迁移支持

### 技术实现

软件启动时会自动创建和管理这些目录，确保：
- **目录完整性**：自动检查和创建缺失的目录
- **权限管理**：合适的文件访问权限设置
- **路径管理**：统一的路径管理器提供访问接口
- **清理机制**：智能缓存清理和空间管理

### 部署优势

这种标准化的文件组织结构带来以下优势：
- **🗂️ 结构清晰**：每个文件夹职责明确，便于维护
- **🔄 易于备份**：可以分类备份用户数据和系统数据
- **⚡ 性能优化**：分离热数据和冷数据，提升访问速度
- **🛡️ 安全隔离**：不同类型文件分开存储，降低安全风险
- **🚀 便于升级**：程序文件和用户数据分离，升级更安全
