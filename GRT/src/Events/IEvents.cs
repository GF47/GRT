using UnityEngine;

namespace GRT.Events
{
    public interface IClick3D
    {
        void OnClick(Camera camera, RaycastHit hit, Vector2 point);
    }

    public interface IClickDown3D
    {
        void OnClickDown(Camera camera, RaycastHit hit, Vector2 point);
    }

    public interface IClickUp3D
    {
        void OnClickUp(Camera camera, RaycastHit hit, Vector2 point);
    }

    public interface IDrag3D
    {
        void OnDrag(Camera camera, RaycastHit hit, Vector2 point);
    }

    public interface IDragDown3D
    {
        void OnDragDown(Camera camera, RaycastHit hit, Vector2 point);
    }

    public interface IDragUp3D
    {
        void OnDragUp(Camera camera, RaycastHit hit, Vector2 point);
    }

    public interface IHold3D
    {
        void OnHold(Camera camera, RaycastHit hit, Vector2 point);
    }

    public interface IEnter3D
    {
        void OnEnter(Camera camera, RaycastHit hit, Vector2 point);
    }

    public interface IExit3D
    {
        void OnExit(Camera camera, RaycastHit hit, Vector2 point);
    }

    public interface IExClick3D
    {
        void OnExClick(Camera camera, RaycastHit hit, Vector2 point);
    }

    public interface IExClickDown3D
    {
        void OnExClickDown(Camera camera, RaycastHit hit, Vector2 point);
    }

    public interface IExClickUp3D
    {
        void OnExClickUp(Camera camera, RaycastHit hit, Vector2 point);
    }

    public interface IExDrag3D
    {
        void OnExDrag(Camera camera, RaycastHit hit, Vector2 point);
    }

    public interface IExDragDown3D
    {
        void OnExDragDown(Camera camera, RaycastHit hit, Vector2 point);
    }

    public interface IExDragUp3D
    {
        void OnExDragUp(Camera camera, RaycastHit hit, Vector2 point);
    }

}