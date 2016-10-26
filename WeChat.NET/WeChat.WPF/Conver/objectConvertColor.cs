using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using WeChat.WPF.Modules.Main.Model;

namespace WeChat.WPF.Conver
{
    class ObjectConvertColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is WeChatUser)
            {
                return (Brush)new BrushConverter().ConvertFromString("#FFEAEAEA");
            }
            return (Brush)new BrushConverter().ConvertFromString("#FFE0E0E0");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
