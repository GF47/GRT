using UnityEngine;

namespace GRT.Events.Triggers
{
    public class PointerDoubleClickTrigger : TriggerDecorator, IPointerDoubleClick
    {
        public override GeneralizedTriggerType Type => GeneralizedTriggerType.OneShot;

        public void OnPointerDoubleClick(Camera camera, RaycastHit hit, Vector2 pos) => Event?.Invoke(camera, hit, pos);
    }
}