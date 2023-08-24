using System.Collections.Generic;

namespace GRT.GInventory.DefaultImpl
{
    public class DefaultDefinition : IDefinition
    {
        public int ID { get; private set; } = IDGenerator.Instance.Generate();

        public string Name { get; }

        public string Description { get; }

        public IDictionary<string, object> Properties { get; private set; } = new Dictionary<string, object>();

        public IList<ISkill> Skills { get; private set; } = new List<ISkill>();

        public DefaultDefinition(string name, string desc)
        {
            Name = name;
            Description = desc;
        }
    }
}