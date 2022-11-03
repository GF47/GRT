using UnityEngine;

namespace GRT.Events.Triggers
{
    public class PointerClickTrigger : TriggerDecorator, IPointerClick
    {
        public override GeneralizedTriggerType Type => GeneralizedTriggerType.OneShot;

        public void OnPointerClick(Camera camera, RaycastHit hit, Vector2 pos) => Event?.Invoke(camera, hit, pos);
    }
}