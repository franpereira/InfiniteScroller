using Kumi.UI;
using UnityEngine;

namespace Kumi.Core
{
    /// <summary>
    /// Behavior that should be executed when a new game is starting.
    /// </summary>
    internal class Begin : MonoBehaviour
    {
        public static Begin Instance { get; private set; }
        private void Awake() => Instance = this;

        /// <summary>
        /// If the player has already played once during this execution
        /// </summary>
        public static bool WarmStart { get; private set; } = false;
        public bool IgnoreWarmStart; //Just for debuging purposes

        private void Start()
        {
            //On a scene restart, because this only happens from the player hitting Restart or Play Again
            //after losing a game, a new game will be triggered immediately.
            if (IgnoreWarmStart == false && WarmStart) PrepareBegin();
        }

        private void OnEnable()
        {
            UIMain.PlayButtonClicked += PrepareBegin;
        }
        private void OnDisable()
        {
            UIMain.PlayButtonClicked -= PrepareBegin;
        }

        void PrepareBegin()
        {
            TimePause.Paused = false;
            TimePause.Enabled = true;
            Events.InvokeBegin();
            WarmStart = true;
        }
    }
}