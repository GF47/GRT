using System;
using UnityEngine;

namespace GRT.Events
{
    public class GEventArgs : EventArgs
    {
        public Camera Camera { get; set; }
        public RaycastHit RaycastHit { get; set; }
        public Vector2 Position { get; set; }

        public GEventArgs(Camera camera, RaycastHit raycastHit, Vector2 position)
        {
            Camera = camera;
            RaycastHit = raycastHit;
            Position = position;
        }
    }
}