using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Cooking
{
    public class GameMenuController : MonoBehaviour
    {
        public TextMeshProUGUI textTimer;

        public CanvasGroup canvasGroupLose;
        public CanvasGroup canvasGroupWin;

        public void UpdateTimer(int timer, bool isVisible)
        {
            textTimer.text = timer.ToString();
            textTimer.gameObject.SetActive(isVisible);
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