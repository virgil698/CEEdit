using CEEdit.Core.Models.Project;
using CEEdit.Core.Models.Blocks;
using CEEdit.Core.Models.Items;
using CEEdit.Core.Models.Recipes;
using CEEdit.Core.Models.Resources;
using CEEdit.Core.Models.Common;

namespace CEEdit.Core.Services.Interfaces
{
    /// <summary>
    /// 验证服务接口
    /// </summary>
    public interface IValidationService
    {
        /// <summary>
        /// 验证完成事件
        /// </summary>
        event EventHandler<ValidationCompletedEventArgs>? ValidationCompleted;

        /// <summary>
        /// 验证进度事件
        /// </summary>
        event EventHandler<ValidationProgressEventArgs>? ValidationProgress;

        /// <summary>
        /// 验证项目
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="validationLevel">验证级别</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        Task<ProjectValidationResult> ValidateProjectAsync(CraftEngineProject project, 
            ValidationLevel validationLevel = ValidationLevel.Warning, CancellationToken cancellationToken = default);

        /// <summary>
        /// 验证方块
        /// </summary>
        /// <param name="block">方块</param>
        /// <param name="context">验证上下文</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateBlockAsync(Block block, ValidationContext? context = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 验证物品
        /// </summary>
        /// <param name="item">物品</param>
        /// <param name="context">验证上下文</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateItemAsync(Item item, ValidationContext? context = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 验证配方
        /// </summary>
        /// <param name="recipe">配方</param>
        /// <param name="context">验证上下文</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateRecipeAsync(Recipe recipe, ValidationContext? context = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 验证资源
        /// </summary>
        /// <param name="resource">资源</param>
        /// <param name="context">验证上下文</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateResourceAsync(ResourceBase resource, ValidationContext? context = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量验证
        /// </summary>
        /// <param name="items">验证项目列表</param>
        /// <param name="context">验证上下文</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果列表</returns>
        Task<List<ValidationResult>> ValidateBatchAsync(IEnumerable<object> items, ValidationContext? context = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 验证ID唯一性
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="itemType">项目类型</param>
        /// <param name="project">项目</param>
        /// <param name="excludeItem">排除的项目</param>
        /// <returns>是否唯一</returns>
        Task<bool> ValidateIdUniquenessAsync(string id, Type itemType, CraftEngineProject project, object? excludeItem = null);

        /// <summary>
        /// 验证依赖关系
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>依赖验证结果</returns>
        Task<DependencyValidationResult> ValidateDependenciesAsync(CraftEngineProject project,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 验证兼容性
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="targetVersion">目标版本</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>兼容性验证结果</returns>
        Task<CompatibilityValidationResult> ValidateCompatibilityAsync(CraftEngineProject project, string targetVersion,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 验证性能影响
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>性能验证结果</returns>
        Task<PerformanceValidationResult> ValidatePerformanceAsync(CraftEngineProject project,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 自定义验证规则
        /// </summary>
        /// <param name="rule">验证规则</param>
        void AddCustomValidationRule(IValidationRule rule);

        /// <summary>
        /// 移除自定义验证规则
        /// </summary>
        /// <param name="ruleName">规则名称</param>
        void RemoveCustomValidationRule(string ruleName);

        /// <summary>
        /// 获取所有验证规则
        /// </summary>
        /// <returns>验证规则列表</returns>
        List<IValidationRule> GetValidationRules();

        /// <summary>
        /// 启用/禁用验证规则
        /// </summary>
        /// <param name="ruleName">规则名称</param>
        /// <param name="enabled">是否启用</param>
        void SetValidationRuleEnabled(string ruleName, bool enabled);

        /// <summary>
        /// 实时验证
        /// </summary>
        /// <param name="item">要验证的项目</param>
        /// <param name="context">验证上下文</param>
        /// <returns>验证结果</returns>
        ValidationResult ValidateRealtime(object item, ValidationContext? context = null);

        /// <summary>
        /// 清理验证缓存
        /// </summary>
        void ClearValidationCache();

        /// <summary>
        /// 获取验证统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        ValidationStatistics GetValidationStatistics();
    }

    #region 事件参数

    /// <summary>
    /// 验证完成事件参数
    /// </summary>
    public class ValidationCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// 验证结果
        /// </summary>
        public ValidationResult Result { get; }

        /// <summary>
        /// 验证的目标
        /// </summary>
        public object Target { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="result">验证结果</param>
        /// <param name="target">验证目标</param>
        public ValidationCompletedEventArgs(ValidationResult result, object target)
        {
            Result = result;
            Target = target;
        }
    }

    /// <summary>
    /// 验证进度事件参数
    /// </summary>
    public class ValidationProgressEventArgs : EventArgs
    {
        /// <summary>
        /// 当前进度（0-100）
        /// </summary>
        public int ProgressPercentage { get; }

        /// <summary>
        /// 当前正在验证的项目
        /// </summary>
        public string CurrentItem { get; }

        /// <summary>
        /// 总项目数
        /// </summary>
        public int TotalItems { get; }

        /// <summary>
        /// 已完成项目数
        /// </summary>
        public int CompletedItems { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="progressPercentage">进度百分比</param>
        /// <param name="currentItem">当前项目</param>
        /// <param name="completedItems">已完成项目数</param>
        /// <param name="totalItems">总项目数</param>
        public ValidationProgressEventArgs(int progressPercentage, string currentItem, int completedItems, int totalItems)
        {
            ProgressPercentage = progressPercentage;
            CurrentItem = currentItem;
            CompletedItems = completedItems;
            TotalItems = totalItems;
        }
    }

    #endregion

    #region 辅助类

    /// <summary>
    /// 验证上下文
    /// </summary>
    public class ValidationContext
    {
        /// <summary>
        /// 关联的项目
        /// </summary>
        public CraftEngineProject? Project { get; set; }

        /// <summary>
        /// 验证级别
        /// </summary>
        public ValidationLevel Level { get; set; } = ValidationLevel.Warning;

        /// <summary>
        /// 验证选项
        /// </summary>
        public ValidationOptions Options { get; set; } = new();

        /// <summary>
        /// 自定义属性
        /// </summary>
        public Dictionary<string, object> Properties { get; set; } = new();
    }

    /// <summary>
    /// 验证选项
    /// </summary>
    public class ValidationOptions
    {
        /// <summary>
        /// 启用实时验证
        /// </summary>
        public bool EnableRealtimeValidation { get; set; } = true;

        /// <summary>
        /// 验证依赖关系
        /// </summary>
        public bool ValidateDependencies { get; set; } = true;

        /// <summary>
        /// 验证性能影响
        /// </summary>
        public bool ValidatePerformance { get; set; } = false;

        /// <summary>
        /// 验证兼容性
        /// </summary>
        public bool ValidateCompatibility { get; set; } = true;

        /// <summary>
        /// 验证资源完整性
        /// </summary>
        public bool ValidateResourceIntegrity { get; set; } = true;

        /// <summary>
        /// 严格模式
        /// </summary>
        public bool StrictMode { get; set; } = false;

        /// <summary>
        /// 忽略的验证规则
        /// </summary>
        public List<string> IgnoredRules { get; set; } = new();
    }

    /// <summary>
    /// 项目验证结果
    /// </summary>
    public class ProjectValidationResult
    {
        /// <summary>
        /// 是否通过验证
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 总体验证级别
        /// </summary>
        public ValidationLevel OverallLevel { get; set; }

        /// <summary>
        /// 方块验证结果
        /// </summary>
        public List<ValidationResult> BlockValidationResults { get; set; } = new();

        /// <summary>
        /// 物品验证结果
        /// </summary>
        public List<ValidationResult> ItemValidationResults { get; set; } = new();

        /// <summary>
        /// 配方验证结果
        /// </summary>
        public List<ValidationResult> RecipeValidationResults { get; set; } = new();

        /// <summary>
        /// 资源验证结果
        /// </summary>
        public List<ValidationResult> ResourceValidationResults { get; set; } = new();

        /// <summary>
        /// 依赖验证结果
        /// </summary>
        public DependencyValidationResult? DependencyValidation { get; set; }

        /// <summary>
        /// 兼容性验证结果
        /// </summary>
        public CompatibilityValidationResult? CompatibilityValidation { get; set; }

        /// <summary>
        /// 性能验证结果
        /// </summary>
        public PerformanceValidationResult? PerformanceValidation { get; set; }

        /// <summary>
        /// 验证摘要
        /// </summary>
        public ValidationSummary Summary { get; set; } = new();
    }

    /// <summary>
    /// 依赖验证结果
    /// </summary>
    public class DependencyValidationResult
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 缺失的依赖
        /// </summary>
        public List<string> MissingDependencies { get; set; } = new();

        /// <summary>
        /// 版本冲突
        /// </summary>
        public List<VersionConflict> VersionConflicts { get; set; } = new();

        /// <summary>
        /// 循环依赖
        /// </summary>
        public List<CircularDependency> CircularDependencies { get; set; } = new();
    }

    /// <summary>
    /// 兼容性验证结果
    /// </summary>
    public class CompatibilityValidationResult
    {
        /// <summary>
        /// 是否兼容
        /// </summary>
        public bool IsCompatible { get; set; }

        /// <summary>
        /// 目标版本
        /// </summary>
        public string TargetVersion { get; set; } = string.Empty;

        /// <summary>
        /// 不兼容的功能
        /// </summary>
        public List<IncompatibleFeature> IncompatibleFeatures { get; set; } = new();

        /// <summary>
        /// 建议的修改
        /// </summary>
        public List<string> SuggestedChanges { get; set; } = new();
    }

    /// <summary>
    /// 性能验证结果
    /// </summary>
    public class PerformanceValidationResult
    {
        /// <summary>
        /// 性能等级
        /// </summary>
        public PerformanceLevel PerformanceLevel { get; set; }

        /// <summary>
        /// 性能问题
        /// </summary>
        public List<PerformanceIssue> Issues { get; set; } = new();

        /// <summary>
        /// 优化建议
        /// </summary>
        public List<string> OptimizationSuggestions { get; set; } = new();

        /// <summary>
        /// 预估内存使用
        /// </summary>
        public long EstimatedMemoryUsage { get; set; }

        /// <summary>
        /// 预估CPU负载
        /// </summary>
        public float EstimatedCpuLoad { get; set; }
    }

    /// <summary>
    /// 验证摘要
    /// </summary>
    public class ValidationSummary
    {
        /// <summary>
        /// 错误数量
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// 警告数量
        /// </summary>
        public int WarningCount { get; set; }

        /// <summary>
        /// 信息数量
        /// </summary>
        public int InfoCount { get; set; }

        /// <summary>
        /// 验证时间
        /// </summary>
        public TimeSpan ValidationTime { get; set; }

        /// <summary>
        /// 验证的项目总数
        /// </summary>
        public int TotalItemsValidated { get; set; }
    }

    /// <summary>
    /// 版本冲突
    /// </summary>
    public class VersionConflict
    {
        /// <summary>
        /// 依赖名称
        /// </summary>
        public string DependencyName { get; set; } = string.Empty;

        /// <summary>
        /// 要求的版本
        /// </summary>
        public string RequiredVersion { get; set; } = string.Empty;

        /// <summary>
        /// 实际版本
        /// </summary>
        public string ActualVersion { get; set; } = string.Empty;

        /// <summary>
        /// 冲突描述
        /// </summary>
        public string ConflictDescription { get; set; } = string.Empty;
    }

    /// <summary>
    /// 循环依赖
    /// </summary>
    public class CircularDependency
    {
        /// <summary>
        /// 依赖链
        /// </summary>
        public List<string> DependencyChain { get; set; } = new();

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// 不兼容的功能
    /// </summary>
    public class IncompatibleFeature
    {
        /// <summary>
        /// 功能名称
        /// </summary>
        public string FeatureName { get; set; } = string.Empty;

        /// <summary>
        /// 引入版本
        /// </summary>
        public string IntroducedInVersion { get; set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 替代方案
        /// </summary>
        public string? Alternative { get; set; }
    }

    /// <summary>
    /// 性能问题
    /// </summary>
    public class PerformanceIssue
    {
        /// <summary>
        /// 问题类型
        /// </summary>
        public PerformanceIssueType Type { get; set; }

        /// <summary>
        /// 严重程度
        /// </summary>
        public Severity Severity { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 影响的组件
        /// </summary>
        public string AffectedComponent { get; set; } = string.Empty;

        /// <summary>
        /// 建议的解决方案
        /// </summary>
        public string SuggestedSolution { get; set; } = string.Empty;
    }

    /// <summary>
    /// 验证统计信息
    /// </summary>
    public class ValidationStatistics
    {
        /// <summary>
        /// 总验证次数
        /// </summary>
        public long TotalValidations { get; set; }

        /// <summary>
        /// 平均验证时间
        /// </summary>
        public TimeSpan AverageValidationTime { get; set; }

        /// <summary>
        /// 缓存命中率
        /// </summary>
        public double CacheHitRate { get; set; }

        /// <summary>
        /// 最近验证次数
        /// </summary>
        public int RecentValidations { get; set; }

        /// <summary>
        /// 错误趋势
        /// </summary>
        public List<ValidationTrendData> ErrorTrend { get; set; } = new();
    }

    /// <summary>
    /// 验证趋势数据
    /// </summary>
    public class ValidationTrendData
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 错误数量
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// 警告数量
        /// </summary>
        public int WarningCount { get; set; }
    }

    #endregion

    #region 枚举

    /// <summary>
    /// 性能等级
    /// </summary>
    public enum PerformanceLevel
    {
        Excellent,
        Good,
        Fair,
        Poor,
        Critical
    }

    /// <summary>
    /// 性能问题类型
    /// </summary>
    public enum PerformanceIssueType
    {
        HighMemoryUsage,
        ExcessiveComputations,
        FrequentFileAccess,
        LargeTextures,
        ComplexModels,
        TooManyEntities,
        NetworkBottleneck
    }

    /// <summary>
    /// 严重程度
    /// </summary>
    public enum Severity
    {
        Low,
        Medium,
        High,
        Critical
    }

    #endregion

    #region 验证规则接口

    /// <summary>
    /// 验证规则接口
    /// </summary>
    public interface IValidationRule
    {
        /// <summary>
        /// 规则名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 规则描述
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 是否启用
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// 应用的类型
        /// </summary>
        Type[] ApplicableTypes { get; }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="item">要验证的项目</param>
        /// <param name="context">验证上下文</param>
        /// <returns>验证结果</returns>
        ValidationResult Validate(object item, ValidationContext? context = null);
    }

    #endregion

    #region 缺失的类型定义

    /// <summary>
    /// 验证结果
    /// </summary>
    public class ValidationServiceResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public ValidationSummary Summary => new ValidationSummary 
        { 
            ErrorCount = Errors.Count, 
            WarningCount = Warnings.Count 
        };
    }


    #endregion
}

