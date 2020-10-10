using OmiyaGames;
using OmiyaGames.Audio;
using OmiyaGames.Global;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare47
{
    public class Explosion : PooledObject
    {
        [SerializeField]
        ParticleSystem particles;
        [SerializeField]
        SoundEffect sound;
        [SerializeField]
        float lifeDurationSeconds = 1f;

        WaitForSeconds waitFor = null;

        public override void Start()
        {
            base.Start();

            // Play particles
            particles.Stop();
            particles.Play();

            // Play sound effect
            sound.Play();

            // Run delay return to pool function
            if (waitFor == null)
            {
                waitFor = new WaitForSeconds(lifeDurationSeconds);
            }
            StartCoroutine(DelayDestory());
        }

        IEnumerator DelayDestory()
        {
            yield return waitFor;
            PoolingManager.Destroy(gameObject);
        }
    }
}
