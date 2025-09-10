using System.Windows;
using System.Windows.Controls;

namespace CEEdit.UI.Views.UserControls
{
    /// <summary>
    /// ResourceManager.xaml 的交互逻辑
    /// </summary>
    public partial class ResourceManager : UserControl
    {
        public ResourceManager()
        {
            InitializeComponent();
        }

        private void FolderTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (DataContext is ViewModels.ResourceManagerViewModel viewModel && e.NewValue != null)
            {
                viewModel.SelectedFolder = e.NewValue;
                viewModel.RefreshFileList();
            }
        }
    }
}
