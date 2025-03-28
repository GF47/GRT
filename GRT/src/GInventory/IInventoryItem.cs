namespace GRT.GInventory
{
    public interface IInventoryItem
    {
        IStack Stack { get; }

        void SetStack(IStack stack);
    }

    public enum InventoryItemOperator
    {
        In, Out, Destroy, Changed
    }
}