using System;

namespace GRT.GEvents.Triggers
{
    [Serializable]
    public class ButtonTrigger<T> : ITrigger<T>
    {
        private GnityEvent<T> _event;
        public string buttonName;

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