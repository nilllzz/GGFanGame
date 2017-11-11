using Microsoft.Xna.Framework.Media;
using System.Threading.Tasks;

namespace GGFanGame.Content
{
    internal static class MusicPlayerHelper
    {
        internal static async Task FadeIn(int delayMs)
        {
            MediaPlayer.Volume = 0f;
            var tempVolume = 0f; // used to reduce calls to the MediaPlayer.Volume API by half

            while (tempVolume < 1f)
            {
                MediaPlayer.Volume += 0.01f;
                tempVolume += 0.01f;
                await Task.Delay(delayMs);
            }
        }
    }
}
