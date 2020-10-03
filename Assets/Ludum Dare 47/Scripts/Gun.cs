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
        float aimingSmoothFactor = 10f;

        [Header("Parent")]
        [SerializeField]
        Rigidbody inheritMomentum;

        [Header("Laser")]
        [SerializeField]
        LaserStraight laser;
        [SerializeField]
        Transform spawnPosition;
        [SerializeField]
        float fireIntervals = 0.5f;
        [SerializeField]
        float startingDelaySeconds = 0.2f;

        float timeToFire = 0;

        public Transform Target
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
            AimAtTarget();

            if (Time.time > timeToFire)
            {
                SpawnLaser();

                // Delay the next fire
                timeToFire = (Time.time + fireIntervals);
            }
        }

        private void AimAtTarget()
        {
            if ((Target != null) && (transform.parent != null))
            {
                // Calculate the vector from gun to aimingAt
                Vector3 lookDirection = Target.position - transform.position;

                // Calculate direction to look
                lookDirection = transform.parent.InverseTransformVector(lookDirection);
                Quaternion rotation = Quaternion.FromToRotation(Vector3.right, lookDirection.normalized);
                //Debug.Log($"look: {lookDirection}");
                //Debug.Log($"angle: {rotation.eulerAngles}");

                // Rotate the gun
                transform.localRotation = Quaternion.Lerp(transform.localRotation, rotation, (aimingSmoothFactor * Time.deltaTime));
            }
        }

        private void SpawnLaser()
        {
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
        }
    }
}
