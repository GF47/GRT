using GRT.FSM;

namespace GRT.GEC
{
    public class GFSM<T, TE> : IGComponent<T, TE>
        where T : class
        where TE : IGEntity<T, TE>
    {
        private FiniteStateMachine _fsm;

        public TE Entity { get; set; }

        public FiniteStateMachine FSM
        {
            get => _fsm; set
            {
                if (_fsm != null)
                {
                    throw new GEntityException<T, TE>(Entity, "an fsm is already exists, repeated setting is not allowed");
                }

                _fsm = value;

                Bind();
            }
        }

        private void Bind()
        {
            if (Entity.TryGetComponent(out GEntityLife<T, TE> lifecycle))
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

        private void Tick(GEntityLife<T, TE> _, float __) => _fsm.Update();

        private void Dispose(GEntityLife<T, TE> _) => _fsm.Reset();

        private void Start(GEntityLife<T, TE> _) => _fsm.Start();
    }
}