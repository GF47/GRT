using GRT.GTask;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GRT.GAssets
{
    public interface ISceneItem : IGDisposable<Scene>
    {
        IAwaitable<IAwaiter<Scene>, Scene> Load(string location);

        IAwaitable<IAwaiter<Scene>, Scene> Add(string location);

        IAwaitable<IAwaiter> Unload();
    }

    public static class SceneItemExtensions<TI> where TI : ISceneItem
    {
        public static Func<TI> Constructor;

        public static async Task<Scene> Load(IGScope scope, string location)
        {
            var item = scope.Find<TI>();
            if (item != null)
            {
                await item.Unload();
                item.DetachFromScope(true);
            }

            var newItem = Constructor == null ? Activator.CreateInstance<TI>() : Constructor();
            newItem.AttachToScope(scope);
            return await newItem.Load(location);
        }

        public static async Task<Scene> Add(IGScope scope, string location)
        {
            var item = scope.Find<TI>();
            if (item != null)
            {
                return await item.Add(location);
            }
            else
            {
                throw new UnityException("main scene not loaded, can not add sub scene");
            }
        }
    }
}