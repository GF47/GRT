using System;

namespace GRT.FSM
{
    public class IntVarCondition : IntRelationalCondition
    {
        public Func<string, int> GetValueFunc;
        public string Name { get; set; }

        public IntVarCondition(string name, RelationalOperator op, int another) : base(op, another)
        {
            Name = name;
        }

        protected override int GetValue() => GetValueFunc == null ? 0 : GetValueFunc(Name);
    }

    public class FloatVarCondition : FloatRelationalCondition
    {
        public Func<string, float> GetValueFunc;
        public string Name { get; set; }

        public FloatVarCondition(string name, RelationalOperator op, float another) : base(op, another)
        {
            Name = name;
        }

        protected override float GetValue() => GetValueFunc == null ? 0f : GetValueFunc(Name);
    }

    public class DoubleVarCondition : DoubleRelationalCondition
    {
        public Func<string, double> GetValueFunc;
        public string Name { get; set; }

        public DoubleVarCondition(string name, RelationalOperator op, double another) : base(op, another)
        {
            Name = name;
        }

        protected override double GetValue() => GetValueFunc == null ? 0d : GetValueFunc(Name);
    }

    public class StringVarCondition : StringRelationalCondition
    {
        public Func<string, string> GetValueFunc;
        public string Name { get; set; }

        public StringVarCondition(string name, string another)
        {
            Name = name;
            this.another = another;
        }

        protected override string GetValue() => GetValueFunc == null ? null : GetValueFunc(Name);
    }
}