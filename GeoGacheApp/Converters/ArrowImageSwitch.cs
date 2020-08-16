using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Geocache.Converters
{
    public class ArrowImageSwitch : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool sortAscending = (bool)value;
            if (sortAscending)
                return "/res/Icons/sortArrowUp.png";
            else return "/res/Icons/sortArrowDown.png";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
