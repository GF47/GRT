using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRT.Editor
{
    public class GF47ColliderUtility : ScriptableObject
    {
        private static bool isActive;

        [MenuItem("Tools/GF47 Editor/Collider Utility")]
        private static void Init()
        {
            if (isActive)
            {
                SceneView.duringSceneGui -= SceneView_duringSceneGui;
            }
            else
            {
                SceneView.duringSceneGui += SceneView_duringSceneGui;
            }
            isActive = !isActive;
        }

        private static void SceneView_duringSceneGui(SceneView view)
        {
            Handles.BeginGUI();

            if (GUILayout.Button("X", GUILayout.Width(40f)))
            {
                SceneView.duringSceneGui -= SceneView_duringSceneGui;
                isActive = false;
            }

            if (GUILayout.Button("Create Collider", GUILayout.Width(200f)))
            {
                if (Selection.activeGameObject != null)
                {
                    var selectedObject = Selection.activeGameObject;

                    var go = new GameObject(selectedObject.name);

                    var meshes = selectedObject.GetComponentsInChildren<MeshFilter>();

                    for (int i = 0; i < meshes.Length; i++)
                    {
                        var mesh = meshes[i];
                        var collider = go.AddChild<BoxCollider>();
                        collider.name = "collider " + i;
                        collider.transform.localScale = mesh.transform.lossyScale;
                        collider.transform.SetPositionAndRotation(mesh.transform.position, mesh.transform.rotation);
                        collider.center = mesh.sharedMesh.bounds.center;
                        collider.size = mesh.sharedMesh.bounds.size;
                    }

                    Selection.activeGameObject = go;
                }
            }

            if (GUILayout.Button("Combine Colliders", GUILayout.Width(200f)))
            {
                var gameObjects = Selection.gameObjects;
                
                if (gameObjects != null)
                {
                    var o = new GameObject("_Combined Collider_");
                    var c = o.AddComponent<BoxCollider>();

                    var max = Vector3.negativeInfinity;
                    var min = Vector3.positiveInfinity;

                    for (int i = 0; i < gameObjects.Length; i++)
                    {
                        var colliders = gameObjects[i].GetComponentsInChildren<Collider>();

                        if (colliders != null)
                        {
                            for (int j = 0; j < colliders.Length; j++)
                            {
                                // bounds.Encapsulate(colliders[j].bounds);

                                max = Vector3.Max(max, colliders[j].bounds.max);
                                min = Vector3.Min(min, colliders[j].bounds.min);
                            }

                        }
                    }

                    c.center = (max + min) / 2f;
                    c.size = max - min;

                    Selection.activeGameObject = o;
                }
            }

            Handles.EndGUI();
        }
    }
}
