using System.Collections.Generic;
using UnityEngine;

namespace Kumi.Utils
{
    public class GameObjectPool : Pool<GameObject>
    {
        protected readonly Transform Parent;
        protected Vector3 Pos;
        
        /// <param name="initialSize">Initial size of the internal list.</param>
        /// <param name="pos">The position where new objects will be instantiated.</param>
        /// <param name="parent">The parent of new objects.</param>
        public GameObjectPool(int initialSize, Vector3 pos, Transform parent) : base(initialSize)
        {
            Parent = parent;
            Pos = pos;
        }

        /// <summary>
        /// Instantiates and adds a clone of the given object at the defined position.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public override GameObject Add(GameObject element) => Add(element, true);

        
        /// <summary>
        /// Adds or clones an existing GameObject.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="clonedInstance">Add a new instance or directly the original.</param>
        /// <returns></returns>
        public GameObject Add(GameObject element, bool clonedInstance)
        {
            if(clonedInstance)
            {
                var go = Object.Instantiate(element, new(Pos.x, Pos.y, element.transform.position.z), Quaternion.identity, Parent);
                return base.Add(go);
            }
            return base.Add(element);
        }

        /// <summary>
        /// Removes and Destroys the given instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public override bool Remove(GameObject element)
        {
            bool removed = base.Remove(element);
            if(removed) Object.Destroy(element);
            return removed;
        }
        
        /// <summary>
        /// Removes and Destroys all objects in the pool.
        /// </summary>
        public override void Clear()
        {
            foreach(var go in List) Object.Destroy(go);
            base.Clear();
        }
    }
}
