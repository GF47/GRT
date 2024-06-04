using UnityEngine;

namespace GRT.GEvents.Pointers
{
    /// <summary>
    /// 使用了鼠标位置和默认射线检测结果
    /// 如果需要自定义位置（比如屏幕正中央等），可以在Case方法内部自行进行射线检测
    /// </summary>
    public abstract class TargetPointer<T> : IGPointer<T>
    {
        private Collider _collider;
        private bool _dragging;
        private float _draggingTimeStamp;
        private float _doubleClickTimeStamp;

        public abstract bool Downing { get; }
        public abstract bool Upping { get; }
        public abstract bool Holding { get; }

        public void Cast(GEventDriver<T> driver, bool casted, RaycastHit hit)
        {
            var raycaster = driver.Raycaster;

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
                if (casted)
                {
                    _draggingTimeStamp = Time.time;

                    _collider = hit.collider;

                    GEventDriver<T>.SendPointerDownEvent(_collider.gameObject, IsInterestedIn, raycaster, hit);
                }
            }
            else if (Upping)
            {
                if (_collider != null)
                {
                    GEventDriver<T>.SendPointerUpEvent(_collider.gameObject, IsInterestedIn, raycaster, hit);

                    if (_dragging)
                    {
                        GEventDriver<T>.SendPointerDragStopEvent(_collider.gameObject, IsInterestedIn, raycaster, hit);
                    }
                    else
                    {
                        GEventDriver<T>.SendPointerClickEvent(_collider.gameObject, IsInterestedIn, raycaster, hit);

                        if (Time.time - _doubleClickTimeStamp < driver.doubleClickThreshold)
                        {
                            GEventDriver<T>.SendPointerDoubleClickEvent(_collider.gameObject, IsInterestedIn, raycaster, hit);
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
                    if (Time.time - _draggingTimeStamp >= driver.dragThreshold)
                    {
                        if (_dragging)
                        {
                            GEventDriver<T>.SendPointerDragEvent(_collider.gameObject, IsInterestedIn, raycaster, hit);
                        }
                        else
                        {
                            _dragging = true;
                            GEventDriver<T>.SendPointerDragStartEvent(_collider.gameObject, IsInterestedIn, raycaster, hit);
                        }
                    }
                }
            }
        }

        public void Reset(GEventDriver<T> driver)
        {
            _draggingTimeStamp = float.PositiveInfinity;
            _doubleClickTimeStamp = float.NegativeInfinity;

            if (_collider != null)
            {
                var raycaster = driver.Raycaster;

                GEventDriver<T>.SendPointerUpEvent(_collider.gameObject, IsInterestedIn, raycaster, default);
                if (_dragging)
                {
                    GEventDriver<T>.SendPointerDragStopEvent(_collider.gameObject, IsInterestedIn, raycaster, default);
                }
            }

            _collider = null;
            _dragging = false;
        }

        protected abstract bool IsInterestedIn(Component com);
    }
}