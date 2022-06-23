using System.Collections.Generic;

namespace GRT.FSM
{
    public abstract class BaseTransition : ITransition
    {
        protected ICollection<ICondition> conditions;

        public int TargetID { get; set; }

        ICollection<ICondition> ITransition.Conditions => conditions;

        bool ITransition.OK
        {
            get
            {
                if (conditions != null)
                {
                    foreach (var condition in conditions)
                    {
                        if (!condition.OK)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        public abstract int Go();
    }
}