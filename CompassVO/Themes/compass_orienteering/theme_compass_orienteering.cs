using System;
using System.Windows;
using System.Windows.Media;

namespace CompassVO.Themes.compass_orienteering
{
  public class theme_compass_orienteering : ICompassTheme
  {
    public theme_compass_orienteering()
    {
    }

    #region Implementation of ICompassTheme

    public Color BackgroundColor
    {
      get { return Colors.White; }
    }

    public Color ForegroundColor
    {
      get
      {
        return Colors.Black;
      }
    }

    public int Order
    {
      get { return 0; }
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
      get { return typeof(compass_orienteering_UI); }
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
      get { return "Orienteering"; }
    }

    public string ThemeImageUrl
    {
      get { return "/Themes/compass_orienteering/images/sample_orienteering.png"; }
    }

    #endregion ICompassTheme Members
  }
}