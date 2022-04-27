namespace GRT.FSM
{
    public abstract class IntRelationalCondition : RelationalCondition<int>
    {
        protected IntRelationalCondition(RelationalOperator op, int another) : base(op, another)
        {
        }

        protected override bool Equal(int a, int b) => a == b;

        protected override bool GreaterThan(int a, int b) => a > b;

        protected override bool GreaterThanOrEqual(int a, int b) => a >= b;

        protected override bool LessThan(int a, int b) => a < b;

        protected override bool LessThanOrEqual(int a, int b) => a <= b;

        protected override bool NotEqual(int a, int b) => a != b;
    }

    public abstract class FloatRelationalCondition : RelationalCondition<float>
    {
        public FloatRelationalCondition(RelationalOperator op, float another) : base(op, another)
        {
        }

        protected override bool Equal(float a, float b) => a == b;

        protected override bool GreaterThan(float a, float b) => a > b;

        protected override bool GreaterThanOrEqual(float a, float b) => a >= b;

        protected override bool LessThan(float a, float b) => a < b;

        protected override bool LessThanOrEqual(float a, float b) => a <= b;

        protected override bool NotEqual(float a, float b) => a != b;
    }

    public abstract class DoubleRelationalCondition : RelationalCondition<double>
    {
        public DoubleRelationalCondition(RelationalOperator op, double another) : base(op, another)
        {
        }

        protected override bool Equal(double a, double b) => a == b;

        protected override bool GreaterThan(double a, double b) => a > b;

        protected override bool GreaterThanOrEqual(double a, double b) => a >= b;

        protected override bool LessThan(double a, double b) => a < b;

        protected override bool LessThanOrEqual(double a, double b) => a <= b;

        protected override bool NotEqual(double a, double b) => a != b;
    }

    public abstract class StringRelationalCondition : ICondition
    {
        public bool IsEqual { get; set; } = true;
        protected string another;

        bool ICondition.OK
        {
            get
            {
                var value = GetValue();
                return IsEqual ? value == another : value != another;
            }
        }

        protected abstract string GetValue();
    }
}