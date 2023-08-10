using System;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public abstract class UBehaviour<T> : MonoBehaviour where T : class, IGComponent<GameObject>
    {
        public T GComponent => _gComponentRef == null ? null : (_gComponentRef.TryGetTarget(out var com) ? com : null);

        private WeakReference<T> _gComponentRef;

        public virtual void Connect(T com)
        {
            _gComponentRef = new WeakReference<T>(com);
        }
    }
}