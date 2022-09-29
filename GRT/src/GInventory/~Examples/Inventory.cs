using GRT.GInventory;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.InventoryUI
{
    public class Inventory : MonoBehaviour, IInventory
    {
        [SerializeField] private Transform _content;
        [SerializeField] private GameObject _template;

        private BaseInventory<InventoryItem> _inventory;

        public event Action<IStack, IStack> Selecting
        {
            add { _inventory.Selecting += value; }
            remove { _inventory.Selecting -= value; }
        }

        private void Awake()
        {
            InventoryItem.Init(_template);

            _inventory = new BaseInventoryImpl { _content = _content };
        }

        public IStack In(IStack stack) => _inventory.In(stack);

        public IStack Out(IStack stack) => _inventory.Out(stack);

        public void OnStackSpawn(IOwner owner, IStack stack) => _inventory.OnStackSpawn(owner, stack);

        public void OnStackDestroy(IStack stack) => _inventory.OnStackDestroy(stack);

        public void OnSelect(IStack stack, IStack old) => _inventory.OnSelect(stack, old);

        private class BaseInventoryImpl : BaseInventory<InventoryItem>
        {
            internal Transform _content;

            protected override InventoryItem GetNewInventoryItem(IStack stack)
            {
                var item = InventoryItem.Get();
                item.transform.SetParent(_content);
                item.Name = stack.Name;
                item.Quantity = stack.Quantity.Value;

                if (!item.TryGetComponent<Button>(out var button))
                {
                    button = item.gameObject.AddComponent<Button>();
                }
                button.onClick.AddListener(() =>
                {
                    OnSelect(stack, null);
                });

                return item;
            }

            protected override void CollectInventoryItem(InventoryItem item)
            {
                if (item.TryGetComponent<Button>(out var button))
                {
                    button.onClick.RemoveAllListeners();
                }

                InventoryItem.Release(item);
            }
        }
    }
}