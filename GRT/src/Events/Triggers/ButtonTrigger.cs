using System;
using UnityEngine;

namespace GRT.Events.Triggers
{
    [Serializable]
    public class ButtonTrigger : ITrigger
    {
        [SerializeField] private GnityEvent _event;
        public string buttonName;

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
    }
}