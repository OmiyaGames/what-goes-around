using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare47
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerShip : MonoBehaviour
    {
        [SerializeField]
        LevelInfo info;

        [Header("Physics")]
        [SerializeField]
        Rigidbody body;
        [SerializeField]
        float gravityStrength = 1000f;

        [Header("Controls")]
        [SerializeField]
        float moveSpeed = 100f;
        [SerializeField]
        float inputSmooth = 10f;

        public Quaternion Orientation
        {
            get;
            private set;
        } = Quaternion.identity;

        public LevelInfo Level
        {
            private get => info;
            set => info = value;
        }

        Vector2 rawInput, finalInput;

        // Update is called once per frame
        void Update()
        {
            // Grab the raw, input direction
            rawInput.x = Input.GetAxis("Horizontal");
            rawInput.y = Input.GetAxis("Vertical");

            // Look into other ways to smooth from last input to new one
            finalInput = Vector2.Lerp(finalInput, rawInput, (Time.deltaTime * inputSmooth));
        }

        private void FixedUpdate()
        {
            // First, update the rotation of this ship
            Orientation = Level.GetOrientation(transform, Game.Camera.transform.up);
            body.MoveRotation(Orientation);

            // Apply gravity
            ApplyForce((Vector3.forward * gravityStrength), ForceMode.Acceleration);

            // Apply movement
            ApplyForce((finalInput * moveSpeed), ForceMode.VelocityChange);
        }

        private void OnDrawGizmosSelected()
        {
            if (Level != null)
            {
                Gizmos.color = Color.blue;
                Vector3 direction = Level.GetGravityDirection(transform);
                Gizmos.DrawLine(transform.position, (transform.position + direction));

                Gizmos.color = Color.red;
                direction = Orientation * Vector3.right;
                Gizmos.DrawLine(transform.position, (transform.position + direction));

                Gizmos.color = Color.green;
                direction = Orientation * Vector3.up;
                Gizmos.DrawLine(transform.position, (transform.position + direction));
            }
        }

        private void ApplyForce(Vector3 direction, ForceMode mode)
        {
            //body.AddForce(ProjectDirectionOnPlane(direction * Time.deltaTime), mode);
            body.AddForce((Orientation * (direction * Time.deltaTime)), mode);
        }

        private void Reset()
        {
            body = GetComponent<Rigidbody>();
        }
    }
}
