using System;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public class GCollider : IGComponent<GameObject>, ILoadable<GameObject>
    {
        public IGEntity<GameObject> GEntity { get; set; }

        public string CustomLocation { get; set; }

        public int Layer { get; set; }

        public Collider Collider => _gColliderRef == null ? null : _gColliderRef.TryGetTarget(out var collider) ? collider : null;

        private WeakReference<Collider> _gColliderRef;

        public void Load(GameObject target)
        {
            GameObject colliderGO;
            if (GameObjectExtension.IsSameGameObjectLocation(CustomLocation, GEntity.Location))
            {
                colliderGO = target;
            }
            else
            {
                colliderGO = CustomLocation.CanBeSplitBy(':', out var scene, out var path)
                    ? GameObjectExtension.FindIn(scene, path)
                    : GameObjectExtension.FindIn(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), path);
            }

            if (colliderGO.isStatic)
            {
                throw new UnityException($"{CustomLocation} is static, you can not use the static collider");
            }

            var collider = colliderGO.GetComponent<Collider>();

            if (collider == null)
            {
                var box = colliderGO.AddComponent<BoxCollider>();
                box.ResizeToWrapChildren();

                collider = box;
            }

            collider.gameObject.layer = Layer;
            collider.gameObject.AddComponent<UComponent<GCollider>>().Attach(this);

            _gColliderRef = new WeakReference<Collider>(collider);
        }

        public GameObject Unload() => Collider?.gameObject; // 没有做还原处理
    }
}