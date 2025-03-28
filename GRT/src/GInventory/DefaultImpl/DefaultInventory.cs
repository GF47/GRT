using System;
using System.Collections.Generic;

namespace GRT.GInventory.DefaultImpl
{
    public abstract class DefaultInventory : IInventory, IObservable<(IInventory, IInventoryItem, InventoryItemOperator)>
    {
        protected IObserver<(IInventory, IInventoryItem, InventoryItemOperator)> _observer;

        public IList<IInventoryItem> Items { get; protected set; } = new List<IInventoryItem>();

        public virtual void Destroy(IStack stack)
        {
            var item = Items.FindExt(i => i.Stack == stack);
            if (item != null)
            {
                ReleaseItem(stack, item);
                Items.Remove(item);

                _observer?.OnNext((this, item, InventoryItemOperator.Destroy));
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
                        var s = si.Merge(stack);

                        _observer?.OnNext((this, i, InventoryItemOperator.In));
                        return s;
                    }
                }
            }

            IInventoryItem item = null;
            InstantiateItem(stack, (s, i) =>
            {
                i.SetStack(s);
                Items.Add(i);
                item = i;
            });

            _observer?.OnNext((this, item, InventoryItemOperator.In));
            return stack;
        }

        public virtual IStack Out(IStack stack)
        {
            var item = Items.FindExt(i => i.Stack == stack);
            if (item != null)
            {
                var s = stack.Separate<DefaultStack>(stack.Quantity.Dose);

                _observer?.OnNext((this, item, InventoryItemOperator.Out));
                return s;
            }

            return stack;
        }

        public IDisposable Subscribe(IObserver<(IInventory, IInventoryItem, InventoryItemOperator)> observer)
        {
            _observer?.OnCompleted();
            _observer = observer;

            return new Unsubscriber<(IInventory, IInventoryItem, InventoryItemOperator)>(_observer, RemoveSubscriber);
        }

        private void RemoveSubscriber(IObserver<(IInventory, IInventoryItem, InventoryItemOperator)> observer)
        {
            if (_observer == observer)
            {
                _observer = null;
            }
        }

        protected abstract void InstantiateItem(IStack stack, Action<IStack, IInventoryItem> callback);

        protected abstract void ReleaseItem(IStack stack, IInventoryItem item);
    }
}