#define USE_ANY_STATE
#undef USE_ANY_STATE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRT.FSM
{
    public class FiniteStateMachine : IState
    {
        private readonly Dictionary<int, IState> _states;
        private int _currentID;
        private IState _currentState;

#if USE_ANY_STATE
        private int _anyID;
        private IState _anyState;
#endif

        public int ID { get; set; }
        public string Name { get; set; }

        public FiniteStateMachine()
        {
            _states = new Dictionary<int, IState>();

#if USE_ANY_STATE
            _anyID = Util.NullStateID;
#endif
        }

        public void StartWith(int id)
        {
            if (_states.TryGetValue(id ,out var state))
            {
                _currentID = id;
                _currentState = state;

                _currentState.OnEnter(id);
            }
        }

#if USE_ANY_STATE
        public void SetAnyStateID(int id)
        {
            if (_states.TryGetValue(id,out var state))
            {
                _anyID = id;
                _anyState = state;
            }
        }
#endif

        public void Add(IState state) => _states.Add(state.ID, state);
        public void Remove(int id) => _states.Remove(id);
        public void Remove(IState state) => _states.Remove(state.ID);

        int IState.GetNext()
        {
            throw new NotImplementedException();
        }

        void IState.OnEnter(int lastID)
        {
            throw new NotImplementedException();
        }

        void IState.OnExit(int nextID)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            _currentState.Update();

            var nextID = _currentState.GetNext();

#if USE_ANY_STATE
            if (Util.IsValid(_anyID))
            {
                var tempAnyID = _anyState.GetNext();
                if (_anyID != tempAnyID)
                {
                    nextID = tempAnyID;
                    _anyState.OnExit(nextID);
                    _anyState.Reset();
                }
            }
#endif

            if (_currentID != nextID)
            {
                if (_states.TryGetValue(nextID, out var state))
                {
                    _currentState.OnExit(nextID);
                    _currentState.Reset();

                    var lastID = _currentID;

                    _currentID = nextID;
                    _currentState = state;

                    _currentState.OnEnter(lastID);
                }
            }
        }

        void IState.AddNext(ITransition transition, int targetID)
        {
            throw new NotImplementedException();
        }

        void IState.RemoveNext(int targetID)
        {
            throw new NotImplementedException();
        }

        void IState.Reset()
        {
            throw new NotImplementedException();
        }
    }
}
