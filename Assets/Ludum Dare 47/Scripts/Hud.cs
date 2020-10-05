using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LudumDare47
{
    public class Hud : MonoBehaviour
    {
        [SerializeField]
        Image[] health;
        [SerializeField]
        TextMeshProUGUI score;

        public int DisplayHealth
        {
            set
            {
                int maxIndex = Mathf.Clamp(value, 0, health.Length);
                for (int index = 0; index < health.Length; ++index)
                {
                    health[index].enabled = (index < maxIndex);
                }
            }
        }

        public int DisplayScore
        {
            set
            {
                score.text = value.ToString();
            }
        }
    }
}
