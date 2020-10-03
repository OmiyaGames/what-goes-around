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

        Vector2 rawInput, finalInput;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            rawInput.x = Input.GetAxis("Horizontal");
            rawInput.y = Input.GetAxis("Vertical");
            rawInput.Normalize();

            // Look into other ways to smooth from last input to new one
            finalInput = Vector2.Lerp(finalInput, rawInput, (Time.deltaTime * inputSmooth));
        }

        private void FixedUpdate()
        {
            // First, update the rotation of this ship
            Orientation = info.GetOrientation(transform);
            body.MoveRotation(Orientation);

            // Apply gravity
            ApplyForce((Vector3.forward * gravityStrength), ForceMode.Acceleration);

            // Apply movement
            ApplyForce((finalInput * moveSpeed), ForceMode.VelocityChange);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 direction = Orientation * Vector3.right;
            Gizmos.DrawLine(transform.position, (transform.position + direction));

            Gizmos.color = Color.green;
            direction = Orientation * Vector3.up;
            Gizmos.DrawLine(transform.position, (transform.position + direction));
        }

        private void ApplyForce(Vector3 direction, ForceMode mode)
        {
            body.AddForce((Orientation * (direction * Time.deltaTime)), mode);
        }

        private void Reset()
        {
            body = GetComponent<Rigidbody>();
        }
    }
}
