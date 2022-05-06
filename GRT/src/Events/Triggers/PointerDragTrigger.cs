using UnityEngine;

namespace GRT.Events.Triggers
{
    public class PointerDragTrigger : TriggerDecorator, IPointerDrag
    {
        public void OnPointerDrag(Camera camera, RaycastHit hit, Vector2 pos) => Event?.Invoke(camera, hit, pos);
    }
}