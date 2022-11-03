using System;
using UnityEngine;
using UnityEngine.Events;

namespace GRT.Events
{
    [Serializable]
    public class GnityEvent : UnityEvent<Camera, RaycastHit, Vector2>
    {
        public static UnityAction<Camera, RaycastHit, Vector2> Convert(GnityEventHandler handler) => new UnityAction<Camera, RaycastHit, Vector2>(handler);
        public static GnityEventHandler Convert(UnityAction<Camera, RaycastHit, Vector2> action) => new GnityEventHandler(action);
    }

    public delegate void GnityEventHandler(Camera camera, RaycastHit hit, Vector2 pos);
}