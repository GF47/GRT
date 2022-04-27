using System;
namespace GRT.FSM
{
    public class NormalState : BaseState
    {
        public Action<int> Entering;
        public Action<int> Exiting;
        public Action<int> Updating;

        public NormalState(int id, string info = "") : base(id, info)
        {
        }

        public override void OnEnter(int lastID) => Entering?.Invoke(lastID);

        public override void OnExit(int nextID) => Exiting?.Invoke(nextID);

        public override void Reset()
        {
        }

        public override void Update()
        {
            Updating?.Invoke(id);
        }
    }
}