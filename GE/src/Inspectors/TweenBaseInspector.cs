/***************************************************************
 * @File Name       : TweenBaseInspector
 * @Author          : GF47
 * @Description     : [TweenBase]类的检视面板，主要是将拖到面板上的[Monobehaviour]类填进[TweenBase.targets]中。
 * @Date            : 2017/1/6 9:25:29
 * @Edit            : none
 **************************************************************/

using System.Collections.Generic;
using GRT;
using GRT.Tween;
using UnityEditor;
using UnityEngine;

namespace GF47Editor.Inspectors
{
    [CustomEditor(typeof(TweenBase))]
    public class TweenBaseInspector : UnityEditor.Editor
    {
        private TweenBase _tweenBase;

        private SerializedProperty _easeType;
        private SerializedProperty _loopType;
        private SerializedProperty _useRealTime;
        private SerializedProperty _delay;
        private SerializedProperty _duration;
        private SerializedProperty _tweenGroup;
        private SerializedProperty _onFinishedUEvent;
        private SerializedProperty _isLateUpdate;

        private readonly GUIContent[] _contents = new[]
        {
            new GUIContent ("EaseType"),
            new GUIContent ("LoopType"),
            new GUIContent ("UseRealTime"),
            new GUIContent ("Delay"),
            new GUIContent ("Duration"),
            new GUIContent ("TweenGroup"),
            new GUIContent ("OnFinishedUEvent"),
            new GUIContent ("isLateUpdate"),
        };

        private bool _isFolded = true;

        void OnEnable()
        {
            _tweenBase = (TweenBase)target;
            if (_tweenBase._iPercentTargets == null)
            {
                _tweenBase._iPercentTargets = new List<MonoBehaviour>();
            }

            _easeType = serializedObject.FindProperty("easeType");
            _loopType = serializedObject.FindProperty("loopType");
            _useRealTime = serializedObject.FindProperty("useRealTime");
            _delay = serializedObject.FindProperty("delay");
            _duration = serializedObject.FindProperty("duration");
            _tweenGroup = serializedObject.FindProperty("tweenGroup");
            _onFinishedUEvent = serializedObject.FindProperty("OnFinishedUEvent");
            _isLateUpdate = serializedObject.FindProperty("isLateUpdate");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.PropertyField(_easeType, _contents[0]);
            EditorGUILayout.PropertyField(_loopType, _contents[1]);
            EditorGUILayout.PropertyField(_useRealTime, _contents[2]);
            EditorGUILayout.PropertyField(_delay, _contents[3]);
            EditorGUILayout.PropertyField(_duration, _contents[4]);
            EditorGUILayout.PropertyField(_tweenGroup, _contents[5]);
            EditorGUILayout.PropertyField(_onFinishedUEvent, _contents[6]);
            EditorGUILayout.PropertyField(_isLateUpdate, _contents[7]);

            serializedObject.ApplyModifiedProperties();

            _isFolded = EditorGUILayout.Foldout(_isFolded, "Targets");
            if (_isFolded)
            {
                EditorGUILayout.BeginVertical();
                for (int i = 0; i < _tweenBase._iPercentTargets.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    MonoBehaviour tempObj = EditorGUILayout.ObjectField(_tweenBase._iPercentTargets[i], typeof(MonoBehaviour), true) as MonoBehaviour;
                    if (tempObj != _tweenBase._iPercentTargets[i])
                    {
                        if (tempObj == null)
                        {
                            Undo.RecordObject(_tweenBase, string.Format("change iPercentTarget[{0}] to null", i));
                            _tweenBase._iPercentTargets[i] = null;
                            EditorUtility.SetDirty(_tweenBase);
                        }
                        else
                        {
                            Undo.RecordObject(_tweenBase, "change iPercentTarget");
                            _tweenBase._iPercentTargets[i] = GetMobehaviourInheritedFromIPercent(tempObj);
                            EditorUtility.SetDirty(_tweenBase);
                        }
                    }
                    if (GUILayout.Button("-", EditorStyles.miniButton))
                    {
                        Undo.RecordObject(_tweenBase, "remove iPercentTarget");
                        _tweenBase._iPercentTargets.RemoveAt(i);
                        EditorUtility.SetDirty(_tweenBase);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("+"))
                {
                    Undo.RecordObject(_tweenBase, "add iPercentTarget");
                    _tweenBase._iPercentTargets.Add(null);
                    EditorUtility.SetDirty(_tweenBase);
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
