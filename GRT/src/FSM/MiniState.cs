using System;
using System.Collections.Generic;

namespace GRT.FSM
{
    public class MiniState : IState
    {
        private int _id;
        private string _info;
        private ICollection<ITransition> _transitions;

        public int ID => _id;
        public string Info => _info;

        ICollection<ITransition> IState.Transitions => _transitions;

        public MiniState(int id, string info)
        {
            if (!Util.IsValid(id))
            {
                throw new ArgumentException($"请将 [ID] 设置为一个非 [{Util.NullStateID}] 的数值", "id");
            }
            _id = id;
            _info = info;

            _transitions = new List<ITransition>();
        }

        ITransition IState.GetNext()
        {
            foreach (var t in _transitions)
            {
                if (t.OK)
                {
                    return t;
                }
            }
            return null;
        }

        void IState.OnEnter(int lastID) { }

        void IState.OnExit(int nextID) { }

        void IState.Reset() { }

        void IState.Update() { }

        public void AddTransition(ITransition transition) => _transitions.Add(transition);

        public void RemoveTransition(ITransition transition) => _transitions.Remove(transition);
    }
}
