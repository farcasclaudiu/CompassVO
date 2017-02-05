using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using System;

namespace CompassVO.Service
{
  public struct CompassReadingEx : ISensorReading
  {
    public DateTimeOffset Timestamp
    {
      get;
      internal set;
    }

    public double HeadingAccuracy
    {
      get;
      internal set;
    }

    public double TrueHeading
    {
      get;
      internal set;
    }

    public Vector3 MagnetometerReading
    {
      get;
      internal set;
    }

    public double MagneticHeading
    {
      get;
      internal set;
    }

    public double MagneticDeclination
    {
      get;
      internal set;
    }

    public CardinalDirection MagneticCardinalDirection
    {
      get;
      internal set;
    }

    public CardinalDirection TrueCardinalDirection
    {
      get;
      internal set;
    }

    public static CardinalDirection GetCardinalDirection(double angle)
    {
      angle = angle % 360;
      if (angle < 22.5 || angle >= 337.5)
      {
        return CardinalDirection.N;
      }
      if (angle >= 22.5 && angle < 67.5)
      {
        return CardinalDirection.NE;
      }
      if (angle >= 67.5 && angle < 112.5)
      {
        return CardinalDirection.E;
      }
      if (angle >= 112.5 && angle < 157.5)
      {
        return CardinalDirection.SE;
      }
      if (angle >= 157.5 && angle < 202.5)
      {
        return CardinalDirection.S;
      }
      if (angle >= 202.5 && angle < 247.5)
      {
        return CardinalDirection.SW;
      }
      if (angle >= 247.5 && angle < 292.5)
      {
        return CardinalDirection.W;
      }
      if (angle >= 292.5 && angle < 337.5)
      {
        return CardinalDirection.NW;
      }
      return CardinalDirection.Unknow;
    }
  }

  public enum CardinalDirection
  {
    Unknow,
    N,
    S,
    E,
    W,
    NE,
    SE,
    NW,
    SW
  }
}