using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace CodeMobileChallenge.Helpers
{
    internal class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isVisible)
            {
                return isVisible ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed; // หรือค่าอื่น ๆ ตามที่ต้องการให้ซ่อน
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
