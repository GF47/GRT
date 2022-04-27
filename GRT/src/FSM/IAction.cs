namespace GRT.FSM
{
    public interface IAction
    {
        bool Completed { get; }
        void Start();
        void Invoke();
        void Reset();
    }
}
