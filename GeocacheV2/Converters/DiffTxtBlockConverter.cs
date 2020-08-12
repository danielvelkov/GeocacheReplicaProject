using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GeocacheV2.Converters
{
    class DiffTxtBlockConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string Difficulties="";
            decimal difficultyRating= System.Convert.ToDecimal(value);
            if (difficultyRating >= 1 && difficultyRating <= 2)
            {
                Difficulties = "easy";
            }
            else if (difficultyRating > 2 && difficultyRating <= 3)
            {
                Difficulties = "normal";
            }
            else if (difficultyRating > 3 && difficultyRating <=4)
            {
                Difficulties = "hard";
            }
            else Difficulties = "impossible";
            return Difficulties;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
