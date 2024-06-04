using UnityEngine;

namespace GRT.GEvents.Triggers
{
    public abstract class TriggerDecorator<T> : MonoBehaviour, ITrigger<T>
    {
        public ITrigger<T> InnerTrigger { get; set; }

        public GnityEvent<T> Event => InnerTrigger.Event;

        public abstract GeneralizedTriggerType Type { get; }
    }
}