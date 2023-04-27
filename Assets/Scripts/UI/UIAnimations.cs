using System.Collections;
using UnityEngine;

namespace Kumi.UI
{
    /// <summary>
    /// Generic routines used to animate UI transforms.
    /// </summary>
    public class UIAnimations
    {
        /// <summary>
        /// Default amount of steps that animations will take.
        /// </summary>
        public const int DefaultSteps = 12;

        //Time between each step.
        readonly WaitForSecondsRealtime shrinkWait = new(0.01f);
        readonly WaitForSecondsRealtime expandWait = new(0.01f);
        readonly WaitForSecondsRealtime displaceWait = new(0.01f);

        /// <summary>
        /// Scales a transform from the current size to 0.
        /// </summary>
        /// <param name="tf"></param>
        /// <param name="steps">Amount of steps the animation will take to complete.</param>
        /// <param name="disable">Disable the gameobject after ending the animation.</param>
        /// <returns></returns>
        public IEnumerator Shrink(Transform tf, int steps = DefaultSteps, bool disable = true)
        {
            Vector3 originalScale = tf.localScale;
            for (float i = 0f; i <= 1f; i += 1f / steps)
            {
                tf.localScale = Vector3.Lerp(originalScale, Vector3.zero, i);
                yield return shrinkWait;
            }

            if (disable) tf.gameObject.SetActive(false);
        }

        /// <summary>
        /// Scales a transform from 0 to 1. It will start from 0 regardless of the scale before.
        /// </summary>
        /// <param name="tf"></param>
        /// <param name="steps">Amount of steps the animation will take to complete.</param>
        /// <param name="enable">Enable the gameobject before performing the animation.</param>
        /// <returns></returns>
        public IEnumerator Expand(Transform tf, int steps = DefaultSteps, bool enable = true)
        {
            tf.localScale = Vector3.zero;
            if (enable) tf.gameObject.SetActive(true);

            Vector3 previousScale = tf.localScale;
            for (float i = 0f; i <= 1f; i += 1f / steps)
            {
                tf.localScale = Vector3.Lerp(previousScale, Vector3.one, i);
                yield return expandWait;
            }
        }

        /// <summary>
        /// Moves a transform from the current position to another in the specified amount of steps.
        /// </summary>
        /// <param name="tf"></param>
        /// <param name="toPos">Target position.</param>
        /// <param name="steps"></param>
        /// <returns></returns>
        public IEnumerator DisplaceTo(Transform tf, Vector3 toPos, int steps = DefaultSteps)
        {
            Vector3 startPos = tf.position;
            for (float i = 0f; i <= 1f; i += 1f / steps)
            {
                tf.position = Vector3.Lerp(startPos, toPos, i);
                yield return displaceWait;
            }
        }
    }
}
