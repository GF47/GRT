using System;

namespace GRT.GInventory.Example
{
    public class ExampleIDGenerator : IDGenerator
    {
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init() => Instance = new ExampleIDGenerator();

        private readonly Random _seed = new Random(DateTime.Now.Millisecond);

        public override int Generate() => _seed.Next();
    }
}