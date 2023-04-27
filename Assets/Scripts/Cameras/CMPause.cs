using Cinemachine;
using UnityEngine;

namespace Kumi.Cameras
{
    public class CMPause : CinemachineExtension
    {
        Vector3 pos;
        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            pos = state.RawPosition;
            //Avoid the camera to show an empty space between the platforms and the side walls because of perspective.
            pos.x = Mathf.Clamp(pos.x, -4f, 4f);
            state.RawPosition = pos;
        }
    }
}
