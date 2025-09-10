namespace CEEdit.Core.Services.Interfaces
{
    /// <summary>
    /// 文件管理服务接口
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// 文件改变事件
        /// </summary>
        event EventHandler<FileChangedEventArgs>? FileChanged;

        /// <summary>
        /// 读取文本文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>文件内容</returns>
        Task<string> ReadTextFileAsync(string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 写入文本文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>写入任务</returns>
        Task WriteTextFileAsync(string filePath, string content, CancellationToken cancellationToken = default);

        /// <summary>
        /// 读取二进制文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>文件内容</returns>
        Task<byte[]> ReadBinaryFileAsync(string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 写入二进制文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="data">文件数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>写入任务</returns>
        Task WriteBinaryFileAsync(string filePath, byte[] data, CancellationToken cancellationToken = default);

        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>是否存在</returns>
        Task<bool> FileExistsAsync(string filePath);

        /// <summary>
        /// 检查目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns>是否存在</returns>
        Task<bool> DirectoryExistsAsync(string directoryPath);

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>创建任务</returns>
        Task CreateDirectoryAsync(string directoryPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>删除任务</returns>
        Task DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <param name="recursive">是否递归删除</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>删除任务</returns>
        Task DeleteDirectoryAsync(string directoryPath, bool recursive = false, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="destinationPath">目标文件路径</param>
        /// <param name="overwrite">是否覆盖</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>复制任务</returns>
        Task CopyFileAsync(string sourcePath, string destinationPath, bool overwrite = false, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="destinationPath">目标文件路径</param>
        /// <param name="overwrite">是否覆盖</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>移动任务</returns>
        Task MoveFileAsync(string sourcePath, string destinationPath, bool overwrite = false, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 复制目录
        /// </summary>
        /// <param name="sourcePath">源目录路径</param>
        /// <param name="destinationPath">目标目录路径</param>
        /// <param name="recursive">是否递归复制</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>复制任务</returns>
        Task CopyDirectoryAsync(string sourcePath, string destinationPath, bool recursive = true, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件信息</returns>
        Task<FileInformation?> GetFileInformationAsync(string filePath);

        /// <summary>
        /// 获取目录信息
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns>目录信息</returns>
        Task<DirectoryInformation?> GetDirectoryInformationAsync(string directoryPath);

        /// <summary>
        /// 列出目录中的文件
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <param name="searchPattern">搜索模式</param>
        /// <param name="recursive">是否递归搜索</param>
        /// <returns>文件列表</returns>
        Task<List<string>> ListFilesAsync(string directoryPath, string searchPattern = "*", bool recursive = false);

        /// <summary>
        /// 列出目录中的子目录
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <param name="searchPattern">搜索模式</param>
        /// <param name="recursive">是否递归搜索</param>
        /// <returns>目录列表</returns>
        Task<List<string>> ListDirectoriesAsync(string directoryPath, string searchPattern = "*", bool recursive = false);

        /// <summary>
        /// 计算文件哈希值
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="algorithm">哈希算法</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>哈希值</returns>
        Task<string> CalculateFileHashAsync(string filePath, HashAlgorithm algorithm = HashAlgorithm.MD5,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 监视文件变化
        /// </summary>
        /// <param name="path">监视路径</param>
        /// <param name="includeSubdirectories">是否包含子目录</param>
        /// <param name="filter">文件筛选器</param>
        /// <returns>监视任务</returns>
        Task StartFileWatchingAsync(string path, bool includeSubdirectories = false, string filter = "*.*");

        /// <summary>
        /// 停止文件监视
        /// </summary>
        /// <param name="path">监视路径</param>
        /// <returns>停止任务</returns>
        Task StopFileWatchingAsync(string path);

        /// <summary>
        /// 停止所有文件监视
        /// </summary>
        /// <returns>停止任务</returns>
        Task StopAllFileWatchingAsync();

        /// <summary>
        /// 获取临时文件路径
        /// </summary>
        /// <param name="extension">文件扩展名</param>
        /// <returns>临时文件路径</returns>
        string GetTempFilePath(string extension = ".tmp");

        /// <summary>
        /// 获取临时目录路径
        /// </summary>
        /// <returns>临时目录路径</returns>
        string GetTempDirectoryPath();

        /// <summary>
        /// 清理临时文件
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>清理任务</returns>
        Task CleanupTempFilesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 压缩文件或目录
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="zipFilePath">压缩文件路径</param>
        /// <param name="compressionLevel">压缩级别</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>压缩任务</returns>
        Task CompressAsync(string sourcePath, string zipFilePath, CompressionLevel compressionLevel = CompressionLevel.Optimal,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="zipFilePath">压缩文件路径</param>
        /// <param name="extractPath">解压路径</param>
        /// <param name="overwrite">是否覆盖</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>解压任务</returns>
        Task ExtractAsync(string zipFilePath, string extractPath, bool overwrite = false,
            CancellationToken cancellationToken = default);
    }

    #region 事件参数和枚举

    /// <summary>
    /// 文件改变事件参数
    /// </summary>
    public class FileChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// 改变类型
        /// </summary>
        public FileChangeType ChangeType { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="changeType">改变类型</param>
        public FileChangedEventArgs(string filePath, FileChangeType changeType)
        {
            FilePath = filePath;
            ChangeType = changeType;
        }
    }

    /// <summary>
    /// 文件改变类型枚举
    /// </summary>
    public enum FileChangeType
    {
        Created,
        Modified,
        Deleted,
        Renamed
    }

    /// <summary>
    /// 哈希算法枚举
    /// </summary>
    public enum HashAlgorithm
    {
        MD5,
        SHA1,
        SHA256,
        SHA512
    }

    /// <summary>
    /// 压缩级别枚举
    /// </summary>
    public enum CompressionLevel
    {
        Optimal,
        Fastest,
        NoCompression,
        SmallestSize
    }

    #endregion

    #region 辅助类

    /// <summary>
    /// 文件信息
    /// </summary>
    public class FileInformation
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FullPath { get; set; } = string.Empty;

        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastWriteTime { get; set; }

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastAccessTime { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string Extension { get; set; } = string.Empty;

        /// <summary>
        /// 是否为只读
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// 是否为隐藏文件
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// 文件哈希值
        /// </summary>
        public string Hash { get; set; } = string.Empty;
    }

    /// <summary>
    /// 目录信息
    /// </summary>
    public class DirectoryInformation
    {
        /// <summary>
        /// 目录名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 目录路径
        /// </summary>
        public string FullPath { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastWriteTime { get; set; }

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastAccessTime { get; set; }

        /// <summary>
        /// 文件数量
        /// </summary>
        public int FileCount { get; set; }

        /// <summary>
        /// 子目录数量
        /// </summary>
        public int DirectoryCount { get; set; }

        /// <summary>
        /// 总大小（字节）
        /// </summary>
        public long TotalSize { get; set; }

        /// <summary>
        /// 是否为隐藏目录
        /// </summary>
        public bool IsHidden { get; set; }
    }

    #endregion
}

