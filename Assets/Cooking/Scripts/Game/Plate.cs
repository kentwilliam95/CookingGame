using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Cooking
{
    public class Plate : MonoBehaviour
    {
        private IIngredient iIngredient;
        public AudioClip audioClipPlate;
        public bool isBusy => iIngredient != null;

        public void AssignIngredient(IIngredient iIngredient)
        {
            //iIngredient.Component.transform.position = transform.position;
            iIngredient.Component.transform.DOMove(transform.position, 0.25f);
            Core.AudioManager.Instance.PlaySfx(audioClipPlate);
            this.iIngredient = iIngredient;
        }

        public void AddToping()
        { 
        
        }

        public void Serve(Customer cust, System.Action<Customer> OnComplete)
        {
            var res = cust.CheckFoodOrder(iIngredient);
            if (res)
            {
                //Debug.Log("Correct!");
                IIngredient temp = iIngredient;
                Customer custTemp = cust;
                iIngredient.Component.transform.DOJump(cust.transform.position, 1f, 1, 0.5f).OnComplete(()=> 
                {
                    custTemp.GiveFood();
                    //Destroy(temp.Component.gameObject);
                    Core.ObjectPool.Instance.UnSpawn(temp.Component.gameObject);
                    new EventFoodServed().Invoke();
                });
                OnComplete?.Invoke(cust);
                iIngredient = null;
            }
        }
    }
}
