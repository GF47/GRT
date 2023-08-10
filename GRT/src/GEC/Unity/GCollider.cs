using UnityEngine;
using System;

namespace GRT.GEC.Unity
{
    public class GCollider : IGComponent<GameObject>, IBorrower<UEntity>
    {
        public IGEntity<GameObject> GEntity { get; set; }

        public string CustomLocation { get; set; }

        public int Layer { get; set; }

        public Collider Collider { get; private set; }

        public ILender<UEntity> Lender { get; private set; }

        public bool Borrow(ILender<UEntity> lender)
        {
            bool deal;
            GameObject colliderGO;

            if (string.IsNullOrEmpty(CustomLocation) || GameObjectExtension.IsSameLocation(CustomLocation, lender.Wares.Location))
            {
                if (lender.Wares.Reference.TryGetTarget(out var go))
                {
                    Lender = lender;
                    deal = true;

                    colliderGO = go;
                }
                else
                {
                    throw new Exception($"can not find the {lender.Wares.Location}");
                }
            }
            else
            {
                deal = false;

                colliderGO = GameObjectExtension.FindByLocation(CustomLocation);
            }

            if (colliderGO.isStatic)
            {
                throw new UnityException($"{CustomLocation ?? lender.Wares.Location} is static, you can not use the static collider");
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

        public void Repay()
        {
            Lender = null;
        }
    }

    public class GColliderContainer : UBehaviour<GCollider> { }
}