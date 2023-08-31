using System.Collections.Generic;

namespace GRT.GInventory
{
    public interface IInventory
    {
        IList<IInventoryItem> Items { get; }

        void Destroy(IStack stack);

        IStack In(IStack stack, bool autoMerge = true);

        IStack Out(IStack stack);
    }
}