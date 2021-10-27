using System;
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



        private void Awake()
        {
            _mainCamera = GetComponent<Camera>();

            _stopwatch = new Stopwatch();
        }

        private void OnDisable()
        {
            if (_stopwatch.IsRunning)
            {
                _stopwatch.Stop();
            }
            _stopwatch.Reset();



            if (_coverCollider != null)
            {
                SendExitEvent(_coverCollider.gameObject, new RaycastHit());
            }
            _coverCollider = null;



            if (_collider != null)
            {
                SendClickUpEvent(_collider.gameObject, new RaycastHit());

                if (_dragging)
                {
                    SendDragUpEvent(_collider.gameObject, new RaycastHit());
                }
                else
                {
                    SendClickEvent(_collider.gameObject, new RaycastHit());
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

            var ray = _mainCamera.ScreenPointToRay(GetPointerPosition());
            if (Physics.Raycast(ray, out var hit, 1000f, castLayer))
            {
                var collider = hit.collider;

                if (_coverCollider != collider)
                {
                    if (_coverCollider != null)
                    {
                        SendExitEvent(_coverCollider.gameObject, hit);
                    }

                    _coverCollider = collider;

                    if (_coverCollider != null)
                    {
                        SendEnterEvent(_coverCollider.gameObject, hit);
                    }
                }

                if (pointerState == POINTER_DOWN)
                {
                    _collider = collider;

                    if (_collider != null)
                    {
                        SendClickDownEvent(_collider.gameObject, hit);
                    }
                }
            }
            else
            {
                if (_coverCollider != null)
                {
                    SendExitEvent(_coverCollider.gameObject, hit);
                }
                _coverCollider = null;
            }

            #endregion camera & ray

            #region click & drag

            if (pointerState == POINTER_UP)
            {
                if (_collider != null)
                {
                    SendClickUpEvent(_collider.gameObject, new RaycastHit());

                    if (_dragging)
                    {
                        SendDragUpEvent(_collider.gameObject, new RaycastHit());
                    }
                    else
                    {
                        SendClickEvent(_collider.gameObject, new RaycastHit());
                    }
                }
                _collider = null;
                _dragging = false;
            }

            if (pointerState == POINTER_HOLD)
            {
                if (_collider != null)
                {
                    if (duration >= dragThreshold)
                    {
                        if (_dragging)
                        {
                            SendDragEvent(_collider.gameObject, hit);
                        }
                        else
                        {
                            _dragging = true;
                            SendDragDownEvent(_collider.gameObject, hit);
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

        protected virtual void SendEnterEvent(GameObject go, RaycastHit hit)
        {
            var coms = GetInterfaces<IEnter3D>(go);
            for (int i = 0; i < coms.Length; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnEnter(_mainCamera, hit);
                }
            }
        }

        protected virtual void SendExitEvent(GameObject go, RaycastHit hit)
        {
            var coms = GetInterfaces<IExit3D>(go);
            for (int i = 0; i < coms.Length; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnExit(_mainCamera, hit);
                }
            }
        }

        protected virtual void SendClickDownEvent(GameObject go, RaycastHit hit)
        {
            var coms = GetInterfaces<IClickDown3D>(go);
            for (int i = 0; i < coms.Length; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnClickDown(_mainCamera, hit);
                }
            }
        }

        protected virtual void SendClickUpEvent(GameObject go, RaycastHit hit)
        {
            var coms = GetInterfaces<IClickUp3D>(go);
            for (int i = 0; i < coms.Length; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnClickUp(_mainCamera, hit);
                }
            }
        }

        protected virtual void SendClickEvent(GameObject go, RaycastHit hit)
        {
            var coms = GetInterfaces<IClick3D>(go);
            for (int i = 0; i < coms.Length; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnClick(_mainCamera, hit);
                }
            }
        }

        protected virtual void SendDragDownEvent(GameObject go, RaycastHit hit)
        {
            var coms = GetInterfaces<IDragDown3D>(go);
            for (int i = 0; i < coms.Length; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnDragDown(_mainCamera, hit);
                }
            }
        }

        protected virtual void SendDragUpEvent(GameObject go, RaycastHit hit)
        {
            var coms = GetInterfaces<IDragUp3D>(go);
            for (int i = 0; i < coms.Length; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnDragUp(_mainCamera, hit);
                }
            }
        }

        protected virtual void SendDragEvent(GameObject go, RaycastHit hit)
        {
            var coms = GetInterfaces<IDrag3D>(go);
            for (int i = 0; i < coms.Length; i++)
            {
                if (coms[i] != null)
                {
                    coms[i].OnDrag(_mainCamera, hit);
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