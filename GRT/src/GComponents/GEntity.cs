using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.GComponents
{
    public class GEntity : IGEntity
    {
        public event Action<GameObject> Binding;

        public string Scene { get; set; }

        public string Path { get; set; }

        public GameObject UObject { get; protected set; }

        public IList<IGComponent> Components { get; protected set; } = new List<IGComponent>();

        IGEntity ILazyBindable.GEntity { get => this; set => throw new NotImplementedException(); }

        public virtual void Bind()
        {
            UObject = GameObjectExtension.FindIn(Scene, Path);

            foreach (var com in Components)
            {
                com.Binding(UObject);
            }
            Binding?.Invoke(UObject);
        }
    }
}