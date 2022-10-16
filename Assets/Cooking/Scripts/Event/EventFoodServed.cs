using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public class EventFoodServed : Core.Event
    {
        public override void Invoke()
        {
            Core.EventManager<EventFoodServed>.Invoke(this);
        }
    }
}
