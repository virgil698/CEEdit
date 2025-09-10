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
    public partial class YamlEditorViewModel : ObservableObject
    {
        private readonly IFileService? _fileService;
        private readonly IValidationService? _validationService;

        [ObservableProperty]
        private string _yamlContent = string.Empty;

        [ObservableProperty]
        private string _currentFileName = "未命名文件.yml";

        [ObservableProperty]
        private string _currentFilePath = string.Empty;

        [ObservableProperty]
        private bool _isDirty = false;

        [ObservableProperty]
        private bool _showLineNumbers = true;

        [ObservableProperty]
        private bool _wordWrap = false;

        [ObservableProperty]
        private int _tabSize = 2;

        [ObservableProperty]
        private string _lineColumnInfo = "行 1, 列 1";

        [ObservableProperty]
        private string _encodingInfo = "UTF-8";

        [ObservableProperty]
        private string _statusMessage = "就绪";

        [ObservableProperty]
        private int _errorCount = 0;

        [ObservableProperty]
        private string _lineNumbersText = "1";

        public ObservableCollection<DocumentOutlineItem> DocumentOutline { get; } = new();
        public ObservableCollection<ValidationError> ValidationErrors { get; } = new();

        public YamlEditorViewModel(IFileService? fileService = null, 
                                  IValidationService? validationService = null)
        {
            _fileService = fileService;
            _validationService = validationService;
            
            // 监听内容变化
            PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(YamlContent))
                {
                    IsDirty = true;
                    UpdateDocumentOutline();
                    await ValidateYamlAsync();
                }
            };
        }

        [RelayCommand]
        private async Task NewFile()
        {
            await Task.CompletedTask;
            if (IsDirty)
            {
                var result = MessageBox.Show("当前文件未保存，是否保存？", 
                                           "确认", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    await SaveFile();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            YamlContent = string.Empty;
            CurrentFileName = "未命名文件.yml";
            CurrentFilePath = string.Empty;
            IsDirty = false;
            StatusMessage = "新建文件";
        }

        [RelayCommand]
        private async Task OpenFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "打开YAML文件",
                Filter = "YAML文件 (*.yml;*.yaml)|*.yml;*.yaml|所有文件 (*.*)|*.*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    if (_fileService != null)
                    {
                        YamlContent = await _fileService.ReadTextFileAsync(openFileDialog.FileName);
                    }
                    else
                    {
                        YamlContent = await File.ReadAllTextAsync(openFileDialog.FileName);
                    }
                    
                    CurrentFilePath = openFileDialog.FileName;
                    CurrentFileName = Path.GetFileName(openFileDialog.FileName);
                    IsDirty = false;
                    StatusMessage = $"已打开: {CurrentFileName}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"打开文件失败: {ex.Message}", 
                                  "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        [RelayCommand]
        private async Task SaveFile()
        {
            if (string.IsNullOrEmpty(CurrentFilePath))
            {
                await SaveAsFile();
                return;
            }

            try
            {
                if (_fileService != null)
                {
                    await _fileService.WriteTextFileAsync(CurrentFilePath, YamlContent);
                }
                else
                {
                    await File.WriteAllTextAsync(CurrentFilePath, YamlContent);
                }
                
                IsDirty = false;
                StatusMessage = $"已保存: {CurrentFileName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存文件失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task SaveAsFile()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "另存为",
                Filter = "YAML文件 (*.yml)|*.yml|YAML文件 (*.yaml)|*.yaml|所有文件 (*.*)|*.*",
                FileName = CurrentFileName
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    if (_fileService != null)
                    {
                        await _fileService.WriteTextFileAsync(saveFileDialog.FileName, YamlContent);
                    }
                    else
                    {
                        await File.WriteAllTextAsync(saveFileDialog.FileName, YamlContent);
                    }
                    
                    CurrentFilePath = saveFileDialog.FileName;
                    CurrentFileName = Path.GetFileName(saveFileDialog.FileName);
                    IsDirty = false;
                    StatusMessage = $"已保存: {CurrentFileName}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存文件失败: {ex.Message}", 
                                  "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        [RelayCommand]
        private void Undo()
        {
            // TODO: 实现撤销功能
            StatusMessage = "撤销操作";
        }

        [RelayCommand]
        private void Redo()
        {
            // TODO: 实现重做功能
            StatusMessage = "重做操作";
        }

        [RelayCommand]
        private void Find()
        {
            // TODO: 实现查找功能
            StatusMessage = "查找功能";
        }

        [RelayCommand]
        private void Replace()
        {
            // TODO: 实现替换功能
            StatusMessage = "替换功能";
        }

        [RelayCommand]
        private async Task Format()
        {
            await Task.CompletedTask;
            try
            {
                // TODO: 实现YAML格式化
                // 这里可以使用YamlDotNet来格式化YAML内容
                StatusMessage = "YAML格式化完成";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"格式化失败: {ex.Message}", 
                              "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task Validate()
        {
            await ValidateYamlAsync();
        }

        private async Task ValidateYamlAsync()
        {
            await Task.CompletedTask;
            try
            {
                ValidationErrors.Clear();
                
                if (_validationService != null)
                {
                    // TODO: 使用验证服务验证YAML
                    // var errors = await _validationService.ValidateYamlAsync(YamlContent);
                    // foreach (var error in errors)
                    // {
                    //     ValidationErrors.Add(error);
                    // }
                }
                else
                {
                    // 简单的YAML验证
                    ValidateYamlSyntax();
                }
                
                ErrorCount = ValidationErrors.Count;
                
                if (ErrorCount == 0)
                {
                    StatusMessage = "YAML语法正确";
                }
                else
                {
                    StatusMessage = $"发现 {ErrorCount} 个错误";
                }
            }
            catch (Exception ex)
            {
                ValidationErrors.Add(new ValidationError
                {
                    Message = $"验证失败: {ex.Message}",
                    Severity = ErrorSeverity.Error,
                    Location = "全局"
                });
                ErrorCount = ValidationErrors.Count;
            }
        }

        private void ValidateYamlSyntax()
        {
            try
            {
                // 使用YamlDotNet进行语法验证
                var deserializer = new YamlDotNet.Serialization.Deserializer();
                deserializer.Deserialize(YamlContent);
            }
            catch (YamlDotNet.Core.YamlException ex)
            {
                ValidationErrors.Add(new ValidationError
                {
                    Message = ex.Message,
                    Severity = ErrorSeverity.Error,
                    Location = $"行 {ex.Start.Line}, 列 {ex.Start.Column}"
                });
            }
            catch (Exception ex)
            {
                ValidationErrors.Add(new ValidationError
                {
                    Message = ex.Message,
                    Severity = ErrorSeverity.Error,
                    Location = "未知位置"
                });
            }
        }

        private void UpdateDocumentOutline()
        {
            DocumentOutline.Clear();
            
            try
            {
                // TODO: 解析YAML结构并生成文档大纲
                // 这里可以分析YAML内容的层次结构
                var lines = YamlContent.Split('\n');
                
                foreach (var line in lines.Take(20)) // 限制显示前20行
                {
                    var trimmed = line.TrimStart();
                    if (!string.IsNullOrEmpty(trimmed) && !trimmed.StartsWith("#"))
                    {
                        var indent = line.Length - trimmed.Length;
                        var colonIndex = trimmed.IndexOf(':');
                        
                        if (colonIndex > 0)
                        {
                            var key = trimmed.Substring(0, colonIndex).Trim();
                            DocumentOutline.Add(new DocumentOutlineItem
                            {
                                Name = key,
                                Type = "键",
                                Icon = "📄",
                                Level = indent / 2
                            });
                        }
                    }
                }
            }
            catch
            {
                // 忽略解析错误
            }
        }

        public void UpdateCursorPosition(int line, int column)
        {
            LineColumnInfo = $"行 {line}, 列 {column}";
        }

        public void OnContentChanged()
        {
            // 更新行号
            var lineCount = YamlContent.Split('\n').Length;
            var lineNumbers = new System.Text.StringBuilder();
            for (int i = 1; i <= lineCount; i++)
            {
                lineNumbers.AppendLine(i.ToString());
            }
            LineNumbersText = lineNumbers.ToString();
        }

        /// <summary>
        /// 加载YAML文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public async Task LoadFileAsync(string filePath)
        {
            try
            {
                if (_fileService != null)
                {
                    YamlContent = await _fileService.ReadTextFileAsync(filePath);
                }
                else
                {
                    YamlContent = await File.ReadAllTextAsync(filePath);
                }
                
                CurrentFilePath = filePath;
                CurrentFileName = Path.GetFileName(filePath);
                IsDirty = false;
                StatusMessage = $"已加载: {CurrentFileName}";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"加载文件失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 设置YAML内容
        /// </summary>
        /// <param name="content">YAML内容</param>
        /// <param name="fileName">文件名</param>
        public void SetContent(string content, string fileName = "")
        {
            YamlContent = content ?? string.Empty;
            
            if (!string.IsNullOrEmpty(fileName))
            {
                CurrentFileName = fileName;
            }
            
            IsDirty = false;
        }

        /// <summary>
        /// 检查是否有未保存的更改
        /// </summary>
        /// <returns>如果有未保存的更改返回true</returns>
        public bool HasUnsavedChanges()
        {
            return IsDirty;
        }
    }

    /// <summary>
    /// 文档大纲项目
    /// </summary>
    public class DocumentOutlineItem
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int Level { get; set; }
        public ObservableCollection<DocumentOutlineItem> Children { get; set; } = new();
    }

    /// <summary>
    /// 验证错误
    /// </summary>
    public class ValidationError
    {
        public string Message { get; set; } = string.Empty;
        public ErrorSeverity Severity { get; set; } = ErrorSeverity.Error;
        public string Location { get; set; } = string.Empty;
    }

    /// <summary>
    /// 错误严重程度
    /// </summary>
    public enum ErrorSeverity
    {
        Info,
        Warning,
        Error
    }
}
