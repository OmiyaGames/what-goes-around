using UnityEngine;

namespace LudumDare47
{
    [RequireComponent(typeof(Rigidbody))]
    public class Turret : MovingObject
    {
        public override Quaternion CalculateLatestOrientation()
        {
            return Game.Level.GetOrientation(transform);
        }
    }
}
