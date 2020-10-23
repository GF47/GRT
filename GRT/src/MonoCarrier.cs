/* ****************************************************************
 * @File Name   :   MonoCarrier
 * @Author      :   GF47
 * @Date        :   2015/7/23 15:34:53
 * @Description :   普通类映射到[MonoBehaviour]的承载器
 * @Edit        :   2015/7/23 15:34:53
 * ***************************************************************/

namespace GRT
{
    using UnityEngine;

    /// <summary>
    /// 普通类映射到[MonoBehaviour]的承载器
    /// </summary>
    public abstract class MonoCarrier<T> : MonoBehaviour
    {
        public abstract T Target { get; set; }
    }
}
