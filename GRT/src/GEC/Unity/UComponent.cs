using System;

namespace GRT.GEC.Unity
{
    public class UComponent<T, TC> : UnityEngine.Component
        where T : class
        where TC : class, IGComponent<T>
    {

        public TC GComponent => _gComponentRef == null ? null : (_gComponentRef.TryGetTarget(out var com) ? com : null);

        private WeakReference<TC> _gComponentRef;

        public void Attach(TC com)
        {
            _gComponentRef = new WeakReference<TC>(com);
        }
    }
}