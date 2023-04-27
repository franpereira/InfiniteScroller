#if UNITY_ANDROID

using Google.Play.Review;
using System.Collections;
using Kumi.UI;
using UnityEngine;

namespace Kumi.Services.GP
{
    /// <summary>
    /// Google Play Store's InAppReview Prompt
    /// </summary>
    public class InAppReview : MonoBehaviour
    {
        ReviewManager reviewManager;
     
        private void OnEnable() => UIScore.NewHighScore += OnNewHighScore;
        private void OnDisable() => UIScore.NewHighScore -= OnNewHighScore;

        void OnNewHighScore(long score)
        {
            if (score > 8000) StartCoroutine(TryLaunchingReview());
        }

        IEnumerator TryLaunchingReview()
        {
            var requestFlowOperation = reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                Debug.Log(requestFlowOperation.Error.ToString());
                yield break;
            }
            var playReviewInfo = requestFlowOperation.GetResult();

            // ------------------------------------------------------------

            var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);
            yield return launchFlowOperation;
            playReviewInfo = null; // Reset the object
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                Debug.Log(launchFlowOperation.Error.ToString());
                yield break;
            }
            // The flow has finished. The API does not indicate whether the user
            // reviewed or not, or even whether the review dialog was shown. Thus, no
            // matter the result, we continue our app flow.
        }
    }
}

#endif
