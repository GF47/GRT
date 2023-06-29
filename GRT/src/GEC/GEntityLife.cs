﻿using System;
using System.Collections.Generic;

namespace GRT.GEC
{
    public class GEntityLife<T> : IGComponent<T>, IGLife<T>, IGStartable<T>, IGTickable<T>, IGDisposable<T>, IGScope
        where T : class
    {
        public event Action<GEntityLife<T>> Starting;

        public event Action<GEntityLife<T>> Disposing;

        public event Action<GEntityLife<T>, float> Ticking;

#if FAST_BUT_SHIT
        public IGStartable AsGStartable => this;
        public IGTickable AsGTickable => this;
        public IGDisposable AsGDisposable => this;
        public IGScope AsGScope => this;
#endif

        public IGEntity<T> GEntity { get; set; }

        public T Object
        {
            get
            {
                if (GEntity == null) return null;
                return GEntity.Reference.TryGetTarget(out var obj) ? obj : null;
            }
        }

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

        void IGScope.Attach(IGLife life)
        {
            if (PendingLives.ContainsKey(life))
            {
                PendingLives[life] = true;
            }
            else
            {
                PendingLives.Add(life, true);
            }
        }

        void IGScope.Detach(IGLife life)
        {
            if (PendingLives.ContainsKey(life))
            {
                PendingLives[life] = false;
            }
            else
            {
                PendingLives.Add(life, false);
            }
        }

        void IGScope.ResolvePendingLives()
        {
            if (PendingLives.Count > 0)
            {
                foreach (var pair in PendingLives)
                {
                    var life = pair.Key;
                    var isAttach = pair.Value;
                    if (isAttach)
                    {
                        Lives.Add(life);
                    }
                    else
                    {
                        Lives.Remove(life);
                    }
                }

                PendingLives.Clear();
            }
        }
    }
}