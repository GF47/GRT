using UnityEngine;

namespace GRT.GInventory
{
    public abstract class BaseWorld<T> : IWorld where T : Component, IWorldObject<T>
    {
        public abstract T Instantiate(IStack stack);

        public void OnStackSpawn(IOwner owner, IStack stack)
        {
            if (stack.AutoSpawn())
            {
                var wo = Instantiate(stack);
                if (wo != null)
                {
                    wo.Stack = stack;
                }
            }
        }

        public void OnStackPutDown(IOwner owner, IStack stack)
        {
            var wo = Instantiate(stack);
            if (wo != null)
            {
                wo.Stack = stack;
            }
        }
    }
}