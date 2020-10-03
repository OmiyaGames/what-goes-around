using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace LudumDare47
{
    public class Game : MonoBehaviour
    {
        private const string InstanceNullMessage = "Game not setup correctly: are you accessing this in Awake?";
        private static Game instance = null;

        public static event Action<Game, LevelInfo, LevelInfo> OnBeforeLevelChanged;

        [SerializeField]
        PlayerShip ship;
        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("camera")]
        FollowCamera cameraScript;
        [SerializeField]
        Reticle reticle;
        [SerializeField]
        LevelInfo currentLevel;

        #region Properties
        public static bool IsReady
        {
            get => (instance != null);
        }

        public static PlayerShip Player
        {
            get
            {
                if (IsReady)
                {
                    return instance.ship;
                }
                else
                {
                    throw new Exception(InstanceNullMessage);
                }
            }
        }

        public static FollowCamera Camera
        {
            get
            {
                if (IsReady)
                {
                    return instance.cameraScript;
                }
                else
                {
                    throw new Exception(InstanceNullMessage);
                }
            }
        }

        public static Reticle Reticle
        {
            get
            {
                if (IsReady)
                {
                    return instance.reticle;
                }
                else
                {
                    throw new Exception(InstanceNullMessage);
                }
            }
        }

        public static LevelInfo Level
        {
            get
            {
                if (IsReady)
                {
                    return instance.currentLevel;
                }
                else
                {
                    throw new Exception(InstanceNullMessage);
                }
            }
            set
            {
                if (instance != null)
                {
                    OnBeforeLevelChanged?.Invoke(instance, instance.currentLevel, value);
                    instance.currentLevel = value;
                }
                else
                {
                    throw new Exception(InstanceNullMessage);
                }
            }
        }
        #endregion

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
