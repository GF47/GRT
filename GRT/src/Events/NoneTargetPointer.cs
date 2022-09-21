using UnityEngine;

namespace GRT.Events
{
    public abstract class NoneTargetPointer : IPointer
    {
        public GnityEvent PointerDowning { get; set; }
        public GnityEvent PointerUpping { get; set; }
        public GnityEvent PointerClicking { get; set; }
        public GnityEvent PointerDoubleClicking { get; set; }
        public GnityEvent PointerDragStarting { get; set; }
        public GnityEvent PointerDragging { get; set; }
        public GnityEvent PointerDragStopping { get; set; }

        private bool _dragging;
        private float _draggingTimeStamp;
        private float _doubleClickTimeStamp;

        public abstract bool Downing { get; }
        public abstract bool Upping { get; }
        public abstract bool Holding { get; }

        public void Case(GEventSystem system, bool _, RaycastHit hit)
        {
            var camera = system.Camera;
            var pos = system.PointerPosition;

            if (Downing)
            {
                _draggingTimeStamp = Time.time;
                PointerDowning?.Invoke(camera, hit, pos);
            }
            else if (Upping)
            {
                PointerUpping?.Invoke(camera, hit, pos);

                if (_dragging)
                {
                    PointerDragStopping?.Invoke(camera, hit, pos);
                }
                else
                {
                    PointerClicking?.Invoke(camera, hit, pos);

                    if (Time.time - _doubleClickTimeStamp < system.doubleClickThreshod)
                    {
                        PointerDoubleClicking?.Invoke(camera, hit, pos);
                    }

                    _doubleClickTimeStamp = Time.time;
                }

                _draggingTimeStamp = float.PositiveInfinity;
                _dragging = false;
            }
            else if (Holding)
            {
                if (Time.time - _draggingTimeStamp >= system.dragThreshold)
                {
                    if (_dragging)
                    {
                        PointerDragging?.Invoke(camera, hit, pos);
                    }
                    else
                    {
                        _dragging = true;
                        PointerDragStarting?.Invoke(camera, hit, pos);
                    }
                }
            }
        }

        public void Reset(GEventSystem system)
        {
            _draggingTimeStamp = float.PositiveInfinity;
            _doubleClickTimeStamp = float.NegativeInfinity;

            if (Holding)
            {
                var camera = system.Camera;
                var pos = system.PointerPosition;

                PointerUpping?.Invoke(camera, default, pos);
                if (_dragging)
                {
                    PointerDragStopping?.Invoke(camera, default, pos);
                }
            }

            _dragging = false;
        }
    }
}