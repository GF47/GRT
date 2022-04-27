using UnityEngine;

namespace GRT.Events
{
    public class ExClickUpTrigger : BaseTrigger, IExClickUp3D
    {
        public void OnExClickUp(Camera camera, RaycastHit hit, Vector2 point) => Event?.Invoke(camera, hit, point);
    }
}
