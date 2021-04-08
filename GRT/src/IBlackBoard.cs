namespace GRT
{
    /// <summary>
    /// 公告板，多用于存放数据
    /// </summary>
    public interface IBlackBoard
    {
        /// <summary>
        /// 从公告板中获取指定键名所对应的数据值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="name">数据所对应的键名</param>
        /// <param name="default">如果公告板中无指定键值，则返回的默认值</param>
        T Get<T>(string name, T @default = default);
        /// <summary>
        /// 从公告板中获取指定键名所对应的数据值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="name">数据所对应的键名</param>
        /// <param name="value">获取到的值</param>
        /// <param name="default">如果公告板中无指定键值，则返回的默认值</param>
        /// <returns>公告板中是否有指定的键值</returns>
        bool Get<T>(string name, out T value, T @default = default);
        /// <summary>
        /// 向公告板中存入键值对
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="name">数据所对应的键名</param>
        /// <param name="value">数据值</param>
        void Set<T>(string name, T value);
    }
}
