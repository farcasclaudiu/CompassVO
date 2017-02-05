using System;
using System.Globalization;
using System.Windows.Data;

namespace CompassVO.Converters
{
  public class NegativeNumberConverter : IValueConverter
  {
    #region Implementation of IValueConverter

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value != null)
      {
        if (value is int)
        {
          return -(int)value;
        }
        if (value is double)
        {
          return -(double)value;
        }
        if (value is long)
        {
          return -(long)value;
        }
        if (value is decimal)
        {
          return -(decimal)value;
        }
      }
      return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return null;
    }

    #endregion Implementation of IValueConverter
  }
}