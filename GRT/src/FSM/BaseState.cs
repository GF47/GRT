using System;
using System.Collections.Generic;

namespace GRT.FSM
{
    public abstract class BaseState<T> : IState<T>
    {
        public ExecuteHandler onEnterAction;
        public ExecuteHandler onUpdateAction;
        public ExecuteHandler onExitAction;

        private readonly int _id;
        private readonly SortedList<T, KeyValuePair<int, ExecuteHandler>> _nextStates;
        private int _nextStateID;

        public int ID { get { return _id; } }

        public virtual bool CanExitSafely { get { return true; } protected set { } }

        protected BaseState(int id)
        {
            if (!FSMUtility.IsLogicalStateID(id))
            {
                throw new ArgumentException(string.Format("请将 [ID] 设置为一个非 [{0}] 的数值", FSMUtility.NullStateID), "id");
            }
            _id = id;
            _nextStates = new SortedList<T, KeyValuePair<int, ExecuteHandler>>();
            _nextStateID = _id;
        }

        public virtual void GetInput(T input)
        {
            if (CanExitSafely)
            {
                if (_nextStates.ContainsKey(input))
                {
                    _nextStateID = _nextStates[input].Key;
                }
            }
        }

        public int GetNextStateID()
        {
            return _nextStateID;
        }

        public virtual void OnEnter(int lastID) { onEnterAction?.Invoke(); }

        public virtual void Update() { onUpdateAction?.Invoke(); }

        public virtual void OnExit(int nextID)
        {
            onExitAction?.Invoke();
            foreach (var pair in _nextStates)
            {
                if (pair.Value.Key == nextID)
                {
                    pair.Value.Value?.Invoke();
                }
            }
        }

        public virtual void Reset()
        {
            CanExitSafely = true;
            _nextStateID = _id;
        }

        public void AddNextState(T input, int stateID, ExecuteHandler action)
        {
            _nextStates.Add(input, new KeyValuePair<int, ExecuteHandler>(stateID, action));
        }

        public void AddNextState(T input, int stateID)
        {
            _nextStates.Add(input, new KeyValuePair<int, ExecuteHandler>(stateID, null));
        }

        public void RemoveNextState(T input)
        {
            _nextStates.Remove(input);
        }
    }
}
