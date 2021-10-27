#define USE_ANY_STATE

using System.Collections.Generic;

namespace GRT.FSM_Old
{
    public class FiniStateMachine<T>
    {
        private readonly SortedList<int, IState<T>> _states;

        private int _currentStateID;
        private IState<T> _currentState;

#if USE_ANY_STATE
        private int _anyStateID;
        private IState<T> _anyState;
#endif

        public FiniStateMachine()
        {
            _states = new SortedList<int, IState<T>>();

#if USE_ANY_STATE
            _anyStateID = FSMUtility.NullStateID;
#endif
        }

        public void StartWith(int id)
        {
            if (_states.ContainsKey(id))
            {
                _currentState = _states[id];
                _currentStateID = id;
                _currentState.OnEnter(id);
            }
        }

#if USE_ANY_STATE

        public void SetAnyStateID(int id)
        {
            if (_states.ContainsKey(id))
            {
                _anyState = _states[id];
                _anyStateID = id;
            }
        }

#endif

        public void Add(IState<T> state)
        {
            _states.Add(state.ID, state);
        }

        public void Remove(int stateID)
        {
            _states.Remove(stateID);
        }

        public void Remove(IState<T> state)
        {
            _states.Remove(state.ID);
        }

        public void Update()
        {
            _currentState.Update();

            int tempStateID = _currentState.GetNextStateID();

#if USE_ANY_STATE
            if (FSMUtility.Validated(_anyStateID))
            {
                int tempAnyStateID = _anyState.GetNextStateID();
                if (_anyStateID != tempAnyStateID)
                {
                    _anyState.Reset();
                    tempStateID = tempAnyStateID;
                }
            }
#endif

            if (_currentStateID != tempStateID)
            {
                if (_states.ContainsKey(tempStateID))
                {
                    _currentState.OnExit(tempStateID);
                    _currentState.Reset();
                    _currentState = _states[tempStateID];
                    int lastID = _currentStateID;
                    _currentStateID = tempStateID;
                    _currentState.OnEnter(lastID);
                }
            }
        }

        public void GetInput(T input)
        {
#if USE_ANY_STATE
            if (FSMUtility.Validated(_anyStateID))
            {
                _anyState.GetInput(input);
                if (_anyStateID != _anyState.GetNextStateID())
                {
                    return;
                }
            }

#endif
            _currentState.GetInput(input);
        }
    }
}