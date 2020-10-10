using OmiyaGames.Audio;
using OmiyaGames.Global;
using System.Collections;
using UnityEngine;

namespace LudumDare47
{
    public class Gun : MonoBehaviour
    {
        [SerializeField]
        Transform aimAt;
        [SerializeField]
        float aimingSmoothFactor = 10f;
        [SerializeField]
        SoundEffect sound;

        [Header("Parent")]
        [SerializeField]
        Rigidbody inheritMomentum;

        [Header("Laser")]
        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("laser")]
        LaserStraight primaryColorLaser;
        [SerializeField]
        LaserStraight secondaryColorLaser;
        [SerializeField]
        Transform[] spawnPositions;

        [Header("Timing")]
        [SerializeField]
        float startingDelaySeconds = 0.2f;
        [SerializeField]
        BeatKeeper.Interval beats;

        Plane rotatePlane = new Plane();
        Quaternion targetRotation = Quaternion.identity;
        Coroutine start = null;
        System.Action<BeatKeeper, BeatKeeper.BeatStats> cacheAction = null;

        public Transform Target
        {
            get => aimAt;
            set => aimAt = value;
        }

        public bool IsSecondaryColor
        {
            get;
            set;
        } = Game.DefaultIsSecondaryColor;

        public static Vector3 GetPointOnPlane(Transform from, Transform target, Vector3 down, ref Plane rotatePlane)
        {
            return GetPointOnPlane(from, target.position, down, ref rotatePlane);
        }

        public static Vector3 GetPointOnPlane(Transform from, Vector3 target, Vector3 down, ref Plane rotatePlane)
        {
            // Update plane of rotation
            rotatePlane.SetNormalAndPosition(down, from.position);

            // Grab the closest point the thing we're aiming for on this plane
            return rotatePlane.ClosestPointOnPlane(target);
        }

        private void OnEnable()
        {
            start = StartCoroutine(DelayScheduling());
        }

        private void OnDisable()
        {
            // Stop any coroutines
            if (start != null)
            {
                StopCoroutine(start);
                start = null;
            }

            // Unschedule laser firing
            if ((Game.IsReady) && (cacheAction != null))
            {
                Game.Beat.Unschedule(beats, cacheAction, true);
            }
        }

        // Update is called once per frame
        void Update()
        {
            AimAtTarget();
        }

        private void OnDrawGizmosSelected()
        {
            if (Game.IsReady && (Target != null) && (transform.parent != null))
            {
                // Update plane of rotation
                rotatePlane.SetNormalAndPosition(Game.Level.GetGravityDirection(transform.position), transform.position);

                // Grab the closest point the thing we're aiming for on this plane
                Vector3 lookDirection = rotatePlane.ClosestPointOnPlane(Target.position);

                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(lookDirection, 0.1f);
            }
        }

        IEnumerator DelayScheduling()
        {
            // Delay before scheduling a turret
            if (startingDelaySeconds > 0f)
            {
                yield return new WaitForSeconds(startingDelaySeconds);
            }

            // Make sure everything is ready
            if (Game.IsReady)
            {
                // Cache the FireLasers function
                if (cacheAction == null)
                {
                    cacheAction = new System.Action<BeatKeeper, BeatKeeper.BeatStats>(FireLasers);
                }

                // Schedule firing lasers on repeat
                Game.Beat.Schedule(beats, cacheAction, true);
            }
        }

        void FireLasers(BeatKeeper keaper, BeatKeeper.BeatStats stats)
        {
            // Spawn all the lasers
            foreach (Transform position in spawnPositions)
            {
                SpawnLaser(position);
            }
        }

        private void AimAtTarget()
        {
            if (Game.IsReady && (Target != null) && (transform.parent != null))
            {
                // Grab the closest point the thing we're aiming for on this plane
                Vector3 lookDirection = GetPointOnPlane(transform, Target, Game.Level.GetGravityDirection(transform.position), ref rotatePlane);

                // Convert the world position to local
                lookDirection = transform.parent.InverseTransformPoint(lookDirection);
                if (Mathf.Approximately(lookDirection.sqrMagnitude, 0f) == false)
                {
                    //Debug.Log($"look: {lookDirection}");
                    lookDirection.Normalize();

                    // Convert this aiming position to a local rotation
                    targetRotation = Quaternion.FromToRotation(Vector3.right, lookDirection);
                    //Debug.Log($"angle: {targetRotation.eulerAngles}");
                }

                // Rotate the gun
                transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, (aimingSmoothFactor * Time.deltaTime));
            }
        }

        private void SpawnLaser(Transform spawnPosition)
        {
            // Determine what to spawn
            LaserStraight spawnPrefab = primaryColorLaser;
            if(IsSecondaryColor)
            {
                spawnPrefab = secondaryColorLaser;
            }

            // Spawn bullet
            LaserStraight bullet = Singleton.Get<PoolingManager>().GetInstance(spawnPrefab, spawnPosition.position, spawnPosition.rotation);
            if (inheritMomentum != null)
            {
                // Add the momentum from a rigidbody
                bullet.InitialVelocity = inheritMomentum.velocity;
            }
            else
            {
                bullet.InitialVelocity = Vector3.zero;
            }

            if(sound != null)
            {
                sound.Play();
            }
        }
    }
}
