using Kumi.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kumi.World
{
    public class BackgroundLayer
    {
        /// <summary>
        /// Controls a group of objects representing the landscape background that could be at the same Z distance from the camera.
        /// </summary>
        /// <param name="initialPool">The pool from where the objects will be selected.</param>
        /// <param name="initialY">The height at where the first object will be placed.</param>
        /// <param name="yAddition">The distance each object will have.</param>
        /// <param name="heightTargetAddition"></param>
        public BackgroundLayer(GameObjectPool initialPool, float initialY, float yAddition, float heightTargetAddition)
        {
            this.Pool = initialPool;
            this.y = initialY;
            this.yAddition = yAddition;
            this.heightTargetAddition = heightTargetAddition;
        }

        internal GameObjectPool Pool;

        //Where the next background will be placed.
        float y;
        float yAddition;
       
        float playerHeightTarget;
        float heightTargetAddition;

        /// <summary>
        /// Puts a new background after the last one if the player has reaeched a certain height.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool ReplaceIfHeight(Transform player)
        {
            if (player.position.y >= playerHeightTarget)
            {
                BackgroundReplacement();
                return true;
            }
            return false;
        }

        internal void BackgroundReplacement()
        {
            playerHeightTarget = float.MaxValue;
            GameObject bk = Pool.Next;
            bk.transform.localPosition = new Vector3(0, y, 0);
            y += yAddition;
            playerHeightTarget = y + heightTargetAddition;
        }
    }
}
