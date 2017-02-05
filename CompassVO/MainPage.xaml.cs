using CompassVO.Model;
using CompassVO.Model.Messages;
using CompassVO.Utils;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Media;
using Phone7.Fx.Preview;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CompassVO
{
  public partial class MainPage : PhoneApplicationPage
  {
    private PhotoCamera cam;
    private UserControl compass_control;
    private bool isCamInit = false;

    // Constructor
    public MainPage()
    {
      InitializeComponent();
      Messenger.Default.Register<CompassThemeChangedMessage>(this, (res) => CreateCompassTheme());
      Messenger.Default.Register<RefreshAppBarMessage>(this, (res) => FillAppBar());
      Messenger.Default.Register<ShowCameraPreviewMessage>(this, (res) => InitCamera(res));
      Messenger.Default.Register<TakePhotoMessage>(this, (res) => TakePhoto());
      Messenger.Default.Register<LoadPhotoMessage>(this, (res) => LoadPhoto());
    }

    protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
    {
      if (App.Locator.Main.IsBackgroundImageEdit && App.Locator.Main.IsCameraActive && !App.Locator.Main.HasPhoto)
      {
        App.Locator.Main.IsInitCamera = true;
        InitCamera(new ShowCameraPreviewMessage(true));
      }
      base.OnNavigatedTo(e);
    }

    private void LoadPhoto()
    {
      if (App.Locator.Main.HasPhoto)
      {
        WriteableBitmap bmp = IsolatedStorageHelper.LoadFromLocalStorage(Constants.FINAL_PHOTO_FILENAME, Constants.PHOTO_FOLDER);
        backPhoto.Source = bmp;
        backPhoto.UpdateLayout();
      }
    }

    private void InitCamera(ShowCameraPreviewMessage res)
    {
      if (cam != null)
        cam.Dispose();
      cam = null;
      isCamInit = false;

      if (res.ShowCamera)
      {
        if ((PhotoCamera.IsCameraTypeSupported(CameraType.Primary) == true))
        {
          // Otherwise, use standard camera on back of device.
          cam = new Microsoft.Devices.PhotoCamera(CameraType.Primary);
          cam.Initialized += new EventHandler<CameraOperationCompletedEventArgs>(cam_Initialized);
          cam.CaptureImageAvailable += new EventHandler<ContentReadyEventArgs>(cam_CaptureImageAvailable);
          CameraButtons.ShutterKeyHalfPressed += new EventHandler(CameraButtons_ShutterKeyHalfPressed);
          CameraButtons.ShutterKeyReleased += new EventHandler(CameraButtons_ShutterKeyReleased);
          CameraButtons.ShutterKeyPressed += new EventHandler(CameraButtons_ShutterKeyPressed);
          //Set the VideoBrush source to the camera.
          viewfinderBrush.SetSource(cam);
        }
      }
    }

    private void CameraButtons_ShutterKeyPressed(object sender, EventArgs e)
    {
      if (cam != null)
      {
        try
        {
          cam.CaptureImage();
        }
        catch (Exception)
        {
        }
      }
    }

    private void CameraButtons_ShutterKeyReleased(object sender, EventArgs e)
    {
      if (cam != null)
      {
        cam.CancelFocus();
      }
    }

    private void CameraButtons_ShutterKeyHalfPressed(object sender, EventArgs e)
    {
      if (cam != null)
      {
        DispatcherHelper.CheckBeginInvokeOnUI(() =>
        {
          cam.Focus();
        });
      }
    }

    private void cam_CaptureImageAvailable(object sender, ContentReadyEventArgs e)
    {
      Dispatcher.BeginInvoke(() =>
      {
        App.Locator.Main.IsCameraActive = false;
        App.Locator.Main.IsSavingPhoto = true;
        MediaHelper.PlaySound("Sounds\\camera.wav");
        InitCamera(new ShowCameraPreviewMessage(false));
      });
      using (MemoryStream ms = new MemoryStream())
      {
        byte[] buffer = new byte[4096];
        int len = 0;
        while ((len = e.ImageStream.Read(buffer, 0, buffer.Length)) > 0)
        {
          ms.Write(buffer, 0, len);
        }
        buffer = ms.ToArray();
        IsolatedStorageHelper.SaveToLocalStorage(Constants.TEMP_PHOTO_FILENAME, Constants.PHOTO_FOLDER, buffer);
        //save also to media lib
        MediaLibrary myMediaLibrary = new MediaLibrary();
        myMediaLibrary.SavePicture(string.Format("mapscan_{0}.jpg", Guid.NewGuid().ToString("N")), buffer);
      }
      //
      Dispatcher.BeginInvoke(() =>
      {
        WriteableBitmap bmp = IsolatedStorageHelper.LoadFromLocalStorage(Constants.TEMP_PHOTO_FILENAME, Constants.PHOTO_FOLDER);
        backPhoto.Source = bmp;

        App.Locator.Main.PhotoOffsetX = -(double)bmp.PixelWidth / 2;
        App.Locator.Main.PhotoOffsetY = -(double)bmp.PixelHeight / 2;
        App.Locator.Main.PhotoScale = (double)1066 / bmp.PixelWidth;
        App.Locator.Main.PhotoAngle = 90;
        App.Locator.Main.SavePhotoSettings();

        App.Locator.Main.HasPhoto = true;
        FillAppBar();
        App.Locator.Main.IsSavingPhoto = false;
      });
    }

    private void cam_Initialized(object sender, CameraOperationCompletedEventArgs e)
    {
      isCamInit = true;
      Dispatcher.BeginInvoke(() =>
      {
        App.Locator.Main.IsInitCamera = false;
      });
    }

    private void FillAppBar()
    {
      AppBar.Buttons.Clear();
      if (App.Locator.Main.IsCompassMode)
      {
        BindableApplicationBarIconButton button1 = new BindableApplicationBarIconButton();
        button1.IconUri = new Uri("/Images/icon_map.png", UriKind.Relative);
        button1.Text = "set map";
        button1.Command = App.Locator.Main.BackPictureEdit;
        AppBar.Buttons.Add(button1);

        if (App.Locator.Main.CurrentTheme.SupportsDialRotation)
        {
          BindableApplicationBarIconButton button2 = new BindableApplicationBarIconButton();
          button2.IconUri = new Uri(App.Locator.Main.IsDialEdit ? "/Images/icon_dial_lock.png" : "/Images/icon_dial_unlock.png", UriKind.Relative);
          button2.Text = "dial";
          button2.Command = App.Locator.Main.CompassDialEdit;
          AppBar.Buttons.Add(button2);
        }

        BindableApplicationBarIconButton button3 = new BindableApplicationBarIconButton();
        button3.IconUri = new Uri("/Images/appbar.feature.settings.rest.png", UriKind.Relative);
        button3.Text = "settings";
        button3.Command = App.Locator.Main.ShowSettings;
        AppBar.Buttons.Add(button3);
      }
      else if (App.Locator.Main.IsBackgroundImageEdit)
      {
        if (!App.Locator.Main.HasPhoto)
        {
          //no photo
          BindableApplicationBarIconButton button1 = new BindableApplicationBarIconButton();
          button1.IconUri = new Uri("/Images/appbar.feature.camera.rest.png", UriKind.Relative);
          button1.Text = "photo";
          button1.Command = App.Locator.Main.TakePhotoCommand;
          AppBar.Buttons.Add(button1);

          BindableApplicationBarIconButton button2 = new BindableApplicationBarIconButton();
          button2.IconUri = new Uri("/Images/appbar.folder.rest.png", UriKind.Relative);
          button2.Text = "browse";
          button2.Command = App.Locator.Main.SelectPhotoCommand;
          AppBar.Buttons.Add(button2);
        }
        else
        {
          BindableApplicationBarIconButton button1 = new BindableApplicationBarIconButton();
          button1.IconUri = new Uri("/Images/appbar.check.rest.png", UriKind.Relative);
          button1.Text = "accept";
          button1.Command = App.Locator.Main.AcceptPhotoEdit;
          AppBar.Buttons.Add(button1);
          //has photo
          BindableApplicationBarIconButton button3 = new BindableApplicationBarIconButton();
          button3.IconUri = new Uri("/Images/appbar.delete.rest.png", UriKind.Relative);
          button3.Text = "remove";
          button3.Command = App.Locator.Main.RemovePhotoCommand;
          AppBar.Buttons.Add(button3);
        }
      }
      else if (App.Locator.Main.IsDialEdit)
      {
        BindableApplicationBarIconButton button1 = new BindableApplicationBarIconButton();
        button1.IconUri = new Uri("/Images/appbar.check.rest.png", UriKind.Relative);
        button1.Text = "accept";
        button1.Command = App.Locator.Main.AcceptDialEdit;
        AppBar.Buttons.Add(button1);
      }

      //about
      BindableApplicationBarMenuItem menu1 = new BindableApplicationBarMenuItem();
      menu1.Text = "about";
      menu1.Command = App.Locator.Main.ShowAbout;
      AppBar.MenuItems.Add(menu1);
    }

    public void CreateCompassTheme()
    {
      if (App.Locator.Main.CurrentTheme != null)
      {
        compassContainer.Children.Clear();
        //
        compass_control = (UserControl)Activator.CreateInstance(App.Locator.Main.CurrentTheme.UIcomponent);
        compass_control.Width = 480;
        compass_control.Height = 800;
        compass_control.HorizontalAlignment = HorizontalAlignment.Center;
        compass_control.VerticalAlignment = VerticalAlignment.Center;
        compassContainer.Children.Add(compass_control);
        compass_control.UpdateLayout();

        //back color
        LayoutRoot.Background = new SolidColorBrush(App.Locator.Main.CurrentTheme.BackgroundColor);
      }
    }

    private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
    {
      //
    }

    private void ContentPanel_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (cam != null && isCamInit && cam.IsFocusSupported)
      {
        cam.CancelFocus();
        cam.Focus();
      }
    }

    private void ContentPanel_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
    {
      if (App.Locator.Main.IsDialEdit)
      {
        MatrixTransform pt = (MatrixTransform)e.ManipulationContainer.TransformToVisual(LayoutRoot);
        App.Locator.Main.CompassDialEditStarted(new Point(pt.Matrix.OffsetX + e.ManipulationOrigin.X, pt.Matrix.OffsetY + e.ManipulationOrigin.Y));
      }
    }

    private void ContentPanel_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
    {
      if (App.Locator.Main.IsDialEdit)
      {
        MatrixTransform pt = (MatrixTransform)e.ManipulationContainer.TransformToVisual(LayoutRoot);
        App.Locator.Main.CompassDialEditDelta(new Point(pt.Matrix.OffsetX + e.ManipulationOrigin.X, pt.Matrix.OffsetY + e.ManipulationOrigin.Y));
      }
    }

    private void ContentPanel_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
      if (App.Locator.Main.IsDialEdit)
      {
        App.Locator.Main.CompassDialEditCompleted();
      }
    }

    private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
    {
      //
      if (!App.Locator.Main.IsCompassMode)
      {
        App.Locator.Main.CancelEdit();
        e.Cancel = true;
      }
    }

    public void TakePhoto()
    {
      if (cam != null && isCamInit)
      {
        try
        {
          cam.CaptureImage();
        }
        catch (Exception)
        {
        }
      }
    }

    private void gl_DragStarted(object sender, DragStartedGestureEventArgs e)
    {
      if (App.Locator.Main.IsBackgroundImageEdit && App.Locator.Main.HasPhoto)
      {
        //
      }
    }

    private const double Rad2Deg = 180.0 / Math.PI;
    private const double Deg2Rad = Math.PI / 180.0;

    private void gl_DragDelta(object sender, DragDeltaGestureEventArgs e)
    {
      if (App.Locator.Main.IsBackgroundImageEdit && App.Locator.Main.HasPhoto)
      {
        double radians = -App.Locator.Main.PhotoAngle * Deg2Rad;
        double msin = Math.Sin(radians);
        double mcos = Math.Cos(radians);
        double x = mcos * e.HorizontalChange - msin * e.VerticalChange;
        double y = msin * e.HorizontalChange + mcos * e.VerticalChange;

        App.Locator.Main.PhotoOffsetX += x / App.Locator.Main.PhotoScale;
        App.Locator.Main.PhotoOffsetY += y / App.Locator.Main.PhotoScale;
      }
    }

    private void gl_PinchDelta(object sender, PinchGestureEventArgs e)
    {
      if (App.Locator.Main.IsBackgroundImageEdit && App.Locator.Main.HasPhoto)
      {
        App.Locator.Main.PhotoAngle = _initPhotoRotation + e.TotalAngleDelta;
        App.Locator.Main.PhotoScale = _initPhotoScale * e.DistanceRatio;
      }
    }

    private double _initPhotoRotation;
    private double _initPhotoScale;

    private void gl_PinchStarted(object sender, PinchStartedGestureEventArgs e)
    {
      if (App.Locator.Main.IsBackgroundImageEdit && App.Locator.Main.HasPhoto)
      {
        _initPhotoRotation = App.Locator.Main.PhotoAngle;
        _initPhotoScale = App.Locator.Main.PhotoScale;
      }
    }
  }
}