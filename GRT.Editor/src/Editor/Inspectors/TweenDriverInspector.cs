using GRT.Tween;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace GRT.Editor.Inspectors
{
    [CustomEditor(typeof(TweenDriver))]
    public class TweenDriverInspector : UnityEditor.Editor
    {
        private TweenDriver _driver;

        private SerializedProperty _useTimeScale;
        private SerializedProperty _delay;
        private SerializedProperty _duration;
        private SerializedProperty _group;
        private SerializedProperty _isLateUpdate;
        private SerializedProperty _ease;
        private SerializedProperty _loop;
        private FieldInfo _iPercentTargetsFI;

        private SerializedProperty _stoppingUEvent;

        private readonly GUIContent[] _contents = new[]
        {
            new GUIContent ("UseTimeScale"),
            new GUIContent ("Delay"),
            new GUIContent ("Duration"),
            new GUIContent ("Group"),
            new GUIContent ("IsLateUpdate"),
            new GUIContent ("Ease"),
            new GUIContent ("Loop"),
            new GUIContent ("StoppingUEvent")
        };

        private bool _isFolded = true;

        private void OnEnable()
        {
            _driver = (TweenDriver)target;
            _iPercentTargetsFI = typeof(TweenDriver).GetField("_iPercentTargets", BindingFlags.NonPublic | BindingFlags.Instance);
            if (_iPercentTargetsFI != null && _iPercentTargetsFI.GetValue(_driver) == null)
            {
                Debug.Log("maybe not execute");
                _iPercentTargetsFI.SetValue(_driver, new List<MonoBehaviour>());
            }

            _useTimeScale = serializedObject.FindProperty("_useTimeScale");
            _delay = serializedObject.FindProperty("_delay");
            _duration = serializedObject.FindProperty("_duration");
            _group = serializedObject.FindProperty("_group");
            _isLateUpdate = serializedObject.FindProperty("_isLateUpdate");
            _ease = serializedObject.FindProperty("_ease");
            _loop = serializedObject.FindProperty("_loop");

            _stoppingUEvent = serializedObject.FindProperty("StoppingUEvent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.PropertyField(_useTimeScale, _contents[0]);
            EditorGUILayout.PropertyField(_delay, _contents[1]);
            EditorGUILayout.PropertyField(_duration, _contents[2]);
            EditorGUILayout.PropertyField(_group, _contents[3]);
            EditorGUILayout.PropertyField(_isLateUpdate, _contents[4]);
            EditorGUILayout.PropertyField(_ease, _contents[5]);
            EditorGUILayout.PropertyField(_loop, _contents[6]);
            EditorGUILayout.PropertyField(_stoppingUEvent, _contents[7]);

            serializedObject.ApplyModifiedProperties();

            _isFolded = EditorGUILayout.Foldout(_isFolded, "Targets");
            if (_isFolded)
            {
                EditorGUILayout.BeginVertical();
                var ipt = _iPercentTargetsFI.GetValue(_driver) as List<MonoBehaviour>;
                for (int i = 0; i < ipt.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    MonoBehaviour tempObj = EditorGUILayout.ObjectField(ipt[i], typeof(MonoBehaviour), true) as MonoBehaviour;
                    if (tempObj != ipt[i])
                    {
                        if (tempObj == null)
                        {
                            Undo.RecordObject(_driver, string.Format("change iPercentTarget[{0}] to null", i));
                            ipt[i] = null;
                            EditorUtility.SetDirty(_driver);
                        }
                        else
                        {
                            Undo.RecordObject(_driver, "change iPercentTarget");
                            ipt[i] = GetMobehaviourInheritedFromIPercent(tempObj);
                            EditorUtility.SetDirty(_driver);
                        }
                    }
                    if (GUILayout.Button("-", EditorStyles.miniButton))
                    {
                        Undo.RecordObject(_driver, "remove iPercentTarget");
                        ipt.RemoveAt(i);
                        EditorUtility.SetDirty(_driver);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("+"))
                {
                    Undo.RecordObject(_driver, "add iPercentTarget");
                    ipt.Add(null);
                    EditorUtility.SetDirty(_driver);
                }

                EditorGUILayout.EndVertical();
            }
        }

        private static MonoBehaviour GetMobehaviourInheritedFromIPercent(MonoBehaviour tempBehaviour)
        {
            if (tempBehaviour == null) { return null; }
            if (tempBehaviour is IPercent) { return tempBehaviour; }

            MonoBehaviour[] behaviours = tempBehaviour.GetComponents<MonoBehaviour>();
            if (behaviours != null)
            {
                for (int j = 0; j < behaviours.Length; j++)
                {
                    if (behaviours[j] is IPercent)
                    {
                        return behaviours[j];
                    }
                }
            }
            return null;
        }
    }
}