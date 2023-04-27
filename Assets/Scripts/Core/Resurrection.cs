using Kumi.Characters;
using Kumi.Cameras;
using Kumi.Device;
using UnityEngine;

namespace Kumi.Core
{
    /// <summary>
    /// When after losing a life, the player still has more left and has to be able to keep playing.
    /// </summary>
    internal class Resurrection : MonoBehaviour
    {
        const float cameraOffset = 12f;
        static readonly Vector2 impulseVelocity = new(0f, 42f);

        public static void Resurrect(EndCondition condition = EndCondition.Other)
        {
            Debug.Log("Resurrect");
            ImpulsePlayer();
            CameraDirector.Instance.Shake(1f, 32);
            Vibration.Vibrate(milliseconds: 100, amplitude: 127);
            Events.InvokeResurrection();
        }

        static void ImpulsePlayer()
        {
            var moveTo = new Vector3(0f, Camera.main.transform.position.y - cameraOffset);
            PlayerCharacter.Instance.transform.position = moveTo;
            PlayerCharacter.Instance.Body.velocity = impulseVelocity;
        }
    }
}