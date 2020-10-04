using OmiyaGames;
using OmiyaGames.Global;
using UnityEngine;

namespace LudumDare47
{
    public class EnemyHealth : Health
    {
        public override void OnCollision(LaserStraight laser)
        {
            // FIXME: look into the state of the laser, and adjust enemy accordingly
            //laser.Power
            laser.Destroy();
        }
    }
}
