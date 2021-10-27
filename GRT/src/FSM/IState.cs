using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRT.FSM
{
    public interface IState
    {
        int ID { get; }
        string Name { get; }

        int GetNext();

        void OnEnter(int lastID);
        void OnExit(int nextID);

        void Update();

        void Reset();

        void AddNext(ITransition transition, int targetID);
        void RemoveNext(int targetID);
    }
}
