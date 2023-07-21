using System.Collections.Generic;

namespace GRT.FSM
{
    public enum LogicalOperator
    {
        AND,
        OR,
        XOR,
        NOT
    }

    public class LogicalCondition : ICondition
    {
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
                        return !A.NullOrOK(); // 这里B就舍掉了

                    case LogicalOperator.XOR:
                        return A.NullOrOK() ^ B.NullOrOK();

                    case LogicalOperator.OR:
                        return A.NullOrOK() || B.NullOrOK();

                    case LogicalOperator.AND:
                    default:
                        return A.NullOrOK() && B.NullOrOK();
                }
            }
        }

        public LogicalCondition(LogicalOperator op, ICondition a, ICondition b = null)
        {
            Operator = op;
            A = a;
            B = b;
        }
    }

    public static class LogicalConditionExtensions
    {
        public static ICondition AND(this ICondition a, ICondition b) => new LogicalCondition(LogicalOperator.AND, a, b);

        public static ICondition OR(this ICondition a, ICondition b) => new LogicalCondition(LogicalOperator.OR, a, b);

        public static ICondition XOR(this ICondition a, ICondition b) => new LogicalCondition(LogicalOperator.XOR, a, b);

        public static ICondition NOT(this ICondition a) => new LogicalNotCondition(a);

        public static bool NullOrOK(this ICondition condition) => condition == null || condition.OK;
    }

    public class LogicalAndCondition : ICondition
    {
        public ICollection<ICondition> InnerConditions { get; private set; }

        public bool OK
        {
            get
            {
                foreach (var condition in InnerConditions)
                {
                    if (condition != null && !condition.OK) return false;
                }
                return true;
            }
        }

        public LogicalAndCondition(ICollection<ICondition> conditions) => InnerConditions = conditions;
    }

    public class LogicalOrCondition : ICondition
    {
        public ICollection<ICondition> InnerConditions { get; private set; }

        public bool OK
        {
            get
            {
                foreach (var condition in InnerConditions)
                {
                    if (condition == null || condition.OK) return true;
                }
                return false;
            }
        }

        public LogicalOrCondition(ICollection<ICondition> conditions) => InnerConditions = conditions;
    }

    public class LogicalNotCondition : ICondition
    {
        public ICondition InnerCondition { get; private set; }

        public bool OK => InnerCondition != null && !InnerCondition.OK;

        public LogicalNotCondition(ICondition condition) => InnerCondition = condition;
    }
}