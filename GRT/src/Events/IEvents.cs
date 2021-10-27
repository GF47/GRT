using UnityEngine;

namespace GRT.Events
{
    public interface IClick3D
    {
        void OnClick(Camera camera, RaycastHit hit);
    }

    public interface IClickDown3D
    {
        void OnClickDown(Camera camera, RaycastHit hit);
    }

    public interface IClickUp3D
    {
        void OnClickUp(Camera camera, RaycastHit hit);
    }

    public interface IDrag3D
    {
        void OnDrag(Camera camera, RaycastHit hit);
    }

    public interface IDragDown3D
    {
        void OnDragDown(Camera camera, RaycastHit hit);
    }

    public interface IDragUp3D
    {
        void OnDragUp(Camera camera, RaycastHit hit);
    }

    public interface IEnter3D
    {
        void OnEnter(Camera camera, RaycastHit hit);
    }

    public interface IExit3D
    {
        void OnExit(Camera camera, RaycastHit hit);
    }

}