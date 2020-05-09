using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CustomControls.Converters
{
    public class GreaterThanMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values != null && values.Length == 2 &&
                double.TryParse(values[0].ToString(), out double number1) &&
                double.TryParse(values[1].ToString(), out double number2) ?
                number1 > number2 : false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
