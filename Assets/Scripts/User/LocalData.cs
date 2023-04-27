using Kumi.Device;
using System;
using UnityEngine;

namespace Kumi.User
{
    /// <summary>
    /// Stores and persists locally the information and settings of the player.
    /// </summary>
    public class LocalData : MonoBehaviour
    {
        /// <summary>
        /// The highest score recorded to be achieved by the player.
        /// </summary>
        public static long HighScore
        {
            get
            {
                long value = Convert.ToInt64(PlayerPrefs.GetString("HS", "0"));
                return value;
            }

            set
            {
                if (value < HighScore) return;
                PlayerPrefs.SetString("HS", value.ToString());
                PlayerPrefs.Save();
            }
        }

        public static bool VibrationEnabled
        {
            get => PlayerPrefs.GetInt("Vibration", 1) == 1;
            set
            {
                Vibration.Enabled = value;
                int boolean = value ? 1 : 0;
                PlayerPrefs.SetInt("Vibration", boolean);
            }
        }

        /// <summary>
        /// If the score should be displayed while playing.
        /// This doesn't mean the score not being shown at the end.
        /// </summary>
        public static bool ShowScore
        {
            get => PlayerPrefs.GetInt("ShowScore", 1) == 1;
            set
            {
                int boolean = value ? 1 : 0;
                PlayerPrefs.SetInt("ShowScore", boolean);
            }
        }
    }
}