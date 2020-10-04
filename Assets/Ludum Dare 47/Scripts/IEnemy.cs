using UnityEngine;

namespace LudumDare47
{
    public interface IEnemy
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

        bool Color
        {
            get;
        }

        void Start();

        void Destroy();

        bool OnHit(int power, bool color);
    }
}
