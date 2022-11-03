using UnityEngine;

namespace GRT.Events.Triggers
{
    public abstract class TriggerDecorator : MonoBehaviour, ITrigger
    {
        public ITrigger InnerTrigger { get; set; }

        public GnityEvent Event => InnerTrigger.Event;

        public abstract GeneralizedTriggerType Type { get; }
    }
}