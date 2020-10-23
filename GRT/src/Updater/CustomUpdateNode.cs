using System;

namespace GRT.Updater
{
    public class CustomUpdateNode : IUpdateNode
    {
        public long ID { get; protected set; }

        public UpdateType Type => UpdateType.PerCustomFrame;

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

        public float Duration { get; set; }
        private float _delta;

        public event Action<float> OnUpdate;

        public CustomUpdateNode(Action<float> callback) : this(callback, 1f) { }
        public CustomUpdateNode(Action<float> callback, float duration)
        {
            OnUpdate = callback;
            Duration = duration < 0.02f ? 1f : duration;
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
            _delta += delta;
            if (_delta > Duration)
            {
                OnUpdate?.Invoke(_delta);
            }
            _delta = 0f;
        }
    }
}
