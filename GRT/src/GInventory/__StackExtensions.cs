using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.GInventory
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

        public static IStack TransferImpl(IStack stack, IInventory inventory)
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

        public static bool HasProperty(this IStack stack, string name, out object value)
            => stack.Properties.TryGetValue(name, out value);

        public static object GetProperty(this IStack stack, string name)
            => stack.HasProperty(name, out var value) ? value : default;

        public static void SetSpawn(this IStack stack, bool spawn)
            => stack.SetProperty(Keywords.SPAWN, spawn);

        public static bool HasSpawn(this IStack stack, out bool spawn)
        {
            if (stack.Properties.TryGetValue(Keywords.SPAWN, out var value) && value is bool b)
            {
                spawn = b;
                return true;
            }
            else
            {
                spawn = default;
                return default;
            }
        }

        public static bool GetSpawn(this IStack stack)
            => stack.HasSpawn(out var spawn) && spawn;

        public static void SetPosition(this IStack stack, Vector3 pos)
            => stack.SetProperty(Keywords.POS, pos);

        public static bool HasPosition(this IStack stack, out Vector3 pos)
        {
            if (stack.Properties.TryGetValue(Keywords.POS, out var value) && value is Vector3 v3)
            {
                pos = v3;
                return true;
            }
            else
            {
                pos = default;
                return false;
            }
        }

        public static Vector3 GetPosition(this IStack stack, Vector3 @default = default)
            => stack.HasPosition(out var pos) ? pos : @default;

        public static void SetRotation(this IStack stack, Vector3 rot)
            => stack.SetProperty(Keywords.ROT, rot);

        public static bool HasRotation(this IStack stack, out Vector3 rot)
        {
            if (stack.Properties.TryGetValue(Keywords.ROT, out var value) && value is Vector3 v3)
            {
                rot = v3;
                return true;
            }
            else
            {
                rot = default;
                return false;
            }
        }

        public static Vector3 GetRotation(this IStack stack, Vector3 @default = default)
            => stack.HasRotation(out var rot) ? rot : @default;

        public static void SetScale(this IStack stack, Vector3 scale)
            => stack.SetProperty(Keywords.SCALE, scale);

        public static bool HasScale(this IStack stack, out Vector3 scale)
        {
            if (stack.Properties.TryGetValue(Keywords.SCALE, out var value) && value is Vector3 v3)
            {
                scale = v3;
                return true;
            }
            else
            {
                scale = default;
                return false;
            }
        }

        public static Vector3 GetScale(this IStack stack)
            => stack.HasScale(out var scale) ? scale : Vector3.one;
    }
}