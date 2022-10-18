using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Cooking
{
    public class UITimer : MonoBehaviour
    {
        private float prevValue = float.MinValue;
        public TextMeshProUGUI textTimer;
        public void Initialize()
        {
            gameObject.SetActive(true);
        }

        public void UpdateUI(float value, bool isActive = true)
        {
            if(prevValue != value)
                textTimer.text = Mathf.CeilToInt(value).ToString();

            gameObject.SetActive(isActive);
            prevValue = value;
        }
    }
}
