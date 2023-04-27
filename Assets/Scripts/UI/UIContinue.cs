using System.Collections;
using Kumi.Core;
using Kumi.Ingredients;
using Kumi.Services.Monetization;
using Kumi.User;
using UnityEngine;
using UnityEngine.UI;

namespace Kumi.UI
{   
    /// <summary>
    /// UI behavior related to offering the player a Continue.
    /// </summary>
    public class UIContinue : MonoBehaviour
    {
        [SerializeField] Transform panel;
        [SerializeField] Slider slider;
        [SerializeField] Button yesButton;

        bool alreadyRewarded; //If the player was already given one on the current game.

        UIAnimations animations = new();

        private void OnEnable() => Events.ContinueQuestion += OnContinue;
        private void OnDisable() => Events.ContinueQuestion -= OnContinue;

        Coroutine autoNegation;

        void OnContinue()
        {
            if (alreadyRewarded || RewardedAds.IsAdLoaded == false || Score.Total < 2000 || Score.Total < LocalData.HighScore / 3)
            {
                Continue.Answer(false);
                return;
            }

            StartCoroutine(AutoNegationSlider());
            StartCoroutine(animations.Expand(panel));
            autoNegation = StartCoroutine(AutoNegation());
        }


        //Auto dismiss the questiom prompt after some seconds.
        const float autoNegationSeconds = 4f;
        IEnumerator AutoNegation()
        {
            yield return new WaitForSecondsRealtime(autoNegationSeconds);
            yesButton.interactable = false;
            PlayerAnswer(false);
        }

        //About the slider that shows the remaining time for accepting.
        const float sliderUpdateTime = 0.025f;
        readonly WaitForSecondsRealtime sliderWaitFor = new(sliderUpdateTime);
        IEnumerator AutoNegationSlider()
        {
            for (float i = 0f; i <= 1f; i += 1f / (autoNegationSeconds / sliderUpdateTime))
            {
                slider.value = Mathf.Lerp(1f, 0f, i);
                yield return sliderWaitFor;
            }
        }

        /// <summary>
        /// For inputting the player decision about continuing or not.
        /// </summary>
        /// <param name="answer"></param>
        public void PlayerAnswer(bool answer)
        {
            StopCoroutine(autoNegation);
            if (answer == false)
            {
                Continue.Answer(false);
                StartCoroutine(animations.Shrink(panel));
                return;
            }

            StartCoroutine(animations.Shrink(panel));
            RewardedAds.AdCompleted += OnAdCompleted;
            RewardedAds.Instance.ShowAd();
        }

        void OnAdCompleted()
        {
            RewardedAds.AdCompleted -= OnAdCompleted;
            alreadyRewarded = true;
            Continue.Answer(true);
        }
    }
}
