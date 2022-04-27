using System.Collections.Generic;

namespace GRT.FSM
{
    public class ActionChain : BaseAction
    {
        private int _indicator;

        public IList<IAction> Chain { get; }

        public override bool Completed => Chain.Count == 0 || Chain[_indicator].Completed;

        public ActionChain(IList<IAction> actions)
        {
            Chain = actions ?? new List<IAction>();
        }

        public override void Invoke()
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

        public override void Reset()
        {
            _indicator = 0;
            foreach (var acton in Chain)
            {
                acton.Reset();
            }
        }

        public override void Start()
        {
            if (_indicator < Chain.Count)
            {
                Chain[_indicator].Start();
            }
        }
    }
}