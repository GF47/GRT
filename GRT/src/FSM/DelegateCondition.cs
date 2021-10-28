using System;

namespace GRT.FSM
{
    public class DelegateCondition : ICondition
    {
        private readonly Func<bool> _condition;
        public bool OK => _condition();

        public DelegateCondition(Func<bool> condition)
        {
            _condition = condition ?? throw new ArgumentNullException("condition");
        }
    }
}