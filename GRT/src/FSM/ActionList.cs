using System.Collections.Generic;

namespace GRT.FSM
{
    public class ActionList : BaseAction
    {
        public IList<IAction> List { get; }

        public override bool Completed
        {
            get
            {
                foreach (var action in List)
                {
                    if (!action.Completed) { return false; }
                }
                return true;
            }
        }

        public ActionList(IList<IAction> actions)
        {
            List = actions ?? new List<IAction>();
        }

        public override void Invoke()
        {
            foreach (var action in List)
            {
                if (!action.Completed)
                {
                    action.Invoke();
                }
            }
        }

        public override void Reset()
        {
            foreach (var action in List)
            {
                action.Reset();
            }
        }

        public override void Start()
        {
            foreach (var action in List)
            {
                action.Start();
            }
        }
    }
}