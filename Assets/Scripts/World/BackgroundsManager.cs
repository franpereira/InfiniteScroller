using Kumi.Characters;
using Kumi.Utils;
using UnityEngine;

namespace Kumi.World
{
    /// <summary>
    /// Manages the representation of the landscape backgrounds on the world.
    /// </summary>
    public class BackgroundsManager : MonoBehaviour
    {

        const int poolSize = 32;
        BackgroundLayer farLayer;
        GameObjectPool farPool;
        [SerializeField] Transform farParent;
        BackgroundLayer nearLayer;
        GameObjectPool nearPool;
        [SerializeField] Transform nearParent;

        void OnEnable()
        {
            StageManager.StageChange += Setup;
        }

        void OnDisable()
        {
            StageManager.StageChange -= Setup;
            PlayerCharacter.Landing -= CheckHeight;
        }

        /// <summary>
        /// Prepares the manager to start working.
        /// </summary>
        public void Setup()
        {
            StageManager.StageChange -= Setup;
            farPool = CreatePool(backgrounds: StageManager.Current.FarBackgrounds, minAmount: poolSize, initPos: new(-100, -100), parent: farParent);
            farLayer = new(farPool, -19.1f, 19.1f, -60f);
            //nearPool = CreatePool(StageManager.Current.NearBackgrounds, poolSize, new(-100, -100), nearParent);
            //nearLayer = new(nearPool, 0f, 19.1f, -60f);            
            for(int i = 0; i < 4; i++)
            {
                farLayer.BackgroundReplacement();
                //nearLayer.BackgroundReplacement();
            }    

            PlayerCharacter.Landing += CheckHeight;
        }
        
        
        GameObjectPool CreatePool(GameObject[] backgrounds, int minAmount, Vector3 initPos, Transform parent)
        {
            GameObjectPool pool = new(minAmount, initPos, parent);
            for (int i = 0; i < minAmount / backgrounds.Length; i++)
            {
                foreach (GameObject prefab in backgrounds)
                {
                    pool.Add(prefab);
                }
            }
            pool.Shuffle();
            return pool;
        }

        void CheckHeight(Transform player)
        {
            farLayer.ReplaceIfHeight(player);
            //nearLayer.ReplaceIfHeight(player);
        }


    }
}