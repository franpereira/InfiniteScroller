using Kumi.Core;
using System;
using UnityEngine;

namespace Kumi.Ingredients
{
    /// <summary>
    /// Stores the time duration of the last game.
    /// </summary>
    public class Duration : MonoBehaviour
    {
        /// <summary>
        /// Time from the start to the end of the game.
        /// </summary>
        public static TimeSpan UntilEnd { get; private set; }

        TimeSpan aux;
        DateTime beginTime;

        
        DateTime startPoint;
        DateTime cutPoint;

        private void Start() => UntilEnd = TimeSpan.Zero;

        private void OnEnable()
        {
            Events.Begin += OnBegin;
            TimePause.Toggled += OnTimePause;
            Events.End += OnEnd;
        }

        private void OnDisable()
        {
            Events.Begin -= OnBegin;
            TimePause.Toggled -= OnTimePause;
            Events.End -= OnEnd;
        }

        void OnBegin()
        {
            aux = TimeSpan.Zero;
            beginTime = DateTime.UtcNow;
            startPoint = beginTime;
        }

        void OnTimePause(bool paused)
        {
            if (paused)
            {
                //Adds the time between the last saved point and the current one to the aux time.
                cutPoint = DateTime.UtcNow;
                PointsToSpan();
            }
            else
            {
                // The time elapsed on the pause is ignored, it starts counting again since the game resumes.
                startPoint = DateTime.UtcNow;
                cutPoint = startPoint;
            }
        }

        void OnEnd()
        {
            cutPoint = DateTime.UtcNow;
            PointsToSpan();
            UntilEnd = aux;
            Debug.Log($"Duration: {Convert.ToInt32(UntilEnd.TotalSeconds)}s");
        }

        void PointsToSpan()
        {
            TimeSpan amount = cutPoint - startPoint;
            aux += amount;
        }
    }
}
