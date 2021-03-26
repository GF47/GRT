using UnityEngine;
using System;

namespace GRT
{
    public class TContainer<T> : MonoBehaviour
    {
        public T Content { get; set; }

        public event Action Awaking;
        public event Action Starting;
        public event Action Enabling;
        public event Action Updating;
        public event Action LateUpdating;
        public event Action FixedUpdating;
        public event Action Disabling;
        public event Action Destroying;

        private void Awake() { Awaking?.Invoke(); }
        private void Start() { Starting?.Invoke(); }
        private void OnEnable() { Enabling?.Invoke(); }
        private void Update() { Updating?.Invoke(); }
        private void LateUpdate() { LateUpdating?.Invoke(); }
        private void FixedUpdate() { FixedUpdating?.Invoke(); }
        private void OnDisable() { Disabling?.Invoke(); }
        private void OnDestroy() { Destroying?.Invoke(); }
    }
}
