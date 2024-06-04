using UnityEngine;

namespace GRT.GEvents.Triggers
{
    public class PointerDragTrigger<T> : TriggerDecorator<T>, IGPointerDrag<T>
    {
        public override GeneralizedTriggerType Type => GeneralizedTriggerType.Keep;

        public void OnPointerDrag(T sender, RaycastHit hit) => Event?.Invoke(sender, hit);
    }
}