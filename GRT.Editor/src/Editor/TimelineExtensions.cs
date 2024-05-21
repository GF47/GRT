// #define UNITY_EDITOR

#if UNITY_EDITOR

using System;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UObject = UnityEngine.Object;

namespace GRT.Editor
{
    public static class TimelineExtensions
    {
        public static event Action<PlayableBinding, PlayableDirector, StringBuilder> Serializing;

        [MenuItem("GameObject/GRT/Get Playable Director Bindings Data", false, 1)]
        public static string GetBindingsData()
        {
            var selectedGO = Selection.activeGameObject;
            if (selectedGO == null || !selectedGO.TryGetComponent<PlayableDirector>(out var director))
            {
                Debug.LogWarning("no playable director selected");
                return null;
            }

            if (director.playableAsset == null)
            {
                Debug.LogWarning("playable director asset is null");
                return null;
            }

            var sb = new StringBuilder(256);
            foreach (var binding in director.playableAsset.outputs)
            {
                if (binding.streamName == "Markers")
                {
                    continue;
                }

                string location;
                switch (binding.sourceObject)
                {
                    case ActivationTrack activationTrack:
                        var go = activationTrack.GetBinding<GameObject>(director);
                        location = go == null ? string.Empty : $"{go.scene.name}:{go.GetPath()}";
                        sb.AppendLine($"<track name=\"{activationTrack.name}\" location=\"{location}\"/>");
                        break;

                    case AudioTrack audioTrack:
                        var audioSource = audioTrack.GetBinding<AudioSource>(director);
                        location = audioSource == null ? string.Empty : $"{audioSource.gameObject.scene.name}:{audioSource.gameObject.GetPath()}";
                        sb.AppendLine($"<track name=\"{audioTrack.name}\" location=\"{location}\"/>");
                        break;

                    case AnimationTrack animationTrack:
                        var animator = animationTrack.GetBinding<Animator>(director);
                        location = animator == null ? string.Empty : $"{animator.gameObject.scene.name}:{animator.gameObject.GetPath()}";
                        sb.AppendLine($"<track name=\"{animationTrack.name}\" location=\"{location}\"/>");
                        break;

                    case SignalTrack signalTrack:
                        var receiver = signalTrack.GetBinding<GameObject>(director);
                        location = receiver == null ? string.Empty : $"{receiver.scene.name}:{receiver.GetPath()}";
                        sb.AppendLine($"<track name=\"{signalTrack.name}\" location=\"{location}\"/>");
                        break;

                    case ControlTrack controlTrack:
                        var clips = controlTrack.GetClips();
                        if (clips != null)
                        {
                            sb.AppendLine($"<track name=\"{controlTrack.name}\">");
                            foreach (var clip in clips)
                            {
                                if (clip.asset is ControlPlayableAsset control)
                                {
                                    var source = director.GetReferenceValue(control.sourceGameObject.exposedName, out bool idValid) as GameObject;
                                    location = source == null ? string.Empty : $"{source.scene.name}:{source.GetPath()}";
                                    sb.AppendLine($"\t<clip name=\"{clip.displayName}\" location=\"{location}\"/>");
                                }
                            }
                            sb.AppendLine("</track>");
                        }
                        break;

                    default:
                        break;
                }
                Serializing?.Invoke(binding, director, sb);
            }

            var str = sb.ToString();
            Debug.Log(str);
            EditorGUIUtility.systemCopyBuffer = str;
            return str;
        }

        public static T GetBinding<T>(this TrackAsset track, PlayableDirector director) where T : UObject
        {
            var obj = director.GetGenericBinding(track);
            if (obj is GameObject go)
            {
                if (typeof(T) == typeof(GameObject))
                {
                    return go as T;
                }
                else
                {
                    return go.GetComponent<T>();
                }
            }
            else if (obj is T target)
            {
                return target;
            }
            else
            {
                return null;
            }
        }
    }
}

#endif