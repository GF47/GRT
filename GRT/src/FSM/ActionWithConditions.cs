using System.Collections.Generic;

namespace GRT.FSM
{
    public class ActionWithConditions : IAction
    {
        protected bool completed;

        public ICollection<ICondition> Conditions { get; }

        public ICollection<IAction> Actions { get; }

        public bool RepeatConditionIfFailed { get; set; } = false;

        public bool Completed => completed;

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
                        if (!RepeatConditionIfFailed) { completed = true; }
                        return;
                    }
                }
            }

            completed = true;
            if (Actions != null)
            {
                foreach (var action in Actions)
                {
                    if (!action.Completed)
                    {
                        completed = false;
                        action.Invoke();
                    }
                }
            }
        }

        public void Reset()
        {
            completed = false;

            if (Actions != null)
            {
                foreach (var action in Actions)
                {
                    action.Reset();
                }
            }

            if (Conditions != null)
            {
                foreach (var condition in Conditions)
                {
                    if (condition is IResetable resetable)
                    {
                        resetable.Reset();
                    }
                }
            }
        }

        public void Start()
        {
            if (Actions != null)
            {
                foreach (var action in Actions)
                {
                    action.Start();
                }
            }
        }
    }
}