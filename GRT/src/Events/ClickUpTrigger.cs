using UnityEngine;

namespace GRT.Events
{
    public class ClickUpTrigger : BaseTrigger, IClickUp3D
    {
        public void OnClickUp(Camera camera, RaycastHit hit, Vector2 point) => Event?.Invoke(camera, hit, point);
    }
}
