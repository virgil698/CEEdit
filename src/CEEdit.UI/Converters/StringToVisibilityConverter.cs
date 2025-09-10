using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CEEdit.UI.Converters
{
    /// <summary>
    /// 将字符串转换为可见性的转换器
    /// 空字符串或null显示占位符，否则隐藏占位符
    /// </summary>
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                // 如果字符串为空或null，显示占位符
                return string.IsNullOrWhiteSpace(str) ? Visibility.Visible : Visibility.Collapsed;
            }
            
            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("StringToVisibilityConverter does not support ConvertBack");
        }
    }
}
