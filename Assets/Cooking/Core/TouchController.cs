using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class TouchController : MonoBehaviour
    {
        private static TouchController instance;
        public static TouchController Instance => instance;
        public Canvas canvas;

        private void Awake()
        {
            instance = this;
        }

        public void DisableInput()
        {
            canvas.enabled = true;
        }

        public void EnableInput()
        {
            canvas.enabled = false;
        }
    }
}