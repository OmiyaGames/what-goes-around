using OmiyaGames;
using OmiyaGames.Global;
using UnityEngine;

namespace LudumDare47
{
    [RequireComponent(typeof(AudioSource))]
    public class BeatKeeper : MonoBehaviour
    {
        [SerializeField]
        AudioSource music;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Reset()
        {
            music = GetComponent<AudioSource>();
        }
    }
}
