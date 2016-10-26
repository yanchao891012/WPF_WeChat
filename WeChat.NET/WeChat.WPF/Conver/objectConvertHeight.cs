using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using WeChat.WPF.Modules.Main.Model;

namespace WeChat.WPF.Conver
{
    class ObjectConvertHeight : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is WeChatUser)
            {
                return new FrameworkElement().Height = double.NaN;
            }
            return new FrameworkElement().Height = 25;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
