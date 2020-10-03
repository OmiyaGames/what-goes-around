using OmiyaGames.Global;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare47
{
    public class Gun : MonoBehaviour
    {
        [SerializeField]
        Transform aimAt;
        [SerializeField]
        Transform spawnPosition;

        [Header("Parent")]
        [SerializeField]
        Rigidbody inheritMomentum;

        [Header("Laser")]
        [SerializeField]
        LaserStraight laser;
        [SerializeField]
        float fireIntervals = 0.5f;
        [SerializeField]
        float startingDelaySeconds = 0.2f;

        float yAngle = 0;
        float timeToFire = 0;

        public Transform AimingAt
        {
            get => aimAt;
            set => aimAt = value;
        }
        private void Start()
        {
            timeToFire = (Time.time + startingDelaySeconds);
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time > timeToFire)
            {
                // Spawn the bullet
                LaserStraight bullet = Singleton.Get<PoolingManager>().GetInstance(laser, spawnPosition.position, spawnPosition.rotation);
                if (inheritMomentum != null)
                {
                    // Add the momentum from a rigidbody
                    bullet.InitialVelocity = inheritMomentum.velocity;
                }
                else
                {
                    bullet.InitialVelocity = Vector3.zero;
                }

                // Delay the next fire
                timeToFire = (Time.time + fireIntervals);
            }
        }
    }
}
