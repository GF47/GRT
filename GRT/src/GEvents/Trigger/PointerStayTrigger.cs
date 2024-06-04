using UnityEngine;

namespace GRT.GEvents.Triggers
{
    public class PointerStayTrigger<T> : MonoBehaviour, IGPointerStay<T>
    {
        public GnityEvent<T> Event;

        public void OnPointerStay(T sender, RaycastHit hit) => Event?.Invoke(sender, hit);
    }
}