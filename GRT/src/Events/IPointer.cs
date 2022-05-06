using UnityEngine;

namespace GRT.Events
{
    public interface IPointer
    {
        void Case(GEventSystem system, bool cased, RaycastHit hit);
        void Reset(GEventSystem system);
    }
}