using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GRT.Events
{
    public class EventSystem3D : MonoBehaviour
    {
        #region 鼠标状态标记

        /// <summary> /// 鼠标空闲 /// </summary>
        public const int POINTER_FREE = 0;

        /// <summary> /// 鼠标抬起 /// </summary>
        public const int POINTER_UP = 1;

        /// <summary> /// 鼠标按住 /// </summary>
        public const int POINTER_HOLD = 2;

        /// <summary> /// 鼠标按下 /// </summary>
        public const int POINTER_DOWN = 3;

        /// <summary> 鼠标右键空闲 </summary>
        public const int EX_POINTER_FREE = 4;

        /// <summary> 鼠标右键抬起 </summary>
        public const int EX_POINTER_UP = 5;

        /// <summary> 鼠标右键按住 </summary>
        public const int EX_POINTER_HOLD = 6;

        /// <summary> 鼠标右键按下 </summary>
        public const int EX_POINTER_DOWN = 7;

        #endregion 鼠标状态标记

        /// <summary> /// 空的射线碰撞 /// </summary>
        private static RaycastHit _emptyHit = new RaycastHit();

        /// <summary> 当前的事件系统实例 </summary>
        public static EventSystem3D Current { get; private set; }

        /// <summary> 射线响应距离 </summary>
        public float distance = 100f;

        /// <summary> /// 拖拽起始阈值 /// </summary>
        public int dragThreshold = 100;

        /// <summary> /// 所影响的层级标识 /// </summary>
        public LayerMask castLayer = 1 << 0;

        /// <summary> /// 主相机 /// </summary>
        private Camera _mainCamera;

        /// <summary> /// 秒表 /// </summary>
        private Stopwatch _stopwatch;

        /// <summary> 秒表 右键 </summary>
        private Stopwatch _stopwatchEx;

        /// <summary> /// 响应鼠标悬浮事件的碰撞体 /// </summary>
        private Collider _coverCollider;

        /// <summary> /// 响应鼠标点击与拖拽事件的碰撞体 /// </summary>
        private Collider _collider;

        /// <summary> /// 是否在拖拽 /// </summary>
        private bool _dragging;

        /// <summary> 响应鼠标右键点击与拖拽事件的碰撞体 </summary>
        private Collider _colliderEx;

        /// <summary> 是否右键在拖拽 </summary>
        private bool _draggingEx;

        private Dictionary<int, Vector4> _block;

        public int Block(Vector4 area)
        {
            var i = GRandom.Get();
            _block.Add(i, area);
            return i;
        }

        public void UnBlock(int i)
        {
            _block.Remove(i);
        }

        public void ModifyBlock(int i, Vector4 area)
        {
            _block[i] = area;
        }

        private bool Blocking(Vector2 pos)
        {
            foreach (var pair in _block)
            {
                var area = pair.Value;

                if (area[0] < pos.x && pos.x < area[2] && area[1] < pos.y && pos.y < area[3])
                {
                    return true;
                }
            }
            return false;
        }

        private void Awake()
        {
            _mainCamera = GetComponent<Camera>();

            _stopwatch = new Stopwatch();
            _stopwatchEx = new Stopwatch();

            _block = new Dictionary<int, Vector4>();
        }

        private void OnEnable()
        {
            Current = this;
        }

        private void OnDisable()
        {
            if (_stopwatch.IsRunning)
            {
                _stopwatch.Stop();
            }
            _stopwatch.Reset();

            if (_stopwatchEx.IsRunning)
            {
                _stopwatchEx.Stop();
            }
            _stopwatchEx.Reset();

            var point = GetPointerPosition();

            if (_coverCollider != null)
            {
                SendExitEvent(_coverCollider.gameObject, _emptyHit, point);
            }
            _coverCollider = null;

            if (_collider != null)
            {
                SendClickUpEvent(_collider.gameObject, _emptyHit, point);

                if (_dragging)
                {
                    SendDragUpEvent(_collider.gameObject, _emptyHit, point);
                }
            }
            _collider = null;
            _dragging = false;

            if (_colliderEx != null)
            {
                SendExClickUpEvent(_colliderEx.gameObject, _emptyHit, point);

                if (_draggingEx)
                {
                    SendExDragUpEvent(_colliderEx.gameObject, _emptyHit, point);
                }
            }
            _colliderEx = null;
            _draggingEx = false;
        }

        private void Update()
        {
            var duration = _stopwatch.ElapsedMilliseconds;
            var durationEx = _stopwatchEx.ElapsedMilliseconds;
            var pointerState = GetPointerState();
            var pointerStateEx = GetExPointerState();

            #region camera & ray

            var point = GetPointerPosition();
            var ray = _mainCamera.ScreenPointToRay(point);
            var hit = _emptyHit;

            if (!Blocking(point) && Physics.Raycast(ray, out hit, distance, castLayer))
            {
                var collider = hit.collider;

                if (_coverCollider != collider)
                {
                    if (_coverCollider != null)
                    {
                        SendExitEvent(_coverCollider.gameObject, hit, point);
                    }

                    _coverCollider = collider;

                    // _coverCollider 肯定不为空了
                    SendEnterEvent(_coverCollider.gameObject, hit, point);
                }

                if (pointerState == POINTER_DOWN)
                {
                    _collider = collider;

                    // _collider 肯定不为空了
                    SendClickDownEvent(_collider.gameObject, hit, point);
                }
                if (pointerStateEx == EX_POINTER_DOWN)
                {
                    _colliderEx = collider;

                    SendExClickDownEvent(_colliderEx.gameObject, hit, point);
                }
            }
            else
            {
                if (_coverCollider != null)
                {
                    SendExitEvent(_coverCollider.gameObject, hit, point);
                }
                _coverCollider = null;
            }

            #endregion camera & ray

            #region click & drag

            if (pointerState == POINTER_UP)
            {
                if (_collider != null)
                {
                    SendClickUpEvent(_collider.gameObject, hit, point);

                    if (_dragging)
                    {
                        SendDragUpEvent(_collider.gameObject, hit, point);
                    }
                    else
                    {
                        SendClickEvent(_collider.gameObject, hit, point);
                    }
                }
                _collider = null;
                _dragging = false;
            }
            else if (pointerState == POINTER_HOLD)
            {
                if (_collider != null)
                {
                    if (duration >= dragThreshold)
                    {
                        if (_dragging)
                        {
                            SendDragEvent(_collider.gameObject, hit, point);
                        }
                        else
                        {
                            _dragging = true;
                            SendDragDownEvent(_collider.gameObject, hit, point);
                        }
                    }
                }
            }

            if (pointerStateEx == EX_POINTER_UP)
            {
                if (_colliderEx != null)
                {
                    SendExClickUpEvent(_colliderEx.gameObject, hit, point);

                    if (_draggingEx)
                    {
                        SendExDragUpEvent(_colliderEx.gameObject, hit, point);
                    }
                    else
                    {
                        SendExClickEvent(_colliderEx.gameObject, hit, point);
                    }
                }
                _colliderEx = null;
                _draggingEx = false;
            }
            else if (pointerStateEx == EX_POINTER_HOLD)
            {
                if (_colliderEx != null)
                {
                    if (durationEx >= dragThreshold)
                    {
                        if (_draggingEx)
                        {
                            SendExDragEvent(_colliderEx.gameObject, hit, point);
                        }
                        else
                        {
                            _draggingEx = true;
                            SendExDragDownEvent(_colliderEx.gameObject, hit, point);
                        }
                    }
                }
            }

            #endregion click & drag
        }

        /// <summary>
        /// 获取鼠标状态
        /// </summary>
        private int GetPointerState()
        {
            var pointerState = POINTER_FREE;

            if (GetPointerHold())
            {
                pointerState = POINTER_HOLD;

                if (GetPointerDown())
                {
                    _stopwatch.Start();

                    pointerState = POINTER_DOWN;
                }
            }
            else if (GetPointerUp())
            {
                _stopwatch.Stop();
                _stopwatch.Reset();

                pointerState = POINTER_UP;
            }

            return pointerState;
        }

        private int GetExPointerState()
        {
            var pointerState = EX_POINTER_FREE;

            if (GetExPointerHold())
            {
                pointerState = EX_POINTER_HOLD;

                if (GetExPointerDown())
                {
                    _stopwatchEx.Start();
                    pointerState = EX_POINTER_DOWN;
                }
            }
            else if (GetExPointerUp())
            {
                _stopwatchEx.Stop();
                _stopwatchEx.Reset();

                pointerState = EX_POINTER_UP;
            }

            return pointerState;
        }

        private GameObject _cache;
        private IList<IDrag3D> _IDragCache;
        private IList<IExDrag3D> _IExDragCache;
        private IList<IHold3D> _IHoldCache;

        private IList<T> GetInterfaces<T>(GameObject go) where T : class
        {
            if (go == _cache)
            {
                if (typeof(T) == typeof(IDrag3D))
                {
                    if (_IDragCache != null) { return (IList<T>)_IDragCache; }
                }
                else if (typeof(T) == typeof(IExDrag3D))
                {
                    if (_IExDragCache != null) { return (IList<T>)_IExDragCache; }
                }
                else if (typeof(T) == typeof(IHold3D))
                {
                    if (_IHoldCache != null) { return (IList<T>)_IHoldCache; }
                }
            }
            else
            {
                _cache = go;
                _IDragCache = null;
                _IExDragCache = null;
                _IHoldCache = null;
            }

            var tempComponents = go.GetComponents<Component>();
            var components = new List<T>(tempComponents.Length);
            for (int i = 0; i < tempComponents.Length; i++)
            {
                var c = tempComponents[i] as T;
                if (c != null)
                {
                    components.Add(c);
                }
            }

            if      (typeof(T) == typeof(IDrag3D))   { _IDragCache   = (IList<IDrag3D>)components; }
            else if (typeof(T) == typeof(IExDrag3D)) { _IExDragCache = (IList<IExDrag3D>)components; }
            else if (typeof(T) == typeof(IHold3D))   { _IHoldCache   = (IList<IHold3D>)components; }
            return components;
        }

        protected virtual void SendEnterEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            var coms = GetInterfaces<IEnter3D>(go);
            for (int i = 0; i < coms.Count; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnEnter(_mainCamera, hit, point);
                }
            }
        }

        protected virtual void SendExitEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            _IHoldCache = null;
            var coms = GetInterfaces<IExit3D>(go);
            for (int i = 0; i < coms.Count; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnExit(_mainCamera, hit, point);
                }
            }
        }

        protected virtual void SendClickDownEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            var coms = GetInterfaces<IClickDown3D>(go);
            for (int i = 0; i < coms.Count; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnClickDown(_mainCamera, hit, point);
                }
            }
        }

        protected virtual void SendClickUpEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            var coms = GetInterfaces<IClickUp3D>(go);
            for (int i = 0; i < coms.Count; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnClickUp(_mainCamera, hit, point);
                }
            }
        }

        protected virtual void SendClickEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            var coms = GetInterfaces<IClick3D>(go);
            for (int i = 0; i < coms.Count; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnClick(_mainCamera, hit, point);
                }
            }
        }

        protected virtual void SendDragDownEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            var coms = GetInterfaces<IDragDown3D>(go);
            for (int i = 0; i < coms.Count; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnDragDown(_mainCamera, hit, point);
                }
            }
        }

        protected virtual void SendDragUpEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            _IDragCache = null;
            var coms = GetInterfaces<IDragUp3D>(go);
            for (int i = 0; i < coms.Count; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnDragUp(_mainCamera, hit, point);
                }
            }
        }

        protected virtual void SendDragEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            var coms = GetInterfaces<IDrag3D>(go);
            for (int i = 0; i < coms.Count; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnDrag(_mainCamera, hit, point);
                }
            }
        }

        protected virtual void SendExClickDownEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            var coms = GetInterfaces<IExClickDown3D>(go);
            for (int i = 0; i < coms.Count; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnExClickDown(_mainCamera, hit, point);
                }
            }
        }

        protected virtual void SendExClickUpEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            var coms = GetInterfaces<IExClickUp3D>(go);
            for (int i = 0; i < coms.Count; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnExClickUp(_mainCamera, hit, point);
                }
            }
        }

        protected virtual void SendExClickEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            var coms = GetInterfaces<IExClick3D>(go);
            for (int i = 0; i < coms.Count; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnExClick(_mainCamera, hit, point);
                }
            }
        }

        protected virtual void SendExDragDownEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            var coms = GetInterfaces<IExDragDown3D>(go);
            for (int i = 0; i < coms.Count; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnExDragDown(_mainCamera, hit, point);
                }
            }
        }

        protected virtual void SendExDragUpEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            _IExDragCache = null;
            var coms = GetInterfaces<IExDragUp3D>(go);
            for (int i = 0; i < coms.Count; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnExDragUp(_mainCamera, hit, point);
                }
            }
        }

        protected virtual void SendExDragEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            var coms = GetInterfaces<IExDrag3D>(go);
            for (int i = 0; i < coms.Count; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnExDrag(_mainCamera, hit, point);
                }
            }
        }

        /***************************************************************
         * 可以重写的方法
         *
         * GetPointer 替换为相应平台的触发方式
         **************************************************************/

        protected virtual Vector3 GetPointerPosition() => Input.mousePosition;

        protected virtual bool GetPointerDown() => Input.GetMouseButtonDown(0);

        protected virtual bool GetPointerUp() => Input.GetMouseButtonUp(0);

        protected virtual bool GetPointerHold() => Input.GetMouseButton(0);

        protected virtual bool GetExPointerDown() => Input.GetMouseButtonDown(1);

        protected virtual bool GetExPointerUp() => Input.GetMouseButtonUp(1);

        protected virtual bool GetExPointerHold() => Input.GetMouseButton(1);
    }
}
