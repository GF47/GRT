namespace GRT
{
    public interface IBlackBoard
    {
        T Get<T>(string name, T @default = default);
        bool Get<T>(string name, out T value, T @default = default);
        void Set<T>(string name, T value);
    }
}
