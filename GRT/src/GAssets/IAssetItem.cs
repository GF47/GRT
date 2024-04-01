using GRT.GTask;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace GRT.GAssets
{
    public interface IAssetItem : IGDisposable
    {
        string Location { get; }

        Type AssetType { get; }

        IAwaitable<IAwaiter<T>, T> Get<T>(string location = null) where T : UObject;
    }

    public static class AssetItemExtensions<TI> where TI : IAssetItem
    {
        public static Func<TI> Constructor;

        public static async Task<T> LoadOrOverride<T>(IGScope scope, string location) where T : UObject
        {
            var item = scope.Find<IAssetItem>(i => i.AssetType == typeof(T));
            if (item == null)
            {
                item = Constructor == null ? Activator.CreateInstance<TI>() : Constructor.Invoke();
                item.AttachToScope(scope);
                return await item.Get<T>(location);
            }
            else if (item.Location != location)
            {
                item.DetachFromScope(true);
                item = Constructor == null ? Activator.CreateInstance<TI>() : Constructor.Invoke();
                item.AttachToScope(scope);
                return await item.Get<T>(location);
            }
            else if (!item.IsAlive)
            {
                return await item.Get<T>(location);
            }
            else
            {
                return await item.Get<T>();
            }
        }

        public static async Task<T> Load<T>(IGScope scope, string location) where T : UObject
        {
            var item = scope.Find<IAssetItem>(i => i.AssetType == typeof(T) && i.Location == location);
            if (item == null)
            {
                item = Constructor == null ? Activator.CreateInstance<TI>() : Constructor.Invoke();
                item.AttachToScope(scope);
                return await item.Get<T>(location);
            }
            else
            {
                return await item.Get<T>();
            }
        }

        public static TI LoadAssetItem(IGScope scope, string location)
        {
            var item = scope.Find<TI>(i => i.AssetType == typeof(GameObject) && i.Location == location);
            if (item == null)
            {
                item = Constructor == null ? Activator.CreateInstance<TI>() : Constructor.Invoke();
                item.AttachToScope(scope);
                return item;
            }
            else
            {
                return item;
            }
        }

        public static async Task<string> LoadText(string location)
        {
            var item = Constructor == null ? Activator.CreateInstance<TI>() : Constructor.Invoke();
            var asset = await item.Get<TextAsset>(location);
            var text = asset.text;
            item.LifeDispose();
            return text;
        }
    }
}