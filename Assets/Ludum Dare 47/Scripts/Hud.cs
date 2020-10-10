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
        Image health;
        [SerializeField]
        TextMeshProUGUI score;

        Image[] allHealth = new Image[] { };

        public int DisplayHealth
        {
            set
            {
                int maxIndex = Mathf.Clamp(value, 0, allHealth.Length);
                for (int index = 0; index < allHealth.Length; ++index)
                {
                    allHealth[index].enabled = (index < maxIndex);
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

        public void Start()
        {
            if (Game.IsReady)
            {
                allHealth = new Image[Game.Player.MaxHealth];
                allHealth[0] = health;

                for (int index = 1; index < allHealth.Length; ++index)
                {
                    GameObject clone = Instantiate(health.gameObject, health.transform.parent);
                    allHealth[index] = clone.GetComponent<Image>();

                    clone.transform.localPosition = Vector3.zero;
                    clone.transform.localRotation = Quaternion.identity;
                    clone.transform.localScale = Vector3.one;
                }
            }
        }
    }
}
