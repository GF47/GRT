using System.Collections.Generic;

namespace GRT.FSM
{
    public interface ITransition
    {
        int TargetID { get; set; }

        ICollection<ICondition> Conditions { get; }

        bool OK { get; }
    }
}