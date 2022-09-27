using System;
using System.Collections.Generic;

namespace GRT.GInventory
{
    public interface IDefinition
    {
        Action<IOwner, IStack> Spawning { get; set; }
        Action<IStack> Destroying { get; set; }

        Action<IOwner, IStack> PickingUp { get; set; }
        Action<IOwner, IStack> PuttingDown { get; set; }

        string Name { get; }

        string Description { get; }

        /// <summary>
        /// 2D图标资源路径
        /// </summary>
        string Icon { get; }

        /// <summary>
        /// 3D模型资源路径
        /// </summary>
        string Model { get; }

        ICollection<ISkill> Skills { get; }
    }
}