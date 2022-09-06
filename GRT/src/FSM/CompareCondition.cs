using System;

namespace GRT.FSM
{
    public enum Operator
    {
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual
    }

    public abstract class CompareCondition<T> : ICondition where T : IComparable<T>, IEquatable<T>
    {
        public virtual Operator Operator { get; set; }

        public abstract T Value { get; }

        public virtual T Another { get; set; }

        bool ICondition.OK
        {
            get
            {
                switch (Operator)
                {
                    case Operator.NotEqual: return !Value.Equals(Another);
                    case Operator.GreaterThan: return Value.CompareTo(Another) > 0;
                    case Operator.LessThan: return Value.CompareTo(Another) < 0;
                    case Operator.GreaterThanOrEqual: return Value.CompareTo(Another) >= 0;
                    case Operator.LessThanOrEqual: return Value.CompareTo(Another) <= 0;
                    case Operator.Equal: default: return Value.Equals(Another);
                }
            }
        }
    }
}