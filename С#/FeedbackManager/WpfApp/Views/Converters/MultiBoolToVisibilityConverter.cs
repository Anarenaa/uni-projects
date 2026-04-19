using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfApp.Views.Converters
{
    public class MultiBoolToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Перевіряємо, чи всі значення є true
            bool allTrue = values.All(v => v is bool b && b);

            return allTrue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
