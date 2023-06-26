namespace GRT.GEC
{
    public interface ILoadable<T> where T : class
    {
        void Load(T target);

        T Unload();
    }
}