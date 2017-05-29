using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Reflection;

namespace Turning
{
    public enum TurningSound {
        None, Place, RotateAndShoot, Explode, Error, TurningSound_Count
    }

    public class SoundManager
    {
        Dictionary<TurningSound,SoundPlayer> players;
        public SoundManager()
        {
            players = new Dictionary<TurningSound, SoundPlayer>();
            var sourceDirectoryResources = SourceDirectory()+"/Resources";
            players[TurningSound.Place] = new SoundPlayer(sourceDirectoryResources + "/drip2.wav");
            players[TurningSound.RotateAndShoot] = new SoundPlayer(sourceDirectoryResources + "/lego_click.wav");
            players[TurningSound.Explode] = new SoundPlayer(sourceDirectoryResources + "/drip1.wav"); 
            players[TurningSound.Error] = new SoundPlayer(sourceDirectoryResources + "/uhoh.wav");
        }

        public void Play(TurningSound sound)
        {
            players[sound].Play();
        }

        private string SourceDirectory()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            path = path.Replace("\\", "/");

            path = path.Replace("/bin", "");
            path = path.Replace("/Debug", "");
            path = path.Replace("/Release", "");
            return path;
        }
    }
}
