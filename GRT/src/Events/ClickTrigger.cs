using UnityEngine;

namespace GRT.Events
{
    public class ClickTrigger : BaseTrigger, IClick3D
    {
        public void OnClick(Camera camera, RaycastHit hit, Vector2 point) => Event?.Invoke(camera, hit, point);
    }
}
