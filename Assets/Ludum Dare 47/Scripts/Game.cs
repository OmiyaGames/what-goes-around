using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using UnityEngine;

namespace LudumDare47
{
    public class Game : MonoBehaviour
    {
        private static Game instance = null;

        [SerializeField]
        PlayerShip ship;
        [SerializeField]
        FollowCamera camera;
        [SerializeField]
        Reticle reticle;

        public static PlayerShip Player
        {
            get
            {
                if (instance != null)
                {
                    return instance.ship;
                }
                else
                {
                    throw new System.Exception("Game not setup correctly: are you accessing this in Awake?");
                }
            }
        }

        public static FollowCamera Camera
        {
            get
            {
                if (instance != null)
                {
                    return instance.camera;
                }
                else
                {
                    throw new System.Exception("Game not setup correctly: are you accessing this in Awake?");
                }
            }
        }

        public static Reticle Reticle
        {
            get
            {
                if (instance != null)
                {
                    return instance.reticle;
                }
                else
                {
                    throw new System.Exception("Game not setup correctly: are you accessing this in Awake?");
                }
            }
        }

        // Start is called before the first frame update
        void Awake()
        {
            instance = this;
        }

        // Update is called once per frame
        void OnDestroy()
        {
            instance = null;
        }
    }
}
