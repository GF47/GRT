using UnityEngine;

namespace GRT.GEvents.Triggers
{
    public class PointerDoubleClickTrigger<T> : TriggerDecorator<T>, IGPointerDoubleClick<T>
    {
        public override GeneralizedTriggerType Type => GeneralizedTriggerType.OneShot;

        public void OnPointerDoubleClick(T sender, RaycastHit hit) => Event?.Invoke(sender, hit);
    }
}