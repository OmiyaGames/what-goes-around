using OmiyaGames;
using OmiyaGames.Global;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

namespace LudumDare47
{
    public class EnemyHealth : Health
    {
        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("test")]
        MovingObject mainEnemyScript;

        IEnemy enemyHealth = null;

        public override void Start()
        {
            base.Start();

            // Conver the main script to interface
            enemyHealth = mainEnemyScript as IEnemy;
        }

        public override void OnCollision(LaserStraight laser)
        {
            // Trigger the hit event on the main enemy script
            if (enemyHealth != null)
            {
                enemyHealth.OnHit(laser.Power, laser.Color);
            }
            laser.Destroy();
        }

        public override void OnCollision(IEnemy enemy)
        {
            // Do nothing
        }
    }
}
