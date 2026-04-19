using System.Globalization;
using System.Windows.Data;
using Core.Models;

namespace WpfApp.Views.Converters
{
    public class RoleToUkrainianConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Role role)
            {
                return role switch
                {
                    Role.Admin => "Адміністратор",
                    Role.Analyst => "Аналітик",
                    Role.Boss => "Керівник",
                    _ => value.ToString()
                };
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
