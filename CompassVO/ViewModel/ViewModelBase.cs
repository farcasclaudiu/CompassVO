using CompassVO.Model;
using CompassVO.Service.Navigation;
using System.Runtime.Serialization;

namespace CompassVO.ViewModel
{
  [DataContract]
  public class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
  {
    private object context;

    public object Context
    {
      get { return context; }
      set
      {
        if (context == value)
          return;
        context = value;
        RaisePropertyChanged("Context");
      }
    }

    /// <summary>
    /// Gets PageNavigation from Container
    /// </summary>
    public INavigationService NavigationService
    {
      get
      {
        INavigationService pageNav = (INavigationService)Container.Instance.Resolve(typeof(NavigationService), "NavigationService");
        return pageNav;
      }
    }
  }
}