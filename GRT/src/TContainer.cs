using System;
using UnityEngine;

namespace GRT
{
    /// <summary>
    /// 承载普通.net类的Unity容器
    /// </summary>
    /// <typeparam name="T">普通的类</typeparam>
    public class TContainer<T> : MonoBehaviour
    {
        /// <summary>
        /// 普通的类实例
        /// </summary>
        public T Content { get; set; }

        public event Action<T> Awaking;

        public event Action<T> Starting;

        public event Action<T> Enabled;

        public event Action<T> Updating;

        public event Action<T> LateUpdating;

        public event Action<T> FixedUpdating;

        public event Action<T> Disabled;

        public event Action<T> Destroyed;

        private void Awake() => Awaking?.Invoke(Content);

        private void Start() => Starting?.Invoke(Content);

        private void OnEnable() => Enabled?.Invoke(Content);

        private void Update() => Updating?.Invoke(Content);

        private void LateUpdate() => LateUpdating?.Invoke(Content);

        private void FixedUpdate() => FixedUpdating?.Invoke(Content);

        private void OnDisable() => Disabled?.Invoke(Content);

        private void OnDestroy() => Destroyed?.Invoke(Content);
    }
}