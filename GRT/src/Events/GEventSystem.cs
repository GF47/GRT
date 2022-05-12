using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.Events
{
    public class GEventSystem : MonoBehaviour
    {
        public static GEventSystem Current { get; private set; }

        private Camera _camera;
        private IList<IPointer> _pointers;
        private Collider _hoveredCollider;

        public float distance = 100f;
        public LayerMask layer = 1 << 0;

        public float dragThreshold = 0.1f;
        public float doubleClickThreshod = 0.5f;

        public Camera Camera => _camera;
        public IList<IPointer> Pointers => _pointers;
        public virtual Vector3 PointerPosition => Input.mousePosition;

        public Blocker Blocker { get; private set; }

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _pointers = new List<IPointer>();
            Blocker = new Blocker();
        }

        // private void OnEnable()
        // {
        //     Current = this;
        // }

        private void OnDisable()
        {
            if (Current == this) { Current = null; }

            foreach (var pointer in _pointers)
            {
                pointer.Reset(this);
            }
        }

        private void Update()
        {
            if (Current != this) { Current = this; }

            bool cased = false;
            RaycastHit hit = default;

            var pos = PointerPosition;
            if (!Blocker.Blocking(pos))
            {
                cased = Physics.Raycast(_camera.ScreenPointToRay(pos), out hit, distance, layer);
            }

            if (cased)
            {
                var collider = hit.collider;
                if (_hoveredCollider != collider)
                {
                    if (_hoveredCollider != null)
                    {
                        SendPointerExitEvent(_hoveredCollider.gameObject, null, _camera, hit, pos);
                    }
                    _hoveredCollider = collider;
                    SendPointerEnterEvent(_hoveredCollider.gameObject, null, _camera, hit, pos);
                }

                SendPointerHoverEvent(_hoveredCollider.gameObject, null, _camera, hit, pos);
            }
            else
            {
                if (_hoveredCollider != null)
                {
                    SendPointerExitEvent(_hoveredCollider.gameObject, null, _camera, hit, pos);
                    _hoveredCollider = null;
                }
            }

            foreach (var pointer in _pointers)
            {
                pointer.Case(this, cased, hit);
            }
        }

        public static void SendPointerEnterEvent(GameObject go, Predicate<Component> predicate, Camera camera, RaycastHit hit, Vector2 pos)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IPointerEnter p)
                    {
                        p.OnPointerEnter(camera, hit, pos);
                    }
                }
            }
        }

        public static void SendPointerExitEvent(GameObject go, Predicate<Component> predicate, Camera camera, RaycastHit hit, Vector2 pos)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IPointerExit p)
                    {
                        p.OnPointerExit(camera, hit, pos);
                    }
                }
            }
        }

        public static void SendPointerHoverEvent(GameObject go, Predicate<Component> predicate, Camera camera, RaycastHit hit, Vector2 pos)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IPointerHover p)
                    {
                        p.OnPointerHover(camera, hit, pos);
                    }
                }
            }
        }

        public static void SendPointerDownEvent(GameObject go, Predicate<Component> predicate, Camera camera, RaycastHit hit, Vector2 pos)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IPointerDown p)
                    {
                        p.OnPointerDown(camera, hit, pos);
                    }
                }
            }
        }

        public static void SendPointerUpEvent(GameObject go, Predicate<Component> predicate, Camera camera, RaycastHit hit, Vector2 pos)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IPointerUp p)
                    {
                        p.OnPointerUp(camera, hit, pos);
                    }
                }
            }
        }

        public static void SendPointerClickEvent(GameObject go, Predicate<Component> predicate, Camera camera, RaycastHit hit, Vector2 pos)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IPointerClick p)
                    {
                        p.OnPointerClick(camera, hit, pos);
                    }
                }
            }
        }

        public static void SendPointerDoubleClickEvent(GameObject go, Predicate<Component> predicate, Camera camera, RaycastHit hit, Vector2 pos)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IPointerDoubleClick p)
                    {
                        p.OnPointerDoubleClick(camera, hit, pos);
                    }
                }
            }
        }

        public static void SendPointerDragStartEvent(GameObject go, Predicate<Component> predicate, Camera camera, RaycastHit hit, Vector2 pos)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IPointerDragStart p)
                    {
                        p.OnPointerDragStart(camera, hit, pos);
                    }
                }
            }
        }

        public static void SendPointerDragStopEvent(GameObject go, Predicate<Component> predicate, Camera camera, RaycastHit hit, Vector2 pos)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IPointerDragStop p)
                    {
                        p.OnPointerDragStop(camera, hit, pos);
                    }
                }
            }
        }

        public static void SendPointerDragEvent(GameObject go, Predicate<Component> predicate, Camera camera, RaycastHit hit, Vector2 pos)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IPointerDrag p)
                    {
                        p.OnPointerDrag(camera, hit, pos);
                    }
                }
            }
        }

        private static IList<Component> GetComponents(GameObject go)
        {
            if (go == _goCache)
            {
                return _comCache;
            }

            _goCache = go;
            _comCache = go.GetComponents<Component>();

            return _comCache;
        }

        private static GameObject _goCache;
        private static IList<Component> _comCache;
    }
}