using UnityEngine;

namespace GRT.Events.Triggers
{
    public interface ITrigger
    {
        GnityEvent Event { get; }
    }
}
