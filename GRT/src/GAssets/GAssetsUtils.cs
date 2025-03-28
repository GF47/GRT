using GRT.GAssets.Local;
using GRT.GAssets.Streaming;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UObject = UnityEngine.Object;

namespace GRT.GAssets
{
    public abstract class GAssetsUtils<TA, TS> where TA : IAssetItem where TS : ISceneItem
    {
        public static IAssetItem CreateAssetItem() => AssetItemExtensions<TA>.Constructor();

        public static ISceneItem CreateSceneItem() => SceneItemExtensions<TS>.Constructor();

        public static async Task<T> LoadOrOverride<T>(IGScope scope, string location) where T : UObject
        {
            return await AssetItemExtensions<TA>.LoadOrOverride<T>(scope, location);
        }

        public static async Task<T> Load<T>(IGScope scope, string location) where T : UObject
        {
            return await AssetItemExtensions<TA>.Load<T>(scope, location);
        }

        public static async Task<string> LoadText(string location)
        {
            if (location.StartsWith(StreamingAssetItem.STREAMING_ASSETS_PLACEHOLDER))
            {
                return await LoadTextFromStreamingAssets(location);
            }
            else if (location.StartsWith(LocalAssetItem.LOCAL_ASSETS_PLACEHOLDER))
            {
                return await LoadTextFromLocalFile(location);
            }
            else
            {
                return await AssetItemExtensions<TA>.LoadText(location);
            }
        }

        public static async Task<Texture2D> LoadTexture(IGScope scope, string location)
        {
            if (location.StartsWith(StreamingAssetItem.STREAMING_ASSETS_PLACEHOLDER))
            {
                return await LoadTextureFromStreamingAssets(location);
            }
            else if (location.StartsWith(LocalAssetItem.LOCAL_ASSETS_PLACEHOLDER))
            {
                return await LoadTextureFromLocalFile(location);
            }
            else
            {
                return await AssetItemExtensions<TA>.Load<Texture2D>(scope, location);
            }
        }

        public static async Task<Scene> LoadScene(IGScope scope, string location)
        {
            return await SceneItemExtensions<TS>.Load(scope, location);
        }

        public static async Task<Scene> AddScene(IGScope scope, string location)
        {
            return await SceneItemExtensions<TS>.Add(scope, location);
        }

        public static async Task<string> LoadTextFromStreamingAssets(string location)
        {
            var item = new StreamingAssetItem();
            var handler = await item.Get(location);
            var text = handler.text;
            item.LifeDispose();
            return text;
        }

        public static async Task<Texture2D> LoadTextureFromStreamingAssets(string location, int x = 0, int y = 0)
        {
            var item = new StreamingAssetItem();
            var handler = await item.Get(location);
            var texture = new Texture2D(x, y);
            texture.LoadImage(handler.data);
            item.LifeDispose();
            return texture;
        }

        public static async Task<T> LoadFromStreamingAssets<T>(string location, Func<byte[], T> constructor)
        {
            var item = new StreamingAssetItem();
            var handler = await item.Get(location);
            var bytes = handler.data;
            var asset = constructor(bytes);
            item.LifeDispose();
            return asset;
        }

        public static async Task<string> LoadTextFromLocalFile(string location)
        {
            var item = new LocalAssetItem();
            var handler = await item.Get(location);
            var text = handler.text;
            item.LifeDispose();
            return text;
        }

        public static async Task<Texture2D> LoadTextureFromLocalFile(string location, int x = 0, int y = 0)
        {
            var item = new LocalAssetItem();
            var handler = await item.Get(location);
            var texture = new Texture2D(x, y);
            texture.LoadImage(handler.data);
            item.LifeDispose();
            return texture;
        }

        public static async Task<T> LoadFromLocalFile<T>(string location, Func<byte[], T> constructor)
        {
            var item = new LocalAssetItem();
            var handler = await item.Get(location);
            var bytes = handler.data;
            var asset = constructor(bytes);
            item.LifeDispose();
            return asset;
        }
    }
}