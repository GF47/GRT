using UnityEngine;

namespace GRT.GEvents
{
    public interface IGPointerEnter<T>
    {
        void OnPointerEnter(T raycaster, RaycastHit hit);
    }

    public interface IGPointerExit<T>
    {
        void OnPointerExit(T raycaster, RaycastHit hit);
    }

    public interface IGPointerStay<T>
    {
        void OnPointerStay(T raycaster, RaycastHit hit);
    }

    public interface IGPointerDown<T>
    {
        void OnPointerDown(T raycaster, RaycastHit hit);
    }

    public interface IGPointerUp<T>
    {
        void OnPointerUp(T raycaster, RaycastHit hit);
    }

    public interface IGPointerClick<T>
    {
        void OnPointerClick(T raycaster, RaycastHit hit);
    }

    public interface IGPointerDoubleClick<T>
    {
        void OnPointerDoubleClick(T raycaster, RaycastHit hit);
    }

    public interface IGPointerDragStart<T>
    {
        void OnPointerDragStart(T raycaster, RaycastHit hit);
    }

    public interface IGPointerDragStop<T>
    {
        void OnPointerDragStop(T raycaster, RaycastHit hit);
    }

    public interface IGPointerDrag<T>
    {
        void OnPointerDrag(T raycaster, RaycastHit hit);
    }
}