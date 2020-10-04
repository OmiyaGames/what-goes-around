using OmiyaGames.Global;
using OmiyaGames.Menus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare47
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerShip : MovingObject, IHit
    {
        [Header("Controls")]
        [SerializeField]
        [Range(0f, 1f)]
        float moveSpeed = 100f;
        [SerializeField]
        float inputSmooth = 10f;

        [Header("Stats")]
        [SerializeField]
        int maxHealth = 5;
        [SerializeField]
        float invincibilityDurationSeconds = 1.5f;

        [Header("Components")]
        [SerializeField]
        PlayerHealth healthManager;

        Vector2 rawInput, finalInput;
        float invincibleFor = -1f;
        int health = 1;

        public bool IsAlive => Health > 0;

        public int Health
        {
            get => health;
            private set => health = Mathf.Clamp(value, 0, maxHealth);
        }

        public bool ShipColor
        {
            get;
            private set;
        }

        public bool IsInvincible
        {
            get
            {
                return ((invincibleFor > 0) && (Time.time < invincibleFor));
            }
        }

        public override void Start()
        {
            base.Start();
            Health = maxHealth;
            invincibleFor = Time.time + invincibilityDurationSeconds;
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

        public bool OnHit(int power, bool color)
        {
            if (ShipColor == color)
            {
                // Eventually add power-up mechanic here
            }
            else if (IsInvincible == false)
            {
                // If not invincible
                Health -= 1;
                invincibleFor = (Time.time + invincibilityDurationSeconds);

                // Check if dead
                if (Health == 0)
                {
                    OnDead();
                }
            }
            return (Health == 0);
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

        private void OnDead()
        {
            if (Game.IsReady)
            {
                // Destroy everything if dead
                // Copy all the enemies into a temporary list
                List<IDestroyable> list = new List<IDestroyable>(Game.AllActiveEnemies);

                // Destroy enemies
                foreach (IDestroyable script in list)
                {
                    script.Destroy();
                }

                // Copy all spawners
                list.Clear();
                list.AddRange(Game.AllActiveSpawners);
                foreach (IDestroyable script in list)
                {
                    script.Destroy();
                }

                // Copy all lasers
                list.Clear();
                list.AddRange(Game.AllActiveLasers);
                foreach (IDestroyable script in list)
                {
                    script.Destroy();
                }
            }

            // Show menus
            Singleton.Get<MenuManager>().Show<LevelFailedMenu>();
            Singleton.Get<MenuManager>().Show<HighScoresMenu>();
        }
    }
}
