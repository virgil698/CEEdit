using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CEEdit.UI.Converters
{
    public class SeverityToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return GetBrush("SecondaryTextBrush");

            string severity = value.ToString() ?? "";
            
            return severity switch
            {
                "Error" => GetBrush("ErrorBrush"),
                "Warning" => GetBrush("WarningBrush"),
                "Info" => GetBrush("InfoBrush"),
                "Success" => GetBrush("SuccessBrush"),
                _ => GetBrush("SecondaryTextBrush")
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Brush GetBrush(string resourceKey)
        {
            try
            {
                return (Brush)Application.Current.FindResource(resourceKey);
            }
            catch
            {
                // 如果找不到资源，返回默认的灰色画刷
                return new SolidColorBrush(Colors.Gray);
            }
        }
    }
}
