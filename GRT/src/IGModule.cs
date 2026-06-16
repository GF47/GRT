using System;
using System.Threading.Tasks;

namespace GRT
{
    public interface IGModule<T> : IGLife, IGStartable, IGDisposable where T : IGModule<T>
    {
        void Init(IGModuleHolder<T> holder);
    }

    public interface IGModuleHolder<T> where T : IGModule<T>
    {
        T GModule { get; }
    }

    public static class GModuleExtensions<T> where T : class, IGModule<T>
    {
        private static readonly WeakReference<T> _reference = new WeakReference<T>(null);

        public static T Instance => _reference.TryGetTarget(out var module) ? module : null;

        public static T Init(IGScope scope, IGModuleHolder<T> holder, Action<T> initializing = null)
        {
            if (_reference.TryGetTarget(out var module))
            {
                throw new InvalidOperationException($"singleton module {nameof(T)} already exist");
            }
            else
            {
                module = Activator.CreateInstance<T>();
                initializing?.Invoke(module);
                module.Init(holder);
                module.AttachToScope(scope, true);

                _reference.SetTarget(module);

                return module;
            }
        }

        public static void Deinit()
        {
            if (_reference.TryGetTarget(out var module))
            {
                module.DetachFromScope(true);

                _reference.SetTarget(null);
            }
        }
    }

    /**************************************************************/

    #region async module

    public interface IGModuleAsync<T> : IGLife, IGStartable, IGDisposable where T : IGModuleAsync<T>
    {
        Task InitAsync(IGModuleHolderAsync<T> holder);
    }

    public interface IGModuleHolderAsync<T> where T : IGModuleAsync<T>
    {
        T GModule { get; }
    }

    public static class GModuleAsyncExtensions<T> where T : class, IGModuleAsync<T>
    {
        private static readonly WeakReference<T> _reference = new WeakReference<T>(null);

        public static T Instance => _reference.TryGetTarget(out var module) ? module : null;

        public static async Task<T> InitAsync(IGScope scope, IGModuleHolderAsync<T> holder, Action<T> initializing = null)
        {
            if (_reference.TryGetTarget(out var module))
            {
                throw new InvalidOperationException($"singleton module {nameof(T)} already exist");
            }
            else
            {
                module = Activator.CreateInstance<T>();
                initializing?.Invoke(module);
                await module.InitAsync(holder);
                module.AttachToScope(scope);

                _reference.SetTarget(module);

                return module;
            }
        }

        public static void Deinit()
        {
            if (_reference.TryGetTarget(out var module))
            {
                module.DetachFromScope(true);

                _reference.SetTarget(null);
            }
        }
    }

    #endregion async module
}