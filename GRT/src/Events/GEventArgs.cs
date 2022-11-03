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

        public GEventArgs(Camera camera, RaycastHit raycastHit, Vector2 position, GeneralizedTriggerType triggerType)
        {
            this.camera = camera;
            this.raycastHit = raycastHit;
            this.position = position;
            this.triggerType = triggerType;
        }

        public GEventArgs()
        {
        }
    }
}