using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking.Database
{
    [CreateAssetMenu(fileName = "Food Database", menuName = "Database/Food Database")]
    public class FoodDatabase : ScriptableObject
    {
        public List<FoodSO> food;
    }
}