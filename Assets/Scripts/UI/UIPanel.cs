using UnityEngine;

namespace Kumi.UI
{
    public class UIPanel : MonoBehaviour
    {
        public static UIPanel Instance;
        protected void Awake() => Instance = this;
    }
}