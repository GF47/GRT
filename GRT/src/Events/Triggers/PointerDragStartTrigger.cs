using UnityEngine;

namespace GRT.Events.Triggers
{
    public class PointerDragStartTrigger : TriggerDecorator, IPointerDragStart
    {
        public void OnPointerDragStart(Camera camera, RaycastHit hit, Vector2 pos) => Event?.Invoke(camera, hit, pos);
    }
}