using System.Collections;
using System.Collections.Generic;

namespace GRT
{
    /// <summary>
    /// 基础公告板，存储或查询键值对
    /// </summary>
    public class BlackBoard : IBlackBoard, IEnumerable<KeyValuePair<string, object>>
    {
        protected readonly Dictionary<string, object> items;

        public BlackBoard()
        {
            items = new Dictionary<string, object>();
        }

        public T Get<T>(string name, T @default = default)
        {
            if (items.TryGetValue(name, out object obj))
            {
                return obj is T tValue ? tValue : @default;
            }
            else
            {
                return @default;
            }
        }

        public bool TryGet<T>(string name, out T value, T @default = default)
        {
            if (items.TryGetValue(name, out object obj))
            {
                if (obj is T tValue)
                {
                    value = tValue;
                    return true;
                }
                else
                {
                    value = @default;
                    return false;
                }
            }
            else
            {
                value = @default;
                return false;
            }
        }

        public T GetFuzzy<T>(T @default, params string[] names)
        {
            foreach (var name in names)
            {
                if (items.TryGetValue(name, out var obj))
                {
                    if (obj is T tValue)
                    {
                        return tValue;
                    }
                }
            }
            return @default;
        }

        public void Set<T>(string name, T value)
        {
            if (items.ContainsKey(name)) { items[name] = value; }
            else { items.Add(name, value); }
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)items).GetEnumerator();
        }
    }
}