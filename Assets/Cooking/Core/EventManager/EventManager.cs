using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public static class EventManager<T> where T : Event
    {
        private static List<EventListener<T>> subscribers;
        public static void AddListener(EventListener<T> evt)
        {
            if (subscribers == null)
                subscribers = new List<EventListener<T>>();

            int index = subscribers.IndexOf(evt);
            if (index < 0)
            {
                subscribers.Add(evt);
            }
        }

        public static void RemoveListener(EventListener<T> evt)
        {
            subscribers.Remove(evt);
        }

        public static void Invoke(T evt)
        {
            for (int i = 0; i < subscribers.Count; i++)
            {
                subscribers[i].OnInvoke(evt);
            }
        }
    }
}