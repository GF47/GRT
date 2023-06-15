using System.Collections.Generic;

namespace GRT.FSM
{
    public class ActionList : IAction
    {
        public IList<IAction> List { get; }

        public bool Completed
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

        public void Invoke()
        {
            foreach (var action in List)
            {
                if (!action.Completed)
                {
                    action.Invoke();
                }
            }
        }

        public void Reset()
        {
            foreach (var action in List)
            {
                action.Reset();
            }
        }

        public void Start()
        {
            foreach (var action in List)
            {
                action.Start();
            }
        }
    }
}