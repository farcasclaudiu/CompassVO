using CompassVO.Model.Messages;
using CompassVO.Utils;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CompassVO.Themes.compass_cardinal
{
  public partial class compass_cardinal_UI : UserControl
  {
    public compass_cardinal_UI()
    {
      InitializeComponent();

      imgCompass.Source = new BitmapImage(new Uri(PhoneThemeDetect.IsDarkTheme() ? @"images\compass_old_needle.png" : @"images\compass_old_needle_light.png", UriKind.Relative));

      CompassUiUtil.UpdateUI(0, this);
      Messenger.Default.Register<UpdateCompassUIMessage>(this, (res) => CompassUiUtil.UpdateCompassUI(res, this));
    }
  }
}