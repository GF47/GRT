namespace GRT
{
    public interface IGProperty<T>
    {
        string Name { get; }

        bool TryGet(out T value);

        bool TrySet(T value);
    }

    public static class GPropertyExtensions
    {
        public static T Get<T>(this IGProperty<T> property, T @default = default) => property.TryGet(out var t) ? t : @default;
    }

    public class Readonly<T> : IGProperty<T>
    {
        private bool _initialized = false;

        private T _value;

        public string Name { get; private set; }

        public Readonly(string name) => Name = name;

        public bool TryGet(out T value)
        {
            value = _value;
            return _initialized;
        }

        public bool TrySet(T value)
        {
            if (_initialized)
            {
                return false;
            }
            else
            {
                _value = value;
                _initialized = true;
                return true;
            }
        }

        public static implicit operator T(Readonly<T> property) => property.Get();
    }
}