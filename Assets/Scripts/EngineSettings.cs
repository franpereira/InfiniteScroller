using UnityEngine;

namespace Kumi
{
    public class EngineSettings : MonoBehaviour
    { 
        void Awake()
        {
#if UNITY_EDITOR == false
            Debug.unityLogger.logEnabled = false;
#endif
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
            Debug.Log($"Refresh Rate: {Application.targetFrameRate}");
        }
    }
}
