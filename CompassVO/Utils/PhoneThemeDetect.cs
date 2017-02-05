using System.Windows;
using System.Windows.Media;

namespace CompassVO.Utils
{
  public class PhoneThemeDetect
  {
    public static Color LightThemeBackground = Color.FromArgb(255, 255, 255, 255);
    public static Color DarkThemeBackground = Color.FromArgb(255, 0, 0, 0);

    public static bool IsDarkTheme()
    {
      SolidColorBrush backgroundBrush = Application.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush;
      return (backgroundBrush.Color == DarkThemeBackground);
    }

    public static bool IsLightTheme()
    {
      SolidColorBrush backgroundBrush = Application.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush;
      return (backgroundBrush.Color == LightThemeBackground);
    }

    public static Color AccentColor()
    {
      return (Color)Application.Current.Resources["PhoneAccentColor"];
    }

    public static SolidColorBrush AccentBrush()
    {
      return Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
    }
  }
}