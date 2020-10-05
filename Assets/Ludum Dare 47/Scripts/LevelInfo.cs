using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace LudumDare47
{
    public class LevelInfo : MonoBehaviour
    {
        [SerializeField]
        Transform center;
        [SerializeField]
        EnemySpawner spawnerPrefab;
        [SerializeField]
        float radius = 20f;

        // FIXME: this part needs help
        [SerializeField]
        BeatKeeper.Interval interval;
        [SerializeField]
        Turret[] spawnEnemyPrefabs;

        #region GetOrientation
        /// <summary>
        /// Get the rotation info
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Quaternion GetOrientation(Vector3 worldPosition, Vector3 localUp)
        {
            return Quaternion.LookRotation(GetGravityDirection(worldPosition), localUp);
            //return Quaternion.FromToRotation(Vector3.forward, GetGravityDirection(worldPosition));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public Quaternion GetOrientation(Transform transform, Vector3 localUp)
        {
            return GetOrientation(transform.position, localUp);
        }

        public Quaternion GetOrientation(Transform transform)
        {
            return GetOrientation(transform, transform.up);
        }
        #endregion

        #region GetGravityDirection
        /// <summary>
        /// The direction for gravity to affect a location
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Vector3 GetGravityDirection(Vector3 worldPosition)
        {
            Vector3 returnDirection = center.position - worldPosition;
            returnDirection.Normalize();
            return returnDirection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public Vector3 GetGravityDirection(Transform transform)
        {
            return GetGravityDirection(transform.position);
        }
        #endregion

        public Vector3 GetRandomSpawnLocation()
        {
            return Random.onUnitSphere * radius;
        }

        private void Start()
        {
            if (Game.IsReady)
            {
                Game.Beat.Schedule(interval, SpawnEnemy, true);
            }
        }

        void SpawnEnemy(BeatKeeper source, BeatKeeper.BeatStats stats)
        {
            int randomIndex = Random.Range(0, spawnEnemyPrefabs.Length);
            Turret spawnPrefab = spawnEnemyPrefabs[randomIndex];
            EnemySpawner.Spawn(spawnerPrefab, spawnPrefab, GetRandomSpawnLocation());
        }
    }
}
