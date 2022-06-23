using System.Collections.Generic;

namespace GRT.FSM
{
    public class Transition : BaseTransition, ITransition
    {
        public Transition(int target, params ICondition[] conditions)
        {
            TargetID = target;

            if (conditions == null || conditions.Length == 0)
            {
                this.conditions = new List<ICondition>();
            }
            else
            {
                this.conditions = conditions;
            }
        }

        public Transition(int target, ICollection<ICondition> conditions)
        {
            TargetID = target;
            this.conditions = conditions;
        }

        public override int Go() => TargetID;
    }
}