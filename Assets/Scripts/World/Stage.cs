using Kumi.World.Cluttering;
using UnityEngine;

namespace Kumi.World
{
    /// <summary>
    /// Represents a world "type" or "biome" with its different style of objects.
    /// </summary>
    [CreateAssetMenu(fileName = "Stage_", menuName = "World/Stage")]
    public class Stage : ScriptableObject
    {
        public GameObject[] PlatformPrefabs;
        public GameObject[] FarBackgrounds;
        public GameObject[] NearBackgrounds;
        public ClutterData[] Cluttering;
    }
}
