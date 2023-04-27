using System.Collections;
using UnityEngine;

namespace Kumi.World.Cluttering
{
    public class ClutterBehaviour : MonoBehaviour
    {
        static readonly WaitForSeconds waitFor = new(2f);
        
        //When this far from the  camera the object will be destroyed.
        const float destroyOffset = -15f;

        protected virtual void Awake() => StartCoroutine(DestroyRoutine());

        IEnumerator DestroyRoutine()
        {
            while (true)
            {
                yield return waitFor;
                float limitPos = Camera.main.transform.position.y + destroyOffset;
                if (transform.position.y < limitPos) Destroy(gameObject);
            }
        }
    }
}
