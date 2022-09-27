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

            return new BaseStack()
            {
                Definition = a.Definition,
                Quantity = a.Quantity.Clone(realQuantity),
                Properties = new Dictionary<string, object>()
                {
                    {InventoryKeyword.SCALE, a.GetScale() },
                    {InventoryKeyword.PICK_QUANTITY, a.PickQuantity() },
                },
            };
        }

        public static void SetProperty(this IStack stack, string name, object value)
        {
            if (stack.Properties.ContainsKey(name)) { stack.Properties[name] = value; }
            else { stack.Properties.Add(name, value); }
        }

        public static Vector3 GetPosition(this IStack stack) =>
            stack.Properties.TryGetValue(InventoryKeyword.POS, out var prop) ? (prop is Vector3 pos ? pos : Vector3.zero) : Vector3.zero;

        public static Vector3 GetRotation(this IStack stack) =>
            stack.Properties.TryGetValue(InventoryKeyword.ROT, out var prop) ? (prop is Vector3 rot ? rot : Vector3.zero) : Vector3.zero;

        public static Vector3 GetScale(this IStack stack) =>
            stack.Properties.TryGetValue(InventoryKeyword.SCALE, out var prop) ? (prop is Vector3 scale ? scale : Vector3.zero) : Vector3.zero;

        public static bool Instantiatable(this IStack stack) => !string.IsNullOrEmpty(stack.Model);

        public static bool AutoSpawn(this IStack stack) =>
            Instantiatable(stack)
            && stack.Properties.TryGetValue(InventoryKeyword.AUTO_SPAWN, out var prop)
            && (prop is bool autoSpawn ? autoSpawn : System.Convert.ToBoolean(prop));

        public static int PickQuantity(this IStack stack) =>
            stack.Properties.TryGetValue(InventoryKeyword.PICK_QUANTITY, out var prop) ? (prop is int quantity ? quantity : 1) : 1;
    }
}