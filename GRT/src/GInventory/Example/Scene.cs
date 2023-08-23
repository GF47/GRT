using GF47.GRT.GInventory;
using GRT.GInventory.DefaultImpl;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GRT.GInventory.Example
{
    public class Scene : MonoBehaviour, IInventory
    {
        public static Scene Instance { get; private set; }

        private void Start()
        {
            Instance = this;

            _inner = new SceneInventoryImpl();
            foreach (var stack in ExampleSettings.Instance.LoadSceneTools())
            {
                stack.Transfer(this);
            }
        }

        #region Inventory

        private IInventory _inner;

        public IDictionary<IStack, IInventoryItem> Stacks => _inner.Stacks;

        public void Destroy(IStack stack) => _inner.Destroy(stack);

        public IStack In(IStack stack, bool autoMerge = true) => _inner.In(stack, false); // 场景作为 Inventory 时, 不能自动合并

        public IStack Out(IStack stack) => _inner.Out(stack);

        private class SceneInventoryImpl : DefaultInventory
        {
            protected override void InstantiateItem(IStack stack, Action<IStack, IInventoryItem> callback)
            {
                if (stack.Definition.HasPrototype(out var prototypeName))
                {
                    var prototype = ExampleSettings.Instance.GetPrototype(prototypeName);
                    if (prototype != null)
                    {
                        var go = Object.Instantiate(prototype);
                        go.SetActive(true);
                        go.transform.position = stack.GetPosition();

                        var item = go.AddComponent<SceneItem>();
                        item.SetStack(stack);
                        callback?.Invoke(stack, item);
                    }
                }
            }

            protected override void ReleaseItem(IStack stack, IInventoryItem item)
            {
                if (item is SceneItem si)
                {
                    Object.Destroy(si.gameObject);
                }
            }
        }

        #endregion Inventory
    }
}