using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cooking.Database;

namespace Cooking
{
    public class MainmenuController : MonoBehaviour, EventListener<EventLevelComplete>
    {
        private MainMenu_Button[] buttons;
        public Canvas canvas;
        public MainMenu_Button templateButton;
        public RectTransform containerButton;
        public void Awake()
        {
            EventManager<EventLevelComplete>.AddListener(this);
        }

        public void OnDestroy()
        {
            EventManager<EventLevelComplete>.RemoveListener(this);
        }

        public void Start()
        {
            var levelGroup = UserData.GetLevelGroup();
            buttons = new MainMenu_Button[levelGroup.Count];

            canvas.worldCamera = CameraController.Instance.uiCamera;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;

            for (int i = 0; i < levelGroup.Count; i++)
            {
                var lg = levelGroup[i];
                var button = Instantiate(templateButton, containerButton);
                button.Initialize(lg.LevelName, lg.GetLevel(), lg.IsGroupLevelValid());
                buttons[i] = button;
            }
        }

        public void OnInvoke(EventLevelComplete evt)
        {
            //Debug.Log("Level complete invoke!");
            var lvlGroup = UserData.GetLevelGroup();
            for (int i = 0; i < buttons.Length; i++)
            {
                bool isGroupValid = lvlGroup[i].IsGroupLevelValid();
                bool isGroupComplete = lvlGroup[i].IsGroupComplete();
                bool isButtonEnable = isGroupValid && !isGroupComplete;
                buttons[i].Initialize(lvlGroup[i].LevelName, lvlGroup[i].GetLevel(), isButtonEnable);
            }
        }

        //private void Update()
        //{
        //    //if (Input.GetKeyDown(KeyCode.A))
        //    //{
        //    //    UserData.CompleteLevel(1);
        //    //}
        //}
    }
}