using UnityEngine;

namespace Kumi.Cameras
{
    public static class CameraHelper
    {
        public static float VFov(float hFov, float aspectRatio)
        {
            float hFovRads = Mathf.Deg2Rad * hFov;
            float vFovRads = 2 * Mathf.Atan(Mathf.Tan(hFovRads / 2) / aspectRatio);
            return vFovRads * Mathf.Rad2Deg;
        }
    }
}
