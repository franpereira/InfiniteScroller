using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System;
using Kumi.Core;

namespace Kumi.Services.Monetization
{
    public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        public static RewardedAds Instance { get; private set; }
        public static bool IsAdLoaded { get; private set; }
        public static event Action AdLoaded;
        public static event Action AdCompleted;


        [SerializeField] string _androidAdUnitId = "Rewarded_Android";
        [SerializeField] string _iOsAdUnitId = "Rewarded_iOS";
        string _adUnitId;
        


        void OnEnable()
        {
            Events.Begin += LoadAd;
        }

        void OnDisable()
        {
            Events.Begin -= LoadAd;
        }

        void Awake()
        {
            Instance = this;
            IsAdLoaded = false;
            // Get the Ad Unit ID for the current platform:
            _adUnitId = Application.platform == RuntimePlatform.IPhonePlayer
                ? _iOsAdUnitId
                : _androidAdUnitId;

        }

        // Load content to the Ad Unit:
        public void LoadAd()
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }

        
        // When the Ad correctly loads.
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            //Debug.Log("Ad Loaded: " + adUnitId);

            if (adUnitId.Equals(_adUnitId))
            {
                IsAdLoaded = true;
                AdLoaded?.Invoke();
                // Configure the button to call the ShowAd() method when clicked:
                //_showAdButton.onClick.AddListener(ShowAd);
            }
        }

        /// <summary>
        /// Display the loaded ad. 
        /// </summary>
        public void ShowAd() => Advertisement.Show(_adUnitId, this);

        // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                Debug.Log("Unity Ads Rewarded Ad Completed");
                // Grant a reward.
                AdCompleted?.Invoke();
                IsAdLoaded = false;

                // Load another ad:
                // Advertisement.Load(_adUnitId, this);
            }
        }

        // Implement Load and Show Listener error callbacks:
        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }

        public void OnUnityAdsShowStart(string adUnitId) { }
        public void OnUnityAdsShowClick(string adUnitId) { }
    }
}