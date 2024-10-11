using System.Collections.Generic;

namespace GRT.FSM
{
    public interface IResetable
    {
        void Reset();
    }

    public interface IActionEnumerable
    {
        IEnumerable<IAction> AEnumerable { get; }
    }

    public interface IAction : IResetable
    {
        bool Completed { get; }
        void Start();
        void Invoke();
    }
}
