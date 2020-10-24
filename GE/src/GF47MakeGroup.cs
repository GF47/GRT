using UnityEditor;
using UnityEngine;

namespace GF47Editor
{
    public class GF47MakeGroup : ScriptableObject
    {
        [MenuItem("Tools/GF47 Editor/Transform/Make Group %&g", false, 1)]
        static void MakeGroup()
        {
            Transform[] trans = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);

            if (trans.Length < 1)
            {
                Debug.Log("You've selected nothing");
            }
            else
            {
                GameObject newParent = new GameObject("_New Group");
                Transform newParentTransform = newParent.transform;
                foreach (Transform t in trans)
                {
                    t.parent = newParentTransform;
                }
            }
        }

        [MenuItem("Tools/GF47 Editor/Transform/Make Group In The Same Position %g", false, 1)]
        static void MakeGroupWithTheSameParent()
        {
            Transform[] trans = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);

            if (trans.Length < 1)
            {
                Debug.Log("You've selected nothing");
            }
            else
            {
                GameObject newParent = new GameObject("_new_group_");
                Transform newParentTransform = newParent.transform;
                Transform originalParent = trans[0].parent;
                if (originalParent)
                {
                    newParentTransform.parent = originalParent;
                    Vector3 pos = Vector3.zero;
                    for (int i = 0; i < trans.Length; i++)
                    {
                        Transform t = trans[i];
                        pos += t.position;
                    }
                    newParentTransform.position = pos / trans.Length;
                }
                foreach (Transform t in trans)
                {
                    t.parent = newParentTransform;
                }
                Selection.activeGameObject = newParent;
            }
        }
    }
}

