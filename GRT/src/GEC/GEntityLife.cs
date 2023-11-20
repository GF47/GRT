using System;
using System.Collections.Generic;

namespace GRT.GEC
{
    public class GEntityLife<T, TE> : IGComponent<T, TE>, IGLife<T>, IGStartable<T>, IGTickable<T>, IGDisposable<T>, IGScope
        where T : class
        where TE : IGEntity<T, TE>
    {
        public event Action<GEntityLife<T, TE>> Starting;

        public event Action<GEntityLife<T, TE>> Disposing;

        public event Action<GEntityLife<T, TE>, float> Ticking;

#if FAST_BUT_SHIT
        public IGStartable AsGStartable => this;
        public IGTickable AsGTickable => this;
        public IGDisposable AsGDisposable => this;
        public IGScope AsGScope => this;
#endif

        public TE GEntity { get; set; }

        public T Object => GEntity?.Puppet;

        public bool IsAlive { get; protected set; }

        public IGScope Scope { get; set; }

        public ICollection<IGLife> Lives { get; protected set; } = new List<IGLife>();

        protected IDictionary<IGLife, bool> PendingLives { get; set; } = new Dictionary<IGLife, bool>();

        public virtual void GStart()
        {
            IsAlive = true;

            Starting?.Invoke(this);
        }

        public virtual void GDispose()
        {
            Disposing?.Invoke(this);

            IsAlive = false;
        }

        public virtual void GTick(float delta)
        {
            Ticking?.Invoke(this, delta);
        }

        void IGScope.___Attach(IGLife life) => PendingLives.AttachImpl(life);

        void IGScope.___Detach(IGLife life) => PendingLives.DetachImpl(life);

        void IGScope.___ResolvePendingLives() => this.ResolvePendingLivesImpl(PendingLives);
    }
}