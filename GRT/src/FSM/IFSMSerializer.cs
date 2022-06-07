using GRT.Serialization;
using System.Collections.Generic;

namespace GRT.FSM
{
    public interface IFSMSerializer<T, Node> where T : ISerializer<Node>
    {
        string Serialize(IList<FiniteStateMachine> fsm);

        IList<FiniteStateMachine> Deserialize(string src);

        FiniteStateMachine ParseFSM(Node node);

        IState ParseState(Node node, int defaultID);

        ITransition ParseTransition(Node node);

        ICondition ParseCondition(Node node);
    }
}