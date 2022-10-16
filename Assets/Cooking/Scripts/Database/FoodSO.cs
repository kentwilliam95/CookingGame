using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking.Database
{
    [CreateAssetMenu(fileName = "Food", menuName = "Database/Food")]
    public class FoodSO: ScriptableObject
    {
        public Sprite sprite;
        public GameObject foodGO;
        public List<IngredientSO> ingredientRequirements;
    }
}