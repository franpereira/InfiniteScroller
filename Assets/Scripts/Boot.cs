using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kumi
{
    public class Boot : MonoBehaviour
    {
        void Start()
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}
