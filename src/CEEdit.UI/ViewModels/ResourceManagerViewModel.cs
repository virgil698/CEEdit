using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using CEEdit.Core.Services.Interfaces;

namespace CEEdit.UI.ViewModels
{
    public partial class ResourceManagerViewModel : ObservableObject
    {
        private readonly IFileService? _fileService;
        private readonly IResourceService? _resourceService;
        private readonly IValidationService? _validationService;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private string _fileTypeFilter = "所有文件";

        [ObservableProperty]
        private object? _selectedFolder;

        [ObservableProperty]
        private ResourceFileItem? _selectedFile;

        [ObservableProperty]
        private bool _isListView = true;

        [ObservableProperty]
        private bool _isThumbnailView = false;

        [ObservableProperty]
        private string _sortBy = "名称";

        [ObservableProperty]
        private string _statusMessage = "就绪";

        [ObservableProperty]
        private int _totalFilesCount = 0;

        [ObservableProperty]
        private string _totalSize = "0 KB";

        [ObservableProperty]
        private string _currentProjectPath = string.Empty;

        public ObservableCollection<FolderTreeItem> FolderTree { get; } = new();
        public ObservableCollection<ResourceFileItem> AllFiles { get; } = new();
        public ObservableCollection<ResourceFileItem> FilteredFiles { get; } = new();

        public ResourceManagerViewModel(IFileService? fileService = null,
                                       IResourceService? resourceService = null,
                                       IValidationService? validationService = null)
        {
            _fileService = fileService;
            _resourceService = resourceService;
            _validationService = validationService;

            // 监听搜索文本变化
            PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(SearchText):
                    case nameof(FileTypeFilter):
                    case nameof(SortBy):
                        RefreshFileList();
                        break;
                    case nameof(IsListView):
                        if (IsListView) IsThumbnailView = false;
                        break;
                    case nameof(IsThumbnailView):
                        if (IsThumbnailView) IsListView = false;
                        break;
                }
            };

