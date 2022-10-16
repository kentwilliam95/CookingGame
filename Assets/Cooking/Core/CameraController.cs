using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance { get; private set; }
        public Camera mainCamera;
        public Camera uiCamera;

        private void Awake()
        {
            if (Instance)
                Debug.LogWarning($"There are duplicate {GetType()}");
            else
                Instance = this;
        }
    }
}