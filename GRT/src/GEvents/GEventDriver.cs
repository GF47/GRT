using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.GEvents
{
    public abstract class GEventDriver<T> : MonoBehaviour
    {
        public abstract T Raycaster { get; }

        // public float rayLength = 100f;
        // public LayerMask layer = 1 << 0;

        public float dragThreshold = 0.3f;
        public float doubleClickThreshold = 0.5f;

        private PointersLinkedList<T> _pointers;
        public PointersLinkedList<T> Pointers => _pointers;

        public RaycastHit LastHit { get; private set; }
        public Collider LastCollider { get; private set; }

        protected abstract bool Cast(out RaycastHit hit);

        protected virtual void Awake()
        {
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
                        SendPointerExitEvent(LastCollider, null, this, hit);
                    }
                    else
                    {
                        // 有可能是上一个物体直接被销毁了，需要自行处理
                    }
                    LastCollider = collider;
                    SendPointerEnterEvent(LastCollider, null, this, hit);
                }

                SendPointerStayEvent(LastCollider, null, this, hit);
            }
            else
            {
                if (LastCollider != null)
                {
                    SendPointerExitEvent(LastCollider, null, this, hit);
                    LastCollider = null;
                }
            }

            _pointers.ForEach(pointer => pointer.Cast(this, cast, hit));

            LastHit = hit;
        }

        protected virtual bool PointerEnterOverride(Collider collider, T raycaster, RaycastHit hit) => true;

        public static void SendPointerEnterEvent(Collider collider, Predicate<Component> predicate, GEventDriver<T> driver, RaycastHit hit)
        {
            if (driver.PointerEnterOverride(collider, driver.Raycaster, hit))
            {
                var coms = GetComponents(collider);
                for (int i = 0; i < coms.Count; i++)
                {
                    var com = coms[i];
                    if (predicate == null || predicate(com))
                    {
                        if (com is IGPointerEnter<T> p)
                        {
                            p.OnPointerEnter(driver.Raycaster, hit);
                        }
                    }
                }
            }
        }

        protected virtual bool PointerExitOverride(Collider collider, T raycaster, RaycastHit hit) => true;

        public static void SendPointerExitEvent(Collider collider, Predicate<Component> predicate, GEventDriver<T> driver, RaycastHit hit)
        {
            if (driver.PointerExitOverride(collider, driver.Raycaster, hit))
            {
                var coms = GetComponents(collider);
                for (int i = 0; i < coms.Count; i++)
                {
                    var com = coms[i];
                    if (predicate == null || predicate(com))
                    {
                        if (com is IGPointerExit<T> p)
                        {
                            p.OnPointerExit(driver.Raycaster, hit);
                        }
                    }
                }
            }
        }

        protected virtual bool PointerStayOverride(Collider collider, T raycaster, RaycastHit hit) => true;

        public static void SendPointerStayEvent(Collider collider, Predicate<Component> predicate, GEventDriver<T> driver, RaycastHit hit)
        {
            if (driver.PointerStayOverride(collider, driver.Raycaster, hit))
            {
                var coms = GetComponents(collider);
                for (int i = 0; i < coms.Count; i++)
                {
                    var com = coms[i];
                    if (predicate == null || predicate(com))
                    {
                        if (com is IGPointerStay<T> p)
                        {
                            p.OnPointerStay(driver.Raycaster, hit);
                        }
                    }
                }
            }
        }

        protected virtual bool PointerDownOverride(Collider collider, T raycaster, RaycastHit hit) => true;

        public static void SendPointerDownEvent(Collider collider, Predicate<Component> predicate, GEventDriver<T> driver, RaycastHit hit)
        {
            if (driver.PointerDownOverride(collider, driver.Raycaster, hit))
            {
                var coms = GetComponents(collider);
                for (int i = 0; i < coms.Count; i++)
                {
                    var com = coms[i];
                    if (predicate == null || predicate(com))
                    {
                        if (com is IGPointerDown<T> p)
                        {
                            p.OnPointerDown(driver.Raycaster, hit);
                        }
                    }
                }
            }
        }

        protected virtual bool PointerUpOverride(Collider collider, T raycaster, RaycastHit hit) => true;

        public static void SendPointerUpEvent(Collider collider, Predicate<Component> predicate, GEventDriver<T> driver, RaycastHit hit)
        {
            if (driver.PointerUpOverride(collider, driver.Raycaster, hit))
            {
                var coms = GetComponents(collider);
                for (int i = 0; i < coms.Count; i++)
                {
                    var com = coms[i];
                    if (predicate == null || predicate(com))
                    {
                        if (com is IGPointerUp<T> p)
                        {
                            p.OnPointerUp(driver.Raycaster, hit);
                        }
                    }
                }
            }
        }

        protected virtual bool PointerClickOverride(Collider collider, T raycaster, RaycastHit hit) => true;

        public static void SendPointerClickEvent(Collider collider, Predicate<Component> predicate, GEventDriver<T> driver, RaycastHit hit)
        {
            if (driver.PointerClickOverride(collider, driver.Raycaster, hit))
            {
                var coms = GetComponents(collider);
                for (int i = 0; i < coms.Count; i++)
                {
                    var com = coms[i];
                    if (predicate == null || predicate(com))
                    {
                        if (com is IGPointerClick<T> p)
                        {
                            p.OnPointerClick(driver.Raycaster, hit);
                        }
                    }
                }
            }
        }

        protected virtual bool PointerDoubleClickOverride(Collider collider, T raycaster, RaycastHit hit) => true;

        public static void SendPointerDoubleClickEvent(Collider collider, Predicate<Component> predicate, GEventDriver<T> driver, RaycastHit hit)
        {
            if (driver.PointerDoubleClickOverride(collider, driver.Raycaster, hit))
            {
                var coms = GetComponents(collider);
                for (int i = 0; i < coms.Count; i++)
                {
                    var com = coms[i];
                    if (predicate == null || predicate(com))
                    {
                        if (com is IGPointerDoubleClick<T> p)
                        {
                            p.OnPointerDoubleClick(driver.Raycaster, hit);
                        }
                    }
                }
            }
        }

        protected virtual bool PointerDragStartOverride(Collider collider, T raycaster, RaycastHit hit) => true;

        public static void SendPointerDragStartEvent(Collider collider, Predicate<Component> predicate, GEventDriver<T> driver, RaycastHit hit)
        {
            if (driver.PointerDragStartOverride(collider, driver.Raycaster, hit))
            {
                var coms = GetComponents(collider);
                for (int i = 0; i < coms.Count; i++)
                {
                    var com = coms[i];
                    if (predicate == null || predicate(com))
                    {
                        if (com is IGPointerDragStart<T> p)
                        {
                            p.OnPointerDragStart(driver.Raycaster, hit);
                        }
                    }
                }
            }
        }

        protected virtual bool PointerDragStopOverride(Collider collider, T raycaster, RaycastHit hit) => true;

        public static void SendPointerDragStopEvent(Collider collider, Predicate<Component> predicate, GEventDriver<T> driver, RaycastHit hit)
        {
            if (driver.PointerDragStopOverride(collider, driver.Raycaster, hit))
            {
                var coms = GetComponents(collider);
                for (int i = 0; i < coms.Count; i++)
                {
                    var com = coms[i];
                    if (predicate == null || predicate(com))
                    {
                        if (com is IGPointerDragStop<T> p)
                        {
                            p.OnPointerDragStop(driver.Raycaster, hit);
                        }
                    }
                }
            }
        }

        protected virtual bool PointerDragOverride(Collider collider, T raycaster, RaycastHit hit) => true;

        public static void SendPointerDragEvent(Collider collider, Predicate<Component> predicate, GEventDriver<T> driver, RaycastHit hit)
        {
            if (driver.PointerDragOverride(collider, driver.Raycaster, hit))
            {
                var coms = GetComponents(collider);
                for (int i = 0; i < coms.Count; i++)
                {
                    var com = coms[i];
                    if (predicate == null || predicate(com))
                    {
                        if (com is IGPointerDrag<T> p)
                        {
                            p.OnPointerDrag(driver.Raycaster, hit);
                        }
                    }
                }
            }
        }

        private static IList<Component> GetComponents(Collider collider)
        {
            var go = collider.GetRealGameObject();
            if (go != _goCache)
            {
                _goCache = go;
                _comCache = go.GetComponents<Component>();
            }

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