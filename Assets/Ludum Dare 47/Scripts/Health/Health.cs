using UnityEngine;
using System.Collections.Generic;

namespace LudumDare47
{
    public abstract class Health : MonoBehaviour
    {
        public const string TagLaser = "Laser";

        protected static readonly Dictionary<Rigidbody, LaserStraight> LaserCache = new Dictionary<Rigidbody, LaserStraight>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="laser">Will always be set to an actual instance.</param>
        public abstract void OnCollision(LaserStraight laser);

        public virtual void Start()
        {
            LaserCache.Clear();
        }

        public void OnCollisionEnter(Collision collision)
        {
            // Attempt to retrieve the laser from a cache
            LaserStraight laser = null;
            if ((LaserCache.TryGetValue(collision.rigidbody, out laser) == false) && (collision.rigidbody.CompareTag(TagLaser) == true))
            {
                // If not cached, but the rigidbody has the right tag, grab the laser component
                laser = collision.rigidbody.GetComponent<LaserStraight>();
                if (laser != null)
                {
                    // If successful, cache the laser
                    LaserCache.Add(collision.rigidbody, laser);
                }
            }

            // Check if retrieve the laser workd
            if (laser != null)
            {
                // Run the event
                OnCollision(laser);
            }
        }
    }
}
