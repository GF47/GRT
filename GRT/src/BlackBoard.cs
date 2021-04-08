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
            object v = null;
            var has = items.ContainsKey(name) && (v = items[name]) is T;
            return has ? (T)v : @default;
        }

        public bool Get<T>(string name, out T value, T @default = default)
        {
            object v = null;
            var has = items.ContainsKey(name) && (v = items[name]) is T;
            value = has ? (T)v : @default;
            return has;
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
