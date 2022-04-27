using UnityEngine;

namespace GRT.Events
{
    public class ExClickDownTrigger : BaseTrigger, IExClickDown3D
    {
        public void OnExClickDown(Camera camera, RaycastHit hit, Vector2 point) => Event?.Invoke(camera, hit, point);
    }
}
