using System;
using System.Collections.Generic;

namespace GRT.FSM
{
    public abstract class BaseState : IState
    {
        protected int id;
        protected string info;
        protected ICollection<ITransition> transitions;

        public int ID => id;
        public string Info => info;
        ICollection<ITransition> IState.Transitions => transitions;

        public BaseState(int id, string info = "")
        {
            if (!Util.IsValid(id))
            {
                throw new ArgumentException($"请将 [ID] 设置为一个非 [{Util.NullStateID}] 的数值", "id");
            }
            this.id = id;
            this.info = info;

            transitions = new List<ITransition>();
        }

        int IState.GetNext()
        {
            foreach (var transition in transitions)
            {
                if (transition.OK)
                {
                    return transition.TargetID;
                }
            }
            return id;
        }

        public abstract void OnEnter(int lastID);

        public abstract void OnExit(int nextID);

        public abstract void Update();

        public abstract void Reset();

        public void AddTransition(ITransition transition) => transitions.Add(transition);

        public void RemoveTransition(ITransition transition) => transitions.Remove(transition);
    }
}