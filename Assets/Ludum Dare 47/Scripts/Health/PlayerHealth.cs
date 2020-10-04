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
            // FIXME: look into the state of the laser, and adjust player accordingly
            //laser.Power
            laser.Destroy();
        }
    }
}
