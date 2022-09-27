﻿using GRT.Events;
using System;
using UnityEngine;

namespace GRT.GInventory.Triggers
{
    public class KeyPointerTrigger : ITrigger
    {
        public event Action<IOwner, IInventory, IStack> Triggering;

        public KeyCode Key => _pointer.key;

        public float HoldTime = float.NegativeInfinity;
        public int ClickCount = 1;

        private float _holdTimeStamp;
        private bool _enabled;

        private readonly KeyNoneTargetPointer _pointer;

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

        public Func<(IOwner, IInventory, IStack)> GetContext { get; set; }

        public KeyPointerTrigger(KeyCode key, float holdTime = float.NegativeInfinity, int clickCount = 1)
        {
            HoldTime = holdTime;
            ClickCount = clickCount;

            _pointer = new KeyNoneTargetPointer { key = key };
            if (HoldTime > 0f)
            {
                _pointer.PointerDragStarting = new GnityEvent();
                _pointer.PointerDragStarting.AddListener((c, h, p) => _holdTimeStamp = Time.time);
                _pointer.PointerDragStopping = new GnityEvent();
                _pointer.PointerDragStopping.AddListener((c, h, p) =>
                {
                    if (_holdTimeStamp + HoldTime <= Time.time)
                    {
                        var (owner, inventory, stack) = GetContext();
                        Triggering?.Invoke(owner, inventory, stack);
                    }
                    _holdTimeStamp = float.NegativeInfinity;
                });
            }
            else if (ClickCount == 2)
            {
                _pointer.PointerDoubleClicking = new GnityEvent();
                _pointer.PointerDoubleClicking.AddListener((c, h, p) =>
                {
                    var (owner, inventory, stack) = GetContext();
                    Triggering?.Invoke(owner, inventory, stack);
                });
            }
            else
            {
                _pointer.PointerClicking = new GnityEvent();
                _pointer.PointerClicking.AddListener((c, h, p) =>
                {
                    var (owner, inventory, stack) = GetContext();
                    Triggering?.Invoke(owner, inventory, stack);
                });
            }
        }

        public override string ToString()
        {
            var verb =
                HoldTime > 0 ? "按住" :
                ClickCount == 2 ? "双击" :
                "单击";

            var attribute = HoldTime > 0 ? $"{HoldTime}秒" : string.Empty;

            return $"{verb} {Key} {attribute}";
        }
    }
}