using UnityEngine;

namespace GRT.GEC.Unity
{
    public static class UEntityExtensions
    {
        public static bool TryGetPuppet(this IProvider<UEntity> provider, out GameObject puppet)
        {
            puppet = provider?.Ware?.Puppet;
            return puppet != null;
        }

        public static GameObject FindPuppet(this UEntity entity)
        {
            if (entity == null) return null;

            entity.Puppet = GameObjectExtension.FindByLocation(entity.Location);

            return entity.Puppet;
        }
    }
}