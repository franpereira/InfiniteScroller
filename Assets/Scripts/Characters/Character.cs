using Kumi.Cameras;
using UnityEngine;

namespace Kumi.Characters
{
    public class Character : MonoBehaviour
    {
        [SerializeField] Rigidbody2D body;
        public Rigidbody2D Body => body;

        [SerializeField] new Collider2D collider;
        public Collider2D Collider => collider;

        const float MinJump = 9.285f; //Min jump force even without acceleration.
        [SerializeField] float acceleration = 0.64f;
        [SerializeField] float jumpMultiplier = 1.74f;
        [SerializeField] float bounceMultiplier = 0.6f;
        float bounceForce;

        #region Input Variables
        /// <summary>
        /// Controls the walking direction. Negative values will make the player walk to the left, positives to the right.
        /// </summary>
        public double HorizontalAxis;
        int jumpRequestWindow; //To allow the player to press the jump button some time before landing.
        
        /// <summary>
        /// Make the character jump if possible.
        /// </summary>
        public void RequestJump() => jumpRequestWindow = 10;
        bool canJump;
        #endregion

        LayerMask platLayer; //The layer that touching it enables the character to jump.

        void Start() => platLayer = LayerMask.GetMask("Plat");

        #region Movement Methods
        void FixedUpdate()
        {
            canJump = collider.IsTouchingLayers(platLayer);
            Vector2 velocity = body.velocity;

            UpdateJumping(ref velocity);
            UpdateMovement(ref velocity);

            body.gravityScale = velocity.y < 0 ? 8f : 4f;
            body.velocity = velocity;
        }

        void UpdateJumping(ref Vector2 velocity)
        {
            if (jumpRequestWindow-- <= 0 || canJump == false) return;
            float jumpForce = Mathf.Abs(velocity.x) * jumpMultiplier;
            velocity.y = jumpForce > MinJump ? jumpForce : MinJump;
            jumpRequestWindow = 0;
        }
        
        readonly Vector3 flippedScale = new(-1, 1, 1);
        void UpdateMovement(ref Vector2 velocity)
        {
            switch (HorizontalAxis)
            {
                case > 0f:
                    velocity += new Vector2(acceleration, 0f);
                    transform.localScale = Vector3.one;
                    break;
                case < 0f:
                    velocity -= new Vector2(acceleration, 0f);
                    transform.localScale = flippedScale;
                    break;
            }
        }
        #endregion

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.enabled && collision.CompareTag("Wall"))
            {
                //Calculates the force before actually colliding against the wall to avoid velocity.x being 0. 
                bounceForce = -(body.velocity.x * bounceMultiplier);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.enabled && collision.collider.CompareTag("Wall"))
            {
                body.velocity = new Vector2(bounceForce, body.velocity.y);
                CameraDirector.Instance.Shake(body.velocity.x * 8, 8);
            }
        }
    }
}