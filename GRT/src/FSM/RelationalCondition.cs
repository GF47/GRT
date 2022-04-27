namespace GRT.FSM
{
    public enum RelationalOperator
    {
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
    }

    public abstract class RelationalCondition<T> : ICondition
    {
        public RelationalOperator Operator { get; set; }
        protected readonly T another;

        bool ICondition.OK
        {
            get
            {
                var value = GetValue();
                switch (Operator)
                {
                    case RelationalOperator.NotEqual:
                        return NotEqual(value, another);

                    case RelationalOperator.GreaterThan:
                        return GreaterThan(value, another);

                    case RelationalOperator.LessThan:
                        return LessThan(value, another);

                    case RelationalOperator.GreaterThanOrEqual:
                        return GreaterThanOrEqual(value, another);

                    case RelationalOperator.LessThanOrEqual:
                        return LessThanOrEqual(value, another);

                    case RelationalOperator.Equal:
                    default:
                        return Equal(value, another);
                }
            }
        }

        public RelationalCondition(RelationalOperator op, T another)
        {
            Operator = op;
            this.another = another;
        }

        protected abstract T GetValue();

        protected abstract bool Equal(T a, T b);

        protected abstract bool NotEqual(T a, T b);

        protected abstract bool GreaterThan(T a, T b);

        protected abstract bool LessThan(T a, T b);

        protected abstract bool GreaterThanOrEqual(T a, T b);

        protected abstract bool LessThanOrEqual(T a, T b);
    }
}