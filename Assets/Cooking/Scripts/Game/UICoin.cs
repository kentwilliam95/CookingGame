using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

namespace Cooking
{
    public class UICoin : MonoBehaviour
    {
        private Camera mainCam;
        private Sequence coinAnimation;
        public SpriteRenderer spriteRenderer;
        public Image image;
        public RectTransform rectTransform;
        public TextMeshProUGUI textCoin;
        public CanvasGroup canvasGroup;

        void OnDisable()
        {
            coinAnimation?.Kill();
        }

        public void Initialize(string textAmount, Vector3 worldPosition)
        {
            if(mainCam == null)
                mainCam = Core.CameraController.Instance.uiCamera;

            if (coinAnimation != null)
                coinAnimation.Kill();

            transform.localScale = Vector3.one;
            transform.position = worldPosition;
            textCoin.text = textAmount;
            var screenPos = mainCam.WorldToScreenPoint(transform.position);

            coinAnimation = DOTween.Sequence();
            coinAnimation.SetAutoKill(false);

            var totalAnimationDuration = 0f;
            var animationDuration = 0.25f;

            coinAnimation.Insert(totalAnimationDuration, rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + 75f, animationDuration * 2f));
            totalAnimationDuration += animationDuration * 2f;

            coinAnimation.Insert(totalAnimationDuration, canvasGroup.DOFade(0f, animationDuration).From(1));
            totalAnimationDuration += animationDuration;

            coinAnimation.InsertCallback(totalAnimationDuration, ()=> 
            {
                transform.SetParent(null);
                Core.ObjectPool.Instance.UnSpawn(gameObject);
            });
        }
    }
}
