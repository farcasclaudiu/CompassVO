using System;
using System.Windows;
using System.Windows.Media;

namespace CompassVO.Themes.compass_digital
{
  public class theme_compass_digital : ICompassTheme
  {
    public theme_compass_digital()
    {
    }

    #region Implementation of ICompassTheme

    public Color BackgroundColor
    {
      get
      {
        return Colors.LightGray;
      }
    }

    public Color ForegroundColor
    {
      get
      {
        return Color.FromArgb(255, 80, 80, 80);
      }
    }

    public int Order
    {
      get { return 1; }
    }

    public bool SupportsDialRotation
    {
      get { return true; }
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
      get { return typeof(compass_digital_UI); }
    }

    private Point needleCenter = new Point(240, 408);

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
      get { return "Digital"; }
    }

    public string ThemeImageUrl
    {
      get { return "/Themes/compass_digital/images/sample_digital.png"; }
    }

    #endregion ICompassTheme Members
  }
}