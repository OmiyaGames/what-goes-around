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
            ship.OnHit(laser.Power, laser.Color, laser.transform);
            laser.Destroy();
        }

        public override void OnCollision(IEnemy enemy)
        {
            Transform location = null;
            MonoBehaviour script = enemy as MonoBehaviour;
            if(script != null)
            {
                location = script.transform;
            }
            ship.OnHit(enemy.BasePower, !ship.IsSecondaryColor, location);
            enemy.Destroy();
        }
    }
}
