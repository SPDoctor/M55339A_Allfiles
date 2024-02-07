using System.Windows.Data;
using System.Windows.Media;

namespace Grades.WPF
{
    public class ListColorConverter : IValueConverter
    {
        static bool flag = false;
        SolidColorBrush brush1 = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
        SolidColorBrush brush2 = new SolidColorBrush(Color.FromArgb(28, 0, 140, 255));
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            flag = !flag;
            return flag ? brush1 : brush2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static void Reset()
        {
            flag = false;
        }
    }
}
