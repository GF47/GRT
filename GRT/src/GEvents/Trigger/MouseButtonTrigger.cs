using System;
using UnityEngine;

namespace GRT.GEvents.Triggers
{
    [Serializable]
    public class MouseButtonTrigger<T> : ITrigger<T>
    {
        private GnityEvent<T> _event;

        // 5个键的鼠标已经属实离谱了
        [Range(0, 5)]
        public int button;

        public GnityEvent<T> Event
        {
            get
            {
                if (_event == null)
                {
                    _event = GnityEvent<T>.Constructor();
                }
                return _event;
            }
            set => _event = value;
        }

        public GeneralizedTriggerType Type => GeneralizedTriggerType.None;
    }
}