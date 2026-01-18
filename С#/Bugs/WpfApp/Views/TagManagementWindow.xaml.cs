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
            if(e.EditAction == DataGridEditAction.Commit)
            {
                var viewModel = DataContext as TagManagementViewModel;
                if (viewModel != null)
                {
                    var selectedTagItem = e.Row.Item as Tag;
                    if (selectedTagItem != null)
                        viewModel.CheckTagName(selectedTagItem.Name);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //зробити збереження списку тегів (якщо змінено) до бази даних
        }
    }
}
