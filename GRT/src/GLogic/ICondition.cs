using System;

namespace GRT.GLogic
{
    public interface ICondition<T>
    {
        bool IsTrue(T arg);
    }

    public class Predication<T> : ICondition<T>
    {
        private readonly Predicate<T> _predicate;

        public Predication(Predicate<T> predicate)
        {
            _predicate = predicate;
        }

        public bool IsTrue(T arg) => _predicate == null || _predicate.Invoke(arg);
    }

    public class TrueCondition<T> : ICondition<T>
    {
        public static TrueCondition<T> Instance { get; private set; } = new TrueCondition<T>();

        public bool IsTrue(T arg) => true;
    }

    public class FalseCondition<T> : ICondition<T>
    {
        public static FalseCondition<T> Instance { get; private set; } = new FalseCondition<T>();

        public bool IsTrue(T arg) => false;
    }
}