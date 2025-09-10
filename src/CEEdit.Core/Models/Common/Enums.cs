namespace CEEdit.Core.Models.Common
{
    /// <summary>
    /// 配方类型枚举
    /// </summary>
    public enum RecipeType
    {
        /// <summary>
        /// 工作台合成
        /// </summary>
        Crafting,

        /// <summary>
        /// 熔炼
        /// </summary>
        Smelting,

        /// <summary>
        /// 高炉
        /// </summary>
        Blasting,

        /// <summary>
        /// 烟熏
        /// </summary>
        Smoking,

        /// <summary>
        /// 篝火烹饪
        /// </summary>
        CampfireCooking,

        /// <summary>
        /// 切石
        /// </summary>
        Stonecutting,

        /// <summary>
        /// 锻造台
        /// </summary>
        SmithingTable,

        /// <summary>
        /// 铁砧锻造
        /// </summary>
        Smithing,

        /// <summary>
        /// 酿造
        /// </summary>
        Brewing,

        /// <summary>
        /// 自定义
        /// </summary>
        Custom
    }

    /// <summary>
    /// 物品类型枚举
    /// </summary>
    public enum ItemType
    {
        /// <summary>
        /// 杂项
        /// </summary>
        Miscellaneous,

        /// <summary>
        /// 杂项（简写）
        /// </summary>
        Misc,

        /// <summary>
        /// 建筑材料
        /// </summary>
        BuildingBlocks,

        /// <summary>
        /// 装饰方块
        /// </summary>
        Decoration,

        /// <summary>
        /// 红石
        /// </summary>
        Redstone,

        /// <summary>
        /// 交通运输
        /// </summary>
        Transportation,

        /// <summary>
        /// 食物
        /// </summary>
        Food,

        /// <summary>
        /// 消耗品
        /// </summary>
        Consumable,

        /// <summary>
        /// 工具
        /// </summary>
        Tools,

        /// <summary>
        /// 工具（简写）
        /// </summary>
        Tool,

        /// <summary>
        /// 武器
        /// </summary>
        Combat,

        /// <summary>
        /// 武器（简写）
        /// </summary>
        Weapon,

        /// <summary>
        /// 盔甲
        /// </summary>
        Armor,

        /// <summary>
        /// 酿造
        /// </summary>
        Brewing,

        /// <summary>
        /// 材料
        /// </summary>
        Materials,

        /// <summary>
        /// 材料（简写）
        /// </summary>
        Material
    }

    /// <summary>
    /// 物品稀有度枚举
    /// </summary>
    public enum Rarity
    {
        /// <summary>
        /// 普通（白色）
        /// </summary>
        Common,

        /// <summary>
        /// 不常见（黄色）
        /// </summary>
        Uncommon,

        /// <summary>
        /// 稀有（水蓝色）
        /// </summary>
        Rare,

        /// <summary>
        /// 史诗（紫色）
        /// </summary>
        Epic,

        /// <summary>
        /// 传说（金色）
        /// </summary>
        Legendary
    }

    /// <summary>
    /// 工具类型枚举
    /// </summary>
    public enum ToolType
    {
        /// <summary>
        /// 无特定工具要求
        /// </summary>
        None,

        /// <summary>
        /// 任何工具
        /// </summary>
        Any,

        /// <summary>
        /// 镐
        /// </summary>
        Pickaxe,

        /// <summary>
        /// 斧
        /// </summary>
        Axe,

        /// <summary>
        /// 铲
        /// </summary>
        Shovel,

        /// <summary>
        /// 锄
        /// </summary>
        Hoe,

        /// <summary>
        /// 剪刀
        /// </summary>
        Shears,

        /// <summary>
        /// 剑
        /// </summary>
        Sword
    }

    /// <summary>
    /// 方块材质枚举
    /// </summary>
    public enum BlockMaterial
    {
        /// <summary>
        /// 石头
        /// </summary>
        Stone,

        /// <summary>
        /// 木头
        /// </summary>
        Wood,

        /// <summary>
        /// 泥土
        /// </summary>
        Dirt,

        /// <summary>
        /// 沙子
        /// </summary>
        Sand,

        /// <summary>
        /// 草
        /// </summary>
        Grass,

        /// <summary>
        /// 金属
        /// </summary>
        Metal,

        /// <summary>
        /// 玻璃
        /// </summary>
        Glass,

        /// <summary>
        /// 布料
        /// </summary>
        Cloth,

        /// <summary>
        /// 羊毛
        /// </summary>
        Wool,

        /// <summary>
        /// 冰
        /// </summary>
        Ice,

        /// <summary>
        /// 雪
        /// </summary>
        Snow,

        /// <summary>
        /// 黏土
        /// </summary>
        Clay,

        /// <summary>
        /// 海绵
        /// </summary>
        Sponge,

        /// <summary>
        /// 植物
        /// </summary>
        Plant,

        /// <summary>
        /// 藤蔓
        /// </summary>
        Vine,

        /// <summary>
        /// 水
        /// </summary>
        Water,

        /// <summary>
        /// 岩浆
        /// </summary>
        Lava,

        /// <summary>
        /// 空气
        /// </summary>
        Air
    }

    /// <summary>
    /// 资源类型枚举
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// 纹理
        /// </summary>
        Texture,

        /// <summary>
        /// 模型
        /// </summary>
        Model,

        /// <summary>
        /// 音效
        /// </summary>
        Sound,

        /// <summary>
        /// 音乐
        /// </summary>
        Music,

        /// <summary>
        /// 语言文件
        /// </summary>
        Language,

        /// <summary>
        /// 数据文件
        /// </summary>
        Data,

        /// <summary>
        /// 着色器
        /// </summary>
        Shader,

        /// <summary>
        /// 字体
        /// </summary>
        Font
    }

    /// <summary>
    /// 模型类型枚举
    /// </summary>
    public enum ModelType
    {
        /// <summary>
        /// 方块模型
        /// </summary>
        Block,

        /// <summary>
        /// 物品模型
        /// </summary>
        Item,

        /// <summary>
        /// 实体模型
        /// </summary>
        Entity,

        /// <summary>
        /// 粒子模型
        /// </summary>
        Particle,

        /// <summary>
        /// 自定义模型
        /// </summary>
        Custom
    }

    /// <summary>
    /// 音效类型枚举
    /// </summary>
    public enum SoundType
    {
        /// <summary>
        /// 环境音效
        /// </summary>
        Ambient,

        /// <summary>
        /// 方块音效
        /// </summary>
        Block,

        /// <summary>
        /// 物品音效
        /// </summary>
        Item,

        /// <summary>
        /// 实体音效
        /// </summary>
        Entity,

        /// <summary>
        /// UI音效
        /// </summary>
        UI,

        /// <summary>
        /// 音乐
        /// </summary>
        Music,

        /// <summary>
        /// 语音
        /// </summary>
        Voice
    }

    /// <summary>
    /// 验证级别枚举
    /// </summary>
    public enum ValidationLevel
    {
        /// <summary>
        /// 信息
        /// </summary>
        Info,

        /// <summary>
        /// 警告
        /// </summary>
        Warning,

        /// <summary>
        /// 错误
        /// </summary>
        Error,

        /// <summary>
        /// 严重错误
        /// </summary>
        Critical
    }

    /// <summary>
    /// 项目状态枚举
    /// </summary>
    public enum ProjectStatus
    {
        /// <summary>
        /// 草稿
        /// </summary>
        Draft,

        /// <summary>
        /// 开发中
        /// </summary>
        InDevelopment,

        /// <summary>
        /// 测试中
        /// </summary>
        Testing,

        /// <summary>
        /// 准备发布
        /// </summary>
        ReadyForRelease,

        /// <summary>
        /// 已发布
        /// </summary>
        Released,

        /// <summary>
        /// 已归档
        /// </summary>
        Archived
    }

    /// <summary>
    /// 构建模式枚举
    /// </summary>
    public enum BuildMode
    {
        /// <summary>
        /// 调试模式
        /// </summary>
        Debug,

        /// <summary>
        /// 发布模式
        /// </summary>
        Release
    }

    /// <summary>
    /// 日志级别枚举
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 跟踪
        /// </summary>
        Trace,

        /// <summary>
        /// 调试
        /// </summary>
        Debug,

        /// <summary>
        /// 信息
        /// </summary>
        Info,

        /// <summary>
        /// 警告
        /// </summary>
        Warning,

        /// <summary>
        /// 错误
        /// </summary>
        Error,

        /// <summary>
        /// 严重错误
        /// </summary>
        Fatal
    }
}

