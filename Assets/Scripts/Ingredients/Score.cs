using Kumi.Characters;
using System;
using UnityEngine;

namespace Kumi.Ingredients
{
    /// <summary>
    /// Manages the Score of the player in the current game.
    /// </summary>
    public class Score : MonoBehaviour
    {
        public static event Action Update;

        public static long Total { get; private set; }
        public static int Combo { get; private set; }

        int lastLandedY; //The Y of the last platform where the player landed.

        void Awake()
        {
            Total = 0;
            Combo = 0;
        }

        private void OnEnable()
        {
            PlayerCharacter.Landing += UpdateScore;
        }
        private void OnDisable()
        {
            PlayerCharacter.Landing -= UpdateScore;
        }

        void UpdateScore(Transform platform)
        {
            int platY = Convert.ToInt32(platform.position.y);
            int difference = platY - lastLandedY;

            switch (difference)
            {
                case > 1:
                    Total += difference * 10;
                    Combo += (difference - 1) * 6;
                    lastLandedY = platY; break;
                case 1:
                    Total += 10 + Combo;
                    Combo = 0;
                    lastLandedY = platY; break;
                default:
                    Combo = 0; break;
            }
            Update?.Invoke();
        }
    }
}