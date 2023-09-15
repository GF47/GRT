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
        /// <param name="consumer">使用方</param>
        void Provide(IConsumer<T> consumer);
    }

    /// <summary>
    /// 使用方
    /// </summary>
    /// <typeparam name="T">物品类型</typeparam>
    public interface IConsumer<T>
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

    public static class Contract<T>
    {
        /// <summary>
        /// 合同
        /// </summary>
        /// <param name="provider">提供方</param>
        /// <param name="consumer">使用方</param>
        public static bool Notarize(IProvider<T> provider, IConsumer<T> consumer)
        {
            if (consumer.Use(provider))
            {
                provider.Provide(consumer);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 停止使用关系
        /// </summary>
        /// <param name="provider">提供方</param>
        /// <param name="consumer">使用方</param>
        public static void Cancel(IProvider<T> provider, IConsumer<T> consumer)
        {
            if (consumer.Provider == provider)
            {
                consumer.Release();
            }
        }
    }
}