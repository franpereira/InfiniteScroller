using Kumi.Services.GPGS;
using UnityEngine;

namespace Kumi.Services
{
    /// <summary>
    /// Integration with Social Services.
    /// </summary>
    public static class Integration
    {
        /// <summary>
        /// If the player is logged into the social platform.
        /// </summary>
        public static bool Authenticated = Social.Active.localUser.authenticated;

        public static readonly ILeaderboards Leaderboards = new GPGSBoards();

    }
}
