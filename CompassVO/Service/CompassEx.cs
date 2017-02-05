using CompassVO.Utils.Filters;
using Microsoft.Devices.Sensors;
using System;

namespace CompassVO.Service
{
  public class CompassEx : IDisposable
  {
    private Compass _compass;
    private bool _needsCalibration;
    private SimpleKalman filteredHeading = new SimpleKalman();

    #region static Properties

    private static bool isInit;
    private static bool _isSupported;

    public static bool IsSupported
    {
      get
      {
        if (!isInit)
        {
          _isSupported = Compass.IsSupported;
          isInit = true;
        }
        return _isSupported;
      }
      set { _isSupported = value; }
    }

    #endregion static Properties

    #region public properties

    public TimeSpan TimeBetweenUpdates { get; set; }

    private CompassReadingEx _currentValue = new CompassReadingEx();

    public CompassReadingEx CurrentValue
    {
      get
      {
        return _currentValue;
      }
    }

    public bool IsDataValid
    {
      get { return _compass != null ? _compass.IsDataValid : false; }
    }

    #endregion public properties

    #region public events

    public event System.EventHandler<CalibrationEventArgs> Calibrate;

    public event System.EventHandler<CalibrationEventArgs> Calibrated;

    public event System.EventHandler<SensorReadingEventArgs<CompassReadingEx>> CurrentValueChanged;

    #endregion public events

    public CompassEx()
    {
      _compass = new Compass();
      _compass.Calibrate += new EventHandler<CalibrationEventArgs>(_compass_Calibrate);
      _compass.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<CompassReading>>(_compass_CurrentValueChanged);
    }

    private void _compass_CurrentValueChanged(object sender, SensorReadingEventArgs<CompassReading> e)
    {
      if (_needsCalibration && e.SensorReading.HeadingAccuracy <= 20)
      {
        _needsCalibration = false;
        Calibrated?.Invoke(sender, new CalibrationEventArgs());
      }

      if (_compass != null)
      {
        ProcessCurrentValue(e.SensorReading);
      }

      if (CurrentValueChanged != null)
      {
        CurrentValueChanged(sender, new SensorReadingEventArgs<CompassReadingEx>() { SensorReading = CurrentValue });
      }
    }

    private void _compass_Calibrate(object sender, CalibrationEventArgs e)
    {
      _needsCalibration = true;
      Calibrate?.Invoke(sender, e);
    }

    private bool isFirstReading;
    private double md;

    private void ProcessCurrentValue(CompassReading compassReading)
    {
      double newmd = compassReading.TrueHeading - compassReading.MagneticHeading;
      if (Math.Abs(newmd) < 45)
      {
        md = newmd;
      }

      if (isFirstReading)
      {
        filteredHeading.init(compassReading.TrueHeading);
        isFirstReading = false;
      }

      double currentFilter = filteredHeading.current();
      double currentRead = compassReading.TrueHeading - md;
      if (Math.Abs(currentRead - currentFilter) > 180)
      {
        if (currentRead > currentFilter)
          currentRead -= 360;
        else if (currentRead < currentFilter)
          currentRead += 360;

        if (Math.Sign(currentRead) == Math.Sign(currentFilter))
        {
          if (currentRead < 0)
          {
            currentRead += 360;
            currentFilter += 360;
            filteredHeading.init(currentFilter);
          }
          else if (currentRead > 360 && currentFilter > 360)
          {
            currentRead -= 360;
            currentFilter -= 360;
            filteredHeading.init(currentFilter);
          }
        }
      }

      currentRead = filteredHeading.update(currentRead);
      currentRead = (currentRead + 360) % 360;
      double trueRead = ((currentRead + md) + 360) % 360;

      _currentValue = new CompassReadingEx()
      {
        HeadingAccuracy = compassReading.HeadingAccuracy,
        TrueHeading = trueRead,
        MagneticHeading = currentRead,
        MagneticDeclination = md,
        MagnetometerReading = compassReading.MagnetometerReading,
        Timestamp = compassReading.Timestamp,
        MagneticCardinalDirection = CompassReadingEx.GetCardinalDirection(currentRead),
        TrueCardinalDirection = CompassReadingEx.GetCardinalDirection(trueRead)
      };
    }

    #region public methods

    public void Start()
    {
      if (IsSupported)
      {
        _compass.TimeBetweenUpdates = this.TimeBetweenUpdates;
        _compass.Start();
      }
    }

    public void Stop()
    {
      if (_compass != null)
        _compass.Stop();
    }

    #endregion public methods

    #region Implementation of IDisposable

    public void Dispose()
    {
      if (_compass != null)
        _compass.Dispose();
    }

    #endregion Implementation of IDisposable
  }
}