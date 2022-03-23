using UnityEngine;

namespace GRT.Events
{
    public class DragUpTrigger : BaseTrigger, IDragUp3D
    {
        public void OnDragUp(Camera camera, RaycastHit hit, Vector2 point) => Event?.Invoke(camera, hit, point);
    }
}
