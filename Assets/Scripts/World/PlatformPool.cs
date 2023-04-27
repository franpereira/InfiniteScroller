using System.Collections.Generic;
using Kumi.Utils;
using UnityEngine;
using UnityEngine.U2D;

namespace Kumi.World
{
    /// <summary>
    /// A pool intended to use with Platforms Objects useful to have quick access to the spline component.
    /// </summary>
    public sealed class PlatformPool : GameObjectPool
    {
        Dictionary<GameObject, Spline> dictionary;

        /// <summary>
        /// Spline of the SpriteShape component of the current pointed platform.
        /// </summary>
        public Spline CurrentSpline => dictionary[Current];

        /// <param name="initialSize">Initial size of the internal list.</param>
        /// <param name="pos">The position where new objects will be instantiated.</param>
        /// <param name="parent">The parent of new objects.</param>
        public PlatformPool(int initialSize, Vector3 pos, Transform parent) : base(initialSize, pos, parent)
        {
            dictionary = new(initialSize);
        }

        /// <summary>
        /// Instantiates a clone of the given Platform. 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public override GameObject Add(GameObject element)
        {
            GameObject platform = base.Add(element);
            Spline spline = platform.GetComponent<SpriteShapeController>().spline;
            dictionary.Add(platform, spline);
            return platform;
        }

        /// <summary>
        /// Removes and Destroys a given platform.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public override bool Remove(GameObject element)
        {
            bool removed = base.Remove(element);
            if (removed) dictionary.Remove(element);
            return removed;
        }

        /// <summary>
        /// Removes and Destroys all platforms in the pool.
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            dictionary.Clear();
            dictionary = null;
        }
    }
}
