using System;
using System.Windows;
using System.Windows.Media;

namespace CompassVO.Themes
{
  public interface ICompassTheme
  {
    string ThemeName { get; }
    string ThemeImageUrl { get; }
    Color BackgroundColor { get; }
    Color ForegroundColor { get; }
    int Order { get; }

    bool SupportsDialRotation { get; }
    bool SupportsBackgroundImages { get; }
    bool SupportsTargets { get; }
    Type UIcomponent { get; }

    double GetAngle(Point point);
  }
}