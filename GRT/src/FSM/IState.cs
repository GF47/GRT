using System.Collections.Generic;
namespace GRT.FSM
{
    public interface IState
    {
        int ID { get; }

        string Info { get; }

        ICollection<ITransition> Transitions { get; }

        int GetNext();

        void OnEnter(int lastID);

        void OnExit(int nextID);

        void Update();

        void Reset();

        void AddTransition(ITransition transition);

        void RemoveTransition(ITransition transition);
    }
}