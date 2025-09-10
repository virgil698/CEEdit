using CEEdit.Core.Models.Project;
using CEEdit.Core.Models.Resources;

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

    #region 事件参数

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

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="oldProject">旧项目</param>
        /// <param name="newProject">新项目</param>
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
        /// 保存的项目
        /// </summary>
        public CraftEngineProject Project { get; }

        /// <summary>
        /// 保存路径
        /// </summary>
        public string SavePath { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="savePath">保存路径</param>
        public ProjectSavedEventArgs(CraftEngineProject project, string savePath)
        {
            Project = project;
            SavePath = savePath;
        }
    }

    #endregion

    #region 辅助类

    /// <summary>
    /// 最近项目信息
    /// </summary>
    public class RecentProject
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 项目路径
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastAccessed { get; set; } = DateTime.Now;

        /// <summary>
        /// 项目是否存在
        /// </summary>
        public bool Exists { get; set; } = true;

        /// <summary>
        /// 项目描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 项目版本
        /// </summary>
        public string Version { get; set; } = string.Empty;
    }

    /// <summary>
    /// 构建结果
    /// </summary>
    public class BuildResult
    {
        /// <summary>
        /// 构建是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 输出路径
        /// </summary>
        public string OutputPath { get; set; } = string.Empty;

        /// <summary>
        /// 构建时间
        /// </summary>
        public TimeSpan BuildTime { get; set; }

        /// <summary>
        /// 错误列表
        /// </summary>
        public List<BuildError> Errors { get; set; } = new();

        /// <summary>
        /// 警告列表
        /// </summary>
        public List<BuildWarning> Warnings { get; set; } = new();

        /// <summary>
        /// 构建日志
        /// </summary>
        public string BuildLog { get; set; } = string.Empty;

        /// <summary>
        /// 生成的文件列表
        /// </summary>
        public List<string> GeneratedFiles { get; set; } = new();
    }

    /// <summary>
    /// 构建错误
    /// </summary>
    public class BuildError
    {
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// 列号
        /// </summary>
        public int ColumnNumber { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; } = string.Empty;
    }

    /// <summary>
    /// 构建警告
    /// </summary>
    public class BuildWarning : BuildError
    {
        // 继承BuildError的所有属性
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
        /// 纹理数量
        /// </summary>
        public int TextureCount { get; set; }

        /// <summary>
        /// 模型数量
        /// </summary>
        public int ModelCount { get; set; }

        /// <summary>
        /// 音效数量
        /// </summary>
        public int SoundCount { get; set; }

        /// <summary>
        /// 总文件数
        /// </summary>
        public int TotalFileCount { get; set; }

        /// <summary>
        /// 项目大小（字节）
        /// </summary>
        public long ProjectSize { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModifiedAt { get; set; }
    }

    #endregion

    #region 缺失的类型定义

    /// <summary>
    /// 构建模式
    /// </summary>
    public enum BuildMode
    {
        Debug,
        Release,
        Distribution
    }


    #endregion
}

