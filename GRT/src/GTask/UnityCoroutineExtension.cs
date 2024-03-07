using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace GRT.GTask
{
    public static class UnityCoroutineExtension
    {
        public static IEnumeratorAwaiter GetAwaiter(this IEnumerator enumerator)
        {
            var awaiter = new IEnumeratorAwaiter();

            if (SynchronizationContext.Current == GCoroutine.UnityContext)
            {
                GCoroutine.YieldThen(enumerator, () => awaiter.Complete());
            }
            else
            {
                GCoroutine.UnityContext.Post(_ =>
                {
                    GCoroutine.YieldThen(enumerator, () => awaiter.Complete());
                }, null);
            }

            return awaiter;
        }

        public static IEnumeratorAwaiter GetAwaiter(this YieldInstruction instruction)
        {
            var awaiter = new IEnumeratorAwaiter();
            if (SynchronizationContext.Current == GCoroutine.UnityContext)
            {
                GCoroutine.YieldThen(instruction, () => awaiter.Complete());
            }
            else
            {
                GCoroutine.UnityContext.Post(_ =>
                {
                    GCoroutine.YieldThen(instruction, () => awaiter.Complete());
                }, null);
            }

            return awaiter;
        }
    }

    public class IEnumeratorAwaiter : IAwaiter
    {
        private bool _isCompleted;

        private Action _continuation;

        public bool IsCompleted => _isCompleted;

        public void GetResult()
        {
            Debug.Assert(_isCompleted);
        }

        public void OnCompleted(Action continuation)
        {
            Debug.Assert(_continuation == null);
            Debug.Assert(!_isCompleted);

            _continuation = continuation;
        }

        public void Complete()
        {
            _isCompleted = true;

            _continuation?.Invoke();
        }
    }
}