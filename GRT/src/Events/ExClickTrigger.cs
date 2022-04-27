using UnityEngine;

namespace GRT.Events
{
    public class ExClickTrigger : BaseTrigger, IExClick3D
    {
        public void OnExClick(Camera camera, RaycastHit hit, Vector2 point) => Event?.Invoke(camera, hit, point);
    }
}
