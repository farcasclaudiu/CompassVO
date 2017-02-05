using System;
using System.Globalization;
using System.Windows.Data;

namespace CompassVO.Converters
{
  public class ConditionalTextConverter : IValueConverter
  {
    #region Implementation of IValueConverter

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value != null && parameter != null && value is bool)
      {
        bool boolVal = (bool)value;
        string[] texts = parameter.ToString().Split('|');
        if (texts.Length == 2)
        {
          return boolVal ? texts[0] : texts[1];
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