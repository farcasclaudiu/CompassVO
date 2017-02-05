using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

namespace CompassVO.Model
{
  [DataContract]
  public class AppSettings
  {
    [IgnoreDataMember]
    public const string APP_SETTINGS_KEY = "APP_SETTINGS_KEY";

    private double _dialAngle;

    [DataMember]
    public double DialAngle
    {
      get { return _dialAngle; }
      set
      {
        if (_dialAngle == value)
          return;
        _dialAngle = value;
      }
    }

    private string _currentTheme;

    [DataMember]
    public string CurrentTheme
    {
      get { return _currentTheme; }
      set
      {
        if (_currentTheme == value)
          return;
        _currentTheme = value;
      }
    }

    private double _photoOffsetX = 0;

    [DataMember]
    public double PhotoOffsetX
    {
      get { return _photoOffsetX; }
      set
      {
        if (_photoOffsetX == value)
          return;
        _photoOffsetX = value;
      }
    }

    private double _photoOffsetY = 0;

    [DataMember]
    public double PhotoOffsetY
    {
      get { return _photoOffsetY; }
      set
      {
        if (_photoOffsetY == value)
          return;
        _photoOffsetY = value;
      }
    }

    private double _photoScale = 1;

    [DataMember]
    public double PhotoScale
    {
      get { return _photoScale; }
      set
      {
        if (_photoScale == value)
          return;
        _photoScale = value;
      }
    }

    private double _photoAngle = 0;

    [DataMember]
    public double PhotoAngle
    {
      get { return _photoAngle; }
      set
      {
        if (_photoAngle == value)
          return;
        _photoAngle = value;
      }
    }

    private bool _isHeadingMagnetic = true;

    [DataMember]
    public bool IsHeadingMagnetic
    {
      get { return _isHeadingMagnetic; }
      set
      {
        if (_isHeadingMagnetic == value)
          return;
        _isHeadingMagnetic = value;
      }
    }

    private AppSettings()
    {
    }

    private static AppSettings _instance;

    public static AppSettings Instance
    {
      get
      {
        if (_instance == null)
        {
          if (IsolatedStorageSettings.ApplicationSettings.Contains(APP_SETTINGS_KEY))
            _instance = IsolatedStorageSettings.ApplicationSettings[APP_SETTINGS_KEY] as AppSettings;
          else
            _instance = new AppSettings();
        }
        return _instance;
      }
    }

    public static object _lock = new object();

    public static void Save()
    {
      lock (_lock)
      {
        if (IsolatedStorageSettings.ApplicationSettings.Contains(APP_SETTINGS_KEY))
        {
          IsolatedStorageSettings.ApplicationSettings[APP_SETTINGS_KEY] = AppSettings.Instance;
        }
        else
        {
          IsolatedStorageSettings.ApplicationSettings.Add(APP_SETTINGS_KEY, AppSettings.Instance);
        }
        IsolatedStorageSettings.ApplicationSettings.Save();
      }
    }

    public static void Load()
    {
      lock (_lock)
      {
        if (IsolatedStorageSettings.ApplicationSettings.Contains(APP_SETTINGS_KEY))
          _instance = IsolatedStorageSettings.ApplicationSettings[APP_SETTINGS_KEY] as AppSettings;
      }
    }
  }
}