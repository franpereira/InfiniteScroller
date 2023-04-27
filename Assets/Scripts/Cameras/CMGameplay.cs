using Cinemachine;
using UnityEngine;

namespace Kumi.Cameras
{
    public class CMGameplay : CinemachineExtension
    {
        public bool ScrollEnabled;
        public float ScrollSpeed;
        public float LastY;
        Vector3 pos;

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            pos = state.RawPosition;
            pos.x = 0f; //Avoid the camera moving to the sides.
            if (pos.y < LastY) pos.y = LastY; //The camera won't go lower, just upwards.
            LastY = pos.y;
            if (ScrollEnabled) pos.y += ScrollSpeed * deltaTime;

            state.RawPosition = pos;
        }
    }
}