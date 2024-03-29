﻿using System;
using System.Collections.Generic;

namespace GRT
{
    /// <summary>
    /// 简单的池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pool<T>
    {
        private Queue<T> _queue;
        private Func<T> _constructor;

        public event Action<T> Creating;
        public event Action<T> Getting;
        public event Action<T> Releasing;
        public event Action<T> Disposing;

        /// <summary>
        /// 池初始化，根据传入的实例化方法来生成新的实例
        /// </summary>
        /// <param name="count">初始数量</param>
        /// <param name="constructor">生成一个新实例的方法</param>
        public void Initialize(int count, Func<T> constructor)
        {
            _queue = new Queue<T>(count);
            _constructor = constructor;
            for (int i = 0; i < count; i++)
            {
                T item = _constructor();
                _queue.Enqueue(item);

                Creating?.Invoke(item);
            }
        }

        /// <summary>
        /// 从池中获取一个实例
        /// </summary>
        /// <param name="callback">获取实例时对其进行必要的处理</param>
        public T Get(Action<T> callback = null)
        {
            T item;
            if (_queue.Count == 0)
            {
                item = _constructor();
                callback?.Invoke(item);
                Getting?.Invoke(item);
                return item;
            }
            item = _queue.Dequeue();
            callback?.Invoke(item);
            Getting?.Invoke(item);
            return item;
        }

        /// <summary>
        /// 从池中获取指定数量的实例
        /// </summary>
        /// <param name="count">需要获取的数量</param>
        /// <param name="callback">获取实例时对其进行必要的处理</param>
        /// <returns>取出的实例数组</returns>
        public T[] Get(int count, Action<T> callback = null)
        {
            T[] result = new T[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = Get(callback);
            }
            return result;
        }

        /// <summary>
        /// 将实例释放，返还给池
        /// </summary>
        /// <param name="item">被释放的实例</param>
        /// <param name="callback">实例被返还回池时对其进行必要的处理</param>
        public void Release(T item, Action<T> callback = null)
        {
            if (item == null)
            {
#if DEBUG
                throw new ArgumentNullException("target", "将空值放入了池中");
#else
                return;
#endif
            }
            callback?.Invoke(item);
            Releasing?.Invoke(item);
            _queue.Enqueue(item);
        }

        /// <summary>
        /// 将实例集合释放，返回给池
        /// </summary>
        /// <param name="items">被释放的实例集合</param>
        /// <param name="callback">实例被返还回池时对其进行必要的处理</param>
        public void Release(ICollection<T> items, Action<T> callback = null)
        {
            foreach (T item in items)
            {
                Release(item, callback);
            }
        }

        /// <summary>
        /// 清空池
        /// </summary>
        public void Dispose(Action<T> callback = null)
        {
            foreach (var item in _queue)
            {
                callback?.Invoke(item);
                Disposing?.Invoke(item);
            }

            _queue.Clear();
        }
    }

    public class PoolWithCache<T>
    {
        private Queue<T> _queue;
        private List<T> _cache;
        private Func<T> _constructor;

        public event Action<T> Creating;
        public event Action<T> Getting;
        public event Action<T> Releasing;
        public event Action<T> Disposing;

        public void Initialize(int count, Func<T> constructor)
        {
            _queue = new Queue<T>(count);
            _cache = new List<T>(count);
            _constructor = constructor;
            for (int i = 0; i < count; i++)
            {
                var item = _constructor();
                _queue.Enqueue(item);
                Creating?.Invoke(item);
            }
        }

        public T Get(Action<T> callback = null)
        {
            var item = _queue.Count > 0 ? _queue.Dequeue() : _constructor();
            callback?.Invoke(item);
            Getting?.Invoke(item);
            _cache.Add(item);
            return item;
        }

        public void Release(T item, Action<T> callback = null)
        {
            if (item == null)
            {
#if DEBUG
                throw new ArgumentNullException(nameof(item), "将空值放入了池中");
#else
                return;
#endif
            }

            _cache.Remove(item);
            callback?.Invoke(item);
            Releasing?.Invoke(item);
            _queue.Enqueue(item);
        }

        public void ReleaseAll(Action<T> callback = null)
        {
            foreach (var item in _cache)
            {
                callback?.Invoke(item);
                Releasing?.Invoke(item);
                _queue.Enqueue(item);
            }

            _cache.Clear();
        }

        public void Dispose(Action<T> callback = null)
        {
            foreach (var item in _cache)
            {
                callback?.Invoke(item);
                Disposing?.Invoke(item);
            }
            _cache.Clear();

            foreach (var item in _queue)
            {
                callback?.Invoke(item);
                Disposing?.Invoke(item);
            }
            _queue.Clear();
        }

        public bool HasAlive(out T itemAlive, Predicate<T> predicate = null)
        {
            foreach (var item in _cache)
            {
                if (predicate == null || predicate(item))
                {
                    itemAlive = item;
                    return true;
                }
            }

            itemAlive = default;
            return false;
        }
    }
}
