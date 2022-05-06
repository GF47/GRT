using UnityEngine;

namespace GRT.Events.Triggers
{
    public class PointerDoubleClickTrigger : TriggerDecorator, IPointerDoubleClick
    {
        public void OnPointerDoubleClick(Camera camera, RaycastHit hit, Vector2 pos) => Event?.Invoke(camera, hit, pos);
    }
}