using UnityEngine;

namespace GRT.GEvents.Triggers
{
    public class PointerClickTrigger<T> : TriggerDecorator<T>, IGPointerClick<T>
    {
        public override GeneralizedTriggerType Type => GeneralizedTriggerType.OneShot;

        public void OnPointerClick(T sender, RaycastHit hit) => Event?.Invoke(sender, hit);
    }
}