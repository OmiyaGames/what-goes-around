﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare47
{
    public class Game : MonoBehaviour
    {
        private const string InstanceNullMessage = "Game not setup correctly: are you accessing this in Awake?";
        private static Game instance = null;

        public static event Action<Game, LevelInfo, LevelInfo> OnBeforeLevelChanged;
        public static event Action<Game, BeatKeeper, BeatKeeper> OnBeforeBeatChanged;

        [SerializeField]
        PlayerShip ship;
        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("camera")]
        FollowCamera cameraScript;
        [SerializeField]
        Reticle reticle;
        [SerializeField]
        LevelInfo currentLevel;
        [SerializeField]
        BeatKeeper beatKeeper;

        readonly HashSet<IEnemy> allActiveEnemies = new HashSet<IEnemy>();
        readonly HashSet<LaserStraight> allActiveLasers = new HashSet<LaserStraight>();
        readonly HashSet<EnemySpawner> allActiveSpawners = new HashSet<EnemySpawner>();

        #region Properties
        public static bool IsReady => (instance != null);

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

        public static BeatKeeper Beat
        {
            get
            {
                if (IsReady)
                {
                    return instance.beatKeeper;
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
                    OnBeforeBeatChanged?.Invoke(instance, instance.beatKeeper, value);
                    instance.beatKeeper = value;
                }
                else
                {
                    throw new Exception(InstanceNullMessage);
                }
            }
        }

        public static HashSet<IEnemy> AllActiveEnemies
        {
            get
            {
                if (IsReady)
                {
                    return instance.allActiveEnemies;
                }
                else
                {
                    throw new Exception(InstanceNullMessage);
                }
            }
        }

        public static HashSet<LaserStraight> AllActiveLasers
        {
            get
            {
                if (IsReady)
                {
                    return instance.allActiveLasers;
                }
                else
                {
                    throw new Exception(InstanceNullMessage);
                }
            }
        }

        public static HashSet<EnemySpawner> AllActiveSpawners
        {
            get
            {
                if (IsReady)
                {
                    return instance.allActiveSpawners;
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
