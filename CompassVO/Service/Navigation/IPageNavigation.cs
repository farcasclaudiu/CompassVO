using System;
using System.Windows.Navigation;

namespace CompassVO.Service.Navigation
{
  public interface INavigationService
  {
    event NavigatingCancelEventHandler Navigating;

    object CurrentContext { get; }

    void NavigateTo(Uri pageUri);

    void NavigateTo(Uri pageUri, object context);

    void GoBack();
  }
}