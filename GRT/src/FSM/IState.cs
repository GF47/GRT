using System.Collections.Generic;
namespace GRT.FSM
{
    public interface IState : IResetable
    {
        int ID { get; }

        string Info { get; }

        ICollection<ITransition> Transitions { get; }

        ITransition GetNext();

        void OnEnter(int lastID);

        void OnExit(int nextID);

        void Update();

        void AddTransition(ITransition transition);

        void RemoveTransition(ITransition transition);
    }
}