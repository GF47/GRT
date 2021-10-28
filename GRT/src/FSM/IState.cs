using System.Collections.Generic;
namespace GRT.FSM
{
    public interface IState
    {
        int ID { get; }

        ICollection<ITransition> Transitions { get; }

        int GetNext();

        void OnEnter(int lastID);

        void OnExit(int nextID);

        void Update();

        void Reset();

        void AddNext(ITransition transition);

        void RemoveNext(ITransition transition);
    }
}