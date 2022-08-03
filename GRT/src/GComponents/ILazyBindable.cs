namespace GRT.GComponents
{
    /// <summary>
    /// 延迟绑定接口, 通常系统加载后需要等待Unity场景初始化完毕, 所以需要延迟等待
    /// </summary>
    public interface ILazyBindable
    {
        /// <summary>
        /// 实体
        /// </summary>
        IGEntity GEntity { get; set; }

        /// <summary>
        /// Unity 物体的层级路径
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// 绑定
        /// </summary>
        void Bind();
    }
}