namespace GRT.GUpdater
{
    public interface IGUpdater
    {
        UpdateMode UpdateMode { get; }

        bool IsAlive { get; }

        void Start();

        void Update(float delta);

        void Stop();
    }
}