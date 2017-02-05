using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace CompassVO.Utils
{
  public class MediaHelper
  {
    public static void PlaySound(string soundFile)
    {
      using (var stream = TitleContainer.OpenStream(soundFile))
      {
        var effect = SoundEffect.FromStream(stream);
        FrameworkDispatcher.Update();
        effect.Play();
      }
    }
  }
}