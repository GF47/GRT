namespace GRT.FSM
{
    public abstract class ApproximatelyEqualCondition<T> : ICondition
    {
        public abstract T Value { get; }

        public abstract T Another { get; }

        bool ICondition.OK => ApproximateEqual(Value, Another);

        protected abstract bool ApproximateEqual(T a, T b);
    }
}