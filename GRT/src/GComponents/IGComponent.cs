using UnityEngine;

namespace GRT.GComponents
{
    /// <summary>
    /// 组件, 实现自定义功能
    /// </summary>
    public interface IGComponent
    {
        /// <summary>
        /// 组件所在的实体
        /// </summary>
        IGEntity GEntity { get; set; }

        /// <summary>
        /// 实体与 Unity 物体绑定
        /// </summary>
        /// <param name="uObject">Unity 物体</param>
        void Binding(GameObject uObject);
    }
}