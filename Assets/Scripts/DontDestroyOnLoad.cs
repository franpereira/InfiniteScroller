using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kumi
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        void Awake() => DontDestroyOnLoad(this.gameObject);
    }
}
