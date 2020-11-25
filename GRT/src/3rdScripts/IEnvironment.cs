using System;

namespace GRT._3rdScripts
{
    public interface IEnvironment<T>
    {
        event Action<T> OnStart;
        bool CanUpdate { get; set; }
        void Start();
        void Update();
        void OnDestroy();
    }
}
