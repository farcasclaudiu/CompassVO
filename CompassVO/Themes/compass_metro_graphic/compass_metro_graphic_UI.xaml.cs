using CompassVO.Model.Messages;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;

namespace CompassVO.Themes.compass_orienteering
{
  public partial class compass_metro_graphic_UI : UserControl
  {
    public compass_metro_graphic_UI()
    {
      InitializeComponent();
      CompassUiUtil.UpdateUI(0, this);
      Messenger.Default.Register<UpdateCompassUIMessage>(this, (res) => CompassUiUtil.UpdateCompassUI(res, this));
    }
  }
}