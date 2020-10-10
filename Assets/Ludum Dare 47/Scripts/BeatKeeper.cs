using System;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare47
{
    [RequireComponent(typeof(AudioSource))]
    public class BeatKeeper : MonoBehaviour
    {
        [SerializeField]
        AudioSource music;
        [SerializeField]
        float firstBeat = 0f;
        [SerializeField]
        float regularBeat = 0.25f;

        #region Helper Classes
        [Serializable]
        public struct Interval
        {
            [SerializeField]
            int numerator;
            [SerializeField]
            int denominator;

            public Interval(int numerator, int denominator)
            {
                this.numerator = numerator;
                this.denominator = denominator;
            }

            public int Numerator => numerator;
            public int Denominator => denominator;

            public double ConvertBeat(double singleBeat)
            {
                double returnBeat = 0;
                if ((denominator > 0) && (Denominator > 0))
                {
                    returnBeat = singleBeat * Numerator;
                    returnBeat /= Denominator;
                }
                return returnBeat;
            }
        }

        public class BeatStats
        {
            public event Action<BeatKeeper, BeatStats> OnRepeat;
            public event Action<BeatKeeper, BeatStats> OnOnce;

            public BeatStats(BeatKeeper parent, Interval interval)
            {
                // Setup interval
                Interval = interval;

                LastTrigger = parent.firstBeat;
                NextTrigger = (parent.firstBeat + Interval.ConvertBeat(parent.regularBeat));
            }

            public Interval Interval
            {
                get;
            }

            public double LastTrigger
            {
                get;
                private set;
            } = 0;

            public double NextTrigger
            {
                get;
                private set;
            } = 0;

            public void RunActions(BeatKeeper parent)
            {
                // Run actions
                OnRepeat?.Invoke(parent, this);

                // Run action before resetting event
                OnOnce?.Invoke(parent, this);
                OnOnce = null;

                // Setup for the next beat
                double cacheToSetLastTrigger = NextTrigger;
                while (IsOnBeat(parent))
                {
                    NextTrigger += Interval.ConvertBeat(parent.regularBeat);

                    // Check if next trigger exceeds music length
                    if (NextTrigger > parent.music.clip.length)
                    {
                        // Reset the next trigger to the start of the music
                        NextTrigger = parent.firstBeat;
                    }
                }
                LastTrigger = cacheToSetLastTrigger;

                //Debug.Log($"Last: {LastTrigger}, Next: {NextTrigger}");
            }

            public bool IsOnBeat(BeatKeeper parent)
            {
                bool returnFlag = false;
                if ((NextTrigger > LastTrigger) && (parent.music.time > NextTrigger))
                {
                    returnFlag = true;
                }
                else if ((Math.Abs(NextTrigger - parent.firstBeat) < Mathf.Epsilon) && (parent.music.time < LastTrigger))
                {
                    returnFlag = true;
                }
                return returnFlag;
            }

            public void CleanUp()
            {
                OnRepeat = null;
                OnOnce = null;
            }
        }
        #endregion

        readonly Dictionary<Interval, BeatStats> allStats = new Dictionary<Interval, BeatStats>();
        readonly List<BeatStats> cacheStats = new List<BeatStats>();

        public BeatStats GetBeatStats(Interval interval)
        {
            BeatStats stats;
            if (allStats.TryGetValue(interval, out stats) == false)
            {
                stats = new BeatStats(this, interval);
                allStats.Add(interval, stats);
            }
            return stats;
        }

        public void Schedule(Interval interval, Action<BeatKeeper, BeatStats> action, bool repeatAction = false)
        {
            BeatStats stats = GetBeatStats(interval);
            if (repeatAction == true)
            {
                stats.OnRepeat += action;
            }
            else
            {
                stats.OnOnce += action;
            }
        }

        public void Unschedule(Interval interval, Action<BeatKeeper, BeatStats> action, bool repeatAction = false)
        {
            BeatStats stats = GetBeatStats(interval);
            if (repeatAction == true)
            {
                stats.OnRepeat -= action;
            }
            else
            {
                stats.OnOnce -= action;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Cache allStats into a temporary list
            // This is in case any function calls within foreach adds a new entry in the dictionary (possible!)
            cacheStats.Clear();
            cacheStats.AddRange(allStats.Values);

            // Go through all stats in the temp list
            foreach (BeatStats stats in cacheStats)
            {
                //Debug.Log(music.time);
                if (stats.IsOnBeat(this))
                {
                    // Move on to next beat
                    stats.RunActions(this);
                }
            }
        }

        private void OnDestroy()
        {
            foreach (BeatStats stats in allStats.Values)
            {
                stats.CleanUp();
            }
            allStats.Clear();
        }

        private void Reset()
        {
            music = GetComponent<AudioSource>();
        }
    }
}
