using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Core.Models;
using WpfApp.Models;
using WpfApp.ViewModels;

namespace WpfApp.Views
{
    /// <summary>
    /// Interaction logic for TagManagementWindow.xaml
    /// </summary>
    public partial class TagManagementWindow : Window
    {
        public TagManagementWindow(ObservableCollection<TagItem> tagItems)
        {
            InitializeComponent();
            DataContext = new TagManagementViewModel(tagItems);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void TagsDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit)
                return;
            if (DataContext is TagManagementViewModel vm && e.Row.Item is Tag tag)
            {
                vm.AddNewTag(tag);
            }
        }

        private void TagsDataGrid_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

    }
}
