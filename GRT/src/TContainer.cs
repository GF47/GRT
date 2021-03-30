using UnityEngine;
using System;

namespace GRT
{
    public class TContainer<T> : MonoBehaviour
    {
        public T Content { get; set; }

        public event Action Awaking;
        public event Action Starting;
        public event Action Enabled;
        public event Action Updating;
        public event Action LateUpdating;
        public event Action FixedUpdating;
        public event Action Disabled;
        public event Action Destroyed;

        private void Awake() { Awaking?.Invoke(); }
        private void Start() { Starting?.Invoke(); }
        private void OnEnable() { Enabled?.Invoke(); }
        private void Update() { Updating?.Invoke(); }
        private void LateUpdate() { LateUpdating?.Invoke(); }
        private void FixedUpdate() { FixedUpdating?.Invoke(); }
        private void OnDisable() { Disabled?.Invoke(); }
        private void OnDestroy() { Destroyed?.Invoke(); }
    }
}
