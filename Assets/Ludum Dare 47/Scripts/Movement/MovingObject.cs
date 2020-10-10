using UnityEngine;
using OmiyaGames;

namespace LudumDare47
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class MovingObject : PooledObject
    {
        [Header("Physics")]
        [SerializeField]
        protected Rigidbody body;
        [SerializeField]
        protected float gravityStrength = 1000f;

        public Quaternion Orientation
        {
            get;
            private set;
        } = Quaternion.identity;

        public abstract Quaternion CalculateLatestOrientation();

        public virtual void FixedUpdate()
        {
            // First, update the rotation of this ship
            Orientation = CalculateLatestOrientation();
            body.MoveRotation(Orientation);

            // Apply gravity
            ApplyForce((Vector3.forward * gravityStrength), ForceMode.Acceleration);
        }

        public void ApplyForce(Vector3 direction, ForceMode mode)
        {
            body.AddForce((Orientation * direction), mode);
        }

        public void Reset()
        {
            body = GetComponent<Rigidbody>();
        }
    }
}
