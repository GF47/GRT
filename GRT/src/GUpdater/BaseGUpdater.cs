using System;

namespace GRT.GUpdater
{
    public abstract class BaseGUpdater : IGUpdater
    {
        public event Action<float> Updating;

        public event Action Starting;

        public event Action StartingOneShot;

        public event Action Stopping;

        public event Action StoppingOneShot;

        public abstract UpdateMode UpdateMode { get; }

        public bool IsAlive { get; protected set; }

        protected BaseGUpdater(Action<float> updating) => Updating = updating;

        public virtual void Start()
        {
            if (IsAlive) { return; }

            GUpdaterDriver.Add(this);
            IsAlive = true;

            Starting?.Invoke();
            StartingOneShot?.Invoke();
            StartingOneShot = null;
        }

        public virtual void Stop()
        {
            if (!IsAlive) { return; }

            GUpdaterDriver.Remove(this);
            IsAlive = false;

            Stopping?.Invoke();
            StoppingOneShot?.Invoke();
            StoppingOneShot = null;
        }

        public virtual void Update(float delta) => Updating?.Invoke(delta);
    }
}