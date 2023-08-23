using System.Collections.Generic;

namespace GRT.GInventory
{
    public interface IInventory
    {
        IDictionary<IStack, IInventoryItem> Stacks { get; }

        void Destroy(IStack stack);

        IStack In(IStack stack, bool autoMerge = true);

        IStack Out(IStack stack);
    }
}