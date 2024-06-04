using UnityEngine;

namespace GRT.GEvents.Triggers
{
    public class PointerEnterTrigger<T> : MonoBehaviour, IGPointerEnter<T>
    {
        public GnityEvent<T> Event;

        public void OnPointerEnter(T sender, RaycastHit hit) => Event?.Invoke(sender, hit);
    }
}