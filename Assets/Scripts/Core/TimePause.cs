using System;
using UnityEngine;

namespace Kumi.Core
{
    /// <summary>
    /// Controls and reports when the game should be paused or not.
    /// </summary>
    public static class TimePause
    {
        public static event Action<bool> Toggled;
        public static bool Paused { get; set; }
        public static bool Enabled { get; set; }
        public static void Pause()
        {
            if (Enabled == false) return;
            Time.timeScale = 0f;
            Paused = true;
            Toggled?.Invoke(true);
        }
        
        public static void Resume()
        {
            if (Enabled == false) return;
            Time.timeScale = 1f;
            Paused = false;
            Toggled?.Invoke(false);
        
        }

        public static void Switch()
        {
            if (Enabled == false) return;
            switch (Paused)
            {
                case true:
                    Resume();
                    break;
                case false:
                    Pause();
                    break;
            }
        }
    }
}
