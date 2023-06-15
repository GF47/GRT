using System.Collections.Generic;

namespace GRT.FSM
{
    public abstract class ConditionWithActions : ICondition
    {
        public ICollection<IAction> SucceedActions { get; protected set; }

        public ICollection<IAction> FailedActions { get; protected set; }

        public ICondition InnerCondition { get; protected set; }

        public bool OK
        {
            get
            {
                var ok = InnerCondition == null || InnerCondition.OK;

                var actions = ok ? SucceedActions : FailedActions;
                if (actions != null)
                {
                    foreach (var action in actions)
                    {
                        ExecuteAction(this, action);
                    }
                }

                return ok;
            }
        }

        protected abstract void ExecuteAction(ICondition condition, IAction action);
    }
}