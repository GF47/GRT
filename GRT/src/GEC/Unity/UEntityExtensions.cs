using UnityEngine;

namespace GRT.GEC.Unity
{
    public static class UEntityExtensions
    {
        public static bool TryGetPoppet(this IProvider<UEntity> provider, out GameObject poppet)
        {
            poppet = provider?.Ware?.Puppet;
            return poppet != null;
        }

        public static void ProvideToComponents(this IProvider<UEntity> provider)
        {
            foreach (var com in provider.Ware.Components)
            {
                if (com is IUser<UEntity> user)
                {
                    Notary<UEntity>.Notarize(provider, user);
                }
            }
        }
    }
}