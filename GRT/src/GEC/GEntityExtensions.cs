namespace GRT.GEC
{
    public static class GEntityExtensions
    {
        public static TC GetComponent<T, TC>(this IGEntity<T> entity)
            where T : class
            where TC : IGComponent<T>
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

        public static void AddComponent<T>(this IGEntity<T> entity, IGComponent<T> com)
            where T : class
        {
            if (!entity.Components.Contains(com))
            {
                entity.Components.Add(com);
                com.GEntity = entity;
            }
        }
    }
}