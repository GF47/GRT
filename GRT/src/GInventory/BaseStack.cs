using System;
using System.Collections.Generic;

namespace GRT.GInventory
{
    public class BaseStack : IStack
    {
        public IDefinition Definition { get; set; }

        public IQuantifiable Quantity { get; set; }

        public IDictionary<string, object> Properties { get; set; }
        public Action<IOwner, IStack> Spawning { get; set; }
        public Action<IStack> Destroying { get; set; }
        public Action<IOwner, IStack> PickingUp { get; set; }
        public Action<IOwner, IStack> PuttingDown { get; set; }

        public string Name => Definition.Name;

        public string Description => Definition.Description;

        public string Icon => Definition.Icon;

        public string Model => Definition.Model;

        public ICollection<ISkill> Skills => Definition.Skills;

        public virtual void Spawn(IOwner owner)
        {
            Definition.Spawning?.Invoke(owner, this);
            Spawning?.Invoke(owner, this);
        }

        public virtual void Destroy()
        {
            // Quantity.ClearValueChangingEvents();
            Definition.Destroying?.Invoke(this);
            Destroying?.Invoke(this);
        }

        public virtual void PickUp(IOwner owner)
        {
            Definition.PickingUp?.Invoke(owner, this);
            PickingUp?.Invoke(owner, this);
        }

        public virtual void PutDown(IOwner owner)
        {
            Definition.PuttingDown?.Invoke(owner, this);
            PuttingDown?.Invoke(owner, this);
        }
    }
}