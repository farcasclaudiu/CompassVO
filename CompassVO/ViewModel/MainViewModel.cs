using CompassVO.Model;
using CompassVO.Model.Messages;
using CompassVO.Service;
using CompassVO.Themes;
using CompassVO.Utils;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Devices;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Environment = System.Environment;

namespace CompassVO.ViewModel
{
  public class MainViewModel : ViewModelBase
  {
    private CompassEx _compass;

    private double initDialAngle = 0;
    private double initDialAngleDrag = 0;
    private double startDialAngle = 0;
    private double deltaDialAngle = 0;

    #region Public Properties

    public const string IsHeadingMagneticPropertyName = "IsHeadingMagnetic";

    public bool IsHeadingMagnetic
    {
      get
      {
        return AppSettings.Instance.IsHeadingMagnetic;
      }

      set
      {
        if (AppSettings.Instance.IsHeadingMagnetic == value)
        {
          return;
        }

        AppSettings.Instance.IsHeadingMagnetic = value;
        AppSettings.Save();
        RaisePropertyChanged(() => IsHeadingMagnetic);
      }
    }

    public const string MagneticHeadingPropertyName = "MagneticHeading";
    private double _magneticheading = 0d;

    public double MagneticHeading
    {
      get
      {
        return _magneticheading;
      }

      set
      {
        if (_magneticheading == value)
          return;

        _magneticheading = value;
        RaisePropertyChanged(() => MagneticHeading);
      }
    }

    public const string TrueHeadingPropertyName = "TrueHeading";
    private double _trueHeading = 0d;

    public double TrueHeading
    {
      get
      {
        return _trueHeading;
      }

      set
      {
        if (_trueHeading == value)
          return;

        _trueHeading = value;
        RaisePropertyChanged(() => TrueHeading);
      }
    }

    public const string MagneticDeclinationPropertyName = "MagneticDeclination";
    private double _magneticDeclination = 0d;

    public double MagneticDeclination
    {
      get
      {
        return _magneticDeclination;
      }

      set
      {
        if (_magneticDeclination == value)
          return;

        _magneticDeclination = value;
        RaisePropertyChanged(() => MagneticDeclination);
      }
    }

    public const string HeadingAccuracyPropertyName = "HeadingAccuracy";
    private double _headingAccuracy = 0d;

    public double HeadingAccuracy
    {
      get
      {
        return _headingAccuracy;
      }

      set
      {
        if (_headingAccuracy == value)
          return;

        _headingAccuracy = value;
        RaisePropertyChanged(() => HeadingAccuracy);
      }
    }

    public const string NeedleAnglePropertyName = "NeedleAngle";
    private double _needleAngle = 0d;

    public double NeedleAngle
    {
      get
      {
        return _needleAngle;
      }

      set
      {
        if (_needleAngle == value)
          return;

        _needleAngle = value;
        RaisePropertyChanged(() => NeedleAngle);
        Messenger.Default.Send(new UpdateCompassUIMessage(-_needleAngle));
      }
    }

    public const string NeedleCardinalDirectionPropertyName = "MagneticCardinalDirection";
    private CardinalDirection _needleCardinalDirection = CardinalDirection.Unknow;

    public CardinalDirection NeedleCardinalDirection
    {
      get
      {
        return _needleCardinalDirection;
      }

      set
      {
        if (_needleCardinalDirection == value)
          return;

        _needleCardinalDirection = value;
        RaisePropertyChanged(() => NeedleCardinalDirection);
      }
    }

    public const string DialAnglePropertyName = "DialAngle";
    private double _dialAngle = 0d;

    public double DialAngle
    {
      get
      {
        return _dialAngle;
      }

      set
      {
        if (_dialAngle == value)
          return;
        _dialAngle = (value + 360) % 360;
        RaisePropertyChanged(() => DialAngle);
        RaisePropertyChanged(() => DialAngleHeading);
      }
    }

    public double DialAngleHeading
    {
      get
      {
        return ((360 - _dialAngle) + 360) % 360;
      }
    }

