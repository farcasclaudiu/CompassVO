using System;
using System.Windows;
using System.Windows.Media;

namespace CompassVO.Themes.compass_night
{
  public class theme_compass_night : ICompassTheme
  {
    public theme_compass_night()
    {
    }

    #region Implementation of ICompassTheme

    public Color BackgroundColor
    {
      get
      {
        return Colors.Black;
      }
    }

    public Color ForegroundColor
    {
      get
      {
        return Color.FromArgb(255, 139, 0, 0);
      }
    }

    public int Order
    {
      get { return 4; }
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
      get { return typeof(compass_night_UI); }
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
      get { return "Night"; }
    }

    public string ThemeImageUrl
    {
      get { return "/Themes/compass_night/images/sample_night.png"; }
    }

    #endregion ICompassTheme Members
  }
}