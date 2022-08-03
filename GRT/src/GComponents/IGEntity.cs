using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.GComponents
{
    /// <summary>
    /// 实体, 特定功能组件的集合, 以及所绑定的 Unity 物体
    /// </summary>
    public interface IGEntity : ILazyBindable
    {
        /// <summary>
        /// 实体绑定时触发事件
        /// </summary>
        event Action<GameObject> Binding;

        /// <summary>
        /// 实体所在的场景名
        /// </summary>
        string Scene { get; set; }

        /// <summary>
        /// 实体所对应的 Unity 物体
        /// </summary>
        GameObject UObject { get; }

        /// <summary>
        /// 组件列表
        /// </summary>
        IList<IGComponent> Components { get; }
    }
}