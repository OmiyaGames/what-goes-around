using OmiyaGames.Global;
using UnityEngine;

namespace LudumDare47
{
    [RequireComponent(typeof(Rigidbody))]
    public class Turret : MovingObject, IEnemy
    {
        [SerializeField]
        int maxHealth = 4;
        [SerializeField]
        int baseScore = 5;
        [SerializeField]
        int basePower = 1;
        [SerializeField]
        bool color;
        [SerializeField]
        Gun gun;

        [Header("Optional")]
        [SerializeField]
        bool aimAtPlayer;

        int health = 0;

        public int BasePower => basePower;
        public bool Color => color;

        public int Health
        {
            get => health;
            private set
            {
                // Clamp the value
                value = Mathf.Clamp(value, 0, maxHealth);
                if (health != value)
                {
                    // Set health
                    health = value;
                }
            }
        }

        public int BaseScore
        {
            get
            {
                return baseScore;
            }
        }

        public override void Start()
        {
            base.Start();

            // Return the enemy health back to max
            Health = maxHealth;

            if (gun != null)
            {
                // Spin the gun in a random direction
                float randomAngle = Random.Range(0f, 360f);
                gun.transform.localRotation = Quaternion.Euler(0, 0, randomAngle);

                // Check if we want to aim at the player
                if ((aimAtPlayer == true) && Game.IsReady)
                {
                    gun.Target = Game.Player.transform;
                }
            }

            // Add to game
            if (Game.IsReady)
            {
                Game.AllActiveEnemies.Add(this);
            }
        }

        public override Quaternion CalculateLatestOrientation()
        {
            return Game.Level.GetOrientation(transform);
        }

        public override void AfterDeactivate(PoolingManager manager)
        {
            base.AfterDeactivate(manager);

            // Remove from game
            if (Game.IsReady)
            {
                Game.AllActiveEnemies.Remove(this);
            }
        }

        public void Destroy()
        {
            PoolingManager.Destroy(gameObject);
        }

        public bool OnHit(int power, bool color)
        {
            bool returnIsDead = false;

            // Decrement health
            Health -= power;
            if (this.Color != color)
            {
                Health -= power;
            }

            // Check if we're dead
            if (health == 0)
            {
                returnIsDead = true;
                Destroy();
            }
            return returnIsDead;
        }
    }
}
