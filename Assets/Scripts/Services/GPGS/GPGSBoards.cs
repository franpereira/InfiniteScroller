using GooglePlayGames;
using UnityEngine;

namespace Kumi.Services.GPGS
{
    /// <summary>
    /// Leaderboards from Google Play Games Services.
    /// </summary>
    public class GPGSBoards : ILeaderboards
    {
        //From now just one leaderboard is supported.
        //On the future this method will be updated to support multiple leaderboards and return one based on current gamemode and context.
        string GetBoardID() => GPGSIds.leaderboard_high_score;
        
        /// <summary>
        /// Shows a screen with all existing leaderboards.
        /// </summary>
        public void ShowAllBoards() => Social.ShowLeaderboardUI();

        /// <summary>
        /// Shows the Leaderboard of the current gamemode.
        /// </summary>
        #if UNITY_ANDROID
        public void ShowCurrentBoard() => PlayGamesPlatform.Instance.ShowLeaderboardUI(GetBoardID());
        #else 
        public void ShowCurrentBoard() => Social.ShowLeaderboardUI();
        #endif

        /// <summary>
        /// Uploads a score to the leaderboard of the current game mode.
        /// </summary>
        /// <param name="score"></param>
        /// <returns>Confirmation</returns>
        public bool ReportScore(long score)
        {
            bool confirmation = false;
            Social.ReportScore(score, GetBoardID(), (bool success) => { confirmation = success; });
            return confirmation;
        }
    }
}