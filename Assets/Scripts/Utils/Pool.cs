using System.Collections.Generic;
using System.Linq;
using Rng = UnityEngine.Random;

namespace Kumi.Utils
{
    /// <summary>
    /// A pool of objects that could be useful to be reutilized to avoid the cost of creating more.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pool<T>
    {
        protected List<T> List;
        protected int Index;
        public Pool(int initialSize)
        {
            List = new(initialSize);
        }

        /// <summary>
        /// Returns the currently pointed (or last returned) item in the pool. 
        /// </summary>
        public T Current => List[Index];

        /// <summary>
        /// Returns a random item from the pool.
        /// </summary>
        public T Random
        {
            get
            {
                int random = Rng.Range(0, List.Count - 1);
                Index = random;
                return List[Index];
            }
        }
        
        /// <summary>
        /// Advances to and returns the item following the last returned.
        /// </summary>
        public T Next
        {
            get
            {
                Index = Index > List.Count - 2 ? 0 : Index + 1;
                return List[Index];
            }
        }

        /// <summary>
        /// The amount of items currently existing on the pool.
        /// </summary>
        public int Size => List.Count;

        /// <summary>
        /// Adds an element to the pool if it isn't already contained.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public virtual T Add(T element)
        {
            bool already = List.Contains(element);
            if (already == false) List.Add(element);
            return element;
        }

        /// <summary>
        /// Removes the given element from the pool.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public virtual bool Remove(T element) => List.Remove(element);

        /// <summary>
        /// Sorts the pool in a random order.
        /// </summary>
        public void Shuffle() => List = List.OrderBy(_ => Rng.Range(0, int.MaxValue)).ToList();

        /// <summary>
        /// Removes all objects from the pool.
        /// </summary>
        public virtual void Clear()
        {
            List.Clear();
            List = null;
        }
    }
}
