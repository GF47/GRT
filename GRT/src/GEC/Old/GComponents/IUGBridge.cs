using System;
using UnityEngine;

namespace GRT.GComponents
{
    /// <summary>
    /// 两种组件的桥接接口, 一般用于功能组件与3D引擎的交互, 以及查找组件
    /// </summary>
    [Obsolete("see GRT.GEC")]
    public interface IUGBridge<U, G>
        where U : Component
        where G : IGComponent
    {
        /// <summary>
        /// Unity 对应的组件
        /// </summary>
        U UComponent { get; }
        /// <summary>
        /// 组件
        /// </summary>
        G GComponent { get; }

        /// <summary>
        /// 进行桥接
        /// </summary>
        void Bridge(U u, G g);
    }
}