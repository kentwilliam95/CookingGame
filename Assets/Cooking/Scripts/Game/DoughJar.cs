using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public class DoughJar : MonoBehaviour
    {
        public GameObject doughGameobject;
        public GameObject GetDough()
        {
            //return Instantiate(doughGameobject);
            return ObjectPool.Instance.Spawn("Dough");
        }
    }
}
