using GRT.Events.Triggers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.GComponents
{
    /// <summary>
    /// 上下文菜单组件
    /// </summary>
    public class GContextMenu : IGComponent
    {
        public delegate void ContextMenuPopupHandler(GContextMenu sender, Vector3 pos);

        /// <summary>
        /// 弹出上下文菜单事件
        /// </summary>
        public static event ContextMenuPopupHandler Popupping;

        /// <summary>
        /// 上下文菜单项列表
        /// </summary>
        public IList<Item> Menu { get; } = new List<Item>();

        public IGEntity GEntity { get; set; }

        public void Binding(GameObject uObject)
        {
            var collider = GEntity.GetComponent<GCollider>();
            if (collider != null)
            {
                var trigger = collider.Collider.gameObject.AddComponent<PointerClickTrigger>();
                trigger.InnerTrigger = new MouseButtonTrigger() { button = 1 };
                trigger.Event.AddListener((camera, hit, pos) =>
                {
                    Popupping?.Invoke(this, hit.point);
                });
            }
        }

        /// <summary>
        /// 菜单项
        /// </summary>
        public class Item
        {
            /// <summary>
            /// 菜单项名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 菜单项事件
            /// </summary>
            public event Action Action;

            /// <summary>
            /// 菜单项事件是否可执行的判断条件
            /// </summary>
            public event Func<bool> Condition;

            /// <summary>
            /// 菜单项事件是否可执行
            /// </summary>
            public bool Executable => Condition == null || Condition();

            /// <summary>
            /// 执行菜单项事件
            /// </summary>
            public void Execute()
            {
                if (Executable)
                {
                    Action?.Invoke();
                }
            }
        }
    }
}