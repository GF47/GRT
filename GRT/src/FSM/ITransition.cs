using System;

namespace GRT.FSM
{
    public interface ITransition
    {
        IState To { get; set; }
        Func<bool> Conditions { get; }
        bool OK { get; }
    }
}
