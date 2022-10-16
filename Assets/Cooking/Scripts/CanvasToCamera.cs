using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public class CanvasToCamera : MonoBehaviour
    {
        private void Start()
        {
            var canvases = GetComponentsInChildren<Canvas>();
            var uiCam = CameraController.Instance.uiCamera;

            for (int i = 0; i < canvases.Length; i++)
            {
                canvases[i].worldCamera = uiCam;
                canvases[i].renderMode = RenderMode.ScreenSpaceCamera;
            }
        }
    }
}