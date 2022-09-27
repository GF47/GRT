using System.Collections.Generic;

namespace GRT.GInventory
{
    public abstract class BaseInventory<T> : IInventory
    {
        public IDictionary<IStack, T> Stacks { get; protected set; } = new Dictionary<IStack, T>();

        public IStack In(IStack stack)
        {
            foreach (var kv in Stacks)
            {
                var stackIn = kv.Key;

                if (stackIn.IsMergeable(stack))
                {
                    return stackIn.Merge(stack);
                }
            }

            Stacks.Add(stack, GetNewInventoryItem());

            stack.Quantity.ValueChanging += Quantity_ValueChanging;

            return stack;
        }

        public IStack Out(IStack stack)
        {
            if (Stacks.ContainsKey(stack))
            {
                var stackOut = stack.Separate(stack.PickQuantity());
                return stackOut;
            }

            return stack;
        }

        public void OnStackSpawn(IOwner owner, IStack stack)
        {
            if (owner != null && owner.Inventory == this)
            {
                if (!stack.AutoSpawn())
                {
                    In(stack);
                }
            }
        }

        public void OnStackDestroy(IStack stack)
        {
            if (Stacks.TryGetValue(stack, out var item))
            {
                stack.Quantity.ValueChanging -= Quantity_ValueChanging;
                CollectInventoryItem(item);
                Stacks.Remove(stack);
            }
        }

        protected abstract void Quantity_ValueChanging(IStack stack, int newValue, int oldValue);

        protected abstract T GetNewInventoryItem();

        protected abstract void CollectInventoryItem(T item);
    }
}