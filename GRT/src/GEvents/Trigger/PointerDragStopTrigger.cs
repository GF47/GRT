using UnityEngine;

namespace GRT.GEvents.Triggers
{
    public class PointerDragStopTrigger<T> : TriggerDecorator<T>, IGPointerDragStop<T>
    {
        public override GeneralizedTriggerType Type => GeneralizedTriggerType.Off;

        public void OnPointerDragStop(T sender, RaycastHit hit) => Event?.Invoke(sender, hit);
    }
}