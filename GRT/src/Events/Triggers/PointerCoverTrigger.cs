using UnityEngine;

namespace GRT.Events.Triggers
{
    public class PointerCoverTrigger : MonoBehaviour, IPointerCover
    {
        public GnityEvent Event;

        public void OnPointerCover(Camera camera, RaycastHit hit, Vector2 pos) => Event?.Invoke(camera, hit, pos);
    }
}