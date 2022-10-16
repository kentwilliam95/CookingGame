using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class Delay
    {
        private bool isStart;
        private float duration;
        public UnityAction action;
        public bool IsStart => isStart;
        public bool IsInitialized => duration > 0;

        public void Initialize(float duration, UnityAction action)
        {
            this.duration = duration;
            this.action = action;
            isStart = true;
        }

        public void Update()
        {
            if (!isStart)
                return;

            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                Stop();
                action?.Invoke();
            }
        }

        public void Stop()
        {
            isStart = false;
            duration = 0;
        }
    }
}