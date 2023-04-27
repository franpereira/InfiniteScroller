using Kumi.Cameras;
using Kumi.Device;
using UnityEngine;

namespace Kumi.Core
{
    /// <summary>
    /// When the player has run out of lives but could get a bonus one.
    /// </summary>
    public class Continue : MonoBehaviour
    {
        
        public static Continue Instance { get; private set; }
        private void Awake() => Instance = this;
        
        internal static void Question()
        {
            TimePause.Enabled = false;
            CameraDirector.Instance.Shake(2f, 64);
            Vibration.Vibrate(milliseconds: 200, amplitude: 255);
            Events.InvokeContinueQuestion();
        }


        public static void Answer(bool answer)
        {
            Events.InvokeContinueAnswer(answer);
            switch (answer)
            {
                case true:
                    Lives.AddOne();
                    Resurrection.Resurrect();
                    TimePause.Enabled = true;
                    break;
                case false:
                    Vibration.Vibrate(milliseconds: 100, amplitude: 31);
                    Events.InvokeEnd();
                    break;
                    
            }
        }
    }
}