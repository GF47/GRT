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

        void ___Attach(IGLife life);

        void ___Detach(IGLife life);

        void ___ResolvePendingLives();

        #endregion holy shit
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

        public virtual void GTick(float delta)
        {
            Ticking?.Invoke(this, delta);
        }
    }

    public class MiniGLifecycleWithScope : MiniGLifecycle, IGScope
    {
#if FAST_BUT_SHIT
        public override IGScope AsGScope => this;
#endif

        ICollection<IGLife> IGScope.Lives { get; } = new List<IGLife>();

        protected IDictionary<IGLife, bool> PendingLives { get; set; } = new Dictionary<IGLife, bool>();

        void IGScope.___Attach(IGLife life) => PendingLives.AttachImpl(life);

        void IGScope.___Detach(IGLife life) => PendingLives.DetachImpl(life);

        void IGScope.___ResolvePendingLives() => this.ResolvePendingLivesImpl(PendingLives);
    }

    public class MiniGLifecycleWithTickAndScope : MiniGLifecycle, IGTickable, IGScope
    {
        public event Action<IGTickable, float> Ticking;

#if FAST_BUT_SHIT
        public override IGTickable AsGTickable => this;
        public override IGScope AsGScope => this;
#endif

        ICollection<IGLife> IGScope.Lives { get; } = new List<IGLife>();

        protected IDictionary<IGLife, bool> PendingLives { get; set; } = new Dictionary<IGLife, bool>();

        public virtual void GTick(float delta)
        {
            Ticking?.Invoke(this, delta);
        }

        void IGScope.___Attach(IGLife life) => PendingLives.AttachImpl(life);

        void IGScope.___Detach(IGLife life) => PendingLives.DetachImpl(life);

        void IGScope.___ResolvePendingLives() => this.ResolvePendingLivesImpl(PendingLives);
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

        public virtual void GTick(float delta)
        {
            Ticking?.Invoke(this, delta);
        }
    }

    public class MiniGLifecycleWithScope<T> : MiniGLifecycle<T>, IGScope
    {
#if FAST_BUT_SHIT
        public override IGScope AsGScope => this;
#endif

        ICollection<IGLife> IGScope.Lives { get; } = new List<IGLife>();

        protected IDictionary<IGLife, bool> PendingLives { get; set; } = new Dictionary<IGLife, bool>();

        public MiniGLifecycleWithScope(T obj) : base(obj)
        {
        }

        void IGScope.___Attach(IGLife life) => PendingLives.AttachImpl(life);

        void IGScope.___Detach(IGLife life) => PendingLives.DetachImpl(life);

        void IGScope.___ResolvePendingLives() => this.ResolvePendingLivesImpl(PendingLives);
    }

    public class MiniGLifecycleWithTickAndScope<T> : MiniGLifecycle<T>, IGTickable<T>, IGScope
    {
        public event Action<IGTickable<T>, float> Ticking;

#if FAST_BUT_SHIT
        public override IGTickable AsGTickable => this;
        public override IGScope AsGScope => this;
#endif

        ICollection<IGLife> IGScope.Lives { get; } = new List<IGLife>();

        protected IDictionary<IGLife, bool> PendingLives { get; set; } = new Dictionary<IGLife, bool>();

        public MiniGLifecycleWithTickAndScope(T obj) : base(obj)
        {
        }

        public virtual void GTick(float delta)
        {
            Ticking?.Invoke(this, delta);
        }

        void IGScope.___Attach(IGLife life) => PendingLives.AttachImpl(life);

        void IGScope.___Detach(IGLife life) => PendingLives.DetachImpl(life);

        void IGScope.___ResolvePendingLives() => this.ResolvePendingLivesImpl(PendingLives);
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
                scope.___ResolvePendingLives();
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
                scope.___ResolvePendingLives();
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
                scope.___ResolvePendingLives();
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
                scope.___ResolvePendingLives();
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
                scope.___ResolvePendingLives();
                foreach (var lifeInScope in scope.Lives)
                {
                    lifeInScope.LifeDispose();
                }
            }

            life.AsGDisposable?.GDispose();
#else
            if (life is IGScope scope)
            {
                scope.___ResolvePendingLives();
                foreach (var lifeInScope in scope.Lives)
                {
                    lifeInScope.LifeDispose();
                }
            }

            if (life is IGDisposable disposable)
            {
                disposable.GDispose();
            }
#endif
        }

        public static void AttachToScope(this IGLife life, IGScope scope, bool autoStart = true)
        {
            life.Scope?.___Detach(life);
            scope.___Attach(life);
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
                life.Scope.___Detach(life);
                life.Scope = null;

                if (life.IsAlive && autoDispose)
                {
                    life.LifeDispose();
                }
            }
        }

        public static void AttachImpl(this IDictionary<IGLife, bool> pendingLives, IGLife life)
        {
            if (pendingLives != null)
            {
                if (pendingLives.ContainsKey(life))
                {
                    pendingLives[life] = true;
                }
                else
                {
                    pendingLives.Add(life, true);
                }
            }
        }

        public static void DetachImpl(this IDictionary<IGLife, bool> pendingLives, IGLife life)
        {
            if (pendingLives != null)
            {
                if (pendingLives.ContainsKey(life))
                {
                    pendingLives[life] = false;
                }
                else
                {
                    pendingLives.Add(life, false);
                }
            }
        }

        public static void ResolvePendingLivesImpl(this IGScope scope, ICollection<KeyValuePair<IGLife, bool>> pendingLives)
        {
            if (pendingLives != null && pendingLives.Count > 0)
            {
                foreach (var pair in pendingLives)
                {
                    var life = pair.Key;
                    var isAttach = pair.Value;
                    if (isAttach)
                    {
                        scope.Lives.Add(life);
                    }
                    else
                    {
                        scope.Lives.Remove(life);
                    }
                }
                pendingLives.Clear();
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

        public static bool Contain<T>(this IGScope scope, out T value, Predicate<T> predicate = null) where T : IGLife
        {
            foreach (var life in scope.Lives)
            {
                if (life is T target && (predicate == null || predicate(target)))
                {
                    value = target;
                    return true;
                }
            }

            value = default;
            return false;
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

        public static bool Contain(this IGScope scope, out object value, Type type)
        {
            foreach (var life in scope.Lives)
            {
                if (type.IsAssignableFrom(life.GetType()))
                {
                    value = life;
                    return true;
                }
            }

            value = null;
            return false;
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