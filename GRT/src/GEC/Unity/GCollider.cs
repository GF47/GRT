using UnityEngine;

namespace GRT.GEC.Unity
{
    public class GCollider : IGComponent<GameObject>, IBorrower<UEntity>
    {
        public IGEntity<GameObject> GEntity { get; set; }

        public string CustomLocation { get; set; }

        public int Layer { get; set; }

        public Collider Collider { get; private set; }

        public ILender<UEntity> Lender { get; private set; }

        public void Borrow(ILender<UEntity> lender)
        {
            Lender = lender;

            var ware = Lender.Wares;

            GameObject colliderGO;
            if (GameObjectExtension.IsSameGameObjectLocation(CustomLocation, ware.Location) && ware.Reference.TryGetTarget(out var go))
            {
                colliderGO = go;
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

            Collider = colliderGO.GetComponent<Collider>();

            if (Collider == null)
            {
                var box = colliderGO.AddComponent<BoxCollider>();
                box.ResizeToWrapChildren();

                Collider = box;
            }

            Collider.gameObject.layer = Layer;
            Collider.gameObject.AddComponent<UComponent<GCollider>>().Connect(this);
        }

        public UEntity Return()
        {
            var ware = Lender.Wares;
            Lender = null;
            return ware;
        }
    }
}