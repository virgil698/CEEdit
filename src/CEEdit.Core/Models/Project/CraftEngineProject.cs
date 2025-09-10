using System.Collections.Generic;
using CEEdit.Core.Models.Blocks;
using CEEdit.Core.Models.Items;
using CEEdit.Core.Models.Recipes;
using CEEdit.Core.Models.Resources;

namespace CEEdit.Core.Models.Project
{
    /// <summary>
    /// CraftEngine项目模型
    /// </summary>
    public class CraftEngineProject
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 项目版本
        /// </summary>
        public string Version { get; set; } = "1.0.0";

        /// <summary>
        /// 项目描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 项目作者
        /// </summary>
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// 许可证
        /// </summary>
        public string License { get; set; } = "MIT";

        /// <summary>
        /// 项目依赖
        /// </summary>
        public List<string> Dependencies { get; set; } = new();

        /// <summary>
        /// 项目设置
        /// </summary>
        public ProjectSettings Settings { get; set; } = new();

        /// <summary>
        /// 方块列表
        /// </summary>
        public List<Block> Blocks { get; set; } = new();

        /// <summary>
        /// 物品列表
        /// </summary>
        public List<Item> Items { get; set; } = new();

        /// <summary>
        /// 配方列表
        /// </summary>
        public List<Recipe> Recipes { get; set; } = new();

        /// <summary>
        /// 资源包
        /// </summary>
        public ResourcePack Resources { get; set; } = new();

        /// <summary>
        /// 项目路径
        /// </summary>
        public string ProjectPath { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 目标CraftEngine版本
        /// </summary>
        public string TargetCraftEngineVersion { get; set; } = "1.20.0";
    }
}

