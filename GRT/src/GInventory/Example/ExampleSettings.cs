using GF47.GRT.GInventory;
using GRT.GInventory.DefaultImpl;
using GRT.GInventory.Quantifiables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.GInventory.Example
{
    public class ExampleSettings : MonoSingleton<ExampleSettings>
    {
        public List<GameObject> gameObjects;
        public List<Sprite> textures;

        public GameObject GetPrototype(string name) => gameObjects.Find(x => x.name == name);

        public Sprite GetIcon(string name) => textures.Find(x => x.name == name);

        public IEnumerable<IStack> LoadSceneTools()
        {
            var data = new (string, string, string, string, int, int)[]
            {
                ("Cube","正方体","rectangle","Cube", 3, 10),
                ("Sphere","球体","circle","Sphere", 3, 1),
                ("Triangle","四面体","triangle","Triangle",3, -1),
            };

            foreach (var (dName, dDescription, dIcon, dPrototype, dCount, dDose) in data)
            {
                var def = new DefaultDefinition(dName, dDescription);
                def.SetIcon(dIcon);
                def.SetPrototype(dPrototype);
                def.Skills.Add(new Shoot());

                var stack = new DefaultStack();
                stack.Init(IDGenerator.Instance.Generate(), def, new Count(dCount) { Dose = dDose });
                var pos = 5 * UnityEngine.Random.onUnitSphere;
                pos.y = Math.Abs(pos.y);
                stack.SetPosition(pos);

                yield return stack;
            }
        }

        public IEnumerable<IStack> LoadPlayerTools()
        {
            var data = new (string, string, string, string, int)[]
            {
                ("Phone","对讲机","cross","Phone", 0),
            };

            foreach(var (dName, dDescription,dIcon, dPrototype, dCount) in data)
            {
                var def = new DefaultDefinition(dName, dDescription);
                def.SetIcon(dIcon);
                def.SetPrototype(dPrototype);

                var stack = new DefaultStack();
                stack.Init(IDGenerator.Instance.Generate(), def, dCount > 0 ? new Count(dCount) { Dose = 1 } : (IQuantifiable)new Singleton());

                yield return stack;
            }
        }
    }
}