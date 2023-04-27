using System;
using UnityEngine;

namespace Kumi.World
{
    public class StageManager : MonoBehaviour
    {
        public static event Action StageChange;
        public static Stage Current { get; private set; }

        [SerializeField] Stage openingStage;

        void Awake()
        {
            ChangeStageTo(openingStage);
        }

        void ChangeStageTo(Stage stage)
        {
            Current = stage;
            StageChange?.Invoke();
        }
    }
}