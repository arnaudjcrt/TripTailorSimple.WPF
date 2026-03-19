using System.Globalization;
using System.Windows.Data;

namespace TripTailorSimple.WPF.Converters;

public class StringEqualsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string current && parameter is string expected)
        {
            return string.Equals(current, expected, StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
