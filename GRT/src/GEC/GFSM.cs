using GRT.FSM;
using System;

namespace GRT.GEC
{
    public class GFSM<T> : IGComponent<T>
        where T : class
    {
        private FiniteStateMachine _fsm;

        public IGEntity<T> GEntity { get; set; }

        public FiniteStateMachine FSM
        {
            get => _fsm; set
            {
                if (_fsm != null)
                {
                    throw new InvalidOperationException($"{GEntity.Location}: There is already has a fsm component, assign repeatedly is not allowed");
                }

                _fsm = value;

                Bind();
            }
        }

        private void Bind()
        {
            var lifecycle = GEntity.GetComponent<T, GEntityLife<T>>();
            if (lifecycle != null)
            {
                lifecycle.Starting += Start;
                lifecycle.Disposing += Dispose;
                lifecycle.Ticking += Tick;

                if (lifecycle.IsAlive)
                {
                    Start(lifecycle);
                }
            }
        }

        private void Tick(GEntityLife<T> life, float delta) => _fsm.Update();

        private void Dispose(GEntityLife<T> life) => _fsm.Reset();

        private void Start(GEntityLife<T> life) => _fsm.Start();
    }
}