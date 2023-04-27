using Kumi.Characters;
using Kumi.Ingredients;
using Kumi.World.Cluttering;
using UnityEngine;

namespace Kumi.World
{
    /// <summary>
    /// Manages the spawning of platforms on the world.
    /// </summary>
    public class PlatformsManager : MonoBehaviour
    {
        const int platsPerReplacement = 19;
        const int poolSize = platsPerReplacement * 4;

        //Where the next platform will be placed.
        float y = 1f;

        PlatformPool platforms;
        [SerializeField] Transform parent;

        //Sprite Shape Nodes Values
        float maxRange = 5.4f;
        float maxSize = 5.4f;
        float minSize = 2.4f;

        //When reached, more platforms should be placed.
        float heightForReplacement;

        void OnEnable()
        {
            StageManager.StageChange += Setup;
            Difficulty.Increased += UpdateSize;
        }

        void OnDisable()
        {
            StageManager.StageChange -= Setup;
            Difficulty.Increased -= UpdateSize;
            PlayerCharacter.Landing -= CheckHeight;
        }

        void UpdateSize()
        {
            if (Difficulty.Value <= 16) maxSize -= 0.15f;
        }

        void CheckHeight(Transform player)
        {
            if (player.position.y >= heightForReplacement)
            {
                PlatformsReplacement();
            }
        }

        /// <summary>
        /// Prepares the manager to start working.
        /// </summary>
        public void Setup()
        {
            StageManager.StageChange -= Setup;
            platforms = CreatePlatformsPool(plats: StageManager.Current.PlatformPrefabs, minAmount: poolSize, initPos: new Vector3(-100, -100), parent: parent);
            PlatformsReplacement();
            PlayerCharacter.Landing += CheckHeight;
        }

        PlatformPool CreatePlatformsPool(GameObject[] plats, int minAmount, Vector3 initPos, Transform parent)
        {
            PlatformPool pool = new(minAmount, initPos, parent);

            for (int i = 0; i < minAmount / plats.Length; i++)
            {
                foreach (GameObject prefab in plats)
                {
                    pool.Add(prefab);
                }
            }

            return pool;
        }

        void PlatformsReplacement()
        {
            heightForReplacement = float.MaxValue;
            for (int i = 0; i < platsPerReplacement; i++)
            {
                GameObject plat = platforms.Next;
                MoveTransform(plat);
                var nodePos = MoveNodes(plat);
                SpawnClutter(plat, nodePos);

                y++;
            }
            heightForReplacement = y - platsPerReplacement * 2;
        }

        void MoveTransform(GameObject plat) => plat.transform.position = new(0, y, 0);
        
        //Moves the nodes of the SpriteShape's Spline.
        (Vector3, Vector3) MoveNodes(GameObject plat)
        {
            var spline = platforms.CurrentSpline;
            (Vector3 pos0, Vector3 pos1) = NewNodesPos();

            //To avoid "too close" exception.
            spline.SetPosition(0, new(-maxRange * 2, 0));
            spline.SetPosition(1, new(maxRange * 2, 0));
            //

            spline.SetPosition(0, pos0);
            spline.SetPosition(1, pos1);
            return (pos0, pos1);

        }

        //Generates new X coordinates for the nodes of the SpriteShape.
        (Vector3, Vector3) NewNodesPos()
        {
            float firstX = Random.Range(-maxRange, maxRange);
            float secondX;
            if (0 < firstX) secondX = Random.Range(firstX - maxSize, firstX - minSize);
            else secondX = Random.Range(firstX + minSize, firstX + maxSize);

            if (firstX < secondX) return (new(firstX, 0), new(secondX, 0));
            return (new(secondX, 0), new(firstX, 0));
        }

        void SpawnClutter(GameObject plat, (Vector3 left, Vector3 right) nodePos)
        {
            //Destroy already existing children
            foreach (Transform child in plat.transform) Destroy(child.gameObject);

            foreach (ClutterData clutter in StageManager.Current.Cluttering)
            {
                var platTF = plat.transform;
                float random = Random.value;
                if (random < clutter.spawnOdds)
                {
                    float x = Random.Range(nodePos.left.x, nodePos.right.x);
                    float y = platTF.position.y + clutter.offsetY;
                    Instantiate(clutter.prefab, new(x, y), Quaternion.identity, platTF);
                }
            }
        }
    }
}