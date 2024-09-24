using GRT.Data;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    public static class GXFactoriesCodeGeneratorEditor
    {
        private static readonly string DIRECTORY_PATH = $"{Application.dataPath}/Scripts/GXFactories";

        [MenuItem("Assets/GX Code Gen/Generate GXFactories")]
        private static void GenerateAll()
        {
            GXFactoriesCodeCenerator.GenerateAll(DIRECTORY_PATH);
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/GX Code Gen/Generate Selected GXFactories")]
        private static void GenerateSelected()
        {
            foreach (var cs in Selection.GetFiltered<MonoScript>(SelectionMode.DeepAssets))
            {
                GXFactoriesCodeCenerator.GenerateFactory(cs.GetClass(), DIRECTORY_PATH);
            }

            AssetDatabase.Refresh();
        }
    }
}
