using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking.Database
{
    public struct IngredientRequirement
    {
        public enum IngredientStatus
        {
            Raw,
            Cooked,
        }

        public IngredientSO ingredient;
        public IngredientStatus status;
    }
}
