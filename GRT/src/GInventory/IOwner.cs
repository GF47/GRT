namespace GRT.GInventory
{
    public interface IOwner
    {
        IInventory Inventory { get; }

        IStack Current { get; }

        void PickUp(IStack stack);

        void PutDown(IStack stack);
    }
}