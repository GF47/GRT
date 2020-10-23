namespace GRT.Updater
{
    using System;

    public enum UpdateType { PerFrame, PerFixedFrame, PerAfterFrame, PerCustomFrame }

    public interface IUpdateNode
    {
        long ID { get; }
        UpdateType Type { get; }
        bool IsUpdating { get; set; }
        event Action<float> OnUpdate;

        void Start();
        void Update(float delta);
        void Stop();

        void Clear();
    }
}
