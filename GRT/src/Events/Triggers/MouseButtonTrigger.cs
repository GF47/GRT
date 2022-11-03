using System;
using UnityEngine;

namespace GRT.Events.Triggers
{
    [Serializable]
    public class MouseButtonTrigger : ITrigger
    {
        [SerializeField] private GnityEvent _event;

        // 5个键的鼠标已经属实离谱了
        [Range(0, 5)]
        public int button;

        public GnityEvent Event
        {
            get
            {
                if (_event == null)
                {
                    _event = new GnityEvent();
                }
                return _event;
            }
            set => _event = value;
        }

        public GeneralizedTriggerType Type => GeneralizedTriggerType.None;
    }
}