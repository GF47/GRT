using System;
using System.Collections.Generic;

namespace GRT.GInventory
{
    public class BaseDefinition : IDefinition
    {
        public Action<IOwner, IStack> Spawning { get; set; }
        public Action<IStack> Destroying { get; set; }
        public Action<IOwner, IStack> PickingUp { get; set; }
        public Action<IOwner, IStack> PuttingDown { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public string Model { get; set; }

        public ICollection<ISkill> Skills { get; set; }
    }
}