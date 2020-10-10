using OmiyaGames.Global;
using UnityEngine;

namespace LudumDare47
{
    [RequireComponent(typeof(Rigidbody))]
    public class Train : Turret
    {
        [Header("Physics")]
        [SerializeField]
        protected Vector2 speedRange = new Vector2(1f, 2f);

        Vector3 movementVector = Vector2.zero;
        public override void Start()
        {
            base.Start();

            transform.rotation = Orientation * Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
            movementVector.x = Random.Range(speedRange.x, speedRange.y);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            // Apply relative force
            ApplyForce(movementVector, ForceMode.VelocityChange);
        }
    }
}
