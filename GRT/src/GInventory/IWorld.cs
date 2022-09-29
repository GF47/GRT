using UnityEngine;

namespace GRT.GInventory
{
    public interface IWorld
    {
        void OnStackSpawn(IOwner owner, IStack stack);

        void OnStackPutDown(IOwner owner, IStack stack);
    }

    public interface IWorldObject<T> where T : Component, IWorldObject<T>
    {
        IStack Stack { get; set; }

        void OnStackPickUp(IOwner owner, IStack stack);

        void OnStackDestroy(IStack stack);
    }
}