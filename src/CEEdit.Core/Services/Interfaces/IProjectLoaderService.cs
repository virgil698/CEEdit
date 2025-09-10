using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CEEdit.Core.Models.Project;

namespace CEEdit.Core.Services.Interfaces
{
    /// <summary>
    /// 项目加载服务接口
    /// </summary>
    public interface IProjectLoaderService
    {
        /// <summary>
        /// 加载项目数据
        /// </summary>
        /// <param name="projectPath">项目文件路径（.ceproj文件）</param>
        /// <returns>加载的项目数据</returns>
        Task<LoadedProjectData?> LoadProjectAsync(string projectPath);

        /// <summary>
        /// 验证项目文件是否有效
        /// </summary>
        /// <param name="projectPath">项目文件路径</param>
        /// <returns>是否为有效的项目文件</returns>
        Task<bool> ValidateProjectAsync(string projectPath);

        /// <summary>
        /// 获取项目基本信息（不加载完整数据）
        /// </summary>
        /// <param name="projectPath">项目文件路径</param>
        /// <returns>项目基本信息</returns>
        Task<ProjectInfo?> GetProjectInfoAsync(string projectPath);
    }

    /// <summary>
    /// 加载的项目数据
    /// </summary>
    public class LoadedProjectData
    {
        /// <summary>
        /// 项目文件路径
        /// </summary>
        public string ProjectPath { get; set; } = string.Empty;

        /// <summary>
        /// 项目目录路径
        /// </summary>
        public string ProjectDirectory { get; set; } = string.Empty;

        /// <summary>
        /// CEEdit项目数据
        /// </summary>
        public CraftEngineProject Project { get; set; } = new();

        /// <summary>
        /// pack.yml数据
        /// </summary>
        public PackYml? PackYml { get; set; }

        /// <summary>
        /// 项目是否成功加载
        /// </summary>
        public bool IsLoaded { get; set; }

        /// <summary>
        /// 加载错误信息
        /// </summary>
        public string? LoadError { get; set; }

        /// <summary>
        /// 项目中的文件列表
        /// </summary>
        public ProjectFileStructure FileStructure { get; set; } = new();
    }

    /// <summary>
    /// 项目基本信息
    /// </summary>
    public class ProjectInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;
        public bool Enable { get; set; } = true;
    }

    /// <summary>
    /// 项目文件结构
    /// </summary>
    public class ProjectFileStructure
    {
        public List<string> Blocks { get; set; } = new();
        public List<string> Items { get; set; } = new();
        public List<string> Recipes { get; set; } = new();
        public List<string> Textures { get; set; } = new();
        public List<string> Models { get; set; } = new();
        public List<string> Sounds { get; set; } = new();
    }
}
