using CEEdit.Core.Models.Project;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CEEdit.Core.Services.Interfaces
{
    /// <summary>
    /// 项目管理服务接口
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        /// 当前项目改变事件
        /// </summary>
        event EventHandler<ProjectChangedEventArgs>? CurrentProjectChanged;

        /// <summary>
        /// 项目保存事件
        /// </summary>
        event EventHandler<ProjectSavedEventArgs>? ProjectSaved;

        /// <summary>
        /// 获取当前项目
        /// </summary>
        CraftEngineProject? CurrentProject { get; }

        /// <summary>
        /// 创建新项目
        /// </summary>
        /// <param name="template">项目模板</param>
        /// <param name="projectName">项目名称</param>
        /// <param name="projectPath">项目路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>创建的项目</returns>
        Task<CraftEngineProject> CreateProjectAsync(ProjectTemplate template, string projectName, 
            string projectPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 从现有插件导入项目
        /// </summary>
        /// <param name="pluginPath">插件路径</param>
        /// <param name="projectPath">项目保存路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>导入的项目</returns>
        Task<CraftEngineProject> ImportProjectFromPluginAsync(string pluginPath, string projectPath,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 打开项目
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>打开的项目</returns>
        Task<CraftEngineProject> OpenProjectAsync(string projectPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 保存项目
        /// </summary>
        /// <param name="project">要保存的项目</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>保存任务</returns>
        Task SaveProjectAsync(CraftEngineProject? project = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 另存为项目
        /// </summary>
        /// <param name="project">要保存的项目</param>
        /// <param name="newPath">新路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>保存任务</returns>
        Task SaveProjectAsAsync(CraftEngineProject project, string newPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 关闭项目
        /// </summary>
        /// <param name="saveChanges">是否保存更改</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>关闭任务</returns>
        Task<bool> CloseProjectAsync(bool saveChanges = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// 验证项目
        /// </summary>
        /// <param name="project">要验证的项目</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateProjectAsync(CraftEngineProject? project = null, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 构建项目
        /// </summary>
        /// <param name="project">要构建的项目</param>
        /// <param name="buildMode">构建模式</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>构建结果</returns>
        Task<BuildResult> BuildProjectAsync(CraftEngineProject? project = null, 
            BuildMode buildMode = BuildMode.Release, string? outputPath = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取最近打开的项目列表
        /// </summary>
        /// <returns>最近项目列表</returns>
        Task<List<RecentProject>> GetRecentProjectsAsync();

        /// <summary>
        /// 添加到最近项目列表
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        /// <param name="projectName">项目名称</param>
        /// <returns>添加任务</returns>
        Task AddToRecentProjectsAsync(string projectPath, string projectName);

        /// <summary>
        /// 从最近项目列表中移除
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        /// <returns>移除任务</returns>
        Task RemoveFromRecentProjectsAsync(string projectPath);

        /// <summary>
        /// 清空最近项目列表
        /// </summary>
        /// <returns>清空任务</returns>
        Task ClearRecentProjectsAsync();

        /// <summary>
        /// 获取项目统计信息
        /// </summary>
        /// <param name="project">项目</param>
        /// <returns>统计信息</returns>
        ProjectStatistics GetProjectStatistics(CraftEngineProject? project = null);

        /// <summary>
        /// 检查项目是否有未保存的更改
        /// </summary>
        /// <param name="project">项目</param>
        /// <returns>是否有未保存的更改</returns>
        bool HasUnsavedChanges(CraftEngineProject? project = null);

        /// <summary>
        /// 标记项目为已修改
        /// </summary>
        /// <param name="project">项目</param>
        void MarkAsModified(CraftEngineProject? project = null);

        /// <summary>
        /// 标记项目为未修改
        /// </summary>
        /// <param name="project">项目</param>
        void MarkAsUnmodified(CraftEngineProject? project = null);
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
    /// 项目模板
    /// </summary>
    public class ProjectTemplate
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 模板描述
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// 模板图标
        /// </summary>
        public string Icon { get; set; } = "";

        /// <summary>
        /// 模板类别
        /// </summary>
        public string Category { get; set; } = "";

        /// <summary>
        /// 预览内容
        /// </summary>
        public string Preview { get; set; } = "";
    }

    #region 事件参数和结果类型

    /// <summary>
    /// 项目改变事件参数
    /// </summary>
    public class ProjectChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 旧项目
        /// </summary>
        public CraftEngineProject? OldProject { get; }
        
        /// <summary>
        /// 新项目
        /// </summary>
        public CraftEngineProject? NewProject { get; }
        
        public ProjectChangedEventArgs(CraftEngineProject? oldProject, CraftEngineProject? newProject)
        {
            OldProject = oldProject;
            NewProject = newProject;
        }
    }

    /// <summary>
    /// 项目保存事件参数
    /// </summary>
    public class ProjectSavedEventArgs : EventArgs
    {
        /// <summary>
        /// 项目
        /// </summary>
        public CraftEngineProject Project { get; }
        
        /// <summary>
        /// 保存路径
        /// </summary>
        public string SavePath { get; }
        
        public ProjectSavedEventArgs(CraftEngineProject project, string savePath)
        {
            Project = project;
            SavePath = savePath;
        }
    }

    /// <summary>
    /// 验证结果
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsValid { get; set; }
        
        /// <summary>
        /// 错误列表
        /// </summary>
        public List<ValidationError> Errors { get; set; } = new();
        
        /// <summary>
        /// 警告列表
        /// </summary>
        public List<ValidationWarning> Warnings { get; set; } = new();
    }

    /// <summary>
    /// 验证错误
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; } = "";
        
        /// <summary>
        /// 错误代码
        /// </summary>
        public string Code { get; set; } = "";
        
        /// <summary>
        /// 文件路径
        /// </summary>
        public string? FilePath { get; set; }
        
        /// <summary>
        /// 行号
        /// </summary>
        public int? LineNumber { get; set; }
    }

    /// <summary>
    /// 验证警告
    /// </summary>
    public class ValidationWarning
    {
        /// <summary>
        /// 警告消息
        /// </summary>
        public string Message { get; set; } = "";
        
        /// <summary>
        /// 警告代码
        /// </summary>
        public string Code { get; set; } = "";
        
        /// <summary>
        /// 文件路径
        /// </summary>
        public string? FilePath { get; set; }
        
        /// <summary>
        /// 行号
        /// </summary>
        public int? LineNumber { get; set; }
    }

    /// <summary>
    /// 构建结果
    /// </summary>
    public class BuildResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        
        /// <summary>
        /// 输出路径
        /// </summary>
        public string OutputPath { get; set; } = "";
        
        /// <summary>
        /// 构建日志
        /// </summary>
        public List<string> Logs { get; set; } = new();
        
        /// <summary>
        /// 错误列表
        /// </summary>
        public List<string> Errors { get; set; } = new();
        
        /// <summary>
        /// 警告列表
        /// </summary>
        public List<string> Warnings { get; set; } = new();
    }

    /// <summary>
    /// 最近项目
    /// </summary>
    public class RecentProject
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; } = "";
        
        /// <summary>
        /// 项目路径
        /// </summary>
        public string Path { get; set; } = "";
        
        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastAccessed { get; set; }
        
        /// <summary>
        /// 项目版本
        /// </summary>
        public string Version { get; set; } = "";
        
        /// <summary>
        /// 项目作者
        /// </summary>
        public string Author { get; set; } = "";
    }

    /// <summary>
    /// 项目统计信息
    /// </summary>
    public class ProjectStatistics
    {
        /// <summary>
        /// 方块数量
        /// </summary>
        public int BlockCount { get; set; }
        
        /// <summary>
        /// 物品数量
        /// </summary>
        public int ItemCount { get; set; }
        
        /// <summary>
        /// 配方数量
        /// </summary>
        public int RecipeCount { get; set; }
        
        /// <summary>
        /// 纹理文件数量
        /// </summary>
        public int TextureCount { get; set; }
        
        /// <summary>
        /// 项目大小（字节）
        /// </summary>
        public long ProjectSize { get; set; }
        
        /// <summary>
        /// 文件总数
        /// </summary>
        public int TotalFiles { get; set; }
    }

    #endregion
}