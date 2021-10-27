using UnityEngine;
using UnityEngine.Events;

namespace GRT.Events
{
    public class EventTrigger3D : MonoBehaviour, IClickDown3D, IClickUp3D, IClick3D, IDragDown3D, IDragUp3D, IDrag3D, IEnter3D, IExit3D
    {
        public UnityEvent<Camera, RaycastHit> onClickDown;
        public UnityEvent<Camera, RaycastHit> onClickUp;
        public UnityEvent<Camera, RaycastHit> onClick;
        public UnityEvent<Camera, RaycastHit> onDragDown;
        public UnityEvent<Camera, RaycastHit> onDragUp;
        public UnityEvent<Camera, RaycastHit> onDrag;
        public UnityEvent<Camera, RaycastHit> onEnter;
        public UnityEvent<Camera, RaycastHit> onExit;

        public void OnClickDown(Camera camera, RaycastHit hit) => onClickDown.Invoke(camera, hit);
        public void OnClickUp(Camera camera, RaycastHit hit) => onClickUp.Invoke(camera, hit);
        public void OnClick(Camera camera, RaycastHit hit) => onClick.Invoke(camera, hit);
        public void OnDragDown(Camera camera, RaycastHit hit) => onDragDown.Invoke(camera, hit);
        public void OnDragUp(Camera camera, RaycastHit hit) => onDragUp.Invoke(camera, hit);
        public void OnDrag(Camera camera, RaycastHit hit) => onDrag.Invoke(camera, hit);
        public void OnEnter(Camera camera, RaycastHit hit) => onEnter.Invoke(camera, hit);
        public void OnExit(Camera camera, RaycastHit hit) => onExit.Invoke(camera, hit);
    }
}