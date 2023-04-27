using System;
using System.Collections;
using Kumi.Core;
using Kumi.Ingredients;
using Kumi.Services;
using Kumi.User;
using TMPro;
using UnityEngine;
using static Kumi.Ingredients.Score;

namespace Kumi.UI
{
    /// <summary>
    /// Behaviour related to showing the score on the UI.
    /// </summary>
    public class UIScore : MonoBehaviour
    {
        public static event Action<long> NewHighScore;

        [SerializeField] GameObject scorePanel;
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TextMeshProUGUI comboText;
        [SerializeField] GameObject livesPanel;
        [SerializeField] TextMeshProUGUI livesText;

        [SerializeField] TextMeshProUGUI finalScoreText;
        [SerializeField] TextMeshProUGUI highScoreText;

        void Subscribe() => Score.Update += UpdateScoreText;
        private void OnEnable()
        {
            Events.Begin += Subscribe;
            Events.End += UpdateFinalText;
            Events.Resurrection += OnResurrection;
        }
        private void OnDisable()
        {
            Events.Begin -= Subscribe;
            Score.Update -= UpdateScoreText;
            Events.End -= UpdateFinalText;
            Events.Resurrection -= OnResurrection;
        }

        //Updates the text on the top panel showed ingame.
        void UpdateScoreText()
        {
            scoreText.text = Total.ToString();
            comboText.text = Combo > 0 ? (Combo + 10).ToString() : string.Empty;
        }

        //Updates the score on the main menu when the game ends.
        void UpdateFinalText()
        {
            scoreText.text = string.Empty;
            comboText.text = string.Empty;

            finalScoreText.text = Score.Total.ToString();
            if (Score.Total > LocalData.HighScore)
            {
                LocalData.HighScore = Score.Total;
                highScoreText.text = "New High Score!";
                Integration.Leaderboards.ReportScore(Score.Total);
                NewHighScore?.Invoke(Score.Total);
                return;
            }
            highScoreText.text = $"High Score: {LocalData.HighScore}";
        }

        void OnResurrection()
        {
            StartCoroutine(ShowLivesRoutine());
        }
        
        //Show the remaining lives after losing one.
        IEnumerator ShowLivesRoutine()
        {
            scorePanel.SetActive(false);
            livesPanel.SetActive(true);
            livesText.text = Lives.Value.ToString();
            yield return new WaitForSecondsRealtime(2f);
            livesPanel.SetActive(false);
            scorePanel.SetActive(true);
          
        }
    }
}
