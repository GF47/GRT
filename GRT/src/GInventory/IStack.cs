using System.Collections.Generic;

namespace GRT.GInventory
{
    public interface IStack
    {
        int ID { get; }

        IDefinition Definition { get; }
        IQuantifiable Quantity { get; }

        IDictionary<string, object> Properties { get; }

        IInventory Inventory { get; set; }

        void Init(int id, IDefinition definition, IQuantifiable quantifiable, IDictionary<string, object> properties);

        void Destroy();

        IStack Transfer(IInventory targetInventory);
    }
}