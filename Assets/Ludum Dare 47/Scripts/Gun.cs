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
        Transform[] spawnPositions;
        [SerializeField]
        float fireIntervals = 0.5f;
        [SerializeField]
        float startingDelaySeconds = 0.2f;

        float timeToFire = 0;
        Plane rotatePlane = new Plane();

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
                // Spawn all the lasers
                foreach(Transform position in spawnPositions)
                {
                    SpawnLaser(position);
                }

                // Delay the next fire
                timeToFire = (Time.time + fireIntervals);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if ((Target != null) && (transform.parent != null) && (Game.IsReady))
            {
                // Update plane of rotation
                rotatePlane.SetNormalAndPosition(Game.Level.GetGravityDirection(transform.position), transform.position);

                // Grab the closest point the thing we're aiming for on this plane
                Vector3 lookDirection = rotatePlane.ClosestPointOnPlane(Target.position);

                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(lookDirection, 0.1f);
            }
        }

        private void AimAtTarget()
        {
            if (Game.IsReady && (Target != null) && (transform.parent != null))
            {
                // Update plane of rotation
                rotatePlane.SetNormalAndPosition(Game.Level.GetGravityDirection(transform.position), transform.position);

                // Grab the closest point the thing we're aiming for on this plane
                Vector3 lookDirection = rotatePlane.ClosestPointOnPlane(Target.position);

                // Convert the world position to local
                lookDirection = transform.parent.InverseTransformPoint(lookDirection);
                //Debug.Log($"look: {lookDirection}");
                lookDirection.Normalize();

                // Convert this aiming position to a local rotation
                Quaternion rotation = Quaternion.FromToRotation(Vector3.right, lookDirection);
                //Debug.Log($"angle: {rotation.eulerAngles}");

                // Rotate the gun
                transform.localRotation = Quaternion.Lerp(transform.localRotation, rotation, (aimingSmoothFactor * Time.deltaTime));
            }
        }

        private void SpawnLaser(Transform spawnPosition)
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
