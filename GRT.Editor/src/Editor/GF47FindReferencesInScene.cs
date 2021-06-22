using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace GF47.Editor
{
    public class GF47FindReferencesInScene : ScriptableObject
    {
        [MenuItem("GameObject/FindReferences", false, -10)]
        public static void Find()
        {
            if (Selection.activeGameObject == null)
            {
                Debug.Log("Select a game object first");
                return;
            }

            var go = Selection.activeGameObject;

            var idht = new Dictionary<int, UnityEngine.Object>();
            idht.Add(go.GetInstanceID(), go);
            foreach (var c in go.GetComponents<Component>())
            {
                idht.Add(c.GetInstanceID(), c);
            }

            var rgos = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var r in rgos)
            {
                var coms = r.GetComponentsInChildren<Component>(true);

                foreach (var c in coms)
                {
                    if (c is Transform) continue;

                    var sc = new SerializedObject(c);

                    var sp = sc.GetIterator();

                    while (sp.NextVisible(true))
                    {
                        if (sp.propertyType == SerializedPropertyType.ObjectReference)
                        {
                            foreach (var cp in idht)
                            {
                                if (sp.objectReferenceInstanceIDValue == cp.Key)
                                {
                                    var path = c.gameObject.name + "</color>.<color=cyan>" + c.GetType() + "</color>.<color=yellow>" + sp.propertyPath + "</color>";

                                    var t = c.transform.parent;

                                    while (t != null)
                                    {
                                        path = t.name + "/" + path;
                                        t = t.parent;
                                    }

                                    Debug.LogWarning($"Find <color=yellow>{go.name}.{cp.Value.GetType()}</color> at <color=lime>{path}", sp.serializedObject.targetObject);
                                }

                            }
                        }
                    }
                }
            }

            Debug.Log($"Find <color=yellow>{go.name}</color> finish.");
        }
    }
}
