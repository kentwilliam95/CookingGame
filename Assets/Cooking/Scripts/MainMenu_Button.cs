using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Core;

namespace Cooking
{
    public class MainMenu_Button : MonoBehaviour
    {
        private Color whiteGray = new Color(225f, 225f, 225f);
        
        public TextMeshProUGUI textLevel;
        public Image image;
        public Button button;
        public Database.Level level;
        public void Initialize(string text, Database.Level level, bool isEnable = true)
        {
            gameObject.SetActive(true);
            textLevel.text = text;
            UpdateFunctionality(isEnable);
            this.level = level;
        }

        public void UpdateFunctionality(bool isEnable)
        {
            if (isEnable)
            {
                image.color = Color.white;
                button.interactable = true;
            }
            else
            {
                image.color = whiteGray;
                button.interactable = false;
            }
        }

        public void Button_OnClick()
        {
            //AppController.Instance.LoadGameScene(false);
            Database.UserData.SelectGroup(level.Group);
            level.LoadLevel();
        }
    }
}