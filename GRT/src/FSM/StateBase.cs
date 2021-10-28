using System;
using System.Collections.Generic;

namespace GRT.FSM
{
    public abstract class StateBase : IState
    {
        public Action Entering;
        public Action Updating;
        public Action Exiting;

        protected ICollection<ITransition> transitions;

        public int ID { get; protected set; }

        ICollection<ITransition> IState.Transitions => transitions;

        public StateBase(int id)
        {
            if (!Util.IsValid(id))
            {
                throw new ArgumentException($"请将 [ID] 设置为一个非 [{Util.NullStateID}] 的数值", "id");
            }
            ID = id;

            transitions = new List<ITransition>();
        }

        public virtual int GetNext()
        {
            foreach (var transition in transitions)
            {
                if (transition.OK)
                {
                    return transition.TargetID;
                }
            }
            return ID;
        }

        public virtual void OnEnter(int lastID) => Entering?.Invoke();

        public virtual void OnExit(int nextID) => Exiting?.Invoke();

        public virtual void Update() => Updating?.Invoke();

        public virtual void Reset()
        {
        }

        public void AddNext(ITransition transition) => transitions.Add(transition);

        public void RemoveNext(ITransition transition) => transitions.Remove(transition);
    }
}