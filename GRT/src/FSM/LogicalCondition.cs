namespace GRT.FSM
{
    public class LogicalCondition : ICondition
    {
        public enum LogicalOperator
        {
            AND,
            OR,
            XOR,
            NOT
        }

        public LogicalOperator Operator { get; set; }
        public ICondition A { get; set; }
        public ICondition B { get; set; }

        bool ICondition.OK
        {
            get
            {
                switch (Operator)
                {
                    case LogicalOperator.NOT:
                        return !IsOK(A); // 这里B就舍掉了

                    case LogicalOperator.XOR:
                        return IsOK(A) ^ IsOK(B);

                    case LogicalOperator.OR:
                        return IsOK(A) || IsOK(B);

                    case LogicalOperator.AND:
                    default:
                        return IsOK(A) && IsOK(B);
                }
            }
        }

        public LogicalCondition(LogicalOperator op, ICondition a, ICondition b = null)
        {
            Operator = op;
            A = a;
            B = b;
        }

        public ICondition AND(ICondition b) => AND(this, b);
        public ICondition OR(ICondition b) => OR(this, b);
        public ICondition XOR(ICondition b) => XOR(this, b);
        public ICondition NOT() => NOT(this);

        public static ICondition AND(ICondition a, ICondition b) => new LogicalCondition(LogicalOperator.AND, a, b);
        public static ICondition OR(ICondition a, ICondition b) => new LogicalCondition(LogicalOperator.OR, a, b);
        public static ICondition XOR(ICondition a, ICondition b) => new LogicalCondition(LogicalOperator.XOR, a, b);
        public static ICondition NOT(ICondition a) => new LogicalCondition(LogicalOperator.NOT, a, null);

        private static bool IsOK(ICondition condition) => condition == null || condition.OK;
    }
}