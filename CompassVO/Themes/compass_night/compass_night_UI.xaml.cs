using CompassVO.Model.Messages;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;

namespace CompassVO.Themes.compass_night
{
  public partial class compass_night_UI : UserControl
  {
    public compass_night_UI()
    {
      InitializeComponent();
      CompassUiUtil.UpdateUI(0, this);
      Messenger.Default.Register<UpdateCompassUIMessage>(this, (res) => CompassUiUtil.UpdateCompassUI(res, this));
    }
  }
}