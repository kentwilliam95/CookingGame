using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Cooking
{
    public class Customer : MonoBehaviour
    {
        public enum State
        {
            WalkingIn,
            WalkingOut,
            Order,
            Waiting
        }

        private float waitCountDown;
        private bool isInitialize;
        private State state;

        private Camera cam;
        private Vector2 botLeft;
        private Vector2 botRight;

        private Database.FoodSO foodOrder;
        private Vector3 walkTargetPosition;
        private Vector3 walkDirection;
        private Vector3 startPosition;
        private float customerSpeed = 3;

        private Sequence bubbleSeq;
        private Sequence walkSeq;
        public SpriteRenderer model;
        public Transform bubbleChatTransform;
        public Transform bubbleChatFoodContainer;
        public SpriteRenderer bubbleChatSpriteRenderer;

        [Header("Audio")]
        public AudioClip audioClipOrder;
        public AudioClip audioClipFinishEating;
        public AudioClip audioClipCoin;

        public float waitDuration;

        public bool IsReadyToServe => state == State.Waiting;

        private void OnDestroy()
        {
            OnDisable();
        }

        private void OnDisable()
        {
            isInitialize = false;
            bubbleSeq?.Kill();
            walkSeq?.Kill();

            bubbleSeq = null;
            walkSeq = null;
        }

        public void Initialize(Database.FoodSO foodOrder, Vector3 targetPosition)
        {
            model.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            waitCountDown = waitDuration;
            state = State.WalkingIn;
            this.foodOrder = foodOrder;
            this.walkTargetPosition = targetPosition;

            if (cam == null)
            {
                cam = Core.CameraController.Instance.mainCamera;
                botLeft = cam.ViewportToWorldPoint(new Vector2(0, 0));
                botRight = cam.ViewportToWorldPoint(Vector3.right);
            }
            InitializeAnimation();

            int random = Random.Range(0, 2);
            if (random == 0)
                transform.position = new Vector3(botLeft.x - 1f, 0f, 0f);
            else if (random == 1)
                transform.position = new Vector3(botRight.x + 1f, 0f, 0f);
            startPosition = transform.position;

            var diff = walkTargetPosition - transform.position;
            walkDirection = diff.normalized;

            bubbleChatSpriteRenderer.gameObject.SetActive(false);
            gameObject.SetActive(true);

            isInitialize = true;
        }

        private void Update()
        {
            if (!isInitialize)
                return;

            if (state == State.WalkingIn)
            {
                if (!walkSeq.IsPlaying())
                    walkSeq.Restart();

                transform.Translate(walkDirection * customerSpeed * Time.deltaTime);

                var percentage = Mathf.InverseLerp(startPosition.x, walkTargetPosition.x, transform.position.x);
                if (percentage >= 1)
                {
                    state = State.Order;
                }
            }
            else if (state == State.WalkingOut)
            {
                if (!walkSeq.IsPlaying())
                    walkSeq.Restart();

                var percentage = Mathf.InverseLerp(walkTargetPosition.x, -startPosition.x, transform.position.x);
                if (percentage >= 1)
                {
                    //Destroy(gameObject);
                    ObjectPool.Instance.UnSpawn(gameObject);
                }
                transform.Translate(walkDirection * customerSpeed * Time.deltaTime);
            }
            else if (state == State.Waiting)
            {
                waitCountDown -= Time.deltaTime;
                if (waitCountDown <= 0)
                {
                    state = State.WalkingOut;
                    HideBubbleChat();
                }
            }
            else if (state == State.Order)
            {
                ShowOrder();
                walkSeq.Pause();
                walkSeq.Complete();
                state = State.Waiting;
            }
        }

        private void InitializeAnimation()
        {
            var animationDuration = 0.25f;
            if (bubbleSeq == null)
            {
                bubbleSeq = DOTween.Sequence();
                bubbleSeq.SetAutoKill(false);

                var totalAnimationDuration = 0f;
                bubbleSeq.Insert(totalAnimationDuration, bubbleChatTransform.transform.DOScale(Vector3.one, animationDuration).From(0));
                totalAnimationDuration += animationDuration;

                bubbleSeq.Insert(totalAnimationDuration, bubbleChatSpriteRenderer.DOFade(1, animationDuration).From(0f));
                totalAnimationDuration += animationDuration;
                bubbleSeq.Pause();

            }

            if (walkSeq == null)
            {
                walkSeq = DOTween.Sequence();
                walkSeq.SetAutoKill(false);
                walkSeq.SetLoops(-1);

                var totalAnimationDuration = 0f;
                walkSeq.Insert(totalAnimationDuration, transform.DOLocalMoveY(0.5f, animationDuration * 1f).SetEase(Ease.Linear));
                totalAnimationDuration += animationDuration * 1f;

                walkSeq.Insert(totalAnimationDuration, transform.DOLocalMoveY(0f, animationDuration * 1f).SetEase(Ease.Linear));
                totalAnimationDuration += animationDuration * 1f;
                walkSeq.Pause();
            }
        }

        private void ShowOrder()
        {
            bubbleChatSpriteRenderer.gameObject.SetActive(true);
            var foodGO = Instantiate(foodOrder.foodGO, bubbleChatFoodContainer);
            foodGO.transform.localPosition = Vector3.zero;
            bubbleSeq.Restart();
            Core.AudioManager.Instance.PlaySfx(audioClipOrder);
        }

        private void HideBubbleChat()
        {
            bubbleSeq.Rewind();
        }

        public bool CheckFoodOrder(IIngredient ingredient)
        {
            return foodOrder.ingredientRequirements.Contains(ingredient.GetIngredient());
        }

        public void GiveFood()
        {
            state = State.WalkingOut;
            HideBubbleChat();
            Core.AudioManager.Instance.PlaySfx(audioClipFinishEating);
            Core.AudioManager.Instance.PlaySfx(audioClipCoin, 1.25f);
        }
    }
}