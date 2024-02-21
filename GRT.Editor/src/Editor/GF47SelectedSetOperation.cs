using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GRT.Editor
{
    public class GF47SelectedSetOperation : EditorWindow
    {
        private static readonly string[] _selectionOperations = new string[]
        {
            "Current Selected",
            "Selection 1",
            "Selection 2",
            "Selection 3",
            "Selection 4",
        };

        private enum SetOperator
        {
            Union, Intersection, RelativeComplement, Complement
        }

        [MenuItem("Tools/GF47 Editor/Selected Set Operation")]
        private static void Init()
        {
            var window = GetWindow<GF47SelectedSetOperation>();
            window._labelFieldStyle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleRight,
            };
            window._labelFieldStyle.normal.textColor = Color.white;
            window.position = new Rect(300f, 300f, 512f, 120f);
            window.minSize = new Vector2(420f, 24f);
            window.Show();
        }

        private GUIStyle _labelFieldStyle;

        private int _selectionA, _selectionB;

        private SetOperator _operator;

        private readonly GameObject[][] _savedSelections = new GameObject[4][];

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Save To", _labelFieldStyle, GUILayout.Width(64));
                if (GUILayout.Button("1", EditorStyles.miniButton, GUILayout.Width(32)))
                {
                    _savedSelections[0] = Selection.gameObjects;
                }
                if (GUILayout.Button("2", EditorStyles.miniButton, GUILayout.Width(32)))
                {
                    _savedSelections[1] = Selection.gameObjects;
                }
                if (GUILayout.Button("3", EditorStyles.miniButton, GUILayout.Width(32)))
                {
                    _savedSelections[2] = Selection.gameObjects;
                }
                if (GUILayout.Button("4", EditorStyles.miniButton, GUILayout.Width(32)))
                {
                    _savedSelections[3] = Selection.gameObjects;
                }

                EditorGUILayout.LabelField("Load", _labelFieldStyle, GUILayout.Width(64));
                if (GUILayout.Button("1", EditorStyles.miniButton, GUILayout.Width(32)))
                {
                    Selection.objects = _savedSelections[0];
                }
                if (GUILayout.Button("2", EditorStyles.miniButton, GUILayout.Width(32)))
                {
                    Selection.objects = _savedSelections[1];
                }
                if (GUILayout.Button("3", EditorStyles.miniButton, GUILayout.Width(32)))
                {
                    Selection.objects = _savedSelections[2];
                }
                if (GUILayout.Button("4", EditorStyles.miniButton, GUILayout.Width(32)))
                {
                    Selection.objects = _savedSelections[3];
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            {
                _selectionA = EditorGUILayout.Popup(_selectionA, _selectionOperations);
                _operator = (SetOperator)EditorGUILayout.EnumPopup(_operator);
                _selectionB = EditorGUILayout.Popup(_selectionB, _selectionOperations);
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Select", GUILayout.Height(64)))
            {
                var selectionA = Select(_selectionA);
                var selectionB = Select(_selectionB);
                switch (_operator)
                {
                    case SetOperator.Intersection:
                        Selection.objects = Intersection(selectionA, selectionB);
                        break;

                    case SetOperator.RelativeComplement:
                        Selection.objects = RelativeComplement(selectionA, selectionB);
                        break;

                    case SetOperator.Complement:
                        Selection.objects = Complement(selectionA, selectionB);
                        break;

                    case SetOperator.Union:
                    default:
                        Selection.objects = Union(selectionA, selectionB);
                        break;
                }
            }
        }

        private GameObject[] Select(int i)
        {
            GameObject[] selection;
            switch (i)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    selection = _savedSelections[i - 1];
                    break;

                case 0:
                default:
                    selection = Selection.gameObjects;
                    break;
            }

            return selection;
        }

        private static GameObject[] Union(GameObject[] a, GameObject[] b) => a.Union(b).ToArray();

        private static GameObject[] Intersection(GameObject[] a, GameObject[] b) => a.Intersect(b).ToArray();

        private static GameObject[] RelativeComplement(GameObject[] a, GameObject[] b) => a.Except(b).ToArray();

        private static GameObject[] Complement(GameObject[] a, GameObject[] b)
        {
            Debug.LogWarning("not implemented");
            return a.Except(b).ToArray();
        }
    }
}