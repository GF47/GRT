using UnityEngine;

namespace GRT.Events
{
    public class DragTrigger : BaseTrigger, IDrag3D
    {
        public void OnDrag(Camera camera, RaycastHit hit, Vector2 point) => Event?.Invoke(camera, hit, point);
    }
}
