using GF47.GRT.GInventory;
using System;
using System.Collections.Generic;

namespace GRT.GInventory.DefaultImpl
{
    public abstract class DefaultInventory : IInventory
    {
        public IDictionary<IStack, IInventoryItem> Stacks { get; protected set; } = new Dictionary<IStack, IInventoryItem>();

        public void Destroy(IStack stack)
        {
            if (Stacks.TryGetValue(stack, out var item))
            {
                ReleaseItem(stack, item);
                Stacks.Remove(stack);
            }
        }

        public virtual IStack In(IStack stack, bool autoMerge = true)
        {
            if (autoMerge)
            {
                foreach (var pair in Stacks)
                {
                    var si = pair.Key;
                    if (si.Definition.ID == stack.Definition.ID)
                    {
                        return si.Merge(stack);
                    }
                }
            }

            InstantiateItem(stack, (s, i) =>
            {
                i.SetStack(s);
                Stacks.Add(s, i);
            });

            return stack;
        }

        public virtual IStack Out(IStack stack)
        {
            if (Stacks.ContainsKey(stack))
            {
                return stack.Separate<DefaultStack>(stack.Quantity.Dose);
            }

            return stack;
        }

        protected abstract void InstantiateItem(IStack stack, Action<IStack, IInventoryItem> callback);

        protected abstract void ReleaseItem(IStack stack, IInventoryItem item);
    }
}