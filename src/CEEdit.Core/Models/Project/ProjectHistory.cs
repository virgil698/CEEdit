using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CEEdit.Core.Models.Project
{
    /// <summary>
    /// 项目历史记录
    /// </summary>
    public class ProjectHistory
    {
        /// <summary>
        /// 最近打开的项目列表
        /// </summary>
        [JsonProperty("recentProjects")]
        public List<ProjectHistoryItem> RecentProjects { get; set; } = new();

        /// <summary>
        /// 收藏的项目列表
        /// </summary>
        [JsonProperty("favoriteProjects")]
        public List<string> FavoriteProjects { get; set; } = new();

        /// <summary>
        /// 最大保存的最近项目数量
        /// </summary>
        [JsonIgnore]
        public const int MaxRecentProjects = 20;

        /// <summary>
        /// 添加最近打开的项目
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        public void AddRecentProject(string projectPath)
        {
            if (string.IsNullOrWhiteSpace(projectPath))
                return;

            // 移除已存在的相同项目
            RecentProjects.RemoveAll(p => string.Equals(p.ProjectPath, projectPath, 
                StringComparison.OrdinalIgnoreCase));

            // 添加到列表顶部
            RecentProjects.Insert(0, new ProjectHistoryItem
            {
                ProjectPath = projectPath,
                ProjectName = System.IO.Path.GetFileNameWithoutExtension(projectPath),
                LastOpenedTime = DateTime.Now,
                LastModifiedTime = System.IO.File.Exists(projectPath) 
                    ? System.IO.File.GetLastWriteTime(projectPath) 
                    : DateTime.Now
            });

            // 保持最大数量限制
            if (RecentProjects.Count > MaxRecentProjects)
            {
                RecentProjects.RemoveRange(MaxRecentProjects, 
                    RecentProjects.Count - MaxRecentProjects);
            }
        }

        /// <summary>
        /// 移除最近项目
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        public void RemoveRecentProject(string projectPath)
        {
            RecentProjects.RemoveAll(p => string.Equals(p.ProjectPath, projectPath, 
                StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 添加收藏项目
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        public void AddFavoriteProject(string projectPath)
        {
            if (!string.IsNullOrWhiteSpace(projectPath) && 
                !FavoriteProjects.Contains(projectPath, StringComparer.OrdinalIgnoreCase))
            {
                FavoriteProjects.Add(projectPath);
            }
        }

        /// <summary>
        /// 移除收藏项目
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        public void RemoveFavoriteProject(string projectPath)
        {
            FavoriteProjects.RemoveAll(p => string.Equals(p, projectPath, 
                StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 检查项目是否已收藏
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        /// <returns>是否已收藏</returns>
        public bool IsFavoriteProject(string projectPath)
        {
            return FavoriteProjects.Contains(projectPath, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 清理不存在的项目
        /// </summary>
        public void CleanupMissingProjects()
        {
            // 清理不存在的最近项目
            RecentProjects.RemoveAll(p => !System.IO.File.Exists(p.ProjectPath));

            // 清理不存在的收藏项目
            FavoriteProjects.RemoveAll(p => !System.IO.File.Exists(p));
        }
    }

    /// <summary>
    /// 项目历史记录项
    /// </summary>
    public class ProjectHistoryItem
    {
        /// <summary>
        /// 项目路径
        /// </summary>
        [JsonProperty("projectPath")]
        public string ProjectPath { get; set; } = string.Empty;

        /// <summary>
        /// 项目名称
        /// </summary>
        [JsonProperty("projectName")]
        public string ProjectName { get; set; } = string.Empty;

        /// <summary>
        /// 最后打开时间
        /// </summary>
        [JsonProperty("lastOpenedTime")]
        public DateTime LastOpenedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [JsonProperty("lastModifiedTime")]
        public DateTime LastModifiedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 项目类型（从项目文件中读取或推断）
        /// </summary>
        [JsonProperty("projectType")]
        public string ProjectType { get; set; } = "CEEdit项目";

        /// <summary>
        /// 项目描述
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 项目标签
        /// </summary>
        [JsonProperty("tags")]
        public List<string> Tags { get; set; } = new();

        /// <summary>
        /// 获取友好的最后打开时间显示
        /// </summary>
        [JsonIgnore]
        public string LastOpenedDisplay
        {
            get
            {
                var timeSpan = DateTime.Now - LastOpenedTime;
                return timeSpan.TotalSeconds switch
                {
                    < 60 => "刚刚",
                    < 3600 => $"{(int)timeSpan.TotalMinutes} 分钟前",
                    < 86400 => $"{(int)timeSpan.TotalHours} 小时前",
                    < 2592000 => $"{(int)timeSpan.TotalDays} 天前",
                    _ => LastOpenedTime.ToString("yyyy-MM-dd")
                };
            }
        }

        /// <summary>
        /// 获取友好的最后修改时间显示
        /// </summary>
        [JsonIgnore]
        public string LastModifiedDisplay
        {
            get
            {
                var timeSpan = DateTime.Now - LastModifiedTime;
                return timeSpan.TotalSeconds switch
                {
                    < 60 => "刚刚",
                    < 3600 => $"{(int)timeSpan.TotalMinutes} 分钟前",
                    < 86400 => $"{(int)timeSpan.TotalHours} 小时前",
                    < 2592000 => $"{(int)timeSpan.TotalDays} 天前",
                    _ => LastModifiedTime.ToString("yyyy-MM-dd")
                };
            }
        }

        /// <summary>
        /// 检查项目文件是否存在
        /// </summary>
        [JsonIgnore]
        public bool ProjectExists => System.IO.File.Exists(ProjectPath);

        /// <summary>
        /// 更新最后打开时间
        /// </summary>
        public void UpdateLastOpenedTime()
        {
            LastOpenedTime = DateTime.Now;
            
            // 同时更新最后修改时间（如果文件存在）
            if (ProjectExists)
            {
                LastModifiedTime = System.IO.File.GetLastWriteTime(ProjectPath);
            }
        }
    }
}
