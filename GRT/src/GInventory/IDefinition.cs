using System.Collections.Generic;

namespace GRT.GInventory
{
    public interface IDefinition
    {
        int ID { get; }

        string Name { get; }

        string Description { get; }

        IDictionary<string, object> Properties { get; }

        IList<ISkill> Skills { get; }
    }
}