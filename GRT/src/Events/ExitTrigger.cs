using UnityEngine;

namespace GRT.Events
{
    public class ExitTrigger : BaseTrigger, IExit3D
    {
        public void OnExit(Camera camera, RaycastHit hit, Vector2 point) => Event?.Invoke(camera, hit, point);
    }
}
