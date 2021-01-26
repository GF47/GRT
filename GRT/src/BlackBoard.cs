using System.Collections;
using System.Collections.Generic;

namespace GRT
{
    public class BlackBoard : IBlackBoard, IEnumerable<KeyValuePair<string, object>>
    {
        private readonly Dictionary<string, object> _items;

        public BlackBoard()
        {
            _items = new Dictionary<string, object>();
        }

        public T Get<T>(string name, T @default = default)
        {
            object v = null;
            var has = _items.ContainsKey(name) && (v = _items[name]) is T;
            return has ? (T)v : @default;
        }

        public bool Get<T>(string name, out T value, T @default = default)
        {
            object v = null;
            var has = _items.ContainsKey(name) && (v = _items[name]) is T;
            value = has ? (T)v : @default;
            return has;
        }

        public void Set<T>(string name, T value)
        {
            if (_items.ContainsKey(name)) { _items[name] = value; }
            else { _items.Add(name, value); }
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_items).GetEnumerator();
        }
    }
}
