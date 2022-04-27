namespace GRT.FSM
{
    public class StringTriggerCondition : TriggerCondition<string>
    {
        static StringTriggerCondition() => EqualFunc = string.Equals;

        public StringTriggerCondition(string expected) : base(expected)
        {
        }
    }
}