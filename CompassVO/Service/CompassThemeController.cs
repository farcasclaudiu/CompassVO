using CompassVO.Themes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CompassVO.Service
{
  public class CompassThemeController
  {
    private static CompassThemeController _instance;

    public static CompassThemeController Instance
    {
      get
      {
        if (_instance == null)
          _instance = new CompassThemeController();
        return _instance;
      }
    }

    private List<ICompassTheme> _allThemes;

    public List<ICompassTheme> GetAllThemes()
    {
      if (_allThemes == null)
      {
        _allThemes = new List<ICompassTheme>();
        foreach (Type appType in Assembly.GetExecutingAssembly().GetTypes())
        {
          if (appType != typeof(ICompassTheme) && appType.GetInterface(typeof(ICompassTheme).FullName, false) != null)
          {
            _allThemes.Add((ICompassTheme)Activator.CreateInstance(appType));
          }
        }
        _allThemes.Sort((a, b) => { return a.Order.CompareTo(b.Order); });
      }

      return _allThemes;
    }
  }
}