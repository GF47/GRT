namespace GRT.GInventory
{
    public static class __DefinitionExtensions
    {
        public static void SetProperty(this IDefinition definition, string name, object value)
        {
            if (definition.Properties.ContainsKey(name)) { definition.Properties[name] = value; }
            else { definition.Properties.Add(name, value); }
        }

        public static bool HasProperty(this IDefinition definition, string name, out object value)
            => definition.Properties.TryGetValue(name, out value);

        public static object GetProperty(this IDefinition definition, string name)
            => definition.HasProperty(name, out var value) ? value : default;

        public static void SetIcon(this IDefinition definition, string icon)
            => definition.SetProperty(Keywords.ICON, icon);

        public static bool HasIcon(this IDefinition definition, out string icon)
        {
            if (definition.Properties.TryGetValue(Keywords.ICON, out var value) && value != null)
            {
                icon = value.ToString();
                return true;
            }
            else
            {
                icon = default;
                return false;
            }
        }

        public static string GetIcon(this IDefinition definition)
            => definition.HasIcon(out var icon) ? icon : default;

        public static void SetPrototype(this IDefinition definition, string prototype)
            => definition.SetProperty(Keywords.PROTOTYPE, prototype);

        public static bool HasPrototype(this IDefinition definition, out string prototype)
        {
            if (definition.Properties.TryGetValue(Keywords.PROTOTYPE, out var value) && value != null)
            {
                prototype = value.ToString();
                return true;
            }
            {
                prototype = default;
                return false;
            }
        }

        public static string GetPrototype(this IDefinition definition)
            => definition.HasPrototype(out var prototype) ? prototype : default;

        // public static int GetDose(this IDefinition definition, int @default = 1) =>
        //     definition.Properties.TryGetValue(Keywords.DOSE, out var value) && value is int dose ? dose : @default;
    }
}