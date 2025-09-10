using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace CEEdit.Core.Models.Project
{
    /// <summary>
    /// pack.yml 文件数据模型
    /// </summary>
    public class PackYml
    {
        /// <summary>
        /// 项目作者
        /// </summary>
        [YamlMember(Alias = "author")]
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// 项目版本
        /// </summary>
        [YamlMember(Alias = "version")]
        public string Version { get; set; } = "0.0.1";

        /// <summary>
        /// 项目描述
        /// </summary>
        [YamlMember(Alias = "description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 项目命名空间
        /// </summary>
        [YamlMember(Alias = "namespace")]
        public string Namespace { get; set; } = string.Empty;

        /// <summary>
        /// 是否启用
        /// </summary>
        [YamlMember(Alias = "enable")]
        public bool Enable { get; set; } = true;

        /// <summary>
        /// 检查pack.yml是否包含有效数据
        /// </summary>
        public bool IsValid => !string.IsNullOrWhiteSpace(Author) || 
                              !string.IsNullOrWhiteSpace(Version) || 
                              !string.IsNullOrWhiteSpace(Description) ||
                              !string.IsNullOrWhiteSpace(Namespace);

        /// <summary>
        /// 获取项目显示名称（优先使用命名空间，其次是作者）
        /// </summary>
        public string GetDisplayName()
        {
            if (!string.IsNullOrWhiteSpace(Namespace))
                return Namespace;
            
            if (!string.IsNullOrWhiteSpace(Author))
                return $"{Author}的项目";
            
            return "未命名项目";
        }

        /// <summary>
        /// 获取完整的项目信息字符串
        /// </summary>
        public string GetFullInfo()
        {
            var parts = new List<string>();
            
            if (!string.IsNullOrWhiteSpace(Author))
                parts.Add($"作者: {Author}");
            
            if (!string.IsNullOrWhiteSpace(Version))
                parts.Add($"版本: {Version}");
            
            if (!string.IsNullOrWhiteSpace(Namespace))
                parts.Add($"命名空间: {Namespace}");
            
            return string.Join(" | ", parts);
        }
    }
}
