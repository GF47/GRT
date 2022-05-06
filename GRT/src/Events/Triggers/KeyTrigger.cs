using System;
using UnityEngine;

namespace GRT.Events.Triggers
{
    [Serializable]
    public class KeyTrigger : ITrigger
    {
        [SerializeField] private GnityEvent _event;
        public KeyCode key;

        public GnityEvent Event { get => _event; set => _event = value; }
    }
}