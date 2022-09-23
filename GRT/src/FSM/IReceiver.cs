namespace GRT.FSM
{
    public interface IReceiver<T> : IAction
    {
        void Receive(T value);
    }
}