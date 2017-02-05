using CompassVO.Themes.compass_orienteering;
using CompassVO.Utils;
using System;
using System.Windows;
using System.Windows.Media;

namespace CompassVO.Themes.compass_metro_graphic
{
  public class theme_compass_metro_graphic : ICompassTheme
  {
    public theme_compass_metro_graphic()
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
        return PhoneThemeDetect.IsDarkTheme() ? Color.FromArgb(255, 102, 204, 255) : Colors.Red;
      }
    }

    public int Order
    {
      get { return 3; }
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
      get { return typeof(compass_metro_graphic_UI); }
    }

    private Point needleCenter = new Point(240, 500);

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
      get { return "Metro graphic"; }
    }

    public string ThemeImageUrl
    {
      get { return "/Themes/compass_metro_graphic/images/sample_metro_graphic.png"; }
    }

    #endregion ICompassTheme Members
  }
}