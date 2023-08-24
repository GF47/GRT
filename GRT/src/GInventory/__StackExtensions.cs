using GRT.GInventory;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GF47.GRT.GInventory
{
    public static class __StackExtensions
    {
        public static IStack Merge(this IStack a, IStack b)
        {
            if (a.Definition.ID == b.Definition.ID)
            {
                a.Quantity.SetValue(a, a.Quantity.Value + b.Quantity.Value);
                b.Quantity.SetValue(b, 0);
            }
            return a;
        }

        public static IStack Separate<T>(this IStack a, int quantity) where T : IStack
        {
            var realQuantity = a.Quantity.Value < quantity ? a.Quantity.Value : quantity;
            a.Quantity.SetValue(a, a.Quantity.Value - realQuantity);

            var properties = new Dictionary<string, object>(a.Properties);

            var b = Activator.CreateInstance<T>();
            b.Init(IDGenerator.Instance.Generate(), a.Definition, a.Quantity.Clone(realQuantity), properties);
            return b;
        }

        public static IStack TransferImpl(this IStack stack, IInventory inventory)
        {
            var newStack = stack.Inventory?.Out(stack) ?? stack;
            newStack.Inventory = inventory;// 这里不能直接使用原有的 stack, 要操作的 stack 是原本的 Inventory 里分离出来的新 stack
            newStack.Inventory?.In(newStack);
            return newStack;
        }

        public static void SetProperty(this IStack stack, string name, object value)
        {
            if (stack.Properties.ContainsKey(name)) { stack.Properties[name] = value; }
            else { stack.Properties.Add(name, value); }
        }

        public static object GetProperty(this IStack stack, string name) =>
            stack.Properties.TryGetValue(name, out var value) ? value : null;

        public static void SetSpawn(this IStack stack, bool spawn) =>
            stack.SetProperty(Keywords.POS, spawn);

        public static void SetPosition(this IStack stack, Vector3 pos) =>
            stack.SetProperty(Keywords.POS, pos);

        public static void SetRotation(this IStack stack, Vector3 rot) =>
            stack.SetProperty(Keywords.ROT, rot);

        public static void SetScale(this IStack stack, Vector3 scale) =>
            stack.SetProperty(Keywords.SCALE, scale);

        public static bool GetSpawn(this IStack stack) =>
            stack.Properties.TryGetValue(Keywords.POS, out var value) && value is bool spawn ? spawn : default;

        public static Vector3 GetPosition(this IStack stack, Vector3 @default = default) =>
            stack.Properties.TryGetValue(Keywords.POS, out var value) && value is Vector3 pos ? pos : @default;

        public static Vector3 GetRotation(this IStack stack, Vector3 @default = default) =>
            stack.Properties.TryGetValue(Keywords.ROT, out var value) && value is Vector3 rot ? rot : @default;

        public static Vector3 GetScale(this IStack stack) =>
            stack.Properties.TryGetValue(Keywords.SCALE, out var value) && value is Vector3 scale ? scale : Vector3.one;
    }
}