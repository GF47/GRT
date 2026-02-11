using System;

namespace GRT.FSM
{
    public class DelegateAction : IAction
    {
        public bool Completed { get; private set; }

        private readonly Action _action;

        public DelegateAction(Action action)
        {
            _action = action ?? throw new ArgumentNullException("action");
        }

        public void Invoke()
        {
            if (!Completed)
            {
                _action?.Invoke();
                Completed = true;
            }
        }

        public void Reset()
        {
            Completed = false;
        }

        void IAction.Start()
        {
        }
    }
}