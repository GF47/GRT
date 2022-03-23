using UnityEngine;

namespace GRT.Events
{
    public class DragDownTrigger : BaseTrigger, IDragDown3D
    {
        public void OnDragDown(Camera camera, RaycastHit hit, Vector2 point) => Event?.Invoke(camera, hit, point);
    }
}
