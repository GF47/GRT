using UnityEngine;

namespace GRT.Events
{
    public class ExDragTrigger : BaseTrigger, IExDrag3D
    {
        public void OnExDrag(Camera camera, RaycastHit hit, Vector2 point) => Event?.Invoke(camera, hit, point);
    }
}
