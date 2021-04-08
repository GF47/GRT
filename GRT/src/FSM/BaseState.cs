using System;
using System.Collections.Generic;

namespace GRT.FSM
{
    public abstract class BaseState<T> : IState<T>
    {
        public Action Entering;
        public Action Updating;
        public Action Exiting;

        private readonly int _id;
        private readonly SortedList<T, KeyValuePair<int, Action>> _nextStates;
        private int _nextStateID;

        public int ID { get { return _id; } }

        public virtual bool CanExitSafely { get => true; protected set { } }

        protected BaseState(int id)
        {
            if (!FSMUtility.Validated(id))
            {
                throw new ArgumentException($"请将 [ID] 设置为一个非 [{FSMUtility.NullStateID}] 的数值", "id");
            }
            _id = id;
            _nextStates = new SortedList<T, KeyValuePair<int, Action>>();
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

        public virtual void OnEnter(int lastID)
        {
            Entering?.Invoke();
        }

        public virtual void Update()
        {
            Updating?.Invoke();
        }

        public virtual void OnExit(int nextID)
        {
            Exiting?.Invoke();
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

        public void AddNextState(T input, int stateID, Action action)
        {
            _nextStates.Add(input, new KeyValuePair<int, Action>(stateID, action));
        }

        public void AddNextState(T input, int stateID)
        {
            _nextStates.Add(input, new KeyValuePair<int, Action>(stateID, null));
        }

        public void RemoveNextState(T input)
        {
            _nextStates.Remove(input);
        }
    }
}