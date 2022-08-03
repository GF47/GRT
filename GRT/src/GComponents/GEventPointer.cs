using GRT.Events;
using UnityEngine;

namespace GRT.GComponents
{
    /// <summary>
    /// 管理指针进入, 悬浮, 离开触发事件的组件
    /// </summary>
    public class GEventPointer : IGComponent
    {
        public delegate void GEventPointerHandler(GEventPointer sender, GEventArgs e);

        /// <summary>
        /// 全局指针进入事件
        /// </summary>
        public static event GEventPointerHandler GlobalEntering;

        /// <summary>
        /// 全局指针离开事件
        /// </summary>
        public static event GEventPointerHandler GlobalExiting;

        /// <summary>
        /// 全局指针悬浮事件
        /// </summary>
        public static event GEventPointerHandler GlobalHovering;

        /// <summary>
        /// 指针进入事件
        /// </summary>
        public event GnityEventHandler Entering;

        /// <summary>
        /// 指针离开事件
        /// </summary>
        public event GnityEventHandler Exiting;

        /// <summary>
        /// 指针悬浮事件
        /// </summary>
        public event GnityEventHandler Hovering;

        public IGEntity GEntity { get; set; }

        public void Binding(GameObject uObject)
        {
            var collider = GEntity.GetComponent<GCollider>();
            if (collider != null)
            {
                var trigger = collider.Collider.gameObject.AddComponent<GEventPointerTrigger>();
                trigger.Bridge(trigger, this);
            }
        }

        /// <summary>
        /// 指针事件触发器
        /// </summary>
        public class GEventPointerTrigger : Behaviour, IUGBridge<GEventPointerTrigger, GEventPointer>, IPointerEnter, IPointerExit, IPointerHover
        {
            public GEventPointerTrigger UComponent { get; private set; }

            public GEventPointer GComponent { get; private set; }

            public void Bridge(GEventPointerTrigger u, GEventPointer g)
            {
                UComponent = u;
                GComponent = g;
            }

            public void OnPointerEnter(Camera camera, RaycastHit hit, Vector2 pos)
            {
                GlobalEntering?.Invoke(GComponent, new GEventArgs(camera, hit, pos));
                GComponent.Entering?.Invoke(camera, hit, pos);
            }

            public void OnPointerExit(Camera camera, RaycastHit hit, Vector2 pos)
            {
                GlobalExiting?.Invoke(GComponent, new GEventArgs(camera, hit, pos));
                GComponent.Exiting?.Invoke(camera, hit, pos);
            }

            public void OnPointerHover(Camera camera, RaycastHit hit, Vector2 pos)
            {
                GlobalHovering?.Invoke(GComponent, new GEventArgs(camera, hit, pos));
                GComponent.Hovering?.Invoke(camera, hit, pos);
            }
        }
    }
}