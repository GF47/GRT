namespace GRT.Updater
{
    using System;

    public enum UpdateType { PerFrame, PerFixedFrame, PerAfterFrame, PerCustomFrame }

    public interface IUpdateNode
    {
        UpdateType Type { get; }
        bool IsActive { get; set; }

        event Action<float> Updating;

        void Start();
        void Update(float delta);
        void Stop();

        void Clear();
    }
}
