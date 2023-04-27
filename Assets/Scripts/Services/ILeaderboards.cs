namespace Kumi.Services
{
    /// <summary>
    /// Leaderboards of the social platform with public scores of the players. 
    /// </summary>
    public interface ILeaderboards
    {
        /// <summary>
        /// Displays the leaderboards service UI.
        /// </summary>
        public void ShowAllBoards();
        /// <summary>
        /// Displays the current leaderboard on the service UI.
        /// </summary>
        public void ShowCurrentBoard();

        /// <summary>
        /// Reports a score to the current leaderboard.
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool ReportScore(long score);
    }
}
