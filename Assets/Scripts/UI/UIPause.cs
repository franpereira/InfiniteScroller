using Kumi.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Kumi.UI
{
    /// <summary>
    /// Behavior of the Pause button.
    /// </summary>
    public class UIPause : MonoBehaviour
    {
        [SerializeField] GameObject pauseButton;
        Image pauseButtonImage;

        const float unpausedAlpha = 0.2f;
        const float pausedAlpha = 0.6f;
        private void Start()
        {
            pauseButtonImage = pauseButton.GetComponent<Image>();
            Color color = pauseButtonImage.color;
            pauseButtonImage.color = new(color.r, color.g, color.b, unpausedAlpha);
        }
        private void OnEnable()
        {
            TimePause.Toggled += OnPauseToggled;
            Events.Begin += ShowButton;
            Events.ContinueQuestion += HideButton;
            Events.Resurrection += ShowButton;
            Events.End += HideButton;
        }
        private void OnDisable()
        {
            TimePause.Toggled -= OnPauseToggled;
            Events.Begin -= ShowButton;
            Events.ContinueQuestion -= HideButton;
            Events.Resurrection -= ShowButton;
            Events.End -= HideButton;
        }

        void ShowButton() => pauseButton.SetActive(true);
        void HideButton() => pauseButton.SetActive(false);

        void OnPauseToggled(bool paused)
        {
            Color color = pauseButtonImage.color;
            float a = paused ? pausedAlpha : unpausedAlpha;
            pauseButtonImage.color = new(color.r, color.g, color.b, a);
        }

        public void PauseButtonClick()
        {
            TimePause.Switch();
        }

#if UNITY_EDITOR == false

        private void OnApplicationFocus(bool hasFocus)
        {
            Debug.Log("OnApplicationFocus");
            if (hasFocus == false && TimePause.Paused == false) TimePause.Pause();
        }

        private void OnApplicationPause(bool pause)
        {
            Debug.Log("OnApplicationPause");
            if (pause == true && TimePause.Paused == false) TimePause.Pause();
        }
#endif 
    }
}
