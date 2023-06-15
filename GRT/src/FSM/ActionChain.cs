using System.Collections.Generic;

namespace GRT.FSM
{
    public class ActionChain : IAction
    {
        private int _indicator;

        public IList<IAction> Chain { get; }

        public bool Completed => Chain.Count == 0 || Chain[Chain.Count - 1].Completed;

        public ActionChain(IList<IAction> actions)
        {
            Chain = actions ?? new List<IAction>();
        }

        public void Invoke()
        {
            if (_indicator < Chain.Count)
            {
                var action = Chain[_indicator];
                action.Invoke();

                if (action.Completed && _indicator + 1 < Chain.Count)
                {
                    Chain[++_indicator].Start();
                }
            }
        }

        public void Reset()
        {
            _indicator = 0;
            foreach (var acton in Chain)
            {
                acton.Reset();
            }
        }

        public void Start()
        {
            if (_indicator < Chain.Count)
            {
                Chain[_indicator].Start();
            }
        }
    }
}