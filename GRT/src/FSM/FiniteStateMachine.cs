#define USE_ANY_STATE
#undef USE_ANY_STATE

using System;
using System.Collections.Generic;

namespace GRT.FSM
{
    public class FiniteStateMachine : BaseState, IState
    {
        public Action<int> Entering;
        public Action<int> Exiting;
        public Action<int> Updating;
        public Action<int, int> Transiting;

        private IState _currentState;
        public IState CurrentState => _currentState.ID == ExitStateID ? this : _currentState;

        public int EntryStateID { get; set; } = Util.EntryStateID;
        public int ExitStateID { get; set; } = Util.ExitStateID;

        public IBlackBoard Variables { get; }

        public Dictionary<int, IState> States { get; private set; }

        public FiniteStateMachine(int id = 0, string info = "") : base(id, info)
        {
            States = new Dictionary<int, IState>();

            _currentState = this;

            Variables = new BlackBoard();
        }

        #region IState

        ITransition IState.GetNext()
        {
            if (_currentState.ID == ExitStateID)
            {
                foreach (var transition in transitions)
                {
                    if (transition.OK)
                    {
                        return transition;
                    }
                }
            }
            return null;
        }

        public override void Update()
        {
            Updating?.Invoke(id);
            if (_currentState != this)
            {
                _currentState.Update();

                if (_currentState.ID == ExitStateID)
                {
                    return;
                }

                var transition = _currentState.GetNext();
                if (transition != null && _currentState.ID != transition.TargetID)
                {
                    if (States.TryGetValue(transition.TargetID, out var state))
                    {
                        _currentState.OnExit(transition.TargetID);
                        _currentState.Reset();

                        var temp = _currentState.ID;

                        _currentState = state;
                        _currentState.OnEnter(temp);

                        transition.Go();
                        Transiting?.Invoke(temp, state.ID);
                    }
                }
            }
        }

        public override void OnEnter(int lastID)
        {
            Entering?.Invoke(lastID);
            if (EntryStateID == id)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Log($"fsm {id} enter id is itself");
#endif
            }
            else if (States.TryGetValue(EntryStateID, out var state))
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
            Exiting?.Invoke(nextID);
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

        public void Add(IState state) => States.Add(state.ID, state);

        public void Remove(int id) => States.Remove(id);

        public void Remove(IState state) => States.Remove(state.ID);

        public void Start()
        {
            (this as IState).OnEnter(Util.EntryStateID);
        }

        public bool IsExiting => _currentState.ID == ExitStateID;
        public bool IsUpdating => _currentState != this;

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