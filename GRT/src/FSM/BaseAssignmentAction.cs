using System.Collections.Generic;

namespace GRT.FSM
{
    public abstract class BaseAssignmentAction<T> : BaseAction
    {
        public IList<IReceiver<T>> Receivers { get; set; }

        public abstract T Value { get; }

        public override bool Completed
        {
            get
            {
                if (Receivers != null)
                {
                    foreach (var action in Receivers)
                    {
                        if (!action.Completed) { return false; }
                    }
                }
                return true;
            }
        }

        public override void Start()
        {
            if (Receivers != null)
            {
                var value = Value;
                foreach (var receiver in Receivers)
                {
                    receiver.Receive(value);
                    receiver.Start();
                }
            }
        }

        public override void Invoke()
        {
            if (Receivers != null)
            {
                foreach (var action in Receivers)
                {
                    if (!action.Completed)
                    {
                        action.Invoke();
                    }
                }
            }
        }

        public override void Reset()
        {
            if (Receivers != null)
            {
                foreach (var action in Receivers)
                {
                    action.Reset();
                }
            }
        }
    }
}