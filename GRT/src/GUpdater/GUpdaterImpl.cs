using System;

namespace GRT.GUpdater
{
    public class PerFrameGUpdater : BaseGUpdater
    {
        public override UpdateMode UpdateMode => UpdateMode.PerFrame;

        public PerFrameGUpdater(Action<float> updating) : base(updating)
        {
        }
    }

    public class PerFixedFrameGUpdater : BaseGUpdater
    {
        public override UpdateMode UpdateMode => UpdateMode.PerFixedFrame;

        public PerFixedFrameGUpdater(Action<float> updating) : base(updating)
        {
        }
    }

    public class PerAfterFrameGUpdater : BaseGUpdater
    {
        public override UpdateMode UpdateMode => UpdateMode.PerAfterFrame;

        public PerAfterFrameGUpdater(Action<float> updating) : base(updating)
        {
        }
    }

    public class CustomIntervalGUpdater : BaseGUpdater
    {
        private float _delta;
        private float _interval;

        public float Interval { get => _interval; set => _interval = Math.Max(0.02f, value); }

        public override UpdateMode UpdateMode => UpdateMode.CustomInterval;

        public CustomIntervalGUpdater(Action<float> updating, float delta = 1f) : base(updating) => Interval = delta;

        public override void Start()
        {
            base.Start();
            _delta = 0f;
        }

        public override void Update(float delta)
        {
            _delta += delta;

            if (_delta > _interval)
            {
                base.Update(_delta);
                _delta -= _interval;
            }
        }

        public void ResetDelta() => _delta = 0f;
    }
}