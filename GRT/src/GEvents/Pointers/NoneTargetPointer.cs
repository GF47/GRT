using UnityEngine;

namespace GRT.GEvents.Pointers
{
    public abstract class NoneTargetPointer<T> : IGPointer<T>
    {
        public GnityEvent<T> PointerDowning { get; set; }
        public GnityEvent<T> PointerUpping { get; set; }
        public GnityEvent<T> PointerClicking { get; set; }
        public GnityEvent<T> PointerDoubleClicking { get; set; }
        public GnityEvent<T> PointerDragStarting { get; set; }
        public GnityEvent<T> PointerDragging { get; set; }
        public GnityEvent<T> PointerDragStopping { get; set; }

        private bool _dragging;
        private float _draggingTimeStamp;
        private float _doubleClickTimeStamp;

        public abstract bool Downing { get; }
        public abstract bool Upping { get; }
        public abstract bool Holding { get; }

        public void Cast(GEventDriver<T> driver, bool _, RaycastHit hit)
        {
            var raycaster = driver.Raycaster;

            if (Downing)
            {
                _draggingTimeStamp = Time.time;
                PointerDowning?.Invoke(raycaster, hit);
            }
            else if (Upping)
            {
                PointerUpping?.Invoke(raycaster, hit);

                if (_dragging)
                {
                    PointerDragStopping?.Invoke(raycaster, hit);
                }
                else
                {
                    PointerClicking?.Invoke(raycaster, hit);

                    if (Time.time - _doubleClickTimeStamp < driver.doubleClickThreshold)
                    {
                        PointerDoubleClicking?.Invoke(raycaster, hit);
                    }

                    _doubleClickTimeStamp = Time.time;
                }

                _draggingTimeStamp = float.PositiveInfinity;
                _dragging = false;
            }
            else if (Holding)
            {
                if (Time.time - _draggingTimeStamp >= driver.dragThreshold)
                {
                    if (_dragging)
                    {
                        PointerDragging?.Invoke(raycaster, hit);
                    }
                    else
                    {
                        _dragging = true;
                        PointerDragStarting?.Invoke(raycaster, hit);
                    }
                }
            }
        }

        public void Reset(GEventDriver<T> driver)
        {
            _draggingTimeStamp = float.PositiveInfinity;
            _doubleClickTimeStamp = float.NegativeInfinity;

            var raycaster = driver.Raycaster;

            if (Holding)
            {
                PointerUpping?.Invoke(raycaster, default);
                if (_dragging)
                {
                    PointerDragStopping?.Invoke(raycaster, default);
                }
            }

            _dragging = false;
        }
    }
}