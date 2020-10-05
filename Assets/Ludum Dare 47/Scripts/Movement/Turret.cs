using OmiyaGames.Global;
using UnityEngine;

namespace LudumDare47
{
    [RequireComponent(typeof(Rigidbody))]
    public class Turret : MovingObject, IEnemy
    {
        public static readonly int ColorField = Animator.StringToHash("Color");
        public static readonly int SpawnTrigger = Animator.StringToHash("Spawn");
        public static readonly int HitTrigger = Animator.StringToHash("Hit");

        [SerializeField]
        int maxHealth = 4;
        [SerializeField]
        int baseScore = 5;
        [SerializeField]
        int basePower = 1;
        [SerializeField]
        Gun gun;
        [SerializeField]
        Animator animator;

        [Header("Optional")]
        [SerializeField]
        bool aimAtPlayer;

        int health = 0;
        bool color = Game.DefaultIsSecondaryColor;

        public int BasePower => basePower;

        public bool IsSecondaryColor
        {
            get => color;
            set
            {
                if (color != value)
                {
                    color = value;
                    gun.IsSecondaryColor = value;
                    animator.SetBool(ColorField, color);
                }
            }
        }

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

            // Play spawn animation
            animator.SetTrigger(SpawnTrigger);

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

        public const float HitPause = 0.05f;
        //public const float ShakeMagnitude = 0.25f;
        public bool OnHit(int power, bool color)
        {
            bool returnIsDead = false;

            // Decrement health
            Health -= power;
            if (this.IsSecondaryColor != color)
            {
                Health -= power;
            }
            animator.SetTrigger(HitTrigger);

            // Check if we're dead
            if (health == 0)
            {
                returnIsDead = true;
                Game.Score += BaseScore;
                //Singleton.Get<CameraManager>().Effects.ShakeOnce(0.25f);
                Singleton.Get<TimeManager>().PauseFor(HitPause);
                Destroy();
            }
            return returnIsDead;
        }
    }
}
