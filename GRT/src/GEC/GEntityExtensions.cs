﻿namespace GRT.GEC
{
    public static class GEntityExtensions
    {
        public static TC GetComponent<T, TE, TC>(this IGEntity<T, TE> entity)
            where T : class
            where TE : IGEntity<T, TE>
            where TC : IGComponent<T, TE>
        {
            foreach (var com in entity.Components)
            {
                if (com is TC tc)
                {
                    return tc;
                }
            }
            return default;
        }

        public static void AddComponent<T, TE>(this TE entity, IGComponent<T, TE> com)
            where T : class
            where TE : IGEntity<T, TE>
        {
            if (!entity.Components.Contains(com))
            {
                entity.Components.Add(com);
                com.Entity = entity;
            }
        }

        public static bool TryGetComponent<T, TE, TC>(this IGEntity<T, TE> entity, out TC component)
            where T : class
            where TE : IGEntity<T, TE>
            where TC : IGComponent<T, TE>
        {
            foreach (var com in entity.Components)
            {
                // if (type.isAssignableFrom(com.GetType()))
                if (com is TC t)
                {
                    component = t;
                    return true;
                }
            }
            component = default;
            return false;
        }
    }
}