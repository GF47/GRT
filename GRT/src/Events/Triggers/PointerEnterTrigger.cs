using UnityEngine;

namespace GRT.Events.Triggers
{
    public class PointerEnterTrigger : MonoBehaviour, IPointerEnter
    {
        public GnityEvent Event;

        public void OnPointerEnter(Camera camera, RaycastHit hit, Vector2 pos) => Event?.Invoke(camera, hit, pos);
    }
}