using CompassVO.Model.Messages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CompassVO.Themes
{
  public static class CompassUiUtil
  {
    private static Storyboard sb;
    public static RotateTransform needleRotation;
    private static UserControl CompassUi;

    public static void UpdateCompassUI(UpdateCompassUIMessage res, UserControl compassUi)
    {
      UpdateUI(res.Angle, compassUi);
    }

    private static void SetControl(UserControl compassUi)
    {
      CompassUi = compassUi;
      needleRotation = compassUi.FindName("needleRotation") as RotateTransform;
    }

    public static void UpdateUI(double angle, UserControl compassUi)
    {
      SetControl(compassUi);

      if (sb != null)
        sb.SkipToFill();

      double newAngle = angle;
      needleRotation.Angle = needleRotation.Angle % 360;
      double diff = newAngle - needleRotation.Angle;
      if (Math.Abs(diff) > 180)
        newAngle += -Math.Sign(diff) * 360;
      sb = StoryBoardEffect(newAngle, 50);
      sb.Begin();
    }

    #region StoryBoard

    private static Storyboard StoryBoardEffect(double needleRotationAngle, int miliseconds)
    {
      //add new storyboard and animation
      Storyboard sb = new Storyboard();
      if (needleRotation != null)
      {
        #region ellipse width

        DoubleAnimation daNeedle = new DoubleAnimation
        {
          To = needleRotationAngle,
          Duration = new Duration(TimeSpan.FromMilliseconds(miliseconds))
        };
        Storyboard.SetTarget(daNeedle, needleRotation);
        PropertyPath myPropertyPath = new PropertyPath(RotateTransform.AngleProperty);
        Storyboard.SetTargetProperty(daNeedle, myPropertyPath);
        sb.Children.Add(daNeedle);

        #endregion ellipse width
      }
      return sb;
    }

    #endregion StoryBoard
  }
}