    public const string PhotoAnglePropertyName = "PhotoAngle";
    private double _photoAngle = 90d;

    public double PhotoAngle
    {
      get
      {
        return _photoAngle;
      }

      set
      {
        if (_photoAngle == value)
          return;

        _photoAngle = value % 360;
        RaisePropertyChanged(() => PhotoAngle);
      }
    }

    public const string PhotoScalePropertyName = "PhotoScale";
    private double _photoScale = 1d;

    public double PhotoScale
    {
      get
      {
        return _photoScale;
      }

      set
      {
        if (_photoScale == value)
          return;

        _photoScale = value;
        RaisePropertyChanged(() => PhotoScale);
      }
    }

    public const string PhotoOffsetXPropertyName = "PhotoOffsetX";
    private double _photoOffsetX = 0d;

    public double PhotoOffsetX
    {
      get
      {
        return _photoOffsetX;
      }

      set
      {
        if (_photoOffsetX == value)
          return;

        _photoOffsetX = value;
        RaisePropertyChanged(() => PhotoOffsetX);
      }
    }

    public const string PhotoOffsetYPropertyName = "PhotoOffsetY";
    private double _photoOffsetY = 0d;

    public double PhotoOffsetY
    {
      get
      {
        return _photoOffsetY;
      }

      set
      {
        if (_photoOffsetY == value)
          return;

        _photoOffsetY = value;
        RaisePropertyChanged(() => PhotoOffsetY);
      }
    }

    public const string MagneticCardinalDirectionPropertyName = "MagneticCardinalDirection";
    private CardinalDirection _magneticCardinalDirection = CardinalDirection.Unknow;

    public CardinalDirection MagneticCardinalDirection
    {
      get
      {
        return _magneticCardinalDirection;
      }

      set
      {
        if (_magneticCardinalDirection == value)
          return;

        _magneticCardinalDirection = value;
        RaisePropertyChanged(() => MagneticCardinalDirection);
      }
    }

    public const string TrueCardinalDirectionPropertyName = "TrueCardinalDirection";
    private CardinalDirection _trueCardinalDirection = CardinalDirection.Unknow;

    public CardinalDirection TrueCardinalDirection
    {
      get
      {
        return _trueCardinalDirection;
      }

      set
      {
        if (_trueCardinalDirection == value)
          return;

        _trueCardinalDirection = value;
        RaisePropertyChanged(() => TrueCardinalDirection);
      }
    }

    public const string NeedsCalibrationPropertyName = "NeedsCalibration";
    private bool _needsCalibration = false;

    public bool NeedsCalibration
    {
      get
      {
        return _needsCalibration;
      }

      set
      {
        if (_needsCalibration == value)
          return;

        _needsCalibration = value;
        RaisePropertyChanged(() => NeedsCalibration);
      }
    }

    public const string IsCameraActivePropertyName = "IsCameraActive";
    private bool _isCameraActive = false;

    public bool IsCameraActive
    {
      get
      {
        return _isCameraActive;
      }

      set
      {
        if (_isCameraActive == value)
          return;

        _isCameraActive = value;
        RaisePropertyChanged(() => IsCameraActive);
        Messenger.Default.Send(new ShowCameraPreviewMessage(_isCameraActive));
      }
    }

    public const string IsDialEditPropertyName = "IsDialEdit";
    private bool _isDialEdit = false;

    public bool IsDialEdit
    {
      get
      {
        return _isDialEdit;
      }

      set
      {
        if (_isDialEdit == value)
          return;

        _isDialEdit = value;
        RaisePropertyChanged(() => IsDialEdit);
      }
    }

    private bool _isDialEditManipulation = false;

    public bool IsDialEditManipulation
    {
      get
      {
        return _isDialEditManipulation;
      }

      set
      {
        if (_isDialEditManipulation == value)
          return;

        _isDialEditManipulation = value;
        RaisePropertyChanged(() => IsDialEditManipulation);
      }
    }

    public const string IsBackgroundImageEditPropertyName = "IsBackgroundImageEdit";
    private bool _isBackgroundImageEdit = false;

