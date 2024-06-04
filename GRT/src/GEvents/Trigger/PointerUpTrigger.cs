using UnityEngine;

namespace GRT.GEvents.Triggers
{
    public class PointerUpTrigger<T> : TriggerDecorator<T>, IGPointerUp<T>
    {
        public override GeneralizedTriggerType Type => GeneralizedTriggerType.Off;

        public void OnPointerUp(T sender, RaycastHit hit) => Event?.Invoke(sender, hit);
    }
}