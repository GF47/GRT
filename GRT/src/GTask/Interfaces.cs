using System.Runtime.CompilerServices;
using System;

namespace GRT.GTask
{
    public interface IAwaitable<out T> where T : IAwaiter
    {
        T GetAwaiter();
    }

    public interface IAwaitable<out T, out U> where T : IAwaiter<U>
    {
        T GetAwaiter();
    }

    public interface IAwaiter : INotifyCompletion
    {
        bool IsCompleted { get; }

        void GetResult();
    }

    public interface IAwaiter<out T> : INotifyCompletion
    {
        bool IsCompleted { get; }

        T GetResult();
    }
}