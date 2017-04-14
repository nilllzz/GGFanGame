using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GGFanGame.FMOD
{
    internal class MusicPlayer// : IGameComponent, IDisposable
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        private System _system;
        private Channel _channel;
        private Sound[] _songs;

        internal bool IsDisposed { get; private set; }

        public void Initialize()
        {
            LoadLibrary(Path.GetFullPath(@"FMOD\fmod.dll"));

            Factory.System_Create(out _system);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~MusicPlayer()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_system != null) _system.release();
                }

                _system = null;

                IsDisposed = true;
            }
        }
    }
}
