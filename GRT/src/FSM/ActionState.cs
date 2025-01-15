using System.Collections;
using System.Collections.Generic;

namespace GRT.FSM
{
    public class ActionState : BaseState, IEnumerable<IResetable>
    {
        public ICollection<IAction> Actions { get; private set; }

        public ActionState(int id, string info = "") : base(id, info)
        {
            Actions = new List<IAction>();
        }

        public override void OnEnter(int lastID)
        {
            foreach (var action in Actions)
            {
                action.Start();
            }
        }

        public override void OnExit(int nextID) { }

        public override void Reset() => this.DeepReset(false);

        public override void Update()
        {
            foreach (var action in Actions)
            {
                if (!action.Completed)
                {
                    action.Invoke();
                }
            }
        }

        public IEnumerator<IResetable> GetEnumerator() => Actions.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Actions.GetEnumerator();
    }
}