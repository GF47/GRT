using System;

namespace GRT.Updater
{
    public abstract class NormalUpdateNode : IUpdateNode
    {
        public abstract UpdateType Type { get; }

        public bool IsActive
        {
            get => isActive;
            set
            {
                if (isActive != value)
                {
                    if (value) { Start(); }
                    else { Stop(); }
                }
            }
        }

        protected bool isActive;

        public event Action<float> Updating;

        public NormalUpdateNode(Action<float> updating)
        {
            Updating = updating;
        }

        public virtual void Clear()
        {
            Stop();
            Updating = null;
        }

        public virtual void Start()
        {
            isActive = MonoUpdater.Add(this);
        }

        public virtual void Stop()
        {
            isActive = MonoUpdater.Remove(this);
        }

        public virtual void Update(float delta)
        {
            Updating?.Invoke(delta);
        }
    }
}
