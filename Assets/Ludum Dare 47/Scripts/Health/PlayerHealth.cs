using OmiyaGames;
using OmiyaGames.Global;
using UnityEngine;

namespace LudumDare47
{
    public class PlayerHealth : Health
    {
        [SerializeField]
        PlayerShip ship;

        public override void OnCollision(LaserStraight laser)
        {
            ship.OnHit(laser.Power, laser.Color);
            laser.Destroy();
        }

        public override void OnCollision(IEnemy enemy)
        {
            enemy.Destroy();
        }
    }
}
