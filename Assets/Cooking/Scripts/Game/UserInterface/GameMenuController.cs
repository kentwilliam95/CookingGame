using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Cooking
{
    public class GameMenuController : MonoBehaviour
    {
        private UITimer selectedTimer;

        public UITimer[] uiTimer;
        public CanvasGroup canvasGroupLose;
        public CanvasGroup canvasGroupWin;

        //private void Start()
        //{
        //    uiTimer = GetComponentsInChildren<UITimer>();
        //    for (int i = 0; i < uiTimer.Length; i++)
        //    {
        //        uiTimer[i].gameObject.SetActive(false);
        //    }
        //}

        public void Initialize(GameController.GameType gameType)
        {
            selectedTimer = uiTimer[(int)gameType];
            selectedTimer.Initialize();
        }

        public void UpdateTimer(int timer, bool isVisible)
        {
            selectedTimer.UpdateUI(timer, isVisible);
        }

        public void ShowLose()
        {
            canvasGroupLose.DOFade(1f, 0.25f);
            canvasGroupLose.blocksRaycasts = true;
            canvasGroupLose.interactable = true;
        }

        public void ShowWin()
        {
            canvasGroupWin.DOFade(1f, 0.25f);
            canvasGroupWin.blocksRaycasts = true;
            canvasGroupWin.interactable = true;
        }

        public void BackToMenu()
        {
            Core.AppController.Instance.LoadScene(Global.SceneMenu, true, ()=> { ObjectPool.Instance.Flush(); });
        }

        public void Retry()
        {
            Core.AppController.Instance.LoadScene(Global.SceneGame, true, () => { ObjectPool.Instance.Flush(); });
        }
    }
}