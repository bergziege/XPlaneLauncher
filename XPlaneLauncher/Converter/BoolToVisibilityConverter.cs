using System;
using System.Globalization;
using System.Runtime.Serialization.Formatters;
using System.Windows;
using System.Windows.Data;

namespace XPlaneLauncher.Converter
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolvalue)
            {
                if (parameter is bool inverted && inverted)
                {
                    boolvalue = !boolvalue;
                }

                if (boolvalue)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}