using System;

namespace GRT.GUpdater
{
    public class CustomIntervalGUpdater : CommonGUpdater
    {
        private float _delta;
        private float _interval;

        public float Interval { get => _interval; set => _interval = Math.Max(0.02f, value); }

        public CustomIntervalGUpdater(float interval = 1f) => _interval = interval;

        public override void GStart()
        {
            base.GStart();
            ResetDelta();
        }

        public override void GTick(float delta)
        {
            _delta += delta;
            if (_delta > _interval)
            {
                base.GTick(_delta);
                _delta -= _interval;
            }
        }

        public void ResetDelta() => _delta = 0f;
    }
}