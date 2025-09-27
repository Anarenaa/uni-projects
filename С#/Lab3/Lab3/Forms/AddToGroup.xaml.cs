using System.Windows;
using Lab3.Models;
namespace Lab3.Forms
{
    public partial class AddToGroup : Window
    {
        private DanceSchool danceSchool;
        public AddToGroup(DanceSchool danceSchool)
        {
            InitializeComponent();
            this.danceSchool = danceSchool;
            listBoxGroups.ItemsSource = danceSchool.GetGroups();
        }

        public Group GetSelectedGroup()
        {
            return listBoxGroups.SelectedItem as Group;
        }

        private void btnAddToGroupSave_Click(object sender, RoutedEventArgs e)
        {
           this.DialogResult = true;
        }
    }
}
