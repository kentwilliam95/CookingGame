using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{ 
    public class EventLevelComplete : Core.Event
    {
        public override void Invoke()
        {
            Core.EventManager<EventLevelComplete>.Invoke(this);
        }
    }
}