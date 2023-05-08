/***************************************************************
 *@File Name     : InspectorAliasAttribute.cs
 *@Author        : GF47
 *@Description   : 字段自定义 Inspector 显示
 *@Data          : 2020-11-10 15:22
 *@Edit          : none
 **************************************************************/

namespace GRT
{
    using System;
    using UnityEngine;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class InspectorAliasAttribute : PropertyAttribute
    {
        public InspectorAliasAttribute(string name, string tips = "")
        {
            Name = name;
            Tips = tips;
        }

        public string Name { get; }
        public string Tips { get; }
    }
}
