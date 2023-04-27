using Kumi.Cameras;
using UnityEngine;

namespace Kumi.Core
{
    /// <summary>
    /// Behaviour when something that could make the player lose a life has happened.
    /// </summary>
    internal class EndConditionCheck : MonoBehaviour
    {
        public static EndConditionCheck Instance { get; private set; }
        private void Awake() => Instance = this;

        private void OnEnable()
        {
            CameraDirector.CameraOvercamePlayer += OnCameraOvercamePlayer;
        }
        private void OnDisable()
        {
            CameraDirector.CameraOvercamePlayer -= OnCameraOvercamePlayer;
        }

        void OnCameraOvercamePlayer() => Check(EndCondition.PlayerUnderCamera);

        void Check(EndCondition condition)
        {
            Events.InvokeEndConditionCheck();
            Lives.LoseOne();
            if (Lives.Value <= 0)
            {
                Debug.Log("Continue Question");
                Continue.Question();
            }
            else
            {
                Resurrection.Resurrect(condition);
            }
        }

    }

    internal enum EndCondition
    {
        PlayerUnderCamera, Other
    }
}