namespace GRT.GInventory
{
    public interface IOwner
    {
        IInventory Inventory { get; }
        IWorld World { get; }
    }
}