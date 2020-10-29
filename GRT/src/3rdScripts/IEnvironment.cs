using System;

namespace GRT._3rdScripts
{
    public interface IEnvironment
    {
        event Action OnStart;
        void Start();
        void Update();
        void OnDestroy();
    }
}
