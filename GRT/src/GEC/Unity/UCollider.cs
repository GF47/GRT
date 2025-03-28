using UnityEngine;

namespace GRT.GEC.Unity
{
    public class UCollider : IGComponent<GameObject, UEntity>, IConsumer<UEntity>
    {
        public UEntity Entity { get; set; }

        public string CustomLocation { get; set; }

        public int Layer { get; set; }

        public Collider RawCollider { get; private set; }

        public IProvider<UEntity> Provider { get; private set; }

        public virtual bool Use(IProvider<UEntity> provider)
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

            RawCollider = go.GetComponent<Collider>();

            if (RawCollider == null)
            {
                var box = go.AddComponent<BoxCollider>();
                box.ResizeToWrapChildren();

                RawCollider = box;
            }

            if (-1 < Layer && Layer < 32)
            {
                RawCollider.gameObject.layer = Layer;
            }
            RawCollider.gameObject.AddComponent<UColliderContainer>().Connect(this);

            return ratify;
        }

        public void Release()
        {
            RawCollider = null;
            Provider = null;
        }
    }

    public class UColliderContainer : UBehaviour<UCollider> { }
}