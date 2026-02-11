using System.Collections;
using System.Collections.Generic;

namespace GRT.FSM
{
    public class ConditionChain : ICondition, IResetable, IEnumerable<IResetable>
    {
        private int _indicator;

        public IList<ICondition> Chain { get; }

        public bool OK
        {
            get
            {
                if (_indicator <= 0)
                {
                    _indicator = 0;
                }
                for (; _indicator < Chain.Count; _indicator++)
                {
                    if (!Chain[_indicator].NullOrOK())
                    {
                        break;
                    }
                }
                return _indicator >= Chain.Count;
            }
        }

        public ConditionChain(IList<ICondition> conditions)
        {
            Chain = conditions ?? new List<ICondition>();
        }

        public void Reset()
        {
            _indicator = 0;
            this.DeepReset(false);
        }

        public IEnumerator<IResetable> GetEnumerator()
        {
            foreach (var condition in Chain)
            {
                if (condition is IResetable resetable)
                {
                    yield return resetable;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}