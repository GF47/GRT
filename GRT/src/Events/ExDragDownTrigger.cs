using UnityEngine;

namespace GRT.Events
{
    public class ExDragDownTrigger : BaseTrigger, IExDragDown3D
    {
        public void OnExDragDown(Camera camera, RaycastHit hit, Vector2 point) => Event?.Invoke(camera, hit, point);
    }
}
