using UnityEngine;
using System.Collections.Generic;

namespace LudumDare47
{
    public abstract class Health : MonoBehaviour
    {
        public const string TagLaser = "Laser";
        public const string TagEnemy = "Enemy";

        protected static readonly Dictionary<Rigidbody, LaserStraight> LaserCache = new Dictionary<Rigidbody, LaserStraight>();
        protected static readonly Dictionary<Rigidbody, IEnemy> EnemyCache = new Dictionary<Rigidbody, IEnemy>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="laser">Will always be set to an actual instance.</param>
        public abstract void OnCollision(LaserStraight laser);
        public abstract void OnCollision(IEnemy enemy);

        public virtual void Start()
        {
            LaserCache.Clear();
        }

        public void OnCollisionEnter(Collision collision)
        {
            EvaluateScript(collision.rigidbody);
        }

        private void EvaluateScript(Rigidbody other)
        {
            // Attempt to retrieve the laser from a cache
            LaserStraight laser = null;
            IEnemy enemy = null;
            if ((LaserCache.TryGetValue(other, out laser) == false) && (EnemyCache.TryGetValue(other, out enemy) == false))
            {
                // If not cached, but the rigidbody has the right tag, grab the laser component
                MovingObject script = other.GetComponent<MovingObject>();
                if (script is LaserStraight)
                {
                    // If successful, cache the laser
                    laser = (LaserStraight)script;
                    LaserCache.Add(other, laser);
                }
                else if (script is IEnemy)
                {
                    // If successful, cache the laser
                    enemy = (IEnemy)script;
                    EnemyCache.Add(other, enemy);
                }
            }

            // Check if retrieve the laser workd
            if (laser != null)
            {
                // Run the event
                OnCollision(laser);
            }
            if (enemy != null)
            {
                // Run the event
                OnCollision(enemy);
            }
        }
    }
}
