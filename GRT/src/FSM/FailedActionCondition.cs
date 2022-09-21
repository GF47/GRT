using System.Collections.Generic;

namespace GRT.FSM
{
    public abstract class FailedActionCondition : ICondition
    {
        public ICollection<IAction> FailedActions { get; set; }
        public ICondition InnerCondition { get; set; }

        public bool OK
        {
            get
            {
                var ok = InnerCondition == null || InnerCondition.OK;

                if (!ok)
                {
                    foreach (var action in FailedActions)
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