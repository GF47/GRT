using UnityEngine;

namespace GRT.Events
{
    /// <summary>
    /// 使用了鼠标位置和默认射线检测结果
    /// 如果需要自定义位置（比如屏幕正中央等），可以在Case方法内部自行进行射线检测
    /// </summary>
    public abstract class BasePointer : IPointer
    {
        private Collider _collider;
        private bool _dragging;
        private float _draggingTimeStamp;
        private float _doubleClickTimeStamp;

        public abstract bool Downing { get; }
        public abstract bool Upping { get; }
        public abstract bool Holding { get; }

        public void Case(GEventSystem system, bool cased, RaycastHit hit)
        {
            var camera = system.Camera;
            var pos = system.PointerPosition;

            if (cased)
            {
                var collider = hit.collider;

                if (Downing)
                {
                    _draggingTimeStamp = Time.time;

                    _collider = collider;

                    GEventSystem.SendPointerDownEvent(_collider.gameObject, IsInterestedIn, camera, hit, pos);
                }
            }

            if (Upping)
            {
                if (_collider != null)
                {
                    GEventSystem.SendPointerUpEvent(_collider.gameObject, IsInterestedIn, camera, hit, pos);

                    if (_dragging)
                    {
                        GEventSystem.SendPointerDragStopEvent(_collider.gameObject, IsInterestedIn, camera, hit, pos);
                    }
                    else
                    {
                        GEventSystem.SendPointerClickEvent(_collider.gameObject, IsInterestedIn, camera, hit, pos);

                        if (Time.time - _doubleClickTimeStamp < system.doubleClickThreshod)
                        {
                            GEventSystem.SendPointerDoubleClickEvent(_collider.gameObject, IsInterestedIn, camera, hit, pos);
                        }

                        _doubleClickTimeStamp = Time.time;
                    }
                }

                _draggingTimeStamp = float.PositiveInfinity;
                _collider = null;
                _dragging = false;
            }
            else if (Holding)
            {
                if (_collider != null)
                {
                    if (Time.time - _draggingTimeStamp >= system.dragThreshold)
                    {
                        if (_dragging)
                        {
                            GEventSystem.SendPointerDragEvent(_collider.gameObject, IsInterestedIn, camera, hit, pos);
                        }
                        else
                        {
                            _dragging = true;
                            GEventSystem.SendPointerDragStartEvent(_collider.gameObject, IsInterestedIn, camera, hit, pos);
                        }
                    }
                }
            }
        }

        public void Reset(GEventSystem system)
        {
            _draggingTimeStamp = float.PositiveInfinity;
            _doubleClickTimeStamp = float.NegativeInfinity;

            if (_collider != null)
            {
                var camera = system.Camera;
                var pos = system.PointerPosition;

                GEventSystem.SendPointerUpEvent(_collider.gameObject, IsInterestedIn, camera, default, pos);
                if (_dragging)
                {
                    GEventSystem.SendPointerDragStopEvent(_collider.gameObject, IsInterestedIn, camera, default, pos);
                }
            }

            _collider = null;
            _dragging = false;
        }

        protected abstract bool IsInterestedIn(Component com);
    }
}