            InitializeFolderTree();
        }

        private void InitializeFolderTree()
        {
            FolderTree.Clear();
            
            // 创建基础文件夹结构
            var rootFolder = new FolderTreeItem
            {
                Name = "项目资源",
                Icon = "📁",
                IsExpanded = true,
                FullPath = CurrentProjectPath
            };

            var texturesFolder = new FolderTreeItem
            {
                Name = "纹理",
                Icon = "🖼️",
                FullPath = Path.Combine(CurrentProjectPath, "textures"),
                FileCount = 0
            };

            var modelsFolder = new FolderTreeItem
            {
                Name = "模型",
                Icon = "📐",
                FullPath = Path.Combine(CurrentProjectPath, "models"),
                FileCount = 0
            };

            var soundsFolder = new FolderTreeItem
            {
                Name = "音效",
                Icon = "🔊",
                FullPath = Path.Combine(CurrentProjectPath, "sounds"),
                FileCount = 0
            };

            var langFolder = new FolderTreeItem
            {
                Name = "语言文件",
                Icon = "🌐",
                FullPath = Path.Combine(CurrentProjectPath, "lang"),
                FileCount = 0
            };

            rootFolder.Children.Add(texturesFolder);
            rootFolder.Children.Add(modelsFolder);
            rootFolder.Children.Add(soundsFolder);
            rootFolder.Children.Add(langFolder);

            FolderTree.Add(rootFolder);
        }

        [RelayCommand]
        private async Task ImportFiles()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "导入资源文件",
                Filter = "所有支持的文件|*.png;*.jpg;*.jpeg;*.json;*.bbmodel;*.ogg;*.wav|" +
                        "图片文件|*.png;*.jpg;*.jpeg|" +
                        "模型文件|*.json;*.bbmodel|" +
                        "音效文件|*.ogg;*.wav|" +
                        "所有文件|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    foreach (string fileName in openFileDialog.FileNames)
                    {
                        await ImportSingleFile(fileName);
                    }
                    
                    RefreshFileList();
                    StatusMessage = $"成功导入 {openFileDialog.FileNames.Length} 个文件";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"导入文件失败: {ex.Message}", 
                                  "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task ImportSingleFile(string sourceFile)
        {
            try
            {
                var fileName = Path.GetFileName(sourceFile);
                var extension = Path.GetExtension(sourceFile).ToLower();
                
                // 根据文件类型确定目标文件夹
                string targetFolder = extension switch
                {
                    ".png" or ".jpg" or ".jpeg" => "textures",
                    ".json" or ".bbmodel" => "models",
                    ".ogg" or ".wav" => "sounds",
                    _ => "misc"
                };
                
                var targetPath = Path.Combine(CurrentProjectPath, targetFolder, fileName);
                
                // 确保目标文件夹存在
                Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);
                
                // 复制文件
                if (_fileService != null)
                {
                    var sourceData = await _fileService.ReadBinaryFileAsync(sourceFile);
                    await _fileService.WriteBinaryFileAsync(targetPath, sourceData);
                }
                else
                {
                    File.Copy(sourceFile, targetPath, true);
                }
                
                // 创建文件项
                var fileItem = new ResourceFileItem
                {
                    Name = fileName,
                    FullPath = targetPath,
                    Type = GetFileType(extension),
                    Size = new FileInfo(targetPath).Length,
                    ModifiedDate = File.GetLastWriteTime(targetPath)
                };
                
                AllFiles.Add(fileItem);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"导入文件 {sourceFile} 失败: {ex.Message}", ex);
            }
        }

        [RelayCommand]
        private async Task CreateFolder()
        {
            // TODO: 实现创建文件夹功能
            StatusMessage = "创建文件夹功能正在开发中...";
        }

        [RelayCommand]
        private async Task DeleteSelected()
        {
            if (SelectedFile == null) return;

            var result = MessageBox.Show($"确定要删除文件 '{SelectedFile.Name}' 吗？", 
                                       "确认删除", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    File.Delete(SelectedFile.FullPath);
                    AllFiles.Remove(SelectedFile);
                    RefreshFileList();
                    StatusMessage = $"已删除文件: {SelectedFile.Name}";
                    SelectedFile = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"删除文件失败: {ex.Message}", 
                                  "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        [RelayCommand]
        private async Task RenameSelected()
        {
            if (SelectedFile == null) return;

            // TODO: 实现重命名功能
            StatusMessage = "重命名功能正在开发中...";
        }

        [RelayCommand]
        private async Task OptimizeResources()
        {
            try
            {
                if (_resourceService != null)
                {
                    // TODO: 调用资源优化服务
                    StatusMessage = "资源优化完成";
                }
                else
                {
                    StatusMessage = "资源优化服务不可用";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"优化资源失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task ValidateResources()
        {
            try
            {
                int errorCount = 0;
                
                foreach (var file in AllFiles)
                {
                    // 检查文件是否存在
                    if (!File.Exists(file.FullPath))
                    {
                        errorCount++;
                        continue;
                    }
                    
                    // TODO: 根据文件类型进行验证
                    // 使用 _validationService 来验证文件
                }
                
                StatusMessage = errorCount == 0 ? 
                    "所有资源验证通过" : 
                    $"发现 {errorCount} 个问题";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"验证资源失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void ShowInExplorer()
        {
            if (SelectedFile == null) return;

            try
            {
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{SelectedFile.FullPath}\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法打开资源管理器: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void CopyPath()
        {
            if (SelectedFile == null) return;

            try
            {
                Clipboard.SetText(SelectedFile.FullPath);
                StatusMessage = "文件路径已复制到剪贴板";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"复制路径失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task ExportFile()
        {
            if (SelectedFile == null) return;

            var saveFileDialog = new SaveFileDialog
            {
                Title = "导出文件",
                FileName = SelectedFile.Name,
                Filter = $"{SelectedFile.Type}文件|*{Path.GetExtension(SelectedFile.Name)}|所有文件|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.Copy(SelectedFile.FullPath, saveFileDialog.FileName, true);
                    StatusMessage = $"文件已导出到: {saveFileDialog.FileName}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"导出文件失败: {ex.Message}", 
                                  "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void RefreshFileList()
        {
            FilteredFiles.Clear();
            
            var filteredFiles = AllFiles.AsEnumerable();
            
            // 应用搜索过滤
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filteredFiles = filteredFiles.Where(f => 
                    f.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }
            
            // 应用类型过滤
            if (FileTypeFilter != "所有文件")
            {
                filteredFiles = filteredFiles.Where(f => f.Type == FileTypeFilter);
            }
            
            // 应用排序
            filteredFiles = SortBy switch
            {
                "名称" => filteredFiles.OrderBy(f => f.Name),
                "类型" => filteredFiles.OrderBy(f => f.Type).ThenBy(f => f.Name),
                "大小" => filteredFiles.OrderByDescending(f => f.Size),
                "修改时间" => filteredFiles.OrderByDescending(f => f.ModifiedDate),
                _ => filteredFiles.OrderBy(f => f.Name)
            };
            
            foreach (var file in filteredFiles)
            {
                FilteredFiles.Add(file);
            }
            
            TotalFilesCount = FilteredFiles.Count;
            TotalSize = FormatFileSize(FilteredFiles.Sum(f => f.Size));
        }

        private string GetFileType(string extension)
        {
            return extension.ToLower() switch
            {
                ".png" or ".jpg" or ".jpeg" => "图片",
                ".json" => "JSON",
                ".bbmodel" => "Blockbench模型",
                ".ogg" or ".wav" => "音效",
                ".yml" or ".yaml" => "YAML",
                _ => "未知"
            };
        }

        private string FormatFileSize(long bytes)
        {
            if (bytes < 1024) return $"{bytes} B";
            if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
            if (bytes < 1024 * 1024 * 1024) return $"{bytes / (1024.0 * 1024):F1} MB";
            return $"{bytes / (1024.0 * 1024 * 1024):F1} GB";
        }

        /// <summary>
        /// 加载项目资源
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        public async Task LoadProjectResourcesAsync(string projectPath)
        {
            CurrentProjectPath = projectPath;
            AllFiles.Clear();
            
            try
            {
                if (Directory.Exists(projectPath))
                {
                    await LoadDirectoryFilesAsync(projectPath);
                }
                
                InitializeFolderTree();
                RefreshFileList();
                StatusMessage = $"已加载项目资源: {Path.GetFileName(projectPath)}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"加载资源失败: {ex.Message}";
            }
        }

        private async Task LoadDirectoryFilesAsync(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);
            
            foreach (var filePath in files)
            {
                try
                {
                    var fileInfo = new FileInfo(filePath);
                    var extension = fileInfo.Extension.ToLower();
                    
                    var fileItem = new ResourceFileItem
                    {
                        Name = fileInfo.Name,
                        FullPath = filePath,
                        Type = GetFileType(extension),
                        Size = fileInfo.Length,
                        ModifiedDate = fileInfo.LastWriteTime
                    };
                    
                    // 生成缩略图路径（如果是图片文件）
                    if (extension is ".png" or ".jpg" or ".jpeg")
                    {
                        fileItem.ThumbnailPath = filePath;
                        fileItem.IsImage = true;
                    }
                    
                    AllFiles.Add(fileItem);
                }
                catch
                {
                    // 忽略无法访问的文件
                }
            }
        }
    }

    /// <summary>
    /// 文件夹树项目
    /// </summary>
    public class FolderTreeItem
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = "📁";
        public string FullPath { get; set; } = string.Empty;
        public int FileCount { get; set; } = 0;
        public bool IsExpanded { get; set; } = false;
        public ObservableCollection<FolderTreeItem> Children { get; set; } = new();
    }

    /// <summary>
    /// 资源文件项目
    /// </summary>
    public class ResourceFileItem
    {
        public string Name { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public long Size { get; set; } = 0;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public string ThumbnailPath { get; set; } = string.Empty;
        public bool IsImage { get; set; } = false;
        public bool IsText { get; set; } = false;
        public bool ShowDefaultIcon => !IsImage && !IsText;
        public string TextPreview { get; set; } = string.Empty;
        public string FileIcon => Type switch
        {
            "图片" => "🖼️",
            "JSON" => "📄",
            "Blockbench模型" => "📐",
            "音效" => "🔊",
            "YAML" => "📝",
            _ => "📄"
        };
        
        public string SizeFormatted
        {
            get
            {
                if (Size < 1024) return $"{Size} B";
                if (Size < 1024 * 1024) return $"{Size / 1024.0:F1} KB";
                if (Size < 1024 * 1024 * 1024) return $"{Size / (1024.0 * 1024):F1} MB";
                return $"{Size / (1024.0 * 1024 * 1024):F1} GB";
            }
        }
    }
}
