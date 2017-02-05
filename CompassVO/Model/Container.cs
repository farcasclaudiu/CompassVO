using MicroIoc;

namespace CompassVO.Model
{
  public sealed class Container
  {
    private Container()
    {
    }

    public static IMicroIocContainer Instance
    {
      get
      {
        return Nested.instance;
      }
    }

    private class Nested
    {
      // Explicit static constructor to tell C# compiler
      // not to mark type as before field init
      static Nested()
      {
      }

      internal static readonly IMicroIocContainer instance =
          new MicroIocContainer();
    }
  }
}