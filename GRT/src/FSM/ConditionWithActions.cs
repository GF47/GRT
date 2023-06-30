using System.Collections.Generic;

namespace GRT.FSM
{
    public abstract class ConditionWithActions : ICondition
    {
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
            var ok = InnerConditions == null || InnerConditions.Count == 0;
            if (ok)
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
    }
}