using UnityEngine;

namespace GRT.Events.Triggers
{
    public class PointerDragStopTrigger : TriggerDecorator, IPointerDragStop
    {
        public override GeneralizedTriggerType Type => GeneralizedTriggerType.Off;

        public void OnPointerDragStop(Camera camera, RaycastHit hit, Vector2 pos) => Event?.Invoke(camera, hit, pos);
    }
}