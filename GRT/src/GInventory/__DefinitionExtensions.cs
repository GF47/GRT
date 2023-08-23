namespace GRT.GInventory
{
    public static class __DefinitionExtensions
    {
        public static bool HasPrototype(this IDefinition definition, out string prototype)
        {
            if (definition.Properties.TryGetValue(Keywords.PROTOTYPE, out var value))
            {
                prototype = value.ToString();
                return true;
            }
            {
                prototype = null;
                return false;
            }
        }

        public static void SetProperty(this IDefinition definition, string name, object value)
        {
            if (definition.Properties.ContainsKey(name)) { definition.Properties[name] = value; }
            else { definition.Properties.Add(name, value); }
        }

        public static void SetIcon(this IDefinition definition, string icon) =>
            definition.SetProperty(Keywords.ICON, icon);

        public static void SetPrototype(this IDefinition definition, string prototype) =>
            definition.SetProperty(Keywords.PROTOTYPE, prototype);

        public static string GetIcon(this IDefinition definition) =>
            definition.Properties.TryGetValue(Keywords.ICON, out var value) ? value.ToString() : null;

        public static int GetDose(this IDefinition definition) =>
            definition.Properties.TryGetValue(Keywords.DOSE, out var value) && value is int dose ? dose : 1;
    }
}