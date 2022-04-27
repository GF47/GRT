#define USE_ANY_STATE
#undef USE_ANY_STATE

using System;
using System.Collections.Generic;

namespace GRT.FSM
{
    public class FiniteStateMachine : BaseState, IState
    {
        private readonly Dictionary<int, IState> _states;

        private IState _currentState;
        internal IState CurrentState => _currentState.ID == ExitStateID ? this : _currentState;

        public int EntryStateID { get; set; } = Util.EntryStateID;
        public int ExitStateID { get; set; } = Util.ExitStateID;

        public IBlackBoard Variables { get; }

        public FiniteStateMachine(int id = 0, string info = "") : base(id, info)
        {
            _states = new Dictionary<int, IState>();

            _currentState = this;

            Variables = new BlackBoard();
        }

        #region IState

        int IState.GetNext()
        {
            if (_currentState.ID == ExitStateID)
            {
                foreach (var transition in transitions)
                {
                    if (transition.OK)
                    {
                        return transition.TargetID;
                    }
                }
            }
            return id;
        }

        public override void Update()
        {
            if (_currentState.ID != id)
            {
                _currentState.Update();

                if (_currentState.ID == ExitStateID)
                {
                    return;
                }

                var next = _currentState.GetNext();
                if (_currentState.ID != next)
                {
                    if (_states.TryGetValue(next, out var state))
                    {
                        _currentState.OnExit(next);
                        _currentState.Reset();

                        var temp = _currentState.ID;

                        _currentState = state;
                        _currentState.OnEnter(temp);
                    }
                }
            }
        }

        public override void OnEnter(int lastID)
        {
            if (EntryStateID == id)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Log($"fsm {id} enter id is itself");
#endif
            }
            else if (_states.TryGetValue(EntryStateID, out var state))
            {
                _currentState = state;
                _currentState.OnEnter(id);
            }
            else
            {
                throw new Exception("EntryStateID is not included in the fsm");
            }
        }

        public override void OnExit(int nextID)
        {
            if (_currentState.ID == ExitStateID)
            {
                _currentState.OnExit(id);
                _currentState.Reset();
            }
        }

        public override void Reset()
        {
            _currentState = this;
        }

        #endregion IState

        #region FSM

        public void Add(IState state) => _states.Add(state.ID, state);

        public void Remove(int id) => _states.Remove(id);

        public void Remove(IState state) => _states.Remove(state.ID);

        public void Start()
        {
            (this as IState).OnEnter(Util.EntryStateID);
        }

        #endregion FSM

        #region Variables

        public string GetStringVariable(string varName)
        {
            return Variables.Get(varName, string.Empty);
        }

        public int GetIntVariable(string varName)
        {
            return Variables.Get(varName, 0);
        }

        public float GetFloatVariable(string varName)
        {
            return Variables.Get(varName, 0f);
        }

        public double GetDoubleVariable(string varName)
        {
            return Variables.Get(varName, 0d);
        }

        #endregion Variables
    }
}