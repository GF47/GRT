using System;

namespace GRT.GComponents
{
    public static class GComponentsUtil
    {
        /// <summary>
        /// 查找指定类型的组件
        /// </summary>
        /// <typeparam name="T">限定为 IGComponent</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>匹配到的指定类型组件</returns>
        public static T GetComponent<T>(this IGEntity entity) where T : IGComponent
        {
            foreach (var com in entity.Components)
            {
                if (com is T t)
                {
                    return t;
                }
            }
            return default;
        }

        /// <summary>
        /// 查找指定类型的组件, 非泛型
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="type">限定为 IGComponent</param>
        /// <returns>匹配到的指定类型组件</returns>
        public static IGComponent GetComponent(this IGEntity entity, Type type)
        {
            foreach (var com in entity.Components)
            {
                if (com.GetType() == type)
                {
                    return com;
                }
            }
            return default;
        }

        /// <summary>
        /// 为实体添加组件, 不会重复添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="com">被添加的组件</param>
        public static void AddComponent(this IGEntity entity, IGComponent com)
        {
            if (!entity.Components.Contains(com))
            {
                entity.Components.Add(com);
                com.GEntity = entity;
            }
        }

        /// <summary>
        /// 字符串是否为 [场景名:物体层级路径] 的格式
        /// </summary>
        public static bool ContainSceneNameInPath(this string str, out string scene, out string path) => str.CanBeSplitBy(':', out scene, out path);
    }
}