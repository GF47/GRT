using System;
using System.Collections.Generic;

namespace GRT
{
    public class Pool<T> where T : class 
    {
        private Queue<T> _queue;
        private Func<T> _createNewFunc;

        public void Initialize(int count, Func<T> createNewFunc)
        {
            _queue = new Queue<T>(count);
            _createNewFunc = createNewFunc;
            for (int i = 0; i < count; i++)
            {
                T item = _createNewFunc();
                _queue.Enqueue(item);
            }
        }

        public T Get(Action<T> callback = null)
        {
            T item;
            if (_queue.Count == 0)
            {
                item = _createNewFunc();
                callback?.Invoke(item); return item;
            }
            item = _queue.Dequeue();
            callback?.Invoke(item); return item;
        }

        public T[] Get(int count, Action<T> callback = null)
        {
            T[] result = new T[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = Get(callback);
            }
            return result;
        }

        public void Reset(T target, Action<T> callback = null)
        {
            if (target == null)
            {
#if DEBUG
                throw new ArgumentNullException("target", "将空值放入了池中");
#else
                return;
#endif
            }
            callback?.Invoke(target); _queue.Enqueue(target);
        }

        public void Reset(ICollection<T> targets, Action<T> callback = null)
        {
            foreach (T target in targets)
            {
                Reset(target, callback);
            }
        }
    }
}