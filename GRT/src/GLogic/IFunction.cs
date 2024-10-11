namespace GRT.GLogic
{
    public interface IFunction<T> : ICell<T>
    {
        void In(T arg);

        void Out(T arg);
    }
}