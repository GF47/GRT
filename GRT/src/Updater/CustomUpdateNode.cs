using System;

namespace GRT.Updater
{
    public class CustomUpdateNode : IUpdateNode
    {
        public UpdateType Type => UpdateType.PerCustomFrame;

        public bool IsActive
        {
            get => isAlive;
            set
            {
                if (isAlive != value)
                {
                    if (value) { Start(); }
                    else { Stop(); }
                }
            }
        }

        protected bool isAlive;

        public float Duration { get; set; }
        private float _delta;

        public event Action<float> Updating;

        public CustomUpdateNode(Action<float> updating, float duration = 1f)
        {
            Updating = updating;
            Duration = duration < 0.02f ? 1f : duration;
        }

        public virtual void Clear()
        {
            Stop();
            Updating = null;
        }

        public virtual void Start()
        {
            isAlive = MonoUpdater.Add(this);
        }

        public virtual void Stop()
        {
            isAlive = MonoUpdater.Remove(this);
        }

        public virtual void Update(float delta)
        {
            _delta += delta;
            if (_delta >= Duration)
            {
                Updating?.Invoke(_delta);
                _delta = 0f;
            }
        }
    }
}
