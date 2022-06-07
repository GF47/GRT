namespace GRT.FSM
{
    public abstract class BaseAction : IAction
    {
        public virtual bool Completed { get; protected set; }

        public virtual void Start() { }

        public abstract void Invoke();

        public virtual void Reset()
        {
            Completed = false;
        }
    }
}
