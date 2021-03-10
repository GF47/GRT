using System;

namespace GRT.Updater
{
    public abstract class NormalUpdateNode : IUpdateNode
    {
        public long ID { get; set; }

        public abstract UpdateType Type { get; }

        public bool IsUpdating
        {
            get { return isUpdating; }
            set
            {
                if (isUpdating != value)
                {
                    isUpdating = value;
                    if (isUpdating) { Start(); }
                    else { Stop(); }
                }
            }
        }

        protected bool isUpdating;

        public event Action<float> Updating;

        public NormalUpdateNode(Action<float> updating)
        {
            Updating = updating;
            ID = DateTime.Now.ToBinary();
        }

        public void Clear()
        {
            Stop();
            Updating = null;
        }

        public void Start()
        {
            MonoUpdater.Add(this);
            isUpdating = true;
        }

        public void Stop()
        {
            MonoUpdater.Remove(this);
            isUpdating = false;
        }

        public void Update(float delta)
        {
            Updating?.Invoke(delta);
        }
    }
}