using System.Collections.Generic;

namespace GRT.GInventory.DefaultImpl
{
    public class DefaultDefinition : IDefinition
    {
        public int ID { get; private set; } = IDGenerator.Instance.Generate();

        public string Name { get; set; }

        public string Description { get; set; }

        public IDictionary<string, object> Properties { get; private set; } = new Dictionary<string, object>();

        public IList<ISkill> Skills { get; private set; } = new List<ISkill>();
    }
}