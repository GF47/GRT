using System;
using UnityEngine;
using UnityEngine.Events;

namespace GRT.Events
{
    [Serializable]
    public class GnityEvent : UnityEvent<Camera, RaycastHit, Vector2> { }

    public static class GnityEventUtil
    {
        public static void Add(this GnityEvent @event, UnityAction<Camera, RaycastHit, Vector2> handler) =>
            @event.AddListener(handler);

        public static void Remove(this GnityEvent @event, UnityAction<Camera, RaycastHit, Vector2> handler) =>
            @event.RemoveListener(handler);

        public static UnityAction<Camera, RaycastHit, Vector2> Wrap_CRV(this Action<Camera, RaycastHit, Vector2> action) =>
            new UnityAction<Camera, RaycastHit, Vector2>(action);

        public static UnityAction<Camera, RaycastHit, Vector2> Wrap_GEH(this GnityEventHandler handler) =>
            new UnityAction<Camera, RaycastHit, Vector2>(handler);
    }
}