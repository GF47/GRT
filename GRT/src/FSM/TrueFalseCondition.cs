namespace GRT.FSM
{
    public class TrueCondition : ICondition
    {
        public bool OK => true;
    }

    public class FalseCondition : ICondition
    {
        public bool OK => false;
    }
}