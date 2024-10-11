using System.Collections.Generic;

namespace GRT.FSM
{
    public abstract class ConditionWithActions : ICondition, IResetable, IActionEnumerable
    {
        IEnumerable<IAction> IActionEnumerable.AEnumerable
        {
            get
            {
                foreach (var a in SucceedActions)
                {
                    yield return a;
                }
                foreach (var a in FailedActions)
                {
                    yield return a;
                }
            }
        }

        public ICollection<IAction> SucceedActions { get; protected set; }

        public ICollection<IAction> FailedActions { get; protected set; }

        public ICollection<ICondition> InnerConditions { get; protected set; }

        public bool OK
        {
            get
            {
                var ok = CheckWithoutInvokingActions();

                var actions = ok ? SucceedActions : FailedActions;
                if (actions != null)
                {
                    foreach (var action in actions)
                    {
                        InvokeAction(this, action);
                    }
                }

                return ok;
            }
        }

        public bool CheckWithoutInvokingActions()
        {
            if (InnerConditions != null)
            {
                foreach (var condition in InnerConditions)
                {
                    if (condition != null && !condition.OK)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected abstract void InvokeAction(ICondition condition, IAction action);

        public void Reset()
        {
            if (InnerConditions != null)
            {
                foreach (var condition in InnerConditions)
                {
                    if (condition is IResetable resetable)
                    {
                        resetable.Reset();
                    }
                }
            }

            if (FailedActions != null)
            {
                foreach (var action in FailedActions)
                {
                    action.Reset();
                }
            }

            if (SucceedActions != null)
            {
                foreach (var action in SucceedActions)
                {
                    action.Reset();
                }
            }
        }
    }
}