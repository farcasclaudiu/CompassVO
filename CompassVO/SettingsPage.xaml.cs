using Microsoft.Phone.Controls;
using System.Windows;
using System.Windows.Controls;

namespace CompassVO
{
  public partial class SettingsPage : PhoneApplicationPage
  {
    private bool isPageLoaded;

    public SettingsPage()
    {
      InitializeComponent();
    }

    private void RadDataBoundListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (isPageLoaded && NavigationService.CanGoBack)
        NavigationService.GoBack();
    }

    private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
    {
      isPageLoaded = true;
    }
  }
}