    public bool IsBackgroundImageEdit
    {
      get
      {
        return _isBackgroundImageEdit;
      }

      set
      {
        if (_isBackgroundImageEdit == value)
          return;

        _isBackgroundImageEdit = value;
        CompasUIOpacity = value ? 0.4 : 1;
        RaisePropertyChanged(() => IsBackgroundImageEdit);
        RaisePropertyChanged(() => HasPhotoAndEditImage);
      }
    }

    public const string IsInitCameraPropertyName = "IsInitCamera";
    private bool _isInitCamera = false;

    public bool IsInitCamera
    {
      get
      {
        return _isInitCamera;
      }

      set
      {
        if (_isInitCamera == value)
          return;

        _isInitCamera = value;
        RaisePropertyChanged(() => IsInitCamera);
      }
    }

    public const string IsCompassModePropertyName = "IsCompassMode";
    private bool _isCompassMode = true;

    public bool IsCompassMode
    {
      get
      {
        return _isCompassMode;
      }

      set
      {
        if (_isCompassMode == value)
          return;

        _isCompassMode = value;
        RaisePropertyChanged(() => IsCompassMode);
      }
    }

    public const string HasPhotoPropertyName = "HasPhoto";
    private bool _hasPhoto = false;

    public bool HasPhoto
    {
      get
      {
        return _hasPhoto;
      }

      set
      {
        if (_hasPhoto == value)
          return;

        _hasPhoto = value;
        RaisePropertyChanged(() => HasPhoto);
        RaisePropertyChanged(() => HasPhotoAndEditImage);
      }
    }

    public List<ICompassTheme> _themes = new List<ICompassTheme>();

    public List<ICompassTheme> Themes
    {
      get
      {
        return _themes;
      }
      set
      {
        _themes = value;
        RaisePropertyChanged(() => Themes);
      }
    }

    public const string CurrentThemePropertyName = "CurrentTheme";
    private ICompassTheme _currentTheme = null;

    public ICompassTheme CurrentTheme
    {
      get
      {
        return _currentTheme;
      }

      set
      {
        if (_currentTheme == value)
          return;

        _currentTheme = value;
        RaisePropertyChanged(() => CurrentTheme);
        CurrentThemeForegroundColor = new SolidColorBrush(_currentTheme.ForegroundColor);

        if (isLoaded)
          Messenger.Default.Send(new CompassThemeChangedMessage());
      }
    }

    public const string CurrentThemeForegroundColorPropertyName = "CurrentThemeForegroundColor";
    private SolidColorBrush _currentThemeForegroundColor;

    public SolidColorBrush CurrentThemeForegroundColor
    {
      get
      {
        return _currentThemeForegroundColor;
      }

      set
      {
        if (_currentThemeForegroundColor == value)
          return;

        _currentThemeForegroundColor = value;
        RaisePropertyChanged(() => CurrentThemeForegroundColor);
      }
    }

    public const string OperationModePropertyName = "OperationMode";
    private OperationModeEnum _operationModeEnum = OperationModeEnum.CompassMode;

    public OperationModeEnum OperationMode
    {
      get
      {
        return _operationModeEnum;
      }

      set
      {
        if (_operationModeEnum == value)
          return;

        _operationModeEnum = value;
        RaisePropertyChanged(() => OperationMode);
      }
    }

    public bool IsLoading
    {
      get
      {
        return !isLoaded;
      }
    }

    public const string CompasUIOpacityPropertyName = "CompasUIOpacity";
    private double _compassUiOpacity = 1d;

    public double CompasUIOpacity
    {
      get
      {
        return _compassUiOpacity;
      }

      set
      {
        if (_compassUiOpacity == value)
          return;

        _compassUiOpacity = value;
        RaisePropertyChanged(() => CompasUIOpacity);
      }
    }

    public bool HasPhotoAndEditImage
    {
      get
      {
        return HasPhoto && IsBackgroundImageEdit;
      }
    }

    private bool _isSavingPhoto;

    public bool IsSavingPhoto
    {
      get
      {
        return _isSavingPhoto;
      }
      set
      {
        if (_isSavingPhoto == value)
          return;
        _isSavingPhoto = value;
        RaisePropertyChanged(() => IsSavingPhoto);
      }
    }

    #endregion Public Properties

    #region public Commands

    public RelayCommand BackPictureEdit { get; set; }
    public RelayCommand CompassDialEdit { get; set; }
    public RelayCommand ShowSettings { get; set; }
    public RelayCommand ShowAbout { get; set; }

    public RelayCommand MainPageLoaded { get; set; }
    public RelayCommand AcceptDialEdit { get; set; }
    public RelayCommand SendFeedbackCommand { get; set; }
    public RelayCommand RateMeCommand { get; set; }
    public RelayCommand OpenWebPageCommand { get; set; }
    public RelayCommand OpenWebPageVOCommand { get; set; }

    public RelayCommand<ICompassTheme> ThemeSelectedCommand { get; set; }

    public RelayCommand TakePhotoCommand { get; set; }
    public RelayCommand SelectPhotoCommand { get; set; }
    public RelayCommand RemovePhotoCommand { get; set; }
    public RelayCommand AcceptPhotoEdit { get; set; }

    #endregion public Commands

    public MainViewModel()
    {
      if (IsInDesignMode)
      {
        // Code runs in Blend --> create design time data.
        InitCompassUI();

        MagneticHeading = 48;
        TrueHeading = 43;
        MagneticDeclination = 5;
        HeadingAccuracy = 10;
        NeedleAngle = -30;
        DialAngle = 20;
      }
      else
      {
        // Code runs "for real"
        if (Compass.IsSupported)
        {
          _compass = new CompassEx();
          _compass.TimeBetweenUpdates = TimeSpan.FromMilliseconds(100);
          _compass.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<CompassReadingEx>>(_compass_CurrentValueChanged);
          _compass.Calibrate += new EventHandler<CalibrationEventArgs>(_compass_Calibrate);
          _compass.Calibrated += new EventHandler<CalibrationEventArgs>(_compass_Calibrated);
        }

        //init commands
        BackPictureEdit = new RelayCommand(() => BackPictureEditAction());
        CompassDialEdit = new RelayCommand(() => CompassDialEditAction());
        ShowSettings = new RelayCommand(() => ShowSettingsAction());
        ShowAbout = new RelayCommand(() => ShowAboutAction());
        MainPageLoaded = new RelayCommand(() => MainPageLoadedAction());
        AcceptDialEdit = new RelayCommand(() => AcceptDialEditAction());
        SendFeedbackCommand = new RelayCommand(() => SendFeedbackAction());
        RateMeCommand = new RelayCommand(() => RateMeAction());
        OpenWebPageCommand = new RelayCommand(() => OpenWebPageAction());
        OpenWebPageVOCommand = new RelayCommand(() => OpenWebPageVOAction());
        ThemeSelectedCommand = new RelayCommand<ICompassTheme>((res) => ThemeSelectedAction(res));
        TakePhotoCommand = new RelayCommand(() => TakePhotoAction());
        SelectPhotoCommand = new RelayCommand(() => SelectPhotoAction());
        RemovePhotoCommand = new RelayCommand(() => RemovePhotoAction());
        AcceptPhotoEdit = new RelayCommand(() => AcceptPhotoEditAction());
      }
    }

    private void OpenWebPageVOAction()
    {
      WebBrowserTask webBrowserTask = new WebBrowserTask();
      webBrowserTask.Uri = new Uri(Constants.WEB_PAGE_VO);
      webBrowserTask.Show();
    }

    private void InitCompassUI()
    {
      if (!IsInDesignMode)
      {
        Themes = CompassThemeController.Instance.GetAllThemes();
        if (!string.IsNullOrEmpty(AppSettings.Instance.CurrentTheme))
        {
          ICompassTheme theme = Themes.SingleOrDefault(t => t.ThemeName == AppSettings.Instance.CurrentTheme);
          if (theme != null)
            CurrentTheme = theme;
          else
            CurrentTheme = Themes[0];
        }
        else
        {
          CurrentTheme = Themes[0];
        }

        DialAngle = AppSettings.Instance.DialAngle;
      }
    }

