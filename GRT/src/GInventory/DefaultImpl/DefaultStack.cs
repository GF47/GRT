using System.Collections.Generic;

namespace GRT.GInventory.DefaultImpl
{
    public class DefaultStack : IStack
    {
        public int ID { get; private set; }

        public IDefinition Definition { get; private set; }

        public IQuantifiable Quantity { get; private set; }

        public IDictionary<string, object> Properties { get; private set; }

        public IInventory Inventory { get; set; }

        public void Init(int id, IDefinition definition, IQuantifiable quantifiable, IDictionary<string, object> properties = null)
        {
            ID = id;
            Definition = definition;
            Quantity = quantifiable;
            Properties = properties ?? new Dictionary<string, object>();
        }

        public virtual void Destroy() => Inventory.Destroy(this);

        public virtual IStack Transfer(IInventory targetInventory) => __StackExtensions.TransferImpl(this, targetInventory);
    }
}