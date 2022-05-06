using UnityEngine;

namespace GRT.Events
{
    public interface IPointerEnter
    {
        void OnPointerEnter(Camera camera, RaycastHit hit, Vector2 pos);
    }

    public interface IPointerExit
    {
        void OnPointerExit(Camera camera, RaycastHit hit, Vector2 pos);
    }

    public interface IPointerCover
    {
        void OnPointerCover(Camera camera, RaycastHit hit, Vector2 pos);
    }

    public interface IPointerDown
    {
        void OnPointerDown(Camera camera, RaycastHit hit, Vector2 pos);
    }

    public interface IPointerUp
    {
        void OnPointerUp(Camera camera, RaycastHit hit, Vector2 pos);
    }

    public interface IPointerClick
    {
        void OnPointerClick(Camera camera, RaycastHit hit, Vector2 pos);
    }

    public interface IPointerDoubleClick
    {
        void OnPointerDoubleClick(Camera camera, RaycastHit hit, Vector2 pos);
    }

    public interface IPointerDragStart
    {
        void OnPointerDragStart(Camera camera, RaycastHit hit, Vector2 pos);
    }

    public interface IPointerDragStop
    {
        void OnPointerDragStop(Camera camera, RaycastHit hit, Vector2 pos);
    }

    public interface IPointerDrag
    {
        void OnPointerDrag(Camera camera, RaycastHit hit, Vector2 pos);
    }
}