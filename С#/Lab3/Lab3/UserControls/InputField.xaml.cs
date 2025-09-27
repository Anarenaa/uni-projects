using System.ComponentModel;
using UserControl = System.Windows.Controls.UserControl;
namespace Lab3.UserControls
{
    /// <summary>
    /// Interaction logic for InputField.xaml
    /// </summary>
    public partial class InputField : UserControl, INotifyPropertyChanged
    {
        public InputField()
        {
            DataContext = this;
            InitializeComponent();
        }

        private string labelText;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string LabelText
        {
            get { return labelText; }
            set 
            { 
                labelText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LabelText"));
            }
        }

    }
}
