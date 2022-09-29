using System;

namespace GRT.GInventory
{
    public interface ITrigger
    {
        bool Enabled { get; set; }

        event Action<IOwner, IStack> Triggering;

        Func<(IOwner, IStack)> GetContext { get; set; }
    }
}