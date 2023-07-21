using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace GRT.GComponents
{
    [Obsolete("see GRT.GEC")]
    public class DynamicGEntity : IGEntity
    {
        public string Scene { get; set; }

        public GameObject UObject { get; protected set; }

        public IList<IGComponent> Components { get; protected set; } = new List<IGComponent>();

        public IGEntity GEntity { get => this; set => throw new NotImplementedException(); }

        public string Path { get; set; }

        public event Action<GameObject> Binding;

        public string TemplatePath { get; set; }

        public static Func<string, Task<GameObject>> GetTemplate;

        public virtual void Bind() => BindAsync();

        private async void BindAsync()
        {
            Assert.IsNotNull(GetTemplate, $"DynamicGEntity.GetTemplate func is null, you can not clone a game object named [{TemplatePath}]");

            var template = await GetTemplate(TemplatePath);
            UObject = GameObject.Instantiate(template);
            UObject.name = $"{template.name} {GRandom.Get()}";

            // Scene 是外部指定的
            var scene = SceneManager.GetSceneByName(Scene);
            if (scene.IsValid())
            {
                SceneManager.MoveGameObjectToScene(UObject, scene);
            }
            else
            {
                Scene = SceneManager.GetActiveScene().name;
            }

            // Path 是自动指定的
            Path = UObject.GetPath();

            foreach (var com in Components)
            {
                com.Binding(UObject);
            }

            Binding?.Invoke(UObject);
        }
    }
}