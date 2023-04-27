using Kumi.Core;
using Kumi.User;
using System;
using TMPro;
using UnityEngine;

namespace Kumi.UI
{
    /// <summary>
    /// Behaviour about the main menu and it's components.
    /// </summary>
    public class UIMain : UIPanel
    {
        public static event Action PlayButtonClicked;
        public static event Action RestartButtonClicked;

        [SerializeField] Transform panel;
        [SerializeField] GameObject playButton;
        [SerializeField] GameObject restartButton;

        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] TextMeshProUGUI highScoreText;

        [SerializeField] Transform hideToObject;

        Vector3 originalPos;

        public void InvokePlay() => PlayButtonClicked?.Invoke();
        public void InvokeRestart() => RestartButtonClicked?.Invoke();
        public void InvokeExit() => Application.Quit();

        private void OnEnable()
        {
            Events.Begin += OnBegin;
            Events.End += OnEnd;
            TimePause.Toggled += TimePauseToggled;
        }
        private void OnDisable()
        {
            Events.Begin -= OnBegin;
            Events.End -= OnEnd;
            TimePause.Toggled -= TimePauseToggled;
        }

        private void Start()
        {
            originalPos = panel.position;
            var highScore = LocalData.HighScore;
            if (highScore == 0) highScoreText.text = string.Empty;
            else highScoreText.text = $"High Score: {highScore}";
        }

        UIAnimations animations = new();

        void ExpandPanel()
        {
            StartCoroutine(animations.DisplaceTo(panel, originalPos));
            StartCoroutine(animations.Expand(panel));
        }

        void ShrinkPanel()
        {
            StartCoroutine(animations.DisplaceTo(panel, hideToObject.position));
            StartCoroutine(animations.Shrink(panel));
        }

        private void TimePauseToggled(bool paused)
        {
            if (paused)
            {
                ExpandPanel();
            }
            else
            {
                ShrinkPanel();   
            }
        }

        void OnBegin()
        {
            playButton.SetActive(false);
            restartButton.SetActive(true);
            ShrinkPanel();
        }

        void OnEnd()
        {
            ExpandPanel();
        }
    }
}