using UnityEngine;

namespace GRT.GEvents
{
    public interface IGPointer<T>
    {
        void Cast(GEventDriver<T> driver, bool casted, RaycastHit hit);

        void Reset(GEventDriver<T> driver);
    }
}