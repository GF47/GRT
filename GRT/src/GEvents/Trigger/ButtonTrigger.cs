using System;
using UnityEngine;

namespace GRT.GEvents.Triggers
{
    [Serializable]
    public class ButtonTrigger<T> : ITrigger<T>
    {
        [SerializeField] private GnityEvent<T> _event;
        public string buttonName;

        public GnityEvent<T> Event
        {
            get
            {
                if (_event == null)
                {
                    _event = new GnityEvent<T>();
                }
                return _event;
            }
            set => _event = value;
        }

        public GeneralizedTriggerType Type => GeneralizedTriggerType.None;
    }
}