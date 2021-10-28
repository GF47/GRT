#define USE_ANY_STATE
#undef USE_ANY_STATE

using System;
using System.Collections.Generic;

namespace GRT.FSM
{
    public class FiniteStateMachine
    {
        public Action Entering;
        public Action Updating;
        public Action Exiting;

        private readonly Dictionary<int, IState> _states;

        private int _currentID;
        private IState _currentState;

        public int CurrentID => _currentID;
        internal IState CurrentState => _currentState;

        public FiniteStateMachine()
        {
            _states = new Dictionary<int, IState>();
        }

        public void StartWith(int id)
        {
            if (_states.TryGetValue(id, out var state))
            {
                _currentID = id;
                _currentState = state;

                _currentState.OnEnter(id);
            }
        }

        public void Add(IState state) => _states.Add(state.ID, state);

        public void Remove(int id) => _states.Remove(id);

        public void Remove(IState state) => _states.Remove(state.ID);

        public void Update()
        {
            _currentState.Update();

            var id = _currentState.GetNext();

            if (_currentID != id)
            {
                if (_states.TryGetValue(id, out var state))
                {
                    _currentState.OnExit(id);
                    _currentState.Reset();

                    var temp = _currentID;

                    _currentID = id;
                    _currentState = state;

                    _currentState.OnEnter(temp);
                }
            }
        }
    }
}