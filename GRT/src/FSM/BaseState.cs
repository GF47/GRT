using System;
using System.Collections.Generic;

namespace GRT.FSM
{
    public abstract class BaseState<T> : IState<T>
    {
        public Action Entering;
        public Action Updating;
        public Action Exiting;

        protected readonly int id;
        protected int nextStateID;

        protected readonly SortedList<T, KeyValuePair<int, Action>> nextStates;

        public int ID { get { return id; } }

        public virtual bool CanExitSafely { get => true; protected set { } }

        protected BaseState(int id)
        {
            if (!FSMUtility.Validated(id))
            {
                throw new ArgumentException($"请将 [ID] 设置为一个非 [{FSMUtility.NullStateID}] 的数值", "id");
            }
            this.id = id;
            nextStates = new SortedList<T, KeyValuePair<int, Action>>();
            nextStateID = this.id;
        }

        public virtual void GetInput(T input)
        {
            if (CanExitSafely)
            {
                if (nextStates.ContainsKey(input))
                {
                    nextStateID = nextStates[input].Key;
                }
            }
        }

        public int GetNextStateID()
        {
            return nextStateID;
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
            foreach (var pair in nextStates)
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
            nextStateID = id;
        }

        public void AddNextState(T input, int stateID, Action action)
        {
            nextStates.Add(input, new KeyValuePair<int, Action>(stateID, action));
        }

        public void AddNextState(T input, int stateID)
        {
            nextStates.Add(input, new KeyValuePair<int, Action>(stateID, null));
        }

        public void RemoveNextState(T input)
        {
            nextStates.Remove(input);
        }
    }
}