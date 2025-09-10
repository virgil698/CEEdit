using CEEdit.Core.Models.Resources;
using CEEdit.Core.Models.Common;

namespace CEEdit.Core.Services.Interfaces
{
    /// <summary>
    /// 资源管理服务接口
    /// </summary>
    public interface IResourceService
    {
        /// <summary>
        /// 资源改变事件
        /// </summary>
        event EventHandler<ResourceChangedEventArgs>? ResourceChanged;

        /// <summary>
        /// 资源加载进度事件
        /// </summary>
        event EventHandler<ResourceLoadProgressEventArgs>? ResourceLoadProgress;

        /// <summary>
        /// 加载纹理
        /// </summary>
        /// <param name="texturePath">纹理路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>纹理对象</returns>
        Task<Texture?> LoadTextureAsync(string texturePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 保存纹理
        /// </summary>
        /// <param name="texture">纹理对象</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>保存任务</returns>
        Task SaveTextureAsync(Texture texture, string outputPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 加载3D模型
        /// </summary>
        /// <param name="modelPath">模型路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>3D模型对象</returns>
        Task<Model3D?> LoadModelAsync(string modelPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 保存3D模型
        /// </summary>
        /// <param name="model">3D模型对象</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="format">保存格式</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>保存任务</returns>
        Task SaveModelAsync(Model3D model, string outputPath, ModelFormat format = ModelFormat.Json,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 加载音频
        /// </summary>
        /// <param name="audioPath">音频路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>音频对象</returns>
        Task<AudioClip?> LoadAudioAsync(string audioPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 保存音频
        /// </summary>
        /// <param name="audio">音频对象</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="format">音频格式</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>保存任务</returns>
        Task SaveAudioAsync(AudioClip audio, string outputPath, AudioFormat format = AudioFormat.Ogg,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 加载语言文件
        /// </summary>
        /// <param name="languageFilePath">语言文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>语言文件对象</returns>
        Task<LanguageFile?> LoadLanguageFileAsync(string languageFilePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 保存语言文件
        /// </summary>
        /// <param name="languageFile">语言文件对象</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>保存任务</returns>
        Task SaveLanguageFileAsync(LanguageFile languageFile, string outputPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 优化资源包
        /// </summary>
        /// <param name="resourcePack">资源包</param>
        /// <param name="optimizationOptions">优化选项</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>优化任务</returns>
        Task<ResourceOptimizationResult> OptimizeResourcePackAsync(ResourcePack resourcePack, 
            ResourceOptimizationOptions optimizationOptions, CancellationToken cancellationToken = default);

        /// <summary>
        /// 验证资源
        /// </summary>
        /// <param name="resource">资源</param>
        /// <param name="validationOptions">验证选项</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateResourceAsync(ResourceBase resource, 
            ResourceValidationOptions? validationOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量验证资源
        /// </summary>
        /// <param name="resources">资源列表</param>
        /// <param name="validationOptions">验证选项</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        Task<List<ValidationResult>> ValidateResourcesAsync(IEnumerable<ResourceBase> resources,
            ResourceValidationOptions? validationOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 导入资源
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="resourceType">资源类型</param>
        /// <param name="importOptions">导入选项</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>导入的资源</returns>
        Task<ResourceBase?> ImportResourceAsync(string sourcePath, ResourceType resourceType,
            ResourceImportOptions? importOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量导入资源
        /// </summary>
        /// <param name="sourcePaths">源路径列表</param>
        /// <param name="importOptions">导入选项</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>导入的资源列表</returns>
        Task<List<ResourceBase>> ImportResourcesAsync(IEnumerable<string> sourcePaths,
            ResourceImportOptions? importOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 导出资源
        /// </summary>
        /// <param name="resource">资源</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="exportOptions">导出选项</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>导出任务</returns>
        Task ExportResourceAsync(ResourceBase resource, string outputPath,
            ResourceExportOptions? exportOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 转换纹理格式
        /// </summary>
        /// <param name="texture">纹理</param>
        /// <param name="targetFormat">目标格式</param>
        /// <param name="quality">质量</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>转换后的纹理</returns>
        Task<Texture?> ConvertTextureFormatAsync(Texture texture, TextureFormat targetFormat, 
            int quality = 95, CancellationToken cancellationToken = default);

        /// <summary>
        /// 调整纹理尺寸
        /// </summary>
        /// <param name="texture">纹理</param>
        /// <param name="newWidth">新宽度</param>
        /// <param name="newHeight">新高度</param>
        /// <param name="resizeMode">调整模式</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>调整后的纹理</returns>
        Task<Texture?> ResizeTextureAsync(Texture texture, int newWidth, int newHeight, 
            ResizeMode resizeMode = ResizeMode.Stretch, CancellationToken cancellationToken = default);

        /// <summary>
        /// 生成纹理缩略图
        /// </summary>
        /// <param name="texture">纹理</param>
        /// <param name="thumbnailSize">缩略图尺寸</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>缩略图数据</returns>
        Task<byte[]?> GenerateThumbnailAsync(Texture texture, int thumbnailSize = 64, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取资源统计信息
        /// </summary>
        /// <param name="resourcePack">资源包</param>
        /// <returns>统计信息</returns>
        ResourceStatistics GetResourceStatistics(ResourcePack resourcePack);

        /// <summary>
        /// 清理未使用的资源
        /// </summary>
        /// <param name="resourcePack">资源包</param>
        /// <param name="usedResourceIds">使用中的资源ID列表</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>清理结果</returns>
        Task<ResourceCleanupResult> CleanupUnusedResourcesAsync(ResourcePack resourcePack, 
            IEnumerable<string> usedResourceIds, CancellationToken cancellationToken = default);

        /// <summary>
        /// 检查资源完整性
        /// </summary>
        /// <param name="resourcePack">资源包</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>完整性检查结果</returns>
        Task<ResourceIntegrityResult> CheckResourceIntegrityAsync(ResourcePack resourcePack, 
            CancellationToken cancellationToken = default);
    }

    #region 事件参数

    /// <summary>
    /// 资源改变事件参数
    /// </summary>
    public class ResourceChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 资源
        /// </summary>
        public ResourceBase Resource { get; }

        /// <summary>
        /// 改变类型
        /// </summary>
        public ResourceChangeType ChangeType { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resource">资源</param>
        /// <param name="changeType">改变类型</param>
        public ResourceChangedEventArgs(ResourceBase resource, ResourceChangeType changeType)
        {
            Resource = resource;
            ChangeType = changeType;
        }
    }

    /// <summary>
    /// 资源加载进度事件参数
    /// </summary>
    public class ResourceLoadProgressEventArgs : EventArgs
    {
        /// <summary>
        /// 资源路径
        /// </summary>
        public string ResourcePath { get; }

        /// <summary>
        /// 进度百分比（0-100）
        /// </summary>
        public int ProgressPercentage { get; }

        /// <summary>
        /// 状态消息
        /// </summary>
        public string StatusMessage { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resourcePath">资源路径</param>
        /// <param name="progressPercentage">进度百分比</param>
        /// <param name="statusMessage">状态消息</param>
        public ResourceLoadProgressEventArgs(string resourcePath, int progressPercentage, string statusMessage)
        {
            ResourcePath = resourcePath;
            ProgressPercentage = progressPercentage;
            StatusMessage = statusMessage;
        }
    }

    #endregion

    #region 枚举

    /// <summary>
    /// 资源改变类型
    /// </summary>
    public enum ResourceChangeType
    {
        Added,
        Modified,
        Removed,
        Renamed
    }

    /// <summary>
    /// 模型格式
    /// </summary>
    public enum ModelFormat
    {
        Json,
        Bbmodel,
        Obj,
        Fbx,
        Gltf
    }

    /// <summary>
    /// 音频格式
    /// </summary>
    public enum AudioFormat
    {
        Ogg,
        Wav,
        Mp3,
        Flac
    }

    /// <summary>
    /// 纹理格式
    /// </summary>
    public enum TextureFormat
    {
        Png,
        Jpg,
        Bmp,
        Tiff,
        Webp
    }

    /// <summary>
    /// 调整模式
    /// </summary>
    public enum ResizeMode
    {
        Stretch,
        Fit,
        Fill,
        Crop
    }

    #endregion

    #region 辅助类

    /// <summary>
    /// 资源优化选项
    /// </summary>
    public class ResourceOptimizationOptions
    {
        /// <summary>
        /// 优化纹理
        /// </summary>
        public bool OptimizeTextures { get; set; } = true;

        /// <summary>
        /// 优化模型
        /// </summary>
        public bool OptimizeModels { get; set; } = true;

        /// <summary>
        /// 优化音频
        /// </summary>
        public bool OptimizeAudio { get; set; } = true;

        /// <summary>
        /// 纹理质量（0-100）
        /// </summary>
        public int TextureQuality { get; set; } = 85;

        /// <summary>
        /// 音频质量（0-100）
        /// </summary>
        public int AudioQuality { get; set; } = 80;

        /// <summary>
        /// 删除未使用的资源
        /// </summary>
        public bool RemoveUnusedResources { get; set; } = false;
    }

    /// <summary>
    /// 资源优化结果
    /// </summary>
    public class ResourceOptimizationResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 优化前的大小
        /// </summary>
        public long OriginalSize { get; set; }

        /// <summary>
        /// 优化后的大小
        /// </summary>
        public long OptimizedSize { get; set; }

        /// <summary>
        /// 节省的空间
        /// </summary>
        public long SpaceSaved => OriginalSize - OptimizedSize;

        /// <summary>
        /// 压缩率
        /// </summary>
        public double CompressionRatio => OriginalSize > 0 ? (double)SpaceSaved / OriginalSize * 100 : 0;

        /// <summary>
        /// 优化的资源数量
        /// </summary>
        public int OptimizedResourceCount { get; set; }

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
    /// 资源验证选项
    /// </summary>
    public class ResourceValidationOptions
    {
        /// <summary>
        /// 检查文件完整性
        /// </summary>
        public bool CheckFileIntegrity { get; set; } = true;

        /// <summary>
        /// 检查格式有效性
        /// </summary>
        public bool CheckFormatValidity { get; set; } = true;

        /// <summary>
        /// 检查性能影响
        /// </summary>
        public bool CheckPerformanceImpact { get; set; } = true;

        /// <summary>
        /// 检查兼容性
        /// </summary>
        public bool CheckCompatibility { get; set; } = true;
    }

    /// <summary>
    /// 资源导入选项
    /// </summary>
    public class ResourceImportOptions
    {
        /// <summary>
        /// 覆盖现有资源
        /// </summary>
        public bool OverwriteExisting { get; set; } = false;

        /// <summary>
        /// 生成缩略图
        /// </summary>
        public bool GenerateThumbnails { get; set; } = true;

        /// <summary>
        /// 自动优化
        /// </summary>
        public bool AutoOptimize { get; set; } = false;

        /// <summary>
        /// 目标目录
        /// </summary>
        public string? TargetDirectory { get; set; }
    }

    /// <summary>
    /// 资源导出选项
    /// </summary>
    public class ResourceExportOptions
    {
        /// <summary>
        /// 包含元数据
        /// </summary>
        public bool IncludeMetadata { get; set; } = true;

        /// <summary>
        /// 压缩输出
        /// </summary>
        public bool CompressOutput { get; set; } = false;

        /// <summary>
        /// 导出格式
        /// </summary>
        public string? ExportFormat { get; set; }
    }

    /// <summary>
    /// 资源清理结果
    /// </summary>
    public class ResourceCleanupResult
    {
        /// <summary>
        /// 已清理的资源数量
        /// </summary>
        public int CleanedResourceCount { get; set; }

        /// <summary>
        /// 释放的空间（字节）
        /// </summary>
        public long SpaceFreed { get; set; }

        /// <summary>
        /// 已清理的资源列表
        /// </summary>
        public List<string> CleanedResources { get; set; } = new();
    }

    /// <summary>
    /// 资源完整性结果
    /// </summary>
    public class ResourceIntegrityResult
    {
        /// <summary>
        /// 所有资源是否完整
        /// </summary>
        public bool IsIntegrityValid { get; set; }

        /// <summary>
        /// 损坏的资源列表
        /// </summary>
        public List<string> CorruptedResources { get; set; } = new();

        /// <summary>
        /// 缺失的资源列表
        /// </summary>
        public List<string> MissingResources { get; set; } = new();

        /// <summary>
        /// 验证错误列表
        /// </summary>
        public List<string> ValidationErrors { get; set; } = new();
    }

    #endregion
}

