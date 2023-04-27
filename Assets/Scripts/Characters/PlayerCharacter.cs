using Kumi.Core;
using Kumi.Device;
using System;
using UnityEngine;

namespace Kumi.Characters
{
    [RequireComponent(typeof(Character))]
    public class PlayerCharacter : MonoBehaviour
    {
        public static PlayerCharacter Instance { get; private set; }
        public static event Action<Transform> Landing;

        Character character;
        public Rigidbody2D Body => character.Body;
        new Collider2D collider;
        void Awake()
        {
            Instance = this;
            character = GetComponent<Character>();
            collider = GetComponent<Collider2D>();
        }
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.enabled && collision.collider.CompareTag("Plat"))
            {
                Vibration.Vibrate(32, 63);
                Landing?.Invoke(collision.transform);
            }
        }

        private void OnEnable()
        {
            Events.ContinueQuestion += DisableCollider;
            Events.End += DisableCollider;
            Events.Resurrection += EnableCollider;
        }
        private void OnDisable()
        {
            Events.ContinueQuestion -= DisableCollider;
            Events.End -= DisableCollider;
            Events.Resurrection -= EnableCollider;
        }

        //To make sure that the player keeps falling when losing instead of staying over any platform.
        void DisableCollider() => collider.enabled = false;
        void EnableCollider() => collider.enabled = true;

    }
}