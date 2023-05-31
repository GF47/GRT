// #define FAST_BUT_SHIT
// #undef FAST_BUT_SHIT

using System;
using System.Collections.Generic;

namespace GRT
{
    public interface IGLife
    {
#if FAST_BUT_SHIT
        IGStartable AsGStartable { get; }
        IGTickable AsGTickable { get; }
        IGDisposable AsGDisposable { get; }
        IGScope AsGScope { get; }
#endif

        bool IsAlive { get; }

        IGScope Scope { get; set; }
    }

    public interface IGStartable : IGLife
    {
        void GStart();
    }

    public interface IGTickable : IGLife
    {
        void GTick(float delta);
    }

    public interface IGDisposable : IGLife
    {
        void GDispose();
    }

    public interface IGScope : IGLife
    {
        ICollection<IGLife> Lives { get; }

        #region holy shit

        void Attach(IGLife life);
        void Detach(IGLife life);
        void ResolvePendingLives();

        #endregion
    }

    public interface IGLife<T> : IGLife
    {
        T Object { get; }
    }

    public interface IGStartable<T> : IGLife<T>, IGStartable
    {
    }

    public interface IGTickable<T> : IGLife<T>, IGTickable
    {
    }

    public interface IGDisposable<T> : IGLife<T>, IGDisposable
    {
    }

    public class MiniGLifecycle : IGStartable, IGDisposable
    {
        public event Action<IGStartable> Starting;

        public event Action<IGDisposable> Disposing;

#if FAST_BUT_SHIT
        public IGStartable AsGStartable => this;
        public virtual IGTickable AsGTickable => null;
        public IGDisposable AsGDisposable => this;
        public virtual IGScope AsGScope => null;
#endif

        public bool IsAlive { get; protected set; }

        public IGScope Scope { get; set; }

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
    }

    public class MiniGLifecycleWithTick : MiniGLifecycle, IGTickable
    {
        public event Action<IGTickable, float> Ticking;

#if FAST_BUT_SHIT
        public override IGTickable AsGTickable => this;
#endif

        public void GTick(float delta)
        {
            Ticking?.Invoke(this, delta);
        }
    }

    public class MiniGLifecycleWithScope : MiniGLifecycle, IGScope
    {
#if FAST_BUT_SHIT
        public override IGScope AsGScope => this;
#endif

        public ICollection<IGLife> Lives { get; protected set; } = new List<IGLife>();

        protected IDictionary<IGLife, bool> PendingLives { get; set; } = new Dictionary<IGLife, bool>();

        public void Attach(IGLife life)
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

        public void Detach(IGLife life)
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

        public void ResolvePendingLives()
        {
            if (PendingLives.Count == 0) { return; }

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

    public class MiniGLifecycleWithTickAndScope : MiniGLifecycle, IGTickable, IGScope
    {
        public event Action<IGTickable, float> Ticking;

#if FAST_BUT_SHIT
        public override IGTickable AsGTickable => this;
        public override IGScope AsGScope => this;
#endif

        public ICollection<IGLife> Lives { get; protected set; } = new List<IGLife>();

        protected IDictionary<IGLife, bool> PendingLives { get; set; } = new Dictionary<IGLife, bool>();

        public void GTick(float delta)
        {
            Ticking?.Invoke(this, delta);
        }

        public void Attach(IGLife life)
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

        public void Detach(IGLife life)
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

        public void ResolvePendingLives()
        {
            if (PendingLives.Count == 0) { return; }

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

    public class MiniGLifecycle<T> : IGStartable<T>, IGDisposable<T>
    {
        public event Action<IGStartable<T>> Starting;

        public event Action<IGDisposable<T>> Disposing;

        public T Object { get; protected set; }

#if FAST_BUT_SHIT
        public IGStartable AsGStartable => this;
        public virtual IGTickable AsGTickable => null;
        public IGDisposable AsGDisposable => this;
        public virtual IGScope AsGScope => null;
#endif

        public bool IsAlive { get; protected set; }

        public IGScope Scope { get; set; }

        public MiniGLifecycle(T obj)
        {
            Object = obj;
        }

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
    }

    public class MiniGLifecycleWithTick<T> : MiniGLifecycle<T>, IGTickable<T>
    {
        public event Action<IGTickable<T>, float> Ticking;

#if FAST_BUT_SHIT
        public override IGTickable AsGTickable => this;
#endif

        public MiniGLifecycleWithTick(T obj) : base(obj)
        {
        }

        public void GTick(float delta)
        {
            Ticking?.Invoke(this, delta);
        }
    }

    public class MiniGLifecycleWithScope<T> : MiniGLifecycle<T>, IGScope
    {
#if FAST_BUT_SHIT
        public override IGScope AsGScope => this;
#endif

        public ICollection<IGLife> Lives { get; protected set; } = new List<IGLife>();

        protected IDictionary<IGLife, bool> PendingLives { get; set; } = new Dictionary<IGLife, bool>();

        public MiniGLifecycleWithScope(T obj) : base(obj)
        {
        }

        public void Attach(IGLife life)
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

        public void Detach(IGLife life)
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

        public void ResolvePendingLives()
        {
            if (PendingLives.Count == 0) { return; }

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

    public class MiniGLifecycleWithTickAndScope<T> : MiniGLifecycle<T>, IGTickable<T>, IGScope
    {
        public event Action<IGTickable<T>, float> Ticking;

#if FAST_BUT_SHIT
        public override IGTickable AsGTickable => this;
        public override IGScope AsGScope => this;
#endif

        public ICollection<IGLife> Lives { get; private set; } = new List<IGLife>();

        protected IDictionary<IGLife, bool> PendingLives { get; set; } = new Dictionary<IGLife, bool>();

        public MiniGLifecycleWithTickAndScope(T obj) : base(obj)
        {
        }

        public void GTick(float delta)
        {
            Ticking?.Invoke(this, delta);
        }

        public void Attach(IGLife life)
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

        public void Detach(IGLife life)
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

        public void ResolvePendingLives()
        {
            if (PendingLives.Count == 0) { return; }

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

    public static class GLifecycleExtensions
    {
        public static void LifeStart(this IGLife life)
        {
#if FAST_BUT_SHIT
            life.AsGStartable?.GStart();

            var scope = life.AsGScope;
            if (scope != null)
            {
                scope.ResolvePendingLives();
                foreach (var lifeInScope in scope.Lives)
                {
                    lifeInScope.LifeStart();
                }
            }
#else
            if (life is IGStartable startable)
            {
                startable.GStart();
            }

            if (life is IGScope scope)
            {
                scope.ResolvePendingLives();
                foreach (var lifeInScope in scope.Lives)
                {
                    lifeInScope.LifeStart();
                }
            }
#endif
        }

        public static void LifeTick(this IGLife life, float delta)
        {
#if FAST_BUT_SHIT
            life.AsGTickable?.GTick(delta);

            var scope = life.AsGScope;
            if (scope != null)
            {
                scope.ResolvePendingLives();
                foreach (var lifeInScope in scope.Lives)
                {
                    lifeInScope.LifeTick(delta);
                }
            }
#else
            if (life is IGTickable tickable)
            {
                tickable.GTick(delta);
            }

            if (life is IGScope scope)
            {
                scope.ResolvePendingLives();
                foreach (var lifeInScope in scope.Lives)
                {
                    lifeInScope.LifeTick(delta);
                }
            }
#endif
        }

        public static void LifeDispose(this IGLife life)
        {
#if FAST_BUT_SHIT
            var scope = life.AsGScope;
            if (scope != null)
            {
                scope.ResolvePendingLives();
                foreach (var lifeInScope in scope.Lives)
                {
                    lifeInScope.LifeDispose();
                }
            }

            life.AsGDisposable?.GDispose();
#else
            if (life is IGScope scope)
            {
                scope.ResolvePendingLives();
                foreach (var lifeInScope in scope.Lives)
                {
                    lifeInScope.LifeDispose();
                }
            }

            if (life is IDisposable disposeable)
            {
                disposeable.Dispose();
            }
#endif
        }

        public static void AttachTo(this IGLife life, IGScope scope, bool autoStart = true)
        {
            life.Scope?.Detach(life);
            scope.Attach(life);
            life.Scope = scope;

            if (autoStart && scope.IsAlive && !life.IsAlive)
            {
                life.LifeStart();
            }
        }

        public static void DetachFromScope(this IGLife life, bool autoDispose = false)
        {
            if (life.Scope != null)
            {
                life.Scope.Detach(life);
                life.Scope = null;

                if (life.IsAlive && autoDispose)
                {
                    life.LifeDispose();
                }
            }
        }

        public static T Find<T>(this IGScope scope, Predicate<T> predicate = null) where T : IGLife
        {
            foreach (var life in scope.Lives)
            {
                if (life is T target && (predicate == null || predicate(target)))
                {
                    return target;
                }
            }

            return default;
        }

        public static T FindInParent<T>(this IGLife life, Predicate<T> predicate = null) where T : IGLife
        {
            if (life.Scope != null)
            {
                if (life.Scope is T target && (predicate == null || predicate(target)))
                {
                    return target;
                }
                else
                {
                    return life.Scope.FindInParent(predicate);
                }
            }
            else
            {
                return default;
            }
        }

        public static object Find(this IGScope scope, Type type)
        {
            foreach (var life in scope.Lives)
            {
                if (type.IsAssignableFrom(life.GetType()))
                {
                    return life;
                }
            }

            return default;
        }

        public static object FindInParent(this IGLife life, Type type)
        {
            if (life.Scope != null)
            {
                if (type.IsAssignableFrom(life.Scope.GetType()))
                {
                    return life.Scope;
                }
                else
                {
                    return life.Scope.FindInParent(type);
                }
            }

            return default;
        }
    }
}