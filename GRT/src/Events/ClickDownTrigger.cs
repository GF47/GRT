using UnityEngine;

namespace GRT.Events
{
    public class ClickDownTrigger : BaseTrigger, IClickDown3D
    {
        public void OnClickDown(Camera camera, RaycastHit hit, Vector2 point) => Event?.Invoke(camera, hit, point);
    }
}
