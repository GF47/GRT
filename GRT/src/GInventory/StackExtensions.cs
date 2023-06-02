using System.Collections.Generic;
using UnityEngine;

namespace GRT.GInventory
{
    public static class StackExtensions
    {
        public static bool IsMergeable(this IDefinition a, IDefinition b)
        {
            return a.Name == b.Name
                && a.Description == b.Description
                && a.Icon == b.Icon
                && a.Model == b.Model;
        }

        public static IStack Merge(this IStack a, IStack b)
        {
            if (a.IsMergeable(b))
            {
                a.Quantity.SetValue(a, a.Quantity.Value + b.Quantity.Value);
                b.Quantity.SetValue(b, 0);
            }
            return a;
        }

        public static IStack Separate(this IStack a, int quantity)
        {
            int realQuantity;
            if (a.Quantity.Value > quantity)
            {
                realQuantity = quantity;
                a.Quantity.SetValue(a, a.Quantity.Value - quantity);
            }
            else
            {
                realQuantity = a.Quantity.Value;
                a.Quantity.SetValue(a, 0);
            }

            var properties = new Dictionary<string, object>();
            foreach (var pair in a.Properties)
            {
                // 按理说除了位置和旋转都应该深拷贝下来
                if (pair.Key != Keywords.POS && pair.Key != Keywords.ROT && pair.Key != Keywords.AUTO_SPAWN)
                {
                    properties.Add(pair.Key, pair.Value);
                }
            }

            return new BaseStack()
            {
                Definition = a.Definition,
                Quantity = a.Quantity.Clone(realQuantity),
                Properties = properties,
            };
        }

        public static void SetProperty(this IStack stack, string name, object value)
        {
            if (stack.Properties.ContainsKey(name)) { stack.Properties[name] = value; }
            else { stack.Properties.Add(name, value); }
        }

        public static object GetProperty(this IStack stack, string name) =>
            stack.Properties.TryGetValue(name, out var value) ? value : null;

        public static Vector3 GetPosition(this IStack stack) =>
            stack.Properties.TryGetValue(Keywords.POS, out var prop) ? (prop is Vector3 pos ? pos : Vector3.zero) : Vector3.zero;

        public static Vector3 GetRotation(this IStack stack) =>
            stack.Properties.TryGetValue(Keywords.ROT, out var prop) ? (prop is Vector3 rot ? rot : Vector3.zero) : Vector3.zero;

        public static Vector3 GetScale(this IStack stack) =>
            stack.Properties.TryGetValue(Keywords.SCALE, out var prop) ? (prop is Vector3 scale ? scale : Vector3.zero) : Vector3.zero;

        public static bool Instantiatable(this IStack stack) => !string.IsNullOrEmpty(stack.Model);

        public static bool AutoSpawn(this IStack stack) =>
            Instantiatable(stack)
            && stack.Properties.TryGetValue(Keywords.AUTO_SPAWN, out var prop)
            && (prop is bool autoSpawn ? autoSpawn : System.Convert.ToBoolean(prop));

        public static int PickQuantity(this IStack stack) =>
            stack.Properties.TryGetValue(Keywords.PICK_QUANTITY, out var prop) ? (prop is int quantity ? quantity : 1) : 1;
    }
}