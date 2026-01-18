using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp.Models;

namespace WpfApp.ViewModels
{
    public class FilterItem : INotifyPropertyChanged
    {
        public TagItem TagItem { get; set; }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
