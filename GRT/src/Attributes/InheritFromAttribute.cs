/***************************************************************
 * @File Name       : InheritFromAttribute
 * @Author          : GF47
 * @Description     : 指定继承关系的属性
 * @Date            : 2017/6/8/星期四 14:37:33
 * @Edit            : none
 **************************************************************/

using System;
using UnityEngine;

namespace GRT
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InheritFromAttribute : PropertyAttribute
    {
        public Type baseType;

        public InheritFromAttribute(Type type)
        {
            baseType = type;
        }
    }
}
