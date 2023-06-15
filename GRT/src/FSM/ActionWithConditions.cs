using System.Collections.Generic;

namespace GRT.FSM
{
    public class ActionWithConditions : IAction
    {
        public ICollection<ICondition> Conditions { get; }

        public ICollection<IAction> Actions { get; }

        public bool Completed
        {
            get
            {
                if (Conditions != null)
                {
                    foreach (var action in Actions)
                    {
                        if (!action.Completed)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        public ActionWithConditions(ICollection<ICondition> conditions, ICollection<IAction> actions)
        {
            Conditions = conditions;
            Actions = actions;
        }

        public void Invoke()
        {
            if (Conditions != null)
            {
                foreach (var condition in Conditions)
                {
                    if (!condition.OK)
                    {
                        return;
                    }
                }
            }

            foreach (var action in Actions)
            {
                if (!action.Completed)
                {
                    action.Invoke();
                }
            }
        }

        public void Reset()
        {
            foreach (var action in Actions)
            {
                action.Reset();
            }
        }

        public void Start()
        {
            foreach (var action in Actions)
            {
                action.Start();
            }
        }
    }
}