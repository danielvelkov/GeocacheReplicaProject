using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GeocacheV2.Converters
{
    class IsNullOrWhiteSpaceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            switch (parameter)
            {
                case "AtLeastOneNecessary":
                    {
                        foreach (string text in values)
                            if (!String.IsNullOrWhiteSpace(text))
                                return true;
                        return false;
                    }
                case "AllNecessary":
                    {
                        foreach (string text in values)
                            if (String.IsNullOrWhiteSpace(text))
                                return false;
                        return true;
                    }
            }
            return false;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
