using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CryptoTracker.Converters
{
    public class SymbolToImageUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string symbol)
            {
                //https://assets.coincap.io/assets/icons/btc@2x.png
                return $"https://assets.coincap.io/assets/icons/{symbol.ToLower()}@2x.png"; // Replace with your image URL format
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