    private void AcceptPhotoEditAction()
    {
      //save image
      string fileSource = Path.Combine(Constants.PHOTO_FOLDER, Constants.TEMP_PHOTO_FILENAME);
      if (IsolatedStorageHelper.FileExist(fileSource))
      {
        //save the temp
        string fileDestination = Path.Combine(Constants.PHOTO_FOLDER, Constants.FINAL_PHOTO_FILENAME);
        IsolatedStorageHelper.CopyFile(fileSource, fileDestination);
        IsolatedStorageHelper.DeleteFile(fileSource);
      }
      else
      {
        //load rotation and scaling values
        AppSettings.Instance.PhotoOffsetX = PhotoOffsetX;
        AppSettings.Instance.PhotoOffsetY = PhotoOffsetY;
        AppSettings.Instance.PhotoScale = PhotoScale;
        AppSettings.Instance.PhotoAngle = PhotoAngle;
        AppSettings.Save();
      }
      //
      SetOperationMode(OperationModeEnum.CompassMode);
    }

    private void RemovePhotoAction()
    {
      //remove photo
      if (MessageBox.Show("Remove background map/image?", "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
      {
        string fileDestination = Path.Combine(Constants.PHOTO_FOLDER, Constants.FINAL_PHOTO_FILENAME);
        IsolatedStorageHelper.DeleteFile(fileDestination);
        HasPhoto = false;
        SetOperationMode(OperationModeEnum.CompassMode);
      }
    }

    private bool isPhotoChooser;

    private void SelectPhotoAction()
    {
      if (!isPhotoChooser)
      {
        isPhotoChooser = true;

        //select photo
        try
        {
          Messenger.Default.Send(new ShowCameraPreviewMessage(false));
          //
          PhotoChooserTask photoChooserTask = new PhotoChooserTask();
          photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
          photoChooserTask.Show();
        }
        catch (Exception)
        {
        }
        finally
        {
          isPhotoChooser = false;
        }
      }
    }

    private void photoChooserTask_Completed(object sender, PhotoResult e)
    {
      isPhotoChooser = false;
      if (e.ChosenPhoto != null && e.ChosenPhoto.Length > 0)
      {
        using (MemoryStream ms = new MemoryStream())
        {
          byte[] buffer = new byte[4096];
          int len = 0;
          while ((len = e.ChosenPhoto.Read(buffer, 0, buffer.Length)) > 0)
          {
            ms.Write(buffer, 0, len);
          }
          IsolatedStorageHelper.SaveToLocalStorage(Constants.FINAL_PHOTO_FILENAME, Constants.PHOTO_FOLDER, ms.ToArray());
        }
        Messenger.Default.Send(new ShowCameraPreviewMessage(false));

        WriteableBitmap bmp = IsolatedStorageHelper.LoadFromLocalStorage(Constants.FINAL_PHOTO_FILENAME, Constants.PHOTO_FOLDER);
        App.Locator.Main.PhotoOffsetX = -(double)bmp.PixelWidth / 2;
        App.Locator.Main.PhotoOffsetY = -(double)bmp.PixelHeight / 2;
        App.Locator.Main.PhotoScale = (double)1066 / bmp.PixelWidth;
        App.Locator.Main.PhotoAngle = 0;
        App.Locator.Main.SavePhotoSettings();

        App.Locator.Main.HasPhoto = true;
        Messenger.Default.Send(new LoadPhotoMessage());
        Messenger.Default.Send(new RefreshAppBarMessage());
      }
      else
      {
        Messenger.Default.Send(new ShowCameraPreviewMessage(true));
      }
    }

    private void TakePhotoAction()
    {
      //take photo
      Messenger.Default.Send(new TakePhotoMessage());
    }

    private void ThemeSelectedAction(ICompassTheme res)
    {
      CurrentTheme = res;
      //save
      AppSettings.Instance.CurrentTheme = res.ThemeName;
      AppSettings.Save();
      //
      App.Locator.NavigationService.GoBack();
    }

    private void OpenWebPageAction()
    {
      WebBrowserTask webBrowserTask = new WebBrowserTask();
      webBrowserTask.Uri = new Uri(Constants.WEB_PAGE);
      webBrowserTask.Show();
    }

    private void SendFeedbackAction()
    {
      EmailComposeTask emailTask = new EmailComposeTask();
      emailTask.Subject = "Compass VO - Feedback and ideas";
      emailTask.To = Constants.JWP_SUPPORT_EMAIL;
      emailTask.Body = "Hi Just Windows Phone!" + Environment.NewLine + Environment.NewLine + "Hereby my feedback:" + Environment.NewLine + "- " + Environment.NewLine + Environment.NewLine + "See ya!";
      emailTask.Show();
    }

    private void RateMeAction()
    {
      try
      {
        MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
        marketplaceReviewTask.Show();
      }
      catch (Exception)
      {
        MessageBox.Show("The application cannot be reviewed!");
      }
    }

    private void AcceptDialEditAction()
    {
      //save
      AppSettings.Instance.DialAngle = DialAngle;
      AppSettings.Save();
      //
      IsDialEdit = false;
      IsCompassMode = true;
      Messenger.Default.Send(new RefreshAppBarMessage());
    }

    private void ShowAboutAction()
    {
      App.Locator.NavigationService.NavigateTo(new Uri("/AboutPage.xaml", UriKind.Relative));
    }

    private bool isLoaded;

    private void MainPageLoadedAction()
    {
      LoadPhoto();

      isLoaded = true;
      RaisePropertyChanged(() => IsLoading);

      //init UI
      InitCompassUI();

      if (Microsoft.Devices.Environment.DeviceType == DeviceType.Emulator)
      {
        MagneticHeading = 55;
        TrueHeading = 50;
        MagneticDeclination = 5;
        HeadingAccuracy = 10;
        NeedleAngle = 55;
        NeedleCardinalDirection = CardinalDirection.NE;
        DialAngle = 0;
        MagneticCardinalDirection = CardinalDirection.NW;
        TrueCardinalDirection = CardinalDirection.NW;
        PhotoAngle = -15;
      }

      Messenger.Default.Send(new RefreshAppBarMessage());

      if (Compass.IsSupported && _compass != null)
      {
        _compass.Start();
      }
      else
      {
        MessageBox.Show("Your device doesn't have compass sensor!");
      }
    }

    private void LoadPhoto()
    {
      LoadPhotoSettings();

      WriteableBitmap bmp = IsolatedStorageHelper.LoadFromLocalStorage(Constants.FINAL_PHOTO_FILENAME, Constants.PHOTO_FOLDER);
      HasPhoto = bmp != null;

      Messenger.Default.Send(new LoadPhotoMessage());
    }

    public void LoadPhotoSettings()
    {
      PhotoOffsetX = AppSettings.Instance.PhotoOffsetX;
      PhotoOffsetY = AppSettings.Instance.PhotoOffsetY;
      PhotoScale = AppSettings.Instance.PhotoScale;
      PhotoAngle = AppSettings.Instance.PhotoAngle;
    }

    public void SavePhotoSettings()
    {
      AppSettings.Instance.PhotoOffsetX = PhotoOffsetX;
      AppSettings.Instance.PhotoOffsetY = PhotoOffsetY;
      AppSettings.Instance.PhotoScale = PhotoScale;
      AppSettings.Instance.PhotoAngle = PhotoAngle;
      AppSettings.Save();
    }

    private void ShowSettingsAction()
    {
      App.Locator.NavigationService.NavigateTo(new Uri("/SettingsPage.xaml", UriKind.Relative));
    }

    private double initPhotoOffsetX;
    private double initPhotoOffsetY;
    private double initPhotoScale;
    private double initPhotoAngle;

    private void BackPictureEditAction()
    {
      if (CurrentTheme.SupportsBackgroundImages)
      {
        if (!HasPhoto)
        {
          IsInitCamera = true;
          IsCameraActive = true;
        }
        else
        {
          //store init values
          initPhotoOffsetX = PhotoOffsetX;
          initPhotoOffsetY = PhotoOffsetY;
          initPhotoScale = PhotoScale;
          initPhotoAngle = PhotoAngle;
        }
        SetOperationMode(OperationModeEnum.PhotoEdit);
      }
    }

    private void CompassDialEditAction()
    {
      if (CurrentTheme.SupportsDialRotation)
      {
        initDialAngle = DialAngle;
        SetOperationMode(OperationModeEnum.DialEdit);
      }
    }

    private void _compass_CurrentValueChanged(object sender, SensorReadingEventArgs<CompassReadingEx> e)
    {
      DispatcherHelper.CheckBeginInvokeOnUI(() =>
      {
        MagneticHeading = e.SensorReading.MagneticHeading;
        TrueHeading = e.SensorReading.TrueHeading;
        MagneticDeclination = e.SensorReading.MagneticDeclination;
        HeadingAccuracy = e.SensorReading.HeadingAccuracy;
        NeedleAngle = IsHeadingMagnetic ? e.SensorReading.MagneticHeading : e.SensorReading.TrueHeading;
        NeedleCardinalDirection = IsHeadingMagnetic ? e.SensorReading.MagneticCardinalDirection : e.SensorReading.TrueCardinalDirection;
        MagneticCardinalDirection = e.SensorReading.MagneticCardinalDirection;
        TrueCardinalDirection = e.SensorReading.TrueCardinalDirection;
      });
    }

    private void _compass_Calibrated(object sender, CalibrationEventArgs e)
    {
      DispatcherHelper.CheckBeginInvokeOnUI(() =>
      {
        NeedsCalibration = false;
        Microsoft.Devices.VibrateController.Default.Start(TimeSpan.FromMilliseconds(300));
      });
    }

    private void _compass_Calibrate(object sender, CalibrationEventArgs e)
    {
      DispatcherHelper.CheckBeginInvokeOnUI(() =>
      {
        NeedsCalibration = true;
        Microsoft.Devices.VibrateController.Default.Start(TimeSpan.FromMilliseconds(300));
      });
    }

    public void CompassDialEditStarted(Point point)
    {
      if (IsDialEdit)
      {
        IsDialEditManipulation = true;
        initDialAngleDrag = DialAngle;
        startDialAngle = CurrentTheme.GetAngle(point);
      }
    }

    public void CompassDialEditCompleted()
    {
      if (IsDialEdit)
      {
        IsDialEditManipulation = false;
      }
    }

    public void CompassDialEditDelta(Point point)
    {
      if (IsDialEdit)
      {
        deltaDialAngle = CurrentTheme.GetAngle(point);
        DialAngle = initDialAngleDrag + (startDialAngle - deltaDialAngle);
      }
    }

    private void SetOperationMode(OperationModeEnum operationMode)
    {
      OperationMode = operationMode;

      switch (operationMode)
      {
        case OperationModeEnum.CompassMode:
          IsCompassMode = true;
          IsBackgroundImageEdit = false;
          IsDialEdit = false;
          break;

        case OperationModeEnum.DialEdit:
          IsCompassMode = false;
          IsBackgroundImageEdit = false;
          IsDialEdit = true;
          break;

        case OperationModeEnum.PhotoEdit:
          IsCompassMode = false;
          IsBackgroundImageEdit = true;
          IsDialEdit = false;
          break;
      }
      Messenger.Default.Send(new RefreshAppBarMessage());
    }

    public void CancelEdit()
    {
      if (IsDialEdit)
      {
        DialAngle = initDialAngle;
      }
      else if (IsBackgroundImageEdit)
      {
        IsCameraActive = false;
        PhotoOffsetX = initPhotoOffsetX;
        PhotoOffsetY = initPhotoOffsetY;
        PhotoScale = initPhotoScale;
        PhotoAngle = initPhotoAngle;
      }

      SetOperationMode(OperationModeEnum.CompassMode);
    }

    public override void Cleanup()
    {
      if (Compass.IsSupported && _compass != null)
      {
        _compass.Stop();
      }
      base.Cleanup();
    }
  }
}