using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public class FryingPan : MonoBehaviour
    {
        private IIngredient iIngredient;
        private float cookDuration;
        
        public ParticleSystem sizzling;
        public Collider2D col2D;
        public AudioClip audioClipCook;
        public bool isBusy => iIngredient != null;
        public void CookIngredient(IIngredient iIngredient)
        {
            this.iIngredient = iIngredient;
            cookDuration = iIngredient.CookDuration;
            iIngredient.Component.transform.position = transform.position;
            col2D.enabled = false;
            Core.AudioManager.Instance.PlaySfx(audioClipCook);
            sizzling.Play();
        }

        private void Update()
        {
            UpdateCooking();   
        }

        public IIngredient GetIngredient()
        {
            if (iIngredient == null)
                return default;

            IIngredient temp = iIngredient;
            iIngredient = null;
            return temp;
        }

        private void UpdateCooking()
        {
            if (iIngredient == null)
                return;

            cookDuration -= Time.deltaTime;
            if (cookDuration <= 0)
            {
                sizzling.Stop();
                iIngredient.Cook();
                col2D.enabled = true;
            }
        }
    }
}
