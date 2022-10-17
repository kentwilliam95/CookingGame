using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public interface IIngredient
    {
        Database.IngredientSO GetIngredient();
        void Cook();
        void Initialize();
        Component Component { get; }
        float CookDuration { get; }
    }
}
