using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking.Database
{
    [CreateAssetMenu(fileName = "Ingredient", menuName = "Database/Ingredient")]
    public class IngredientSO: ScriptableObject
    {
        public Sprite sprite;
        public Sprite appliedToFoodSprite;
    }
}