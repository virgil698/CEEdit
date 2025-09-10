using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CEEdit.Core.Models.Project;

namespace CEEdit.Core.Services.Interfaces
{
    /// <summary>
    /// 项目历史记录服务接口
    /// </summary>
    public interface IProjectHistoryService
    {
        /// <summary>
        /// 项目历史记录发生变化时触发
        /// </summary>
        event Action<ProjectHistory>? HistoryChanged;

        /// <summary>
        /// 初始化项目历史服务
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// 获取项目历史记录
        /// </summary>
        /// <returns>项目历史记录</returns>
        Task<ProjectHistory> GetProjectHistoryAsync();

        /// <summary>
        /// 保存项目历史记录
        /// </summary>
        /// <param name="history">项目历史记录</param>
        Task SaveProjectHistoryAsync(ProjectHistory history);

        /// <summary>
        /// 添加最近打开的项目
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        /// <param name="projectType">项目类型</param>
        /// <param name="description">项目描述</param>
        Task AddRecentProjectAsync(string projectPath, string? projectType = null, string? description = null);

        /// <summary>
        /// 移除最近项目
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        Task RemoveRecentProjectAsync(string projectPath);

        /// <summary>
        /// 获取最近项目列表
        /// </summary>
        /// <param name="maxCount">最大数量</param>
        /// <returns>最近项目列表</returns>
        Task<List<ProjectHistoryItem>> GetRecentProjectsAsync(int maxCount = 10);

        /// <summary>
        /// 添加收藏项目
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        Task AddFavoriteProjectAsync(string projectPath);

        /// <summary>
        /// 移除收藏项目
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        Task RemoveFavoriteProjectAsync(string projectPath);

        /// <summary>
        /// 获取收藏项目列表
        /// </summary>
        /// <returns>收藏项目列表</returns>
        Task<List<ProjectHistoryItem>> GetFavoriteProjectsAsync();

        /// <summary>
        /// 检查项目是否已收藏
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        /// <returns>是否已收藏</returns>
        Task<bool> IsFavoriteProjectAsync(string projectPath);

        /// <summary>
        /// 清理不存在的项目
        /// </summary>
        Task CleanupMissingProjectsAsync();

        /// <summary>
        /// 搜索项目
        /// </summary>
        /// <param name="searchText">搜索文本</param>
        /// <param name="includeRecent">包含最近项目</param>
        /// <param name="includeFavorites">包含收藏项目</param>
        /// <returns>搜索结果</returns>
        Task<List<ProjectHistoryItem>> SearchProjectsAsync(string searchText, bool includeRecent = true, bool includeFavorites = true);

        /// <summary>
        /// 更新项目信息
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        /// <param name="projectType">项目类型</param>
        /// <param name="description">描述</param>
        /// <param name="tags">标签</param>
        Task UpdateProjectInfoAsync(string projectPath, string? projectType = null, string? description = null, List<string>? tags = null);
    }
}
