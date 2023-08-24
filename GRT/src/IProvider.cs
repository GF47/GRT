namespace GRT
{
    /// <summary>
    /// 供应方
    /// </summary>
    /// <typeparam name="T">物品类型</typeparam>
    public interface IProvider<T>
    {
        /// <summary>
        /// 物品
        /// </summary>
        T Ware { get; }

        /// <summary>
        /// 向使用方提供物品
        /// </summary>
        /// <param name="user">使用方</param>
        void Provide(IUser<T> user);
    }

    /// <summary>
    /// 使用方
    /// </summary>
    /// <typeparam name="T">物品类型</typeparam>
    public interface IUser<T>
    {
        /// <summary>
        /// 供应方
        /// </summary>
        IProvider<T> Provider { get; }

        /// <summary>
        /// 尝试使用供应方提供的物品
        /// </summary>
        /// <param name="provider">供应方</param>
        /// <returns>是否保存使用状态</returns>
        bool Use(IProvider<T> provider);

        /// <summary>
        /// 停止使用物品
        /// </summary>
        void Release();
    }

    public static class Notary<T>
    {
        /// <summary>
        /// 使用关系成立
        /// </summary>
        /// <param name="provider">提供方</param>
        /// <param name="user">使用方</param>
        public static void Notarize(IProvider<T> provider, IUser<T> user)
        {
            if (user.Use(provider))
            {
                provider.Provide(user);
            }
        }

        /// <summary>
        /// 停止使用关系
        /// </summary>
        /// <param name="provider">提供方</param>
        /// <param name="user">使用方</param>
        public static void Cancel(IProvider<T> provider, IUser<T> user)
        {
            if (user.Provider == provider)
            {
                user.Release();
            }
        }
    }
}