using Cooking.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Cooking
{
    public class Dough : MonoBehaviour, IIngredient
    {
        private float cookCountDown;
        private bool isDone;
        public Database.IngredientSO ingredient;
        public SpriteRenderer spriteRenderer;
        public Color rawColor;
        public Color doneColor;
        public ParticleSystem smoke;

        public float cookTime;
        public float CookDuration => cookTime;

        public Component Component => this;

        public void Initialize()
        {
            spriteRenderer.color = rawColor;
            cookCountDown = cookTime;
            isDone = false;
            transform.DOScale(1, 0.25f).From(0);
        }

        public void Cooking()
        {
            if (isDone)
                return;

            cookCountDown -= Time.deltaTime;
            if (cookCountDown <= 0f)
            {
                Cook();
                isDone = true;
            }
        }

        public IngredientSO GetIngredient()
        {
            return ingredient;
        }

        public void Cook()
        {
            smoke.Play();
            spriteRenderer.color = doneColor;
        }
    }
}
