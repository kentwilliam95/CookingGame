using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Cooking
{
    public class PanelCoin : MonoBehaviour
    {
        public TextMeshProUGUI textUI;
        private void Update()
        {
            textUI.text = $"{Database.UserData.Coins}";
        }
    }
}
