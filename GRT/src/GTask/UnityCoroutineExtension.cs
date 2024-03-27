using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace GRT.GTask
{
    public static class UnityCoroutineExtension
    {
        public static InternalAwaiter GetAwaiter(this IEnumerator enumerator)
        {
            var awaiter = new InternalAwaiter();

            if (SynchronizationContext.Current == GCoroutine.UnityContext)
            {
                GCoroutine.YieldThen(enumerator, () => awaiter.SetComplete());
            }
            else
            {
                GCoroutine.UnityContext.Post(_ =>
                {
                    GCoroutine.YieldThen(enumerator, () => awaiter.SetComplete());
                }, null);
            }

            return awaiter;
        }

        public static InternalAwaiter GetAwaiter(this YieldInstruction instruction)
        {
            var awaiter = new InternalAwaiter();
            if (SynchronizationContext.Current == GCoroutine.UnityContext)
            {
                GCoroutine.YieldThen(instruction, () => awaiter.SetComplete());
            }
            else
            {
                GCoroutine.UnityContext.Post(_ =>
                {
                    GCoroutine.YieldThen(instruction, () => awaiter.SetComplete());
                }, null);
            }

            return awaiter;
        }

        public static InternalAwaiter GetAwaiter(this (IEnumerator, Func<bool>) awaitable)
        {
            var (enumerator, predicate) = awaitable;
            var awaiter = new InternalAwaiter();

            if (SynchronizationContext.Current == GCoroutine.UnityContext)
            {
                GCoroutine.YieldThen(enumerator, () => awaiter.SetComplete(predicate));
            }
            else
            {
                GCoroutine.UnityContext.Post(_ =>
                {
                    GCoroutine.YieldThen(enumerator, () => awaiter.SetComplete(predicate));
                }, null);
            }

            return awaiter;
        }

        public static InternalAwaiter GetAwaiter(this (YieldInstruction, Func<bool>) awaitable)
        {
            var (instruction, predicate) = awaitable;
            var awaiter = new InternalAwaiter();

            if (SynchronizationContext.Current == GCoroutine.UnityContext)
            {
                GCoroutine.YieldThen(instruction, () => awaiter.SetComplete(predicate));
            }
            else
            {
                GCoroutine.UnityContext.Post(_ =>
                {
                    GCoroutine.YieldThen(instruction, () => awaiter.SetComplete(predicate));
                }, null);
            }

            return awaiter;
        }
    }

    public class InternalAwaitable : IAwaitable<IAwaiter>
    {
        private readonly IAwaiter _awaiter;

        public InternalAwaitable(IAwaiter awaiter) => _awaiter = awaiter;

        public IAwaiter GetAwaiter() => _awaiter;
    }

    public class InternalAwaiter : IAwaiter
    {
        private bool _isCompleted;

        private Action _continuation;

        public bool IsCompleted => _isCompleted;

        public void GetResult() => Debug.Assert(_isCompleted);

        public void OnCompleted(Action continuation)
        {
            Debug.Assert(_continuation == null);
            Debug.Assert(!_isCompleted);

            _continuation = continuation;
        }

        public void SetComplete(Func<bool> predicate = null)
        {
            _isCompleted = predicate == null || predicate();

            if (_isCompleted)
            {
                _continuation?.Invoke();
            }
            else
            {
#if UNITY_EDITOR
                throw new UnityException($"{predicate.Method.Name} is not true");
#else
                throw new UnityException("predicate is not true");
#endif
            }
        }
    }
}