using System;

namespace GRT.Updater
{
    public abstract class NormalUpdater : IUpdater
    {
        protected bool isAlive;

        public abstract UpdateType Type { get; }

        public bool IsAlive => isAlive;

        public event Action<float> Updating;
        public event Action Starting;
        public event Action Stopping;
        public event Action StartingOneShot;
        public event Action StoppingOneShot;

        public NormalUpdater(Action<float> updating)
        {
            Updating = updating;
        }

        public virtual void Start()
        {
            if (isAlive) { return; }

            UpdateDriver.Add(this);
            isAlive = true;

            Starting?.Invoke();
            StartingOneShot?.Invoke();
            StartingOneShot = null;
        }

        public virtual void Stop()
        {
            if (!isAlive) { return; }

            UpdateDriver.Remove(this);
            isAlive = false;

            Stopping?.Invoke();
            StoppingOneShot?.Invoke();
            StoppingOneShot = null;
        }

        void IUpdater.Update(float delta)
        {
            Updating?.Invoke(delta);
        }
    }
}