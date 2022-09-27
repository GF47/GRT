using System;

namespace GRT.GInventory
{
    public interface ITrigger
    {
        bool Enabled { get; set; }

        event Action<IOwner, IInventory, IStack> Triggering;

        Func<(IOwner, IInventory, IStack)> GetContext { get; set; }
    }
}