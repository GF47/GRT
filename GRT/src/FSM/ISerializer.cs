using System.Collections.Generic;

namespace GRT.FSM
{
    public interface ISerializer
    {
        IList<FiniteStateMachine> Deserialize(string src);
        string Serialize(IList<FiniteStateMachine> fsm);
    }
}
