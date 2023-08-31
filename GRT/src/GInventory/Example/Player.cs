using GRT.GInventory.DefaultImpl;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace GRT.GInventory.Example
{
    public class Player : MonoBehaviour, IInventory
    {
        [SerializeField] private Transform _content;
        [SerializeField] private GameObject _prototype;

        private void Start()
        {
            _inner = new PlayerInventoryImpl
            {
                content = _content,
                prototype = _prototype
            };

            foreach (var stack in ExampleSettings.Instance.LoadPlayerTools())
            {
                stack.Transfer(this);
            }

            _currentStackButton.onClick.AddListener(() =>
            {
                if (CurrentStack!= null && CurrentStack.Definition.Skills.Count > 0)
                {
                    CurrentStack.Definition.Skills[0].Invoke(CurrentStack);
                }
            });
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0) && Camera.main.Raycast(out var hit))
            {
                var item = hit.collider.gameObject.GetInterface<IInventoryItem>();
                item?.Stack.Transfer(this);
            }
        }

        public void PutDownCurrent() => CurrentStack?.Transfer(Scene.Instance);

        #region Current Stack

        [Header("Current Stack")]
        [SerializeField] private Button _currentStackButton;
        [SerializeField] private Image _currentStackIcon;
        [SerializeField] private Text _currentStackName;
        [SerializeField] private Text _currentStackCount;

        private IStack _currentStack;

        public IStack CurrentStack
        {
            get => _currentStack;
            set
            {
                if (_currentStack == value) return;

                if (_currentStack != null)
                {
                    _currentStack.Quantity.Changing -= CurrentStackQuantityChanging;
                }

                _currentStack = value;
                if (_currentStack != null)
                {
                    _currentStack.Quantity.Changing += CurrentStackQuantityChanging;
                    _currentStackIcon.sprite = ExampleSettings.Instance.GetIcon(_currentStack.Definition.GetIcon());
                    _currentStackName.text = _currentStack.Definition.Name;
                    _currentStackCount.text = _currentStack.Quantity.Value.ToString();
                }
            }
        }

        private void CurrentStackQuantityChanging(IStack stack, int nv, int ov)
        {
            if (nv <= 0)
            {
                _currentStack = null;
                _currentStackIcon.sprite = null;
                _currentStackName.text = null;
                _currentStackCount.text = null;
            }
            else
            {
                _currentStackCount.text = nv.ToString();
            }
        }

        #endregion Current Stack

        #region Inventory

        private PlayerInventoryImpl _inner;

        public IList<IInventoryItem> Items => _inner.Items;

        public void Destroy(IStack stack) => _inner.Destroy(stack);

        public IStack In(IStack stack, bool autoMerge = true) => CurrentStack = _inner.In(stack, autoMerge);

        public IStack Out(IStack stack) => _inner.Out(stack);

        private class PlayerInventoryImpl : DefaultInventory
        {
            public Transform content;
            public GameObject prototype;

            protected override void InstantiateItem(IStack stack, Action<IStack, IInventoryItem> callback)
            {
                var go = Object.Instantiate(prototype);
                go.SetActive(true);
                go.transform.SetParent(content);

                var item = go.GetComponent<PlayerItem>();
                item.SetStack(stack);
                callback?.Invoke(stack, item);
            }

            protected override void ReleaseItem(IStack stack, IInventoryItem item)
            {
                if (item is PlayerItem pi)
                {
                    Object.Destroy(pi.gameObject);
                }
            }
        }

        #endregion Inventory
    }
}