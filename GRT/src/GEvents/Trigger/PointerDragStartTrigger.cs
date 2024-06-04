using UnityEngine;

namespace GRT.GEvents.Triggers
{
    public class PointerDragStartTrigger<T> : TriggerDecorator<T>, IGPointerDragStart<T>
    {
        public override GeneralizedTriggerType Type => GeneralizedTriggerType.On;

        public void OnPointerDragStart(T sender, RaycastHit hit) => Event?.Invoke(sender, hit);
    }
}