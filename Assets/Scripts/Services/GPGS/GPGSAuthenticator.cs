using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

namespace Kumi.Services.GPGS
{
    /// <summary>
    /// Logs the user in to Google Play Games Services.
    /// </summary>
    public class GPGSAuthenticator : MonoBehaviour
    {
        #if UNITY_ANDROID
        public static PlayGamesClientConfiguration Config;
        void Awake()
        {
            if (PlayGamesPlatform.Instance.IsAuthenticated() == false)
            {
                Initialize();
                PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) => { });
            }
        }

        void Initialize()
        {
            Config = new PlayGamesClientConfiguration.Builder()
                .Build();
            
            PlayGamesPlatform.InitializeInstance(Config);
            //Recommended for debugging:
            PlayGamesPlatform.DebugLogEnabled = false;
            //Activate the Google Play Games Platform
            PlayGamesPlatform.Activate();
        }

        public bool RequestSignIn()
        {
            bool success = false;
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, (result) =>
            {
                if (result == SignInStatus.Success) success = true;
            });
            return success;
        }
        #endif
    }
}
