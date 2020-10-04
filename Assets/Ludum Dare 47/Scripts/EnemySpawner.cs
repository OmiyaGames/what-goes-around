using OmiyaGames;
using OmiyaGames.Global;
using UnityEngine;

namespace LudumDare47
{
    public class EnemySpawner : PooledObject, IDestroyable
    {
        [SerializeField]
        ParticleSystem burstParticles;
        [SerializeField]
        Transform spawnLocation;

        [Header("Spawn")]
        [SerializeField]
        Turret spawnEnemy;
        [SerializeField]
        BeatKeeper.Interval scheduleOnInterval;

        System.Action<BeatKeeper, BeatKeeper.BeatStats> scheduledAction;

        public static EnemySpawner Spawn(EnemySpawner prefab, Turret spawnEnemy, Vector3 position)
        {
            EnemySpawner returnSpawner = null;
            if (Game.IsReady)
            {
                // Spawn the spawner
                returnSpawner = Singleton.Get<PoolingManager>().GetInstance(prefab, position, Quaternion.identity);

                // Update spawn
                returnSpawner.spawnEnemy = spawnEnemy;

                // Fix orientation
                returnSpawner.transform.forward = Game.Level.GetGravityDirection(position);
            }
            // Return the spawner
            return returnSpawner;
        }

        public override void Start()
        {
            base.Start();

            scheduledAction = new System.Action<BeatKeeper, BeatKeeper.BeatStats>(SpawnEnemy);

            // Restart particles
            burstParticles.Stop();
            burstParticles.Play();

            // Schedule spawning
            if (Game.IsReady)
            {
                Game.Beat.Schedule(scheduleOnInterval, scheduledAction, false);
            }

            // Add to game
            if (Game.IsReady)
            {
                Game.AllActiveSpawners.Add(this);
            }
        }

        public override void AfterDeactivate(PoolingManager manager)
        {
            base.AfterDeactivate(manager);

            // Remove from game
            if (Game.IsReady)
            {
                Game.AllActiveSpawners.Remove(this);
            }
        }

        public void Destroy()
        {
            Game.Beat.Unschedule(scheduleOnInterval, scheduledAction, false);
            PoolingManager.Destroy(gameObject);
        }

        void SpawnEnemy(BeatKeeper keeper, BeatKeeper.BeatStats on)
        {
            if (Game.IsReady && Game.Player.IsAlive && isActiveAndEnabled)
            {
                Singleton.Get<PoolingManager>().GetInstance(spawnEnemy, spawnLocation.position, spawnLocation.rotation);
            }
            Destroy();
        }
    }
}
