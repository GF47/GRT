using System.Collections.Generic;

namespace GRT.GInventory
{
    public interface IStack : IDefinition
    {
        // event Action<IInventory, IInventory> Transferring;

        IDefinition Definition { get; }

        IQuantifiable Quantity { get; }

        IDictionary<string, object> Properties { get; set; }

        void Spawn(IOwner owner);

        void Destroy();

        void PickUp(IOwner owner);

        void PutDown(IOwner owner);
    }
}