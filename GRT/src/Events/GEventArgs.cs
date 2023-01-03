using GRT.Events.Triggers;
using System;
using UnityEngine;

namespace GRT.Events
{
    public class GEventArgs : EventArgs
    {
        public Camera camera;
        public RaycastHit raycastHit;
        public Vector2 position;
        public GeneralizedTriggerType triggerType;
    }
}