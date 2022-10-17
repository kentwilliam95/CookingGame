using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core;

namespace Cooking
{
    public class PanelCoin : MonoBehaviour, EventListener<EventFoodServed>
    {
        public TextMeshProUGUI textUI;
        private void Awake()
        {
            EventManager<EventFoodServed>.AddListener(this);    
        }

        private void OnDestroy()
        {
            EventManager<EventFoodServed>.RemoveListener(this);
        }

        private void Start()
        {
            UpdateUI();
        }

        public void OnInvoke(EventFoodServed evt)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            textUI.text = $"{Database.UserData.Coins}";
        }
    }
}
