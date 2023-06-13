using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CryptoTracker.Converters
{
    public class NumberShortFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double number)
            {
                if (number >= 1_000_000_000)
                {
                    return number.ToString("#,,,.00b", CultureInfo.InvariantCulture);
                }
                else if (number >= 1_000_000)
                {
                    return number.ToString("#,,.00m", CultureInfo.InvariantCulture);
                }
                else if (number >= 1000 )
                {
                    return number.ToString("#,00k", CultureInfo.InvariantCulture);
                }
                else
                {
                    return number.ToString("#,00", CultureInfo.InvariantCulture);
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
