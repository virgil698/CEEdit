using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CEEdit.UI.Models
{
    /// <summary>
    /// 项目树项类型
    /// </summary>
    public enum ProjectItemType
    {
        Project,
        Folder,
        Block,
        Item,
        Recipe,
        Texture,
        Model,
        Audio,
        Other
    }

    /// <summary>
    /// 项目树项目
    /// </summary>
    public class ProjectTreeItem : INotifyPropertyChanged
    {
        private string _name = "";
        private string _icon = "";
        private bool _isExpanded = false;
        private bool _isSelected = false;

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// 项目图标
        /// </summary>
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        /// <summary>
        /// 项目类型
        /// </summary>
        public ProjectItemType Type { get; set; }

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        /// <summary>
        /// 关联的数据对象
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// 子项目
        /// </summary>
        public ObservableCollection<ProjectTreeItem> Children { get; set; } = new ObservableCollection<ProjectTreeItem>();

        /// <summary>
        /// 父项目
        /// </summary>
        public ProjectTreeItem? Parent { get; set; }

        /// <summary>
        /// 是否有子项
        /// </summary>
        public bool HasChildren => Children.Count > 0;

        /// <summary>
        /// 属性变更事件
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 添加子项
        /// </summary>
        /// <param name="child">子项</param>
        public void AddChild(ProjectTreeItem child)
        {
            child.Parent = this;
            Children.Add(child);
            OnPropertyChanged(nameof(HasChildren));
        }

        /// <summary>
        /// 移除子项
        /// </summary>
        /// <param name="child">子项</param>
        public void RemoveChild(ProjectTreeItem child)
        {
            child.Parent = null;
            Children.Remove(child);
            OnPropertyChanged(nameof(HasChildren));
        }

        /// <summary>
        /// 获取完整路径
        /// </summary>
        /// <returns>路径字符串</returns>
        public string GetFullPath()
        {
            if (Parent == null)
                return Name;
            
            return $"{Parent.GetFullPath()}/{Name}";
        }

        /// <summary>
        /// 获取项目的显示文本
        /// </summary>
        /// <returns>显示文本</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
