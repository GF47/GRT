namespace GRT.GInventory
{
    public interface IInventoryItem
    {
        string Name { get; set; }
        string Description { get; set; }
        string Icon { get; set; }
        int Quantity { get; set; }

        void OnQuantityValueChange(IStack stack, int value, int oldValue);
    }
}