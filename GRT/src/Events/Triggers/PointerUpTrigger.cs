using UnityEngine;

namespace GRT.Events.Triggers
{
    public class PointerUpTrigger : TriggerDecorator, IPointerUp
    {
        public override GeneralizedTriggerType Type => GeneralizedTriggerType.Off;

        public void OnPointerUp(Camera camera, RaycastHit hit, Vector2 pos) => Event?.Invoke(camera, hit, pos);
    }
}