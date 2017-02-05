using CompassVO.Utils;
using System;
using System.Windows;
using System.Windows.Media;

namespace CompassVO.Themes.compass_cardinal
{
  public class theme_compass_cardinal : ICompassTheme
  {
    public theme_compass_cardinal()
    {
    }

    #region Implementation of ICompassTheme

    public Color BackgroundColor
    {
      get
      {
        return PhoneThemeDetect.IsDarkTheme() ? PhoneThemeDetect.DarkThemeBackground : PhoneThemeDetect.LightThemeBackground;
      }
    }

    public Color ForegroundColor
    {
      get
      {
        return PhoneThemeDetect.IsDarkTheme() ? PhoneThemeDetect.LightThemeBackground : PhoneThemeDetect.DarkThemeBackground;
      }
    }

    public int Order
    {
      get { return 2; }
    }

    public bool SupportsDialRotation
    {
      get { return false; }
    }

    public bool SupportsBackgroundImages
    {
      get { return true; }
    }

    public bool SupportsTargets
    {
      get { return true; }
    }

    public Type UIcomponent
    {
      get { return typeof(compass_cardinal_UI); }
    }

    private Point needleCenter = new Point(240, 295);

    public double GetAngle(Point point)
    {
      return Angle(needleCenter, point);
    }

    private const double Rad2Deg = 180.0 / Math.PI;
    private const double Deg2Rad = Math.PI / 180.0;

    private double Angle(Point start, Point end)
    {
      return Math.Atan2(start.Y - end.Y, end.X - start.X) * Rad2Deg;
    }

    #endregion Implementation of ICompassTheme

    #region ICompassTheme Members

    public string ThemeName
    {
      get { return "Cardinal rose"; }
    }

    public string ThemeImageUrl
    {
      get { return "/Themes/compass_cardinal/images/sample_old.png"; }
    }

    #endregion ICompassTheme Members
  }
}