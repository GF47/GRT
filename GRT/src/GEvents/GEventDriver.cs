using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.GEvents
{
    public abstract class GEventDriver<T> : MonoBehaviour
    {
        private T _raycaster;

        public T Raycaster => _raycaster;

        // public float rayLength = 100f;
        // public LayerMask layer = 1 << 0;

        public float dragThreshold = 0.1f;
        public float doubleClickThreshold = 0.5f;

        private PointersLinkedList<T> _pointers;
        public PointersLinkedList<T> Pointers => _pointers;

        public RaycastHit LastHit { get; private set; }
        public Collider LastCollider { get; private set; }

        protected abstract T GetRaycaster();

        protected abstract bool Cast(out RaycastHit hit);

        protected virtual void Awake()
        {
            if (_raycaster == null) { _raycaster = GetRaycaster(); }

            _pointers = new PointersLinkedList<T>();
        }

        private void OnDisable()
        {
            _pointers.ForEach(pointer => pointer.Reset(this));
        }

        private void Update()
        {
            var cast = Cast(out var hit);

            if (cast)
            {
                var collider = hit.collider;
                if (LastCollider != collider)
                {
                    if (LastCollider != null)
                    {
                        SendPointerExitEvent(LastCollider.gameObject, null, _raycaster, hit);
                    }
                    else
                    {
                        // 有可能是上一个物体直接被销毁了，需要自行处理
                    }
                    LastCollider = collider;
                    SendPointerEnterEvent(LastCollider.gameObject, null, _raycaster, hit);
                }

                SendPointerStayEvent(LastCollider.gameObject, null, _raycaster, hit);
            }
            else
            {
                if (LastCollider != null)
                {
                    SendPointerExitEvent(LastCollider.gameObject, null, _raycaster, hit);
                    LastCollider = null;
                }
            }

            _pointers.ForEach(pointer => pointer.Cast(this, cast, hit));

            LastHit = hit;
        }

        public static void SendPointerEnterEvent(GameObject go, Predicate<Component> predicate, T raycaster, RaycastHit hit)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IGPointerEnter<T> p)
                    {
                        p.OnPointerEnter(raycaster, hit);
                    }
                }
            }
        }

        public static void SendPointerExitEvent(GameObject go, Predicate<Component> predicate, T raycaster, RaycastHit hit)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IGPointerExit<T> p)
                    {
                        p.OnPointerExit(raycaster, hit);
                    }
                }
            }
        }

        public static void SendPointerStayEvent(GameObject go, Predicate<Component> predicate, T raycaster, RaycastHit hit)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IGPointerStay<T> p)
                    {
                        p.OnPointerStay(raycaster, hit);
                    }
                }
            }
        }

        public static void SendPointerDownEvent(GameObject go, Predicate<Component> predicate, T raycaster, RaycastHit hit)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IGPointerDown<T> p)
                    {
                        p.OnPointerDown(raycaster, hit);
                    }
                }
            }
        }

        public static void SendPointerUpEvent(GameObject go, Predicate<Component> predicate, T raycaster, RaycastHit hit)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IGPointerUp<T> p)
                    {
                        p.OnPointerUp(raycaster, hit);
                    }
                }
            }
        }

        public static void SendPointerClickEvent(GameObject go, Predicate<Component> predicate, T raycaster, RaycastHit hit)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IGPointerClick<T> p)
                    {
                        p.OnPointerClick(raycaster, hit);
                    }
                }
            }
        }

        public static void SendPointerDoubleClickEvent(GameObject go, Predicate<Component> predicate, T raycaster, RaycastHit hit)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IGPointerDoubleClick<T> p)
                    {
                        p.OnPointerDoubleClick(raycaster, hit);
                    }
                }
            }
        }

        public static void SendPointerDragStartEvent(GameObject go, Predicate<Component> predicate, T raycaster, RaycastHit hit)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IGPointerDragStart<T> p)
                    {
                        p.OnPointerDragStart(raycaster, hit);
                    }
                }
            }
        }

        public static void SendPointerDragStopEvent(GameObject go, Predicate<Component> predicate, T raycaster, RaycastHit hit)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IGPointerDragStop<T> p)
                    {
                        p.OnPointerDragStop(raycaster, hit);
                    }
                }
            }
        }

        public static void SendPointerDragEvent(GameObject go, Predicate<Component> predicate, T raycaster, RaycastHit hit)
        {
            var coms = GetComponents(go);
            for (int i = 0; i < coms.Count; i++)
            {
                var com = coms[i];
                if (predicate == null || predicate(com))
                {
                    if (com is IGPointerDrag<T> p)
                    {
                        p.OnPointerDrag(raycaster, hit);
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

    public class PointersLinkedList<T>
    {
        private readonly Item<T> _head = new Item<T>(null);

        public void ForEach(Action<IGPointer<T>> action)
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

        public bool Contains(IGPointer<T> pointer)
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

        public P Find<P>(Predicate<P> predicate) where P : IGPointer<T>
        {
            var current = _head.Next;
            while (current != null)
            {
                if (current.Pointer is P pointer && predicate(pointer))
                {
                    return pointer;
                }

                current = current.Next;
            }

            return default;
        }

        public bool Add(IGPointer<T> pointer)
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

                last.Next = new Item<T>(pointer);
                return true;
            }
            return false;
        }

        public bool Remove(IGPointer<T> pointer)
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

        private class Item<TT>
        {
            public Item<TT> Next { get; set; }

            public IGPointer<TT> Pointer { get; private set; }

            public Item(IGPointer<TT> pointer)
            {
                Pointer = pointer;
            }
        }
    }
}