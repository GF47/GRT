using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRT.FSM
{
    public abstract class StateBase : IState
    {
        public int ID => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public void AddNext(ITransition transition, int targetID)
        {
            throw new NotImplementedException();
        }

        public int GetNext()
        {
            throw new NotImplementedException();
        }

        public void OnEnter(int lastID)
        {
            throw new NotImplementedException();
        }

        public void OnExit(int nextID)
        {
            throw new NotImplementedException();
        }

        public void RemoveNext(int targetID)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
