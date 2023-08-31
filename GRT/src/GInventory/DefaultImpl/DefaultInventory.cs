using GRT;
using System;
using System.Collections.Generic;

namespace GRT.GInventory.DefaultImpl
{
    public abstract class DefaultInventory : IInventory
    {
        public IList<IInventoryItem> Items { get; protected set; } = new List<IInventoryItem>();

        public void Destroy(IStack stack)
        {
            var item = Items.Find(i => i.Stack == stack);
            if (item != null)
            {
                ReleaseItem(stack, item);
                Items.Remove(item);
            }
        }

        public virtual IStack In(IStack stack, bool autoMerge = true)
        {
            if (autoMerge)
            {
                foreach (var i in Items)
                {
                    var si = i.Stack;
                    if (si.Definition.ID == stack.Definition.ID)
                    {
                        return si.Merge(stack);
                    }
                }
            }

            InstantiateItem(stack, (s, i) =>
            {
                i.SetStack(s);
                Items.Add(i);
            });

            return stack;
        }

        public virtual IStack Out(IStack stack)
        {
            var item = Items.Find(i => i.Stack == stack);
            if (item != null)
            {
                return stack.Separate<DefaultStack>(stack.Quantity.Dose);
            }

            return stack;
        }

        protected abstract void InstantiateItem(IStack stack, Action<IStack, IInventoryItem> callback);

        protected abstract void ReleaseItem(IStack stack, IInventoryItem item);
    }
}