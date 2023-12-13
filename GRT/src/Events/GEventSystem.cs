using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.Events
{
    public class GEventSystem : MonoBehaviour
    {
        private Camera _camera;
        private PointersLinkedList _pointers;

        public float distance = 100f;
        public LayerMask layer = 1 << 0;

        public float dragThreshold = 0.1f;
        public float doubleClickThreshod = 0.5f;

        public Camera Camera => _camera;
        public PointersLinkedList Pointers => _pointers;
        public RaycastHit LastHit { get; private set; }
        public Collider LastCollider { get; private set; }
        public virtual Vector3 PointerPosition => Input.mousePosition;

        public bool Blocking { get; private set; }
        public Blocker Blocker { get; private set; }
        public static Blocker GlobalBlocker { get; } = new Blocker();

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _pointers = new PointersLinkedList();
            Blocker = new Blocker();
        }

        private void OnDisable()
        {
            _pointers.ForEach(pointer => pointer.Reset(this));
        }

        private void Update()
        {
            bool cased = false;
            RaycastHit hit = default;

            var pos = PointerPosition;
            Blocking = OffScreen() || Blocker.Blocking(pos) || GlobalBlocker.Blocking(pos);
            if (!Blocking)
            {
                cased = Physics.Raycast(_camera.ScreenPointToRay(pos), out hit, distance, layer);
            }

            if (cased)
            {
                var collider = hit.collider;
                if (LastCollider != collider)
                {
                    if (LastCollider != null)
                    {
                        SendPointerExitEvent(LastCollider.gameObject, null, _camera, hit, pos);
                    }
                    else
                    {
                        // 有可能是上一个物体直接被销毁了，需要自行处理
                    }
                    LastCollider = collider;
                    SendPointerEnterEvent(LastCollider.gameObject, null, _camera, hit, pos);
                }

                SendPointerHoverEvent(LastCollider.gameObject, null, _camera, hit, pos);
            }
            else
            {
                if (LastCollider != null)
                {
                    SendPointerExitEvent(LastCollider.gameObject, null, _camera, hit, pos);
                    LastCollider = null;
                }
            }

            _pointers.ForEach(pointer => pointer.Case(this, cased, hit));

            LastHit = hit;
        }

        private bool OffScreen()
        {
            var pos = PointerPosition;
            var area = Screen.safeArea;
            return pos.x < area.x
                || pos.x > area.width
                || pos.y < area.y
                || pos.y > area.height;
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

    public class PointersLinkedList
    {
        private readonly Item _head = new Item(null);

        public void ForEach(Action<IPointer> action)
        {
            if (action != null)
            {
                var current = _head.Next;
                while (current != null)
                {
                    action(current.Pointer);
                    current = current.Next;
                }
            }
        }

        public bool Contains(IPointer pointer)
        {
            if (pointer != null)
            {
                var current = _head.Next;
                while (current != null)
                {
                    if (current.Pointer == pointer)
                    {
                        return true;
                    }
                    current = current.Next;
                }
            }
            return false;
        }

        public T Find<T>(Predicate<T> predicate) where T : IPointer
        {
            var current = _head.Next;
            while(current != null)
            {
                if (current.Pointer is T tPointer && predicate(tPointer))
                {
                    return tPointer;
                }

                current = current.Next;
            }

            return default;
        }

        public bool Add(IPointer pointer)
        {
            if (pointer != null)
            {
                var current = _head.Next;
                var last = _head;
                while (current != null)
                {
                    if (current.Pointer == pointer)
                    {
                        // already exist
                        return false;
                    }

                    last = current;
                    current = current.Next;
                }

                last.Next = new Item(pointer);
                return true;
            }
            return false;
        }

        public bool Remove(IPointer pointer)
        {
            if (pointer != null)
            {
                var current = _head.Next;
                var last = _head;
                while (current != null)
                {
                    if (current.Pointer == pointer)
                    {
                        last.Next = current.Next;
                        return true;
                    }

                    last = current;
                    current = current.Next;
                }
            }
            return false;
        }

        private class Item
        {
            public Item Next { get; set; }

            public IPointer Pointer { get; private set; }

            public Item(IPointer pointer)
            {
                Pointer = pointer;
            }
        }
    }
}