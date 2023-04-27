using Kumi.Core;
using Kumi.User;
using TMPro;
using UnityEngine;

namespace Kumi.UI
{
    /// <summary>
    /// Behavior related to the Settings Menu.
    /// </summary>
    public class UISettings : MonoBehaviour
    {

        [SerializeField] GameObject settingsPanel;
        [SerializeField] Transform originatesFrom;
        [SerializeField] GameObject scorePanel;

        [SerializeField] TextMeshProUGUI vibrationText;
        [SerializeField] TextMeshProUGUI showScoreText;

        UIAnimations animations = new();
        Vector3 originalPos;

        private void OnEnable()
        {
            TimePause.Toggled += OnTimePauseToggled;
            Events.Begin += OnBegin;
        }
        private void OnDisable()
        {
            TimePause.Toggled -= OnTimePauseToggled;
            Events.Begin -= OnBegin;
        }

        private void Start()
        {
            originalPos = settingsPanel.transform.position;
            UpdateVibrationText();
            UpdateShowScoreText();
        }

        void OnBegin()
        {
            scorePanel.SetActive(LocalData.ShowScore);
        }
        void OnTimePauseToggled(bool paused)
        {
            if (paused == false && settingsPanel.activeSelf) CloseSettings(false);
        }

        const int animationSteps = UIAnimations.DefaultSteps / 2;
        public void OpenSettings()
        {
            settingsPanel.transform.position = originatesFrom.position;
            StartCoroutine(animations.DisplaceTo(settingsPanel.transform, originalPos, animationSteps));
            StartCoroutine(animations.Expand(settingsPanel.transform, animationSteps));
        }

        public void CloseSettings(bool anim = true)
        {
            if (anim)
            {
                StartCoroutine(animations.DisplaceTo(settingsPanel.transform, originatesFrom.position, animationSteps));
                StartCoroutine(animations.Shrink(settingsPanel.transform, animationSteps));
            }
            else settingsPanel.SetActive(false);
        }

        public void ToggleVibration()
        {
            LocalData.VibrationEnabled = !LocalData.VibrationEnabled;
            UpdateVibrationText();

        }

        public void ToggleShowScore()
        {
            var show = !LocalData.ShowScore;
            LocalData.ShowScore = show;
            scorePanel.SetActive(show);
            UpdateShowScoreText();
        }

        void UpdateVibrationText() => vibrationText.text = LocalData.VibrationEnabled ? "Vibration: On" : "Vibration: Off";
        void UpdateShowScoreText() => showScoreText.text = LocalData.ShowScore ? "Show Score: On" : "Show Score: Off";
    }
}
