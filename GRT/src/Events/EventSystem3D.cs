using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GRT.Events
{
    public class EventSystem3D : MonoBehaviour
    {
        /// <summary> /// 鼠标空闲 /// </summary>
        public const int POINTER_FREE = 0;

        /// <summary> /// 鼠标抬起 /// </summary>
        public const int POINTER_UP = 1;

        /// <summary> /// 鼠标按住 /// </summary>
        public const int POINTER_HOLD = 2;

        /// <summary> /// 鼠标按下 /// </summary>
        public const int POINTER_DOWN = 3;

        /// <summary> /// 空的射线碰撞 /// </summary>
        private static RaycastHit _emptyHit = new RaycastHit();


        public static EventSystem3D Current { get; private set; }

        /// <summary> /// 拖拽起始阈值 /// </summary>
        public int dragThreshold = 100;

        /// <summary> /// 所影响的层级标识 /// </summary>
        public LayerMask castLayer = 1 << 0;



        /// <summary> /// 主相机 /// </summary>
        private Camera _mainCamera;

        /// <summary> /// 秒表 /// </summary>
        private Stopwatch _stopwatch;

        /// <summary> /// 响应鼠标悬浮事件的碰撞体 /// </summary>
        private Collider _coverCollider;

        /// <summary> /// 响应鼠标点击与拖拽事件的碰撞体 /// </summary>
        private Collider _collider;

        /// <summary> /// 是否在拖拽 /// </summary>
        private bool _dragging;

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

        private  bool Blocking(Vector2 pos)
        {
            foreach (var pair in _block)
            {
                var area = pair.Value;

                if (area[0]<pos.x&&pos.x<area[2] && area[1]<pos.y&& pos.y < area[3])
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
        }

        private void Update()
        {
            var duration = _stopwatch.ElapsedMilliseconds;
            var pointerState = GetPointerState();

            #region camera & ray

            var point = GetPointerPosition();
            var ray = _mainCamera.ScreenPointToRay(point);
            var hit = _emptyHit;

            if (!Blocking(point) && Physics.Raycast(ray, out hit, 1000f, castLayer))
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



        private T[] GetInterfaces<T>(GameObject go) where T : class
        {
            return Array.ConvertAll(go.GetComponents<Component>(), c => c as T);
        }

        protected virtual void SendEnterEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            var coms = GetInterfaces<IEnter3D>(go);
            for (int i = 0; i < coms.Length; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnEnter(_mainCamera, hit, point);
                }
            }
        }

        protected virtual void SendExitEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            var coms = GetInterfaces<IExit3D>(go);
            for (int i = 0; i < coms.Length; i++)
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
            for (int i = 0; i < coms.Length; i++)
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
            for (int i = 0; i < coms.Length; i++)
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
            for (int i = 0; i < coms.Length; i++)
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
            for (int i = 0; i < coms.Length; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnDragDown(_mainCamera, hit, point);
                }
            }
        }

        protected virtual void SendDragUpEvent(GameObject go, RaycastHit hit, Vector2 point)
        {
            var coms = GetInterfaces<IDragUp3D>(go);
            for (int i = 0; i < coms.Length; i++)
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
            for (int i = 0; i < coms.Length; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnDrag(_mainCamera, hit, point);
                }
            }
        }



        /***************************************************************
         * 可以重写的方法
         *
         * GetPointer 替换为相应平台的触发方式
         **************************************************************/

        protected virtual bool GetPointerDown()
        {
            return Input.GetMouseButtonDown(0);
        }

        protected virtual bool GetPointerUp()
        {
            return Input.GetMouseButtonUp(0);
        }

        protected virtual bool GetPointerHold()
        {
            return Input.GetMouseButton(0);
        }

        protected virtual Vector3 GetPointerPosition()
        {
            return Input.mousePosition;
        }
    }
}