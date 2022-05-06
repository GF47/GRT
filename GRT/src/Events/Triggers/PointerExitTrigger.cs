using UnityEngine;

namespace GRT.Events.Triggers
{
    public class PointerExitTrigger : MonoBehaviour, IPointerExit
    {
        public GnityEvent Event;

        public void OnPointerExit(Camera camera, RaycastHit hit, Vector2 pos) => Event?.Invoke(camera, hit, pos);
    }
}