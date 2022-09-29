using GRT.Events;
using System;
using System.Collections;
using UnityEngine;

namespace GRT.GInventory.Triggers
{
    public class MouseButtonPointerTrigger : ITrigger
    {
        public event Action<IOwner, IStack> Triggering;

        public int MouseButton => _pointer.mouseButton;

        public float HoldTime = float.NegativeInfinity;
        public int ClickCount = 1;

        private float _holdTimeStamp;
        private bool _enabled;

        private readonly MouseButtonNoneTargetPointer _pointer;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                var pointers = GEventSystem.Current.Pointers;
                if (_enabled)
                {
                    pointers.Add(_pointer);
                }
                else
                {
                    pointers.Remove(_pointer);
                }
            }
        }

        public Func<(IOwner, IStack)> GetContext { get; set; }

        public MouseButtonPointerTrigger(int mouseButton = 0, float holdTime = float.NegativeInfinity, int clickCount = 1)
        {
            HoldTime = holdTime;
            ClickCount = clickCount;

            _pointer = new MouseButtonNoneTargetPointer { mouseButton = mouseButton };
            if (HoldTime > 0f)
            {
                _pointer.PointerDragStarting = new GnityEvent();
                _pointer.PointerDragStarting.AddListener((c, h, p) => _holdTimeStamp = Time.time);
                _pointer.PointerDragStopping = new GnityEvent();
                _pointer.PointerDragStopping.AddListener((c, h, p) =>
                {
                    if (_holdTimeStamp + HoldTime <= Time.time)
                    {
                        var (owner, stack) = GetContext();
                        Triggering?.Invoke(owner, stack);
                    }
                    _holdTimeStamp = float.NegativeInfinity;
                });
            }
            else if (ClickCount == 2)
            {
                _pointer.PointerDoubleClicking = new GnityEvent();
                _pointer.PointerDoubleClicking.AddListener((c, h, p) =>
                {
                    var (owner, stack) = GetContext();
                    Triggering?.Invoke(owner, stack);
                });
            }
            else
            {
                _pointer.PointerClicking = new GnityEvent();
                _pointer.PointerClicking.AddListener((c, h, p) =>
                {
                    var (owner, stack) = GetContext();
                    Triggering?.Invoke(owner, stack);
                });
            }
        }

        public override string ToString()
        {
            var verb =
                HoldTime > 0 ? "按住" :
                ClickCount == 2 ? "双击" :
                "单击";

            var button =
                MouseButton == 0 ? "鼠标左键" :
                MouseButton == 1 ? "鼠标右键" :
                MouseButton == 2 ? "鼠标中键" :
                MouseButton.ToString();

            var attribute = HoldTime > 0 ? $"{HoldTime}秒" : string.Empty;

            return $"{verb} {button} {attribute}";
        }
    }
}