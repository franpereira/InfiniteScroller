using System;
using UnityEngine;

namespace Kumi.World.Cluttering
{
    /// <summary>
    /// Behaviour of the Hearts that can be picked by the player.
    /// </summary>
    public class Heart : ClutterBehaviour
    {
        public static event Action Picked;

        Animator animator;
        new Collider2D collider;
        static readonly int pickedTrigger = Animator.StringToHash("Picked");

        protected override void Awake()
        {
            base.Awake();
            collider = GetComponent<Collider2D>();
            animator = GetComponent<Animator>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.enabled && other.CompareTag("Player"))
            {
                collider.enabled = false;
                Picked?.Invoke();
                animator.SetTrigger(pickedTrigger);
            }
        }
    }
}