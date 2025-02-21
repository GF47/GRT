using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GRT.Editor
{
    public class GF47GetPathToString : ScriptableObject
    {
        private const string EDITOR_PATH_VIM = @"gvim.bat";
        private const string EDITOR_PATH_NPP = @"notepad++.exe";
        private const string EDITOR_PATH_NOTEPAD = "notepad.exe";

        [MenuItem("Assets/GF47 Editor/OpenSelectedByGivenTool &o", false, 0)]
        private static void OpenSelectedByGivenTool()
        {
            CopyAbsolutePath();
            try
            {
                Process.Start("\"" + EDITOR_PATH_VIM + "\"", "\"" + EditorGUIUtility.systemCopyBuffer + "\"");
                /*
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.ErrorDialog = true;
                p.Start();
                string cmd = "gvim.exe " + "\"" + EditorGUIUtility.systemCopyBuffer + "\"";
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.WriteLine("exit");
                p.WaitForExit();
                cmd = p.StandardOutput.ReadToEnd();
                UnityEngine.Debug.Log("output:\n" + cmd);
                cmd = p.StandardError.ReadToEnd();
                UnityEngine.Debug.Log("error:\n" + cmd);
                p.Close();
                //*/
            }
            catch (Exception)
            {
                UnityEngine.Debug.Log("no vim");
                try
                {
                    Process.Start("\"" + EDITOR_PATH_NPP + "\"", "\"" + EditorGUIUtility.systemCopyBuffer + "\"");
                }
                catch (Exception)
                {
                    UnityEngine.Debug.Log("no notepad++");

                    Process.Start("\"" + EDITOR_PATH_NOTEPAD + "\"", "\"" + EditorGUIUtility.systemCopyBuffer + "\"");
                }
            }
        }

        [MenuItem("Assets/GF47 Editor/GetName", false, 0)]
        private static void CopyName()
        {
            Object selected = GetSelectedObject();
            if (selected == null)
            {
                UnityEngine.Debug.Log("Nothing Selected");
                return;
            }
            string pathString = selected.name;
            UnityEngine.Debug.Log(pathString);
            EditorGUIUtility.systemCopyBuffer = pathString;
        }

        [MenuItem("Assets/GF47 Editor/Get AssetPath #&c", false, 0)]
        private static void CopyAssetPath()
        {
            Object selected = GetSelectedObject();
            if (selected == null)
            {
                UnityEngine.Debug.Log("Nothing Selected");
                return;
            }
            string path = AssetDatabase.GetAssetPath(selected);
            UnityEngine.Debug.Log(path);
            EditorGUIUtility.systemCopyBuffer = path;
        }

        [MenuItem("Assets/GF47 Editor/GetSlectionPath", false, 0)]
        private static void CopyPath()
        {
            Object selected = GetSelectedObject();
            if (selected == null)
            {
                UnityEngine.Debug.Log("Nothing Selected");
                return;
            }
            string pathString = AssetDatabase.GetAssetPath(selected).Remove(0, 7);
            pathString = pathString.Remove(pathString.LastIndexOf(".", StringComparison.Ordinal));
            UnityEngine.Debug.Log(pathString);
            EditorGUIUtility.systemCopyBuffer = pathString;
        }

        [MenuItem("Assets/GF47 Editor/GetSelectionAbsolutePath", false, 0)]
        private static void CopyAbsolutePath()
        {
            Object selected = GetSelectedObject();
            if (selected == null)
            {
                UnityEngine.Debug.Log("Nothing Selected");
                return;
            }
            string pathString = Path.GetFullPath(AssetDatabase.GetAssetPath(selected));
            UnityEngine.Debug.Log(pathString);
            EditorGUIUtility.systemCopyBuffer = pathString;
        }

        private static Object GetSelectedObject()
        {
            if (Selection.objects.Length == 0)
            {
                return null;
            }
            return Selection.objects[0];
        }

        /**
         * 获取场景物体的层级
         */
        [MenuItem("GameObject/GF47 Editor/Get Transform Hierarchy", false, 0)]
        private static void GetTransformHierarchy()
        {
            var trans = Selection.activeTransform;
            if (trans != null)
            {
                var hierarchy = trans.name;
                while ((trans = trans.parent) != null)
                {
                    hierarchy = trans.name + '/' + hierarchy;
                }
                UnityEngine.Debug.Log(hierarchy);
                EditorGUIUtility.systemCopyBuffer = hierarchy;
            }
            else
            {
                UnityEngine.Debug.Log("you've selected nothing");
            }
        }

        /**
         * 获取场景物体的 [场景名:层级]
         */
        [MenuItem("GameObject/GF47 Editor/Get Transform Hierarchy With Scene Name", false, 0)]
        private static void GetTransformHierarchyWithSceneName()
        {
            var trans = Selection.activeTransform;
            if (trans != null)
            {
                var parent = trans.parent;
                var hierarchy = trans.name;
                while (parent != null)
                {
                    trans = parent;
                    hierarchy = trans.name + '/' + hierarchy;
                    parent = trans.parent;
                }

                hierarchy = trans.gameObject.scene.name + ':' + hierarchy;
                UnityEngine.Debug.Log(hierarchy);
                EditorGUIUtility.systemCopyBuffer = hierarchy;
            }
            else
            {
                UnityEngine.Debug.Log("you've selected nothing");
            }
        }
    }
}
