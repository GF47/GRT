using UnityEngine;

namespace GRT.GEvents.Triggers
{
    public class PointerDownTrigger<T> : TriggerDecorator<T>, IGPointerDown<T>
    {
        public override GeneralizedTriggerType Type => GeneralizedTriggerType.On;

        public void OnPointerDown(T sender, RaycastHit hit) => Event?.Invoke(sender, hit);
    }
}