using UnityEngine;
using System;

namespace GRT
{
    public class TContainer<T> : MonoBehaviour
    {
        public T Content { get; set; }

        public event Action OnAwake;
        public event Action OnStart;
        public event Action OnEnable_;
        public event Action OnUpdate;
        public event Action OnLateUpdate;
        public event Action OnFixedUpdate;
        public event Action OnDisable_;
        public event Action OnDestroy_;

        private void Awake() { OnAwake?.Invoke(); }
        private void Start() { OnStart?.Invoke(); }
        private void OnEnable() { OnEnable_?.Invoke(); }
        private void Update() { OnUpdate?.Invoke(); }
        private void LateUpdate() { OnLateUpdate?.Invoke(); }
        private void FixedUpdate() { OnFixedUpdate?.Invoke(); }
        private void OnDisable() { OnDisable_?.Invoke(); }
        private void OnDestroy() { OnDestroy_?.Invoke(); }
    }
}
