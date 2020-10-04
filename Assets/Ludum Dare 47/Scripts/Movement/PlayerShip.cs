using UnityEngine;

namespace LudumDare47
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerShip : MovingObject
    {
        [Header("Controls")]
        [SerializeField]
        [Range(0f, 1f)]
        float moveSpeed = 100f;
        [SerializeField]
        float inputSmooth = 10f;

        [Header("Components")]
        [SerializeField]
        PlayerHealth healthManager;

        Vector2 rawInput, finalInput;

        public bool IsAlive
        {
            get => true;
        }

        // Update is called once per frame
        void Update()
        {
            // Grab the raw, input direction
            rawInput.x = Input.GetAxis("Horizontal");
            rawInput.y = Input.GetAxis("Vertical");

            // Look into other ways to smooth from last input to new one
            finalInput = Vector2.Lerp(finalInput, rawInput, (Time.deltaTime * inputSmooth));
        }

        public override void FixedUpdate()
        {
            // Apply orientation and gravity
            base.FixedUpdate();

            // Apply movement
            ApplyForce((finalInput * moveSpeed), ForceMode.VelocityChange);
        }

        public override Quaternion CalculateLatestOrientation()
        {
            return Game.Level.GetOrientation(transform, Game.Camera.transform.up);
        }

        private void OnDrawGizmosSelected()
        {
            if (Game.IsReady && (Game.Level != null))
            {
                Gizmos.color = Color.blue;
                Vector3 direction = Game.Level.GetGravityDirection(transform);
                Gizmos.DrawLine(transform.position, (transform.position + direction));

                Gizmos.color = Color.red;
                direction = Orientation * Vector3.right;
                Gizmos.DrawLine(transform.position, (transform.position + direction));

                Gizmos.color = Color.green;
                direction = Orientation * Vector3.up;
                Gizmos.DrawLine(transform.position, (transform.position + direction));
            }
        }
    }
}
