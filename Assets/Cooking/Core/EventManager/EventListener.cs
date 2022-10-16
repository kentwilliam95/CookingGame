using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public interface EventListener<Event>
    {
        void OnInvoke(Event evt);
    }
}