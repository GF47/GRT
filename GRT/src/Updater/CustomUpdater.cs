using System;

namespace GRT.Updater
{
    public class CustomUpdater : IUpdater
    {
        private bool _isAlive;
        private float _duration = 1f;
        private float _delta;

        public UpdateType Type => UpdateType.CustomFrame;

        public bool IsAlive => _isAlive;
        public float Duration { get => _duration; set => _duration = Math.Max(0.02f, value); }

        public event Action<float> Updating;
        public event Action Starting;
        public event Action Stopping;
        public event Action StartingOneShot;
        public event Action StoppingOneShot;

        public CustomUpdater(Action<float> updating, float duration = 1f)
        {
            Updating = updating;
            Duration = duration;
        }

        public void Start()
        {
            if (_isAlive) { return; }

            UpdateDriver.Add(this);
            _isAlive = true;

            Starting?.Invoke();
            StartingOneShot?.Invoke();
            StartingOneShot = null;
        }

        public void Stop()
        {
            if (!_isAlive) { return; }

            UpdateDriver.Remove(this);
            _isAlive = false;

            Stopping?.Invoke();
            StoppingOneShot?.Invoke();
            StoppingOneShot = null;
        }

        void IUpdater.Update(float delta)
        {
            _delta += delta;

            if (_delta >= _duration)
            {
                Updating?.Invoke(_delta);
                _delta -= _duration;
            }
        }
    }
}