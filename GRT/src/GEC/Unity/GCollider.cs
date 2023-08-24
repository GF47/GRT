using UnityEngine;

namespace GRT.GEC.Unity
{
    public class GCollider : IGComponent<GameObject>, IUser<GameObject>
    {
        public IGEntity<GameObject> GEntity { get; set; }

        public string CustomLocation { get; set; }

        public int Layer { get; set; }

        public Collider Collider { get; private set; }

        public IProvider<GameObject> Provider { get; private set; }

        public bool Use(IProvider<GameObject> provider)
        {
            bool deal;
            GameObject colliderGO;

            var entity = provider as IGEntity<GameObject>;
            if (string.IsNullOrEmpty(CustomLocation) || GameObjectExtension.IsSameLocation(CustomLocation, entity.Location))
            {
                Provider = entity;
                deal = true;
                colliderGO = entity.Ware;
            }
            else
            {
                deal = false;
                colliderGO = GameObjectExtension.FindByLocation(CustomLocation);
            }

            if (colliderGO.isStatic)
            {
                throw new UnityException($"{CustomLocation ?? entity.Location} is static, you can not use the static collider");
            }

            Collider = colliderGO.GetComponent<Collider>();

            if (Collider == null)
            {
                var box = colliderGO.AddComponent<BoxCollider>();
                box.ResizeToWrapChildren();

                Collider = box;
            }

            if (-1 < Layer && Layer < 32)
            {
                Collider.gameObject.layer = Layer;
            }
            Collider.gameObject.AddComponent<GColliderContainer>().Connect(this);

            return deal;
        }

        public void Release()
        {
            Provider = null;
        }
    }

    public class GColliderContainer : UBehaviour<GCollider>
    { }
}