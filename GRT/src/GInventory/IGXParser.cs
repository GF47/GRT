using GRT.Data;
using System.Collections.Generic;

namespace GRT.GInventory
{
    public interface IDefinitionPropertiesParser
    {
        IEnumerable<KeyValuePair<string, object>> Parse<T>(IGX<T> gx, T node);
    }

    public interface IStackPropertiesParser
    {
        IEnumerable<KeyValuePair<string, object>> Parse<T>(IGX<T> gx, T node, IDefinition definition);
    }
}