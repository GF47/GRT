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
        void GTick();
    }

    public interface IGDisposable : IGLife
    {
        void GDispose();
    }

    public interface IGScope : IGLife
    {
        ICollection<IGLife> Lives { get; }
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

    public class MiniGLifecycle<T> : IGStartable<T>, IGDisposable<T>
    {
        public event Action<IGStartable<T>> Starting;

        public event Action<IGDisposable<T>> Disposing;

        public T Object { get; protected set; }

        public IGStartable AsGStartable => this;

        public virtual IGTickable AsGTickable => null;

        public IGDisposable AsGDisposable => this;

        public virtual IGScope AsGScope => null;

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
        public event Action<IGTickable<T>> Ticking;

        public override IGTickable AsGTickable => this;

        public MiniGLifecycleWithTick(T obj) : base(obj)
        {
        }

        public void GTick()
        {
            Ticking?.Invoke(this);
        }
    }

    public class MiniGLifecycleWithScope<T> : MiniGLifecycle<T>, IGScope
    {
        public override IGScope AsGScope => this;

        public ICollection<IGLife> Lives { get; protected set; } = new List<IGLife>();

        public MiniGLifecycleWithScope(T obj) : base(obj)
        {
        }
    }

    public class MiniGLifecycleWithTickAndScope<T> : MiniGLifecycle<T>, IGTickable<T>, IGScope
    {
        public event Action<IGTickable<T>> Ticking;

        public override IGTickable AsGTickable => this;

        public override IGScope AsGScope => this;

        public ICollection<IGLife> Lives { get; private set; } = new List<IGLife>();

        public MiniGLifecycleWithTickAndScope(T obj) : base(obj)
        {
        }

        public void GTick()
        {
            Ticking?.Invoke(this);
        }
    }

    public static class GLifeExtensions
    {
        public static void LifeStart(this IGLife life)
        {
#if FAST_BUT_SHIT
            var startable = life.AsGStartable;
            if (startable != null)
            {
                startable.GStart();
            }

            var scope = life.AsGScope;
            if (scope != null)
            {
                foreach (var lifeInScope in scope.Lives)
                {
                    lifeInScope.LifeStart();
                }
            }
#else
            if (life is IStartable startable)
            {
                startable.Start();
            }

            if (life is IScope scope)
            {
                foreach (var lifeInScope in scope.Lives)
                {
                    lifeInScope.LifeStart();
                }
            }
#endif
        }

        public static void LifeTick(this IGLife life)
        {
#if FAST_BUT_SHIT
            var tickable = life.AsGTickable;
            if (tickable != null)
            {
                tickable.GTick();
            }

            var scope = life.AsGScope;
            if (scope != null)
            {
                foreach (var lifeInScope in scope.Lives)
                {
                    lifeInScope.LifeTick();
                }
            }
#else
            if (life is ITickable tickable)
            {
                tickable.Tick();
            }

            if (life is IScope scope)
            {
                foreach (var lifeInScope in scope.Lives)
                {
                    lifeInScope.LifeTick();
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
                foreach (var lifeInScope in scope.Lives)
                {
                    lifeInScope.LifeDispose();
                }
            }

            var disposeable = life.AsGDisposable;
            if (disposeable != null)
            {
                disposeable.GDispose();
            }
#else
            if (life is IScope scope)
            {
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
            if (life.Scope != null)
            {
                life.Scope.Lives.Remove(life);
            }
            scope.Lives.Add(life);
            life.Scope = scope;
            if (autoStart && scope.IsAlive && !life.IsAlive)
            {
                life.LifeStart();
            }
        }

        public static void Detach(this IGLife life)
        {
            if (life.Scope != null)
            {
                life.Scope.Lives.Remove(life);
                life.Scope = null;
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

        public static T FindInScope<T>(this IGLife life, Predicate<T> predicate = null) where T : IGLife
        {
            if (life.Scope != null)
            {
                foreach (var lifeInScope in life.Scope.Lives)
                {
                    if (lifeInScope is T target && (predicate == null || predicate(target)))
                    {
                        return target;
                    }
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
    }
}
