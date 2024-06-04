using UnityEngine;

namespace GRT.GEvents.Triggers
{
    public class PointerExitTrigger<T> : MonoBehaviour, IGPointerExit<T>
    {
        public GnityEvent<T> Event;

        public void OnPointerExit(T sender, RaycastHit hit) => Event?.Invoke(sender, hit);
    }
}