using OmiyaGames;
using OmiyaGames.Global;
using UnityEngine;

namespace LudumDare47
{
    [RequireComponent(typeof(Rigidbody))]
    public class LaserStraight : MovingObject, IDestroyable
    {
        [Header("Stats")]
        [SerializeField]
        int power = 1;
        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("color")]
        bool isSecondaryColor = true;
        [SerializeField]
        float lifeDurationSeconds = 1f;

        [Header("Physics")]
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

        public bool Color => isSecondaryColor;

        public override void Start()
        {
            base.Start();

            deactivateAfter = Time.time + lifeDurationSeconds;
            Body.velocity = (InitialVelocity + CalculateLatestOrientation() * movementSpeed);

            // Add to game
            if (Game.IsReady)
            {
                Game.AllActiveLasers.Add(this);
            }
        }

        public override Quaternion CalculateLatestOrientation()
        {
            return Game.Level.GetOrientation(transform);
        }

        public override void FixedUpdate()
        {
            // Apply orientation and gravity
            base.FixedUpdate();

            // Check if we should deactivate soon
            if (Time.time < deactivateAfter)
            {
                // Apply relative force
                ApplyForce((movementSpeed * Time.deltaTime), ForceMode.VelocityChange);
            }
            else
            {
                Destroy();
            }
        }

        public override void AfterDeactivate(PoolingManager manager)
        {
            base.AfterDeactivate(manager);

            // Remove from game
            if (Game.IsReady)
            {
                Game.AllActiveLasers.Remove(this);
            }
        }

        public void Destroy()
        {
            PoolingManager.Destroy(gameObject);
        }
    }
}
