using GRT.Data;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    public static class GXFactoriesCodeGeneratorEditor
    {
        private static readonly string DIRECTORY_PATH = $"{Application.dataPath}/Scripts/GXFactories";

        [MenuItem("Assets/Project/Generate GXFactories")]
        private static void GenerateAll()
        {
            GXFactoriesCodeCenerator.GenerateAll(DIRECTORY_PATH);
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/Project/Generate Selected GXFactories")]
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