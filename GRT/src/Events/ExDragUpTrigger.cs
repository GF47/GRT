using UnityEngine;

namespace GRT.Events
{
    public class ExDragUpTrigger : BaseTrigger, IExDragUp3D
    {
        public void OnExDragUp(Camera camera, RaycastHit hit, Vector2 point) => Event?.Invoke(camera, hit, point);
    }
}
