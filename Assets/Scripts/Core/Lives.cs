using Kumi.World.Cluttering;
using UnityEngine;

namespace Kumi.Core
{
    /// <summary>
    /// Manages the number of lives of the player.
    /// </summary>
    public class Lives : MonoBehaviour
    {
        /// <summary>
        /// How many lives the player currently has.
        /// </summary>
        public static int Value { get; private set; }
        /// <summary>
        /// The number of additional lives the player has earned on the current game.
        /// </summary>
        public static int Obtained { get; private set; }

        private void Start()
        {
            Value = 1;
            Obtained = 0;
        }

        private void OnEnable()
        {
            Heart.Picked += AddOne;
            Events.End += OnEnd;
        }
        private void OnDisable()
        {
            Heart.Picked -= AddOne;
            Events.End -= OnEnd;
        }

        void OnEnd()
        {
            Debug.Log($"Obtained lives: {Obtained}");
        }

        internal static void AddOne()
        {
            Value++;
            Obtained++;
        }

        internal static void LoseOne() => --Value;
    }
}