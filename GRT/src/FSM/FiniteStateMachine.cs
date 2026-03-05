#define USE_ANY_STATE
#undef USE_ANY_STATE

using System;
using System.Collections.Generic;

namespace GRT.FSM
{
    public class FiniteStateMachine : BaseState, IState
    {
        public Action<int, int> Transiting;

        public int EntryStateID { get; set; } = Util.EntryStateID;

        public int ExitStateID { get; set; } = Util.ExitStateID;

        private IState _currentState;

        public IState CurrentOrSelf => IsExiting ? this : _currentState;

        public IState CurrentOrEntry => IsUpdating ? _currentState : States[EntryStateID];

        public bool IsExiting => _currentState.ID == ExitStateID;

        public bool IsUpdating => _currentState != this;

        public Dictionary<int, IState> States { get; private set; }

        public FiniteStateMachine(int id = 0, string info = "") : base(id, info)
        {
            States = new Dictionary<int, IState>();

            _currentState = this;
        }

        public void Add(IState state) => States.Add(state.ID, state);

        public void Remove(int id) => States.Remove(id);

        public void Remove(IState state) => States.Remove(state.ID);

        public void Start()
        {
            (this as IState).OnEnter(Util.EntryStateID);
        }

        /// <summary>
        /// Are you sure?
        /// </summary>
        public void ___SetCurrentState(int targetID, bool asTransiting = false, Action<int, int> setting = null)
        {
            var isUpdating = IsUpdating;

            if (!isUpdating || _currentState.ID != targetID)
            {
                if (States.TryGetValue(targetID, out var state))
                {
                    if (isUpdating)
                    {
                        _currentState.OnExit(targetID);
                        _currentState.Reset();
                    }

                    var lastID = _currentState.ID;

                    _currentState = state;
                    _currentState.OnEnter(lastID);

                    if (asTransiting)
                    {
                        Transiting?.Invoke(lastID, _currentState.ID);
                    }

                    setting?.Invoke(lastID, _currentState.ID);
                }
                else
                {
                    throw new ArgumentException($"id {targetID} is not included in the fsm", nameof(targetID));
                }
            }
        }

        #region IState

        public Action<int> Entering;
        public Action<int> Exiting;
        public Action<int> Updating;

        ITransition IState.GetNext()
        {
            if (IsExiting)
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
            if (IsUpdating)
            {
                _currentState.Update();

                if (IsExiting)
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

                        var lastID = _currentState.ID;

                        _currentState = state;
                        _currentState.OnEnter(lastID);

                        transition.Go();
                        Transiting?.Invoke(lastID, _currentState.ID);
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
                UnityEngine.Debug.LogWarning($"fsm {id} entry id is itself");
#endif
            }
            else if (States.TryGetValue(EntryStateID, out var state))
            {
                _currentState = state;
                _currentState.OnEnter(id);
            }
            else
            {
                throw new Exception($"state {EntryStateID} is not included in the fsm {id}");
            }
        }

        public override void OnExit(int nextID)
        {
            Exiting?.Invoke(nextID);
            if (IsUpdating && IsExiting)
            {
                _currentState.OnExit(id);
                _currentState.Reset();
            }
        }

        public override void Reset()
        {
            _currentState = this;
            foreach (var pair in States)
            {
                pair.Value.Reset();
            }
        }

        #endregion IState
    }
}