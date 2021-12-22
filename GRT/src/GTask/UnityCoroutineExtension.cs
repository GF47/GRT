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

            if (SynchronizationContext.Current == Coroutines.UnityContext)
            {
                Coroutines.StartACoroutineWithCallback(enumerator, () =>
                {
                    awaiter.Complete();
                });
            }
            else
            {
                Coroutines.UnityContext.Post(_ =>
                {
                    Coroutines.StartACoroutineWithCallback(enumerator, () =>
                    {
                        awaiter.Complete();
                    });
                }, null);
            }

            return awaiter;
        }
    }

    public class IEnumeratorAwaiter : IAwaiter
    {
        bool _isCompleted;

        Action _continuation;

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
        }
    }
}
