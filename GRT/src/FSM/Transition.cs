using System.Collections.Generic;

namespace GRT.FSM
{
    public class Transition : ITransition
    {
        private ICollection<ICondition> _conditions;

        public int TargetID { get; set; }

        ICollection<ICondition> ITransition.Conditions => _conditions;

        bool ITransition.OK
        {
            get
            {
                if (_conditions != null)
                {
                    foreach (var condition in _conditions)
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

        public Transition(int target, params ICondition[] conditions)
        {
            TargetID = target;
            _conditions = conditions;
        }

        public Transition(int target, ICollection<ICondition> conditions)
        {
            TargetID = target;
            _conditions = conditions;
        }
    }
}