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
        private static bool _isActive;

        [MenuItem("Tools/GF47 Editor/Collider Utility")]
        private static void Init()
        {
            _isActive = !_isActive;
            SceneView.duringSceneGui -= SceneView_duringSceneGui;
            if (_isActive)
            {
                SceneView.duringSceneGui += SceneView_duringSceneGui;
            }
        }

        private static void SceneView_duringSceneGui(SceneView view)
        {
            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(0f, 0f, 256f, 128f), EditorStyles.textArea);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Collider Utility", EditorStyles.boldLabel, GUILayout.Width(200f));
                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(48f)))
                    {
                        _isActive = false;
                        SceneView.duringSceneGui -= SceneView_duringSceneGui;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Label("Create Collider To Wrap Selected");
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("On Selected", GUILayout.Width(120f)))
                    {
                        if (Selection.activeGameObject != null)
                        {
                            var go = Selection.activeGameObject;
                            Undo.RecordObject(go, "Add Collider");

                            Mesh mesh;
                            if (go.TryGetComponent<MeshFilter>(out var mf))
                            {
                                mesh = mf.sharedMesh;
                            }
                            else if (go.TryGetComponent<MeshCollider>(out var mc))
                            {
                                mesh = mc.sharedMesh;
                            }
                            else
                            {
                                throw new Exception($"{go.name} does not have a mesh");
                            }

                            var collider = go.AddComponent<BoxCollider>();
                            collider.center = mesh.bounds.center;
                            collider.size = mesh.bounds.size;

                            Undo.FlushUndoRecordObjects();
                        }
                    }
                    if (GUILayout.Button("After Selected", GUILayout.Width(120f)))
                    {
                        if (Selection.activeGameObject != null)
                        {
                            var selected = Selection.activeGameObject.transform;

                            var go = new GameObject($"{selected.name}_collider");
                            Undo.RegisterCreatedObjectUndo(go, "Create Collider");
                            Undo.RecordObject(go, "Add Collider");

                            Mesh mesh;
                            if (selected.gameObject.TryGetComponent<MeshFilter>(out var mf))
                            {
                                mesh = mf.sharedMesh;
                            }
                            else if (selected.gameObject.TryGetComponent<MeshCollider>(out var mc))
                            {
                                mesh = mc.sharedMesh;
                            }
                            else
                            {
                                throw new Exception($"{go.name} does not have a mesh");
                            }

                            var collider = go.AddComponent<BoxCollider>();
                            var transform = collider.transform;
                            transform.SetParent(selected.parent);
                            transform.SetSiblingIndex(selected.GetSiblingIndex() + 1);
                            transform.localScale = selected.lossyScale;
                            transform.SetPositionAndRotation(selected.position, selected.rotation);
                            collider.center = mesh.bounds.center;
                            collider.size = mesh.bounds.size;

                            Undo.FlushUndoRecordObjects();
                        }
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Label("Create Colliders");
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("By MeshFilters", GUILayout.Width(120f)))
                    {
                        if (Selection.activeGameObject != null)
                        {
                            var selectedObject = Selection.activeGameObject;

                            var go = new GameObject(selectedObject.name);
                            Undo.RegisterCreatedObjectUndo(go, "Create Collider");
                            Undo.RecordObject(go, "Add Collider");

                            var meshes = selectedObject.GetComponentsInChildren<MeshFilter>();
                            if (meshes != null && meshes.Length > 0)
                            {
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
                            }

                            Undo.FlushUndoRecordObjects();

                            Selection.activeGameObject = go;
                        }
                    }
                    if (GUILayout.Button("By MeshColliders", GUILayout.Width(120f)))
                    {
                        if (Selection.activeGameObject != null)
                        {
                            var selectedObject = Selection.activeGameObject;

                            var go = new GameObject(selectedObject.name);
                            Undo.RegisterCreatedObjectUndo(go, "Create Collider");
                            Undo.RecordObject(go, "Add Collider");

                            var meshColliders = selectedObject.GetComponentsInChildren<MeshCollider>();
                            if (meshColliders != null && meshColliders.Length > 0)
                            {
                                for (int i = 0; i < meshColliders.Length; i++)
                                {
                                    var mc = meshColliders[i];
                                    var collider = go.AddChild<BoxCollider>();
                                    collider.name = "collider " + i;
                                    collider.transform.localScale = mc.transform.lossyScale;
                                    collider.transform.SetPositionAndRotation(mc.transform.position, mc.transform.rotation);
                                    collider.center = mc.sharedMesh.bounds.center;
                                    collider.size = mc.sharedMesh.bounds.size;
                                }
                            }

                            Undo.FlushUndoRecordObjects();

                            Selection.activeGameObject = go;
                        }
                    }
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button("Combine", GUILayout.Width(240f)))
                {
                    var gameObjects = Selection.gameObjects;

                    if (gameObjects != null)
                    {
                        var go = new GameObject("_Combined Collider_");

                        Undo.RegisterCreatedObjectUndo(go, "Create Collider");
                        Undo.RecordObject(go, "Add Collider");

                        var c = go.AddComponent<BoxCollider>();

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

                        Undo.FlushUndoRecordObjects();

                        Selection.activeGameObject = go;
                    }
                }
            }
            GUILayout.EndArea();

            Handles.EndGUI();
        }
    }
}
