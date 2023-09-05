using UnityEngine;

namespace GRT.GEC.Unity
{
    public class GCollider : IGComponent<GameObject, UEntity>, IUser<UEntity>
    {
        public UEntity GEntity { get; set; }

        public string CustomLocation { get; set; }

        public int Layer { get; set; }

        public Collider Collider { get; private set; }

        public IProvider<UEntity> Provider { get; private set; }

        public bool Use(IProvider<UEntity> provider)
        {
            bool ratify;
            GameObject go;

            var entity = provider.Ware;
            if (string.IsNullOrEmpty(CustomLocation) || GameObjectExtension.IsSameLocation(CustomLocation, entity.Location))
            {
                Provider = provider;
                ratify = true;
                go = entity.Puppet;
            }
            else
            {
                ratify = false;
                go = GameObjectExtension.FindByLocation(CustomLocation);
            }

            if (go.isStatic)
            {
                throw new UnityException($"{CustomLocation ?? entity.Location} is static, you can not use the static collider");
            }

            Collider = go.GetComponent<Collider>();

            if (Collider == null)
            {
                var box = go.AddComponent<BoxCollider>();
                box.ResizeToWrapChildren();

                Collider = box;
            }

            if (-1 < Layer && Layer < 32)
            {
                Collider.gameObject.layer = Layer;
            }
            Collider.gameObject.AddComponent<GColliderContainer>().Connect(this);

            return ratify;
        }

        public void Release()
        {
            Collider = null;
            Provider = null;
        }
    }

    public class GColliderContainer : UBehaviour<GCollider> { }
}