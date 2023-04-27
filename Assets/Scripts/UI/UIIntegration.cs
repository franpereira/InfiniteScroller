using Kumi.Services;
using UnityEngine;

namespace Kumi.UI
{
    public class UIIntegration : MonoBehaviour
    {
        public void ShowAllBoards() => Integration.Leaderboards.ShowAllBoards();
        public void ShowCurrentBoard() => Integration.Leaderboards.ShowCurrentBoard();
    }
}
