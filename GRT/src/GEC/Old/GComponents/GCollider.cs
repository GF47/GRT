using System;
using UnityEngine;

namespace GRT.GComponents
{
    [Obsolete("see GRT.GEC")]
    public class GCollider : IGComponent
    {
        /// <summary>
        /// 静态物体是否可以绑定, 有的时候静态物体绑定碰撞体会崩溃
        /// </summary>
        public static bool StaticIsValid { get; set; } = true;

        /// <summary>
        /// 碰撞体所在场景名
        /// </summary>
        private string _scene;
        /// <summary>
        /// 碰撞体物体的层级路径
        /// </summary>
        private string _path;

        /// <summary>
        /// 碰撞体
        /// </summary>
        public Collider Collider { get; set; }

        public IGEntity GEntity { get; set; }

        /// <summary>
        /// Unity 物体所在的层级, 一般用于物理检测
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// 碰撞体所在的场景名, 如果不指定, 则返回实体所在的场景名
        /// </summary>
        public string Scene { get => _scene ?? GEntity.Scene; set => _scene = value; }

        /// <summary>
        /// 碰撞体的层级路径, 如果不指定, 则返回实体的层级路径
        /// </summary>
        public string Path { get => _path ?? GEntity.Path; set => _path = value; }

        public void Binding(GameObject uObject)
        {
            // 如果不专门指定碰撞体的路径, 则直接使用实体自身
            // 有时候碰撞体并不能等同于渲染组件
            var target = Path == GEntity.Path ? uObject : GameObjectExtension.FindIn(Scene, Path);

            if (target != null)
            {
                // 判断是否为静态物体, 由于静态物体有可能会在运行期合并mesh, 所以会出现一些问题, 甚至导致崩溃
                if (target.isStatic && !StaticIsValid)
                {
                    throw new UnityException($"{Scene}:{Path}--->{target} is static, you can not change a static collider");
                }

                Collider = target.GetComponentInChildren<Collider>();
                if (Collider == null)
                {
                    var box = target.AddComponent<BoxCollider>();
                    ColliderAutoCalculateBounds(box);

                    Collider = box;
                }

                Collider.gameObject.layer = Layer;

                var bridge = Collider.gameObject.AddComponent<GColliderBridge>();
                bridge.Bridge(Collider, this);
            }
        }

        /// <summary>
        /// 计算所有子级渲染组件的边界框, 并用指定的盒形碰撞体进行包裹
        /// </summary>
        /// <param name="box"></param>
        private static void ColliderAutoCalculateBounds(BoxCollider box)
        {
            var center = Vector3.zero;
            var renderers = box.gameObject.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                center += renderers[i].bounds.center;
            }
            center /= renderers.Length;

            var bounds = new Bounds(center, Vector3.zero);
            for (int i = 0; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            box.center = box.transform.worldToLocalMatrix.MultiplyPoint(bounds.center); // bounds.center - box.transform.position; 这个没有旋转
            box.size = box.transform.worldToLocalMatrix.MultiplyVector(bounds.size);
        }

        /// <summary>
        /// 组件与 Unity 碰撞体组件的桥接
        /// </summary>
        public class GColliderBridge : Component, IUGBridge<Collider, GCollider>
        {
            public Collider UComponent { get; private set; }

            public GCollider GComponent { get; private set; }

            public void Bridge(Collider u, GCollider g)
            {
                UComponent = u;
                GComponent = g;
            }
        }
    }
}