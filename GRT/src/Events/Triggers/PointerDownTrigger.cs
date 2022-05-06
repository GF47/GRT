using UnityEngine;

namespace GRT.Events.Triggers
{
    public class PointerDownTrigger : TriggerDecorator, IPointerDown
    {
        public void OnPointerDown(Camera camera, RaycastHit hit, Vector2 pos) => Event?.Invoke(camera, hit, pos);
    }
}