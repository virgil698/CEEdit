using System;

namespace CEEdit.Core.Models.Project
{
    /// <summary>
    /// 项目创建信息
    /// </summary>
    public class ProjectCreationInfo
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; } = string.Empty;

        /// <summary>
        /// 项目位置
        /// </summary>
        public string ProjectLocation { get; set; } = string.Empty;

        /// <summary>
        /// 项目版本
        /// </summary>
        public string ProjectVersion { get; set; } = "1.0.0";

        /// <summary>
        /// 项目作者
        /// </summary>
        public string ProjectAuthor { get; set; } = string.Empty;

        /// <summary>
        /// 项目描述
        /// </summary>
        public string ProjectDescription { get; set; } = string.Empty;

        /// <summary>
        /// 是否启用项目
        /// </summary>
        public bool IsProjectEnabled { get; set; } = true;

        /// <summary>
        /// 选择的模板
        /// </summary>
        public string SelectedTemplate { get; set; } = "basic";

        /// <summary>
        /// 是否初始化Git仓库
        /// </summary>
        public bool InitGit { get; set; } = false;

        /// <summary>
        /// 是否添加示例代码
        /// </summary>
        public bool AddSampleCode { get; set; } = true;

        /// <summary>
        /// 是否创建README文件
        /// </summary>
        public bool CreateReadme { get; set; } = true;

        /// <summary>
        /// 获取项目命名空间（使用小写项目名称）
        /// </summary>
        public string GetNamespace()
        {
            return ProjectName.ToLowerInvariant().Replace(" ", "_");
        }

        /// <summary>
        /// 获取完整项目路径
        /// </summary>
        public string GetFullProjectPath()
        {
            return System.IO.Path.Combine(ProjectLocation, ProjectName);
        }
    }
}
