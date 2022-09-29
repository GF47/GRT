using System;
using System.Collections.Generic;

namespace GRT.GInventory
{
    public abstract class BaseInventory<T> : IInventory where T : IInventoryItem
    {
        public event Action<IStack, IStack> Selecting;

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

            var item = GetNewInventoryItem(stack);
            stack.Quantity.ValueChanging += item.OnQuantityValueChange;
            Stacks.Add(stack, item);

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
            if (!stack.AutoSpawn())
            {
                In(stack);
            }
        }

        public void OnStackDestroy(IStack stack)
        {
            if (Stacks.TryGetValue(stack, out var item))
            {
                stack.Quantity.ValueChanging -= item.OnQuantityValueChange;
                CollectInventoryItem(item);
                Stacks.Remove(stack);
            }
        }

        public void OnSelect(IStack stack, IStack old) => Selecting?.Invoke(stack, old);

        protected abstract T GetNewInventoryItem(IStack stack);

        protected abstract void CollectInventoryItem(T item);
    }
}