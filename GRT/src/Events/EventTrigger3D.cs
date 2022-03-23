using System;
using UnityEngine;
using UnityEngine.Events;

namespace GRT.Events
{
    public class EventTrigger3D : MonoBehaviour, IClickDown3D, IClickUp3D, IClick3D, IDragDown3D, IDragUp3D, IDrag3D, IEnter3D, IExit3D
    {
        public Event3D onClickDown;
        public Event3D onClickUp;
        public Event3D onClick;
        public Event3D onDragDown;
        public Event3D onDragUp;
        public Event3D onDrag;
        public Event3D onEnter;
        public Event3D onExit;

        public void OnClickDown(Camera camera, RaycastHit hit, Vector2 point) => onClickDown?.Invoke(camera, hit, point);
        public void OnClickUp(Camera camera, RaycastHit hit, Vector2 point) => onClickUp?.Invoke(camera, hit, point);
        public void OnClick(Camera camera, RaycastHit hit, Vector2 point) => onClick?.Invoke(camera, hit, point);
        public void OnDragDown(Camera camera, RaycastHit hit, Vector2 point) => onDragDown?.Invoke(camera, hit, point);
        public void OnDragUp(Camera camera, RaycastHit hit, Vector2 point) => onDragUp?.Invoke(camera, hit, point);
        public void OnDrag(Camera camera, RaycastHit hit, Vector2 point) => onDrag?.Invoke(camera, hit, point);
        public void OnEnter(Camera camera, RaycastHit hit, Vector2 point) => onEnter?.Invoke(camera, hit, point);
        public void OnExit(Camera camera, RaycastHit hit, Vector2 point) => onExit?.Invoke(camera, hit, point);
    }

    [Serializable]
    public class Event3D : UnityEvent<Camera, RaycastHit, Vector2> { }
}