using CompassVO.Model.Messages;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;

namespace CompassVO.Themes.compass_digital
{
  public partial class compass_digital_UI : UserControl
  {
    public compass_digital_UI()
    {
      InitializeComponent();
      CompassUiUtil.UpdateUI(0, this);
      Messenger.Default.Register<UpdateCompassUIMessage>(this, (res) => CompassUiUtil.UpdateCompassUI(res, this));
    }
  }
}