using Kumi.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kumi.Core
{
    /// <summary>
    /// When the scene has to be restarted.
    /// </summary>
    internal class Restart : MonoBehaviour
    {
        private void OnEnable()
        {
            UIMain.RestartButtonClicked += InvokeRestart;
        }
        private void OnDisable()
        {
            UIMain.RestartButtonClicked -= InvokeRestart;
        }

        void InvokeRestart()
        {
            Events.InvokeRestart();
            Time.timeScale = 1f;
            SceneManager.LoadScene(1);
        }
    }
}