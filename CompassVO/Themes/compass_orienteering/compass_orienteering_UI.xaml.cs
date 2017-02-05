using CompassVO.Model.Messages;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;
using System.Windows.Controls;

namespace CompassVO.Themes.compass_orienteering
{
  public partial class compass_orienteering_UI : UserControl, INotifyPropertyChanged
  {
    public compass_orienteering_UI()
    {
      this.DataContext = this;
      InitializeComponent();
      CompassUiUtil.UpdateUI(0, this);
      Messenger.Default.Register<UpdateCompassUIMessage>(this, (res) => CompassUiUtil.UpdateCompassUI(res, this));
    }

    #region Implementation of INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Implementation of INotifyPropertyChanged
  }
}