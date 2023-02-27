using GRT.Events;
using GRT.Events.Triggers;
using UnityEngine;

namespace GRT.GComponents
{
    /// <summary>
    /// 可点击组件
    /// </summary>
    public class Clickable : IGComponent
    {
        public delegate void ClickableHandler(Clickable sender, GEventArgs e);

        /// <summary>
        /// 全局点击事件
        /// </summary>
        public static ClickableHandler ClickingAt;

        /// <summary>
        /// 点击事件
        /// </summary>
        public event GnityEventHandler Clicking;

        public IGEntity GEntity { get; set; }

        public void Binding(GameObject uObject)
        {
            var collider = GEntity.GetComponent<GCollider>();
            if (collider != null)
            {
                var trigger = collider.Collider.gameObject.AddComponent<PointerClickTrigger>();
                trigger.InnerTrigger = new MouseButtonTrigger() { button = 0 };
                trigger.Event.AddListener((camera, hit, position) =>
                {
                    ClickingAt?.Invoke(this, new GEventArgs() { camera = camera, raycastHit = hit, position = position, triggerType = GeneralizedTriggerType.OneShot });
                    Clicking?.Invoke(camera, hit, position);
                });
            }
        }
    }
}