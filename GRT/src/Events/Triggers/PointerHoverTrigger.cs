using UnityEngine;

namespace GRT.Events.Triggers
{
    public class PointerHoverTrigger : MonoBehaviour, IPointerHover
    {
        public GnityEvent Event;

        public void OnPointerHover(Camera camera, RaycastHit hit, Vector2 pos) => Event?.Invoke(camera, hit, pos);
    }
}