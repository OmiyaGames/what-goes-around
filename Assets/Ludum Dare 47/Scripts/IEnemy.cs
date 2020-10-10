using UnityEngine;

namespace LudumDare47
{
    public interface IEnemy : IHit, IDestroyable
    {
        int Health
        {
            get;
        }

        int BaseScore
        {
            get;
        }

        int BasePower
        {
            get;
        }

        bool IsSecondaryColor
        {
            get;
        }

        void Start();
    }
}
