namespace GRT.Updater
{
    using System;

    public enum UpdateType { PerFrame, PerFixedFrame, PerAfterFrame, PerCustomFrame }

    public interface IUpdateNode
    {
        long ID { get; set; }
        UpdateType Type { get; }
        bool IsUpdating { get; set; }
        event Action<float> Updating;

        void Start();
        void Update(float delta);
        void Stop();

        void Clear();
    }
}
