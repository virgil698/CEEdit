using System.Collections.Generic;

namespace CEEdit.Core.Models.Project
{
    /// <summary>
    /// 项目模板模型
    /// </summary>
    public class ProjectTemplate
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 模板描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 模板分类
        /// </summary>
        public TemplateCategory Category { get; set; } = TemplateCategory.Basic;

        /// <summary>
        /// 模板路径
        /// </summary>
        public string TemplatePath { get; set; } = string.Empty;

        /// <summary>
        /// 预览图片路径
        /// </summary>
        public string PreviewImagePath { get; set; } = string.Empty;

        /// <summary>
        /// 模板标签
        /// </summary>
        public List<string> Tags { get; set; } = new();

        /// <summary>
        /// 所需的CraftEngine版本
        /// </summary>
        public string RequiredCraftEngineVersion { get; set; } = "1.20.0";

        /// <summary>
        /// 模板版本
        /// </summary>
        public string Version { get; set; } = "1.0.0";

        /// <summary>
        /// 模板作者
        /// </summary>
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否为内置模板
        /// </summary>
        public bool IsBuiltIn { get; set; } = false;

        /// <summary>
        /// 模板配置参数
        /// </summary>
        public Dictionary<string, TemplateParameter> Parameters { get; set; } = new();
    }

    /// <summary>
    /// 模板分类枚举
    /// </summary>
    public enum TemplateCategory
    {
        Basic,
        Advanced,
        Blocks,
        Items,
        Recipes,
        FullMod,
        Custom
    }

    /// <summary>
    /// 模板参数
    /// </summary>
    public class TemplateParameter
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 参数显示名称
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// 参数描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 参数类型
        /// </summary>
        public ParameterType Type { get; set; } = ParameterType.String;

        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; } = string.Empty;

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool Required { get; set; } = false;

        /// <summary>
        /// 可选值（仅适用于Select类型）
        /// </summary>
        public List<string> Options { get; set; } = new();
    }

    /// <summary>
    /// 参数类型枚举
    /// </summary>
    public enum ParameterType
    {
        String,
        Integer,
        Boolean,
        Select,
        FilePath,
        DirectoryPath
    }
}

