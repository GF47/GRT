using System;
using UnityEngine;

namespace GRT.GEvents.Triggers
{
    [Serializable]
    public class MouseButtonTrigger<T> : ITrigger<T>
    {
        [SerializeField] private GnityEvent<T> _event;

        // 5个键的鼠标已经属实离谱了
        [Range(0, 5)]
        public int button;

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