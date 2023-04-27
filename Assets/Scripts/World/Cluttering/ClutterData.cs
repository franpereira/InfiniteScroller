using UnityEngine;

namespace Kumi.World.Cluttering
{
    /// <summary>
    /// An GameObject that is intended to spawn over platforms.
    /// </summary>
    [CreateAssetMenu(menuName = "World/Clutter", fileName = "Clutter_")]
    public class ClutterData : ScriptableObject
    {
        public GameObject prefab;
        public float offsetY;
        public float spawnOdds;
    }
}
