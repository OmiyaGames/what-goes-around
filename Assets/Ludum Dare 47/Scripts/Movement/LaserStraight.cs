using OmiyaGames;
using OmiyaGames.Global;
using UnityEngine;

namespace LudumDare47
{
    [RequireComponent(typeof(Rigidbody))]
    public class LaserStraight : MovingObject
    {
        [Header("Stats")]
        [SerializeField]
        int power = 1;
        [SerializeField]
        float lifeDurationSeconds = 1f;
        [SerializeField]
        Vector3 movementSpeed = new Vector3(1f, 0f);

        float deactivateAfter = 0f;

        public Vector3 InitialVelocity
        {
            private get;
            set;
        }

        public Rigidbody Body
        {
            get => body;
            private set => body = value;
        }

        public int Power
        {
            get => power;
        }

        public override void Start()
        {
            base.Start();

            // FIXME: figure out orientation
            deactivateAfter = Time.time + lifeDurationSeconds;
            Body.velocity = (InitialVelocity + CalculateLatestOrientation() * movementSpeed);
        }

        public override Quaternion CalculateLatestOrientation()
        {
            return Game.Level.GetOrientation(transform);
        }

        public override void FixedUpdate()
        {
            // Apply orientation and gravity
            base.FixedUpdate();

            // Apply relative force
            ApplyForce((movementSpeed * Time.deltaTime), ForceMode.VelocityChange);
        }

        public void Destroy()
        {
            PoolingManager.Destroy(gameObject);
        }

        private void Update()
        {
            if (Time.time > deactivateAfter)
            {
                Destroy();
            }
        }
    }
}
