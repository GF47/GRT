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

        public static void ProvideToComponents(this IProvider<UEntity> provider)
        {
            foreach (var com in provider.Ware.Components)
            {
                if (com is IConsumer<UEntity> consumer)
                {
                    Contract<UEntity>.Notarize(provider, consumer);
                }
            }
        }
    }
}