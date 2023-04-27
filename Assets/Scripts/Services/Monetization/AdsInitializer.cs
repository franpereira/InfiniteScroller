using UnityEngine;
using UnityEngine.Advertisements;

namespace Kumi.Services.Monetization
{
    /// <summary>
    /// Initializes the Unity Ads service.
    /// </summary>
    public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
    {
        [SerializeField] string _androidGameId;
        [SerializeField] string _iOsGameId;
        [SerializeField] bool _testMode = true;
        [SerializeField] bool _enablePerPlacementMode = true;
        private string _gameId;

        void Awake()
        {
            InitializeAds();
        }

        public void InitializeAds()
        {
            _gameId = Application.platform == RuntimePlatform.IPhonePlayer
                ? _iOsGameId
                : _androidGameId;
            Advertisement.Initialize(_gameId, _testMode, this);
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
    }
}