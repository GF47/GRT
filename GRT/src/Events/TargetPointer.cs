using UnityEngine;

namespace GRT.Events
{
    /// <summary>
    /// 使用了鼠标位置和默认射线检测结果
    /// 如果需要自定义位置（比如屏幕正中央等），可以在Case方法内部自行进行射线检测
    /// </summary>
    public abstract class TargetPointer : IPointer
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

            //  |            | down  | hold  | up    | free  |                    |
            //  +------------+-------+-------+-------+-------+--------------------+
            //  | casted     | --2-- | PHOLD | PUP   | --1-- | cached collider    |
            //  | no casted  | --2-- | PHOLD | PUP   | --1-- |                    |
            //  +------------+-------+-------+-------+-------+--------------------+
            //  | casted     | PDOWN | KEEP  | KEEP  | --1-- | no cached collider |
            //  | no casted  | KEEP  | KEEP  | KEEP  | --1-- |                    |
            //
            //  1: 不可能达到, free状态, collider一定是null
            //  2: 不可能达到, 进入down状态时, collider一定是null, 因为上一次的up状态会把collider置为null
            //  PDOWN: pointer down
            //  PHOLD: pointer hold, drag or drag start
            //  PUP  : pointer up, drag stop or click or double click
            //  KEEP : 无操作

            if (Downing)
            {
                if (cased)
                {
                    _draggingTimeStamp = Time.time;

                    _collider = hit.collider;

                    GEventSystem.SendPointerDownEvent(_collider.gameObject, IsInterestedIn, camera, hit, pos);
                }
            }
            else if (Upping)
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