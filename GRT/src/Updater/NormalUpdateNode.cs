using System;

namespace GRT.Updater
{
    public abstract class NormalUpdateNode : IUpdateNode
    {
        public long ID { get; protected set; }
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

        public event Action<float> OnUpdate;

        public NormalUpdateNode(Action<float> callback)
        {
            OnUpdate = callback;
            ID = DateTime.Now.ToBinary();
        }

        public void Clear()
        {
            Stop();
            OnUpdate = null;
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
            OnUpdate?.Invoke(delta);
        }
    }
}
