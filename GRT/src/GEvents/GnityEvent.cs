using System;
using UnityEngine;
using UnityEngine.Events;

namespace GRT.GEvents
{
    public abstract class GnityEvent<T> : UnityEvent<T, RaycastHit>
    {
        public static Func<GnityEvent<T>> Constructor;

        public static UnityAction<T, RaycastHit> Convert(GnityEventHandler<T> handler) => new UnityAction<T, RaycastHit>(handler);

        public static GnityEventHandler<T> Convert(UnityAction<T, RaycastHit> action) => new GnityEventHandler<T>(action);
    }

    public delegate void GnityEventHandler<T>(T sender, RaycastHit hit);
}