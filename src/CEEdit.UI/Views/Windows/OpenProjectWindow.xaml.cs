using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CEEdit.Core.Models.Project;
using CEEdit.Core.Services.Interfaces;

namespace CEEdit.UI.Views.Windows
{
    /// <summary>
    /// OpenProjectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OpenProjectWindow : Window
    {
        private string _currentPath = "";
        private List<ProjectInfo> _currentProjects = new();
        private ProjectInfo? _selectedProject;
        private readonly IProjectHistoryService _historyService;

        public string? SelectedProjectPath { get; private set; }
        public new bool DialogResult { get; private set; } = false;

        public OpenProjectWindow(IProjectHistoryService? historyService = null)
        {
            InitializeComponent();
            _historyService = historyService ?? new CEEdit.Core.Services.Implementations.ProjectHistoryService();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            // 设置默认路径
            var defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CEEdit Projects");
            await SetCurrentPathAsync(defaultPath);
            
            // 加载最近项目
            await LoadRecentProjectsAsync();
        }

        private async Task SetCurrentPathAsync(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                _currentPath = path;
                CurrentPathTextBox.Text = path;
                
                await ScanProjectsInDirectoryAsync(path);
                UpdateUI();
            }
            catch (Exception ex)
            {
                StatusText.Text = $"无法访问路径: {ex.Message}";
            }
        }

        private async Task ScanProjectsInDirectoryAsync(string directoryPath)
        {
            _currentProjects.Clear();
            
            try
            {
                if (!Directory.Exists(directoryPath))
                    return;

                StatusText.Text = "正在扫描项目...";

                // 扫描子目录寻找项目
                var subdirectories = Directory.GetDirectories(directoryPath);
                
                foreach (var subdir in subdirectories)
                {
                    var projectInfo = await TryLoadProjectInfoAsync(subdir);
                    if (projectInfo != null)
                    {
                        _currentProjects.Add(projectInfo);
                    }
                }

                StatusText.Text = _currentProjects.Any() 
                    ? $"找到 {_currentProjects.Count} 个项目" 
                    : "在此文件夹中未找到任何CraftEngine项目";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"扫描项目时出错: {ex.Message}";
            }
        }

        private async Task<ProjectInfo?> TryLoadProjectInfoAsync(string projectPath)
        {
            try
            {
                // 检查CraftEngine addon结构
                var configurationDir = Path.Combine(projectPath, "configuration");
                var resourcepackDir = Path.Combine(projectPath, "resourcepack");
                var ceProjectFile = Path.Combine(projectPath, ".ceproject");

                // 必须有configuration目录或resourcepack目录才被认为是CraftEngine addon
                if (!Directory.Exists(configurationDir) && !Directory.Exists(resourcepackDir))
                    return null;

                var projectInfo = new ProjectInfo
                {
                    Path = projectPath,
                    Name = Path.GetFileName(projectPath),
                    LastModified = Directory.GetLastWriteTime(projectPath)
                };

                // 尝试从pack.yml读取信息（在项目根目录）
                var packYmlFile = Path.Combine(projectPath, "pack.yml");
                if (File.Exists(packYmlFile))
                {
                    await LoadProjectInfoFromPackYmlAsync(projectInfo, packYmlFile);
                }

                // 统计配置文件
                if (Directory.Exists(configurationDir))
                {
                    var configFiles = Directory.GetFiles(configurationDir, "*.yml", SearchOption.AllDirectories);
                    
                    // 根据文件名和内容判断类型
                    foreach (var file in configFiles)
                    {
                        var fileName = Path.GetFileName(file).ToLower();
                        if (fileName.Contains("block"))
                            projectInfo.BlockCount++;
                        else if (fileName.Contains("item"))
                            projectInfo.ItemCount++;
                        else if (fileName.Contains("recipe"))
                            projectInfo.RecipeCount++;
                        else
                        {
                            // 如果文件名不明确，尝试读取内容判断
                            try
                            {
                                var content = await File.ReadAllTextAsync(file);
                                if (content.Contains("blockType") || content.Contains("hardness"))
                                    projectInfo.BlockCount++;
                                else if (content.Contains("itemType") || content.Contains("stackSize"))
                                    projectInfo.ItemCount++;
                                else if (content.Contains("recipe") || content.Contains("ingredients"))
                                    projectInfo.RecipeCount++;
                                else
                                    projectInfo.ItemCount++; // 默认归类为物品
                            }
                            catch
                            {
                                projectInfo.ItemCount++; // 读取失败时默认归类为物品
                            }
                        }
                    }
                }

                // CraftEngine addon项目总是有效的，只要有基本结构
                projectInfo.IsValid = Directory.Exists(configurationDir) || Directory.Exists(resourcepackDir);

                return projectInfo;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载项目信息失败 {projectPath}: {ex.Message}");
                return null;
            }
        }

        private async Task LoadProjectInfoFromPackYmlAsync(ProjectInfo projectInfo, string packYmlPath)
        {
            try
            {
                var content = await File.ReadAllTextAsync(packYmlPath);
                
                // 简单的YAML解析（可以改用YamlDotNet）
                var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();
                    if (trimmedLine.StartsWith("name:"))
                    {
                        projectInfo.Name = trimmedLine.Substring(5).Trim().Trim('"', '\'');
                    }
                    else if (trimmedLine.StartsWith("version:"))
                    {
                        projectInfo.Version = trimmedLine.Substring(8).Trim().Trim('"', '\'');
                    }
                    else if (trimmedLine.StartsWith("author:"))
                    {
                        projectInfo.Author = trimmedLine.Substring(7).Trim().Trim('"', '\'');
                    }
                    else if (trimmedLine.StartsWith("description:"))
                    {
                        projectInfo.Description = trimmedLine.Substring(12).Trim().Trim('"', '\'');
                    }
                }
                
                // 如果pack.yml中没有名称，使用项目目录名
                if (string.IsNullOrEmpty(projectInfo.Name))
                {
                    projectInfo.Name = Path.GetFileName(Path.GetDirectoryName(packYmlPath)) ?? "";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"解析pack.yml失败: {ex.Message}");
            }
        }

        private int CountYmlFiles(string directoryPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                    return 0;
                
                return Directory.GetFiles(directoryPath, "*.yml", SearchOption.AllDirectories).Length;
            }
            catch
            {
                return 0;
            }
        }

        private async Task LoadRecentProjectsAsync()
        {
            try
            {
                var history = await _historyService.GetProjectHistoryAsync();
                var recentProjects = history.RecentProjects
                    .OrderByDescending(p => p.LastOpenedTime)
                    .Take(10)
                    .ToList();

                RecentProjectsControl.ItemsSource = recentProjects;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载最近项目失败: {ex.Message}");
            }
        }

        private void UpdateUI()
        {
            ProjectListPanel.Children.Clear();

            if (!_currentProjects.Any())
            {
                var noProjectsText = new TextBlock
                {
                    Text = "在此文件夹中未找到任何CraftEngine项目",
                    FontSize = 12,
                    Foreground = (System.Windows.Media.Brush)FindResource("SecondaryTextBrush"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(20)
                };
                ProjectListPanel.Children.Add(noProjectsText);
                return;
            }

            foreach (var project in _currentProjects.OrderBy(p => p.Name))
            {
                var projectCard = CreateProjectCard(project);
                ProjectListPanel.Children.Add(projectCard);
            }
        }

        private Border CreateProjectCard(ProjectInfo project)
        {
            var border = new Border
            {
                Style = (Style)FindResource("ProjectCardStyle"),
                Tag = project
            };

            var stackPanel = new StackPanel();

            // 项目名称
            var nameText = new TextBlock
            {
                Text = project.Name,
                FontSize = 14,
                FontWeight = FontWeights.Medium,
                Margin = new Thickness(0, 0, 0, 4)
            };
            stackPanel.Children.Add(nameText);

            // 项目信息
            var infoPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 4) };

            if (!string.IsNullOrEmpty(project.Version))
            {
                var versionText = new TextBlock
                {
                    Text = $"v{project.Version}",
                    FontSize = 10,
                    Foreground = (System.Windows.Media.Brush)FindResource("SecondaryTextBrush"),
                    Margin = new Thickness(0, 0, 8, 0)
                };
                infoPanel.Children.Add(versionText);
            }

            if (!string.IsNullOrEmpty(project.Author))
            {
                var authorText = new TextBlock
                {
                    Text = $"by {project.Author}",
                    FontSize = 10,
                    Foreground = (System.Windows.Media.Brush)FindResource("SecondaryTextBrush")
                };
                infoPanel.Children.Add(authorText);
            }

            stackPanel.Children.Add(infoPanel);

            // 统计信息
            var statsText = new TextBlock
            {
                Text = $"🧱{project.BlockCount}  🎒{project.ItemCount}  ⚗️{project.RecipeCount}",
                FontSize = 10,
                Foreground = (System.Windows.Media.Brush)FindResource("SecondaryTextBrush"),
                Margin = new Thickness(0, 4, 0, 0)
            };
            stackPanel.Children.Add(statsText);

            // 状态指示器
            var statusPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 6, 0, 0) };
            var statusIndicator = new System.Windows.Shapes.Ellipse
            {
                Width = 6,
                Height = 6,
                Fill = project.IsValid 
                    ? (System.Windows.Media.Brush)FindResource("SuccessBrush")
                    : (System.Windows.Media.Brush)FindResource("ErrorBrush"),
                Margin = new Thickness(0, 0, 4, 0)
            };
            var statusText = new TextBlock
            {
                Text = project.IsValid ? "有效项目" : "项目配置不完整",
                FontSize = 9,
                Foreground = (System.Windows.Media.Brush)FindResource("SecondaryTextBrush")
            };
            statusPanel.Children.Add(statusIndicator);
            statusPanel.Children.Add(statusText);
            stackPanel.Children.Add(statusPanel);

            border.Child = stackPanel;
            border.MouseLeftButtonUp += (s, e) => SelectProject(project);

            return border;
        }

        private void SelectProject(ProjectInfo project)
        {
            _selectedProject = project;
            UpdateProjectPreview(project);
            OpenButton.IsEnabled = project.IsValid;
            StatusText.Text = project.IsValid ? $"已选择项目: {project.Name}" : "选择的项目配置不完整，无法打开";
        }

        private void UpdateProjectPreview(ProjectInfo project)
        {
            EmptyPreviewPanel.Visibility = Visibility.Collapsed;
            ProjectInfoPanel.Visibility = Visibility.Visible;

            ProjectNameText.Text = project.Name;
            ProjectVersionText.Text = string.IsNullOrEmpty(project.Version) ? "未指定" : project.Version;
            ProjectAuthorText.Text = string.IsNullOrEmpty(project.Author) ? "未知" : project.Author;
            ProjectDescriptionText.Text = string.IsNullOrEmpty(project.Description) ? "无描述" : project.Description;
            ProjectPathText.Text = project.Path;

            ProjectStatusIndicator.Fill = project.IsValid
                ? (System.Windows.Media.Brush)FindResource("SuccessBrush")
                : (System.Windows.Media.Brush)FindResource("ErrorBrush");
            ProjectStatusText.Text = project.IsValid ? "有效项目" : "配置不完整";

            BlockCountText.Text = $"{project.BlockCount} 个方块";
            ItemCountText.Text = $"{project.ItemCount} 个物品";
            RecipeCountText.Text = $"{project.RecipeCount} 个配方";
        }

        private async void BrowseFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "选择包含CraftEngine项目的文件夹",
                UseDescriptionForTitle = true,
                SelectedPath = _currentPath
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                await SetCurrentPathAsync(dialog.SelectedPath);
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await ScanProjectsInDirectoryAsync(_currentPath);
            UpdateUI();
        }

        private async void RecentProject_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ProjectHistoryItem projectItem)
            {
                if (Directory.Exists(projectItem.ProjectPath))
                {
                    var projectInfo = await TryLoadProjectInfoAsync(projectItem.ProjectPath);
                    if (projectInfo != null)
                    {
                        SelectProject(projectInfo);
                    }
                }
                else
                {
                    MessageBox.Show("项目路径不存在，可能已被移动或删除。", "项目不存在", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProject != null)
            {
                SelectedProjectPath = _selectedProject.Path;
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

    /// <summary>
    /// 项目信息类
    /// </summary>
    public class ProjectInfo
    {
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
        public string Version { get; set; } = "";
        public string Author { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime LastModified { get; set; }
        public int BlockCount { get; set; }
        public int ItemCount { get; set; }
        public int RecipeCount { get; set; }
        public bool IsValid { get; set; }
    }
}
