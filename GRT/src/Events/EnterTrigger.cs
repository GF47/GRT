using UnityEngine;

namespace GRT.Events
{
    public class EnterTrigger : BaseTrigger, IEnter3D
    {
        public void OnEnter(Camera camera, RaycastHit hit, Vector2 point) => Event?.Invoke(camera, hit, point);
    }
}
