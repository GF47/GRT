using GRT.GEvents.Triggers;
using System;
using UnityEngine;

namespace GRT.GEvents
{
    public class GEventArgs<T> : EventArgs
    {
        public T sender;
        public RaycastHit hit;
        public GeneralizedTriggerType triggerType;
    }
}