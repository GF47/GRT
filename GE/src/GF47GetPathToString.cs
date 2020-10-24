using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GF47Editor
{
    public class GF47GetPathToString : ScriptableObject
    {
        private const string EditorPath = @"gvim.bat";

        [MenuItem("Assets/GF47 Editor/OpenSelectedByGivenTool &o", false, 0)]
        static void OpenSelectedByGivenTool()
        {
            CopyAbsolutePath();
            try
            {
                Process.Start("\"" + EditorPath + "\"","\"" +  EditorGUIUtility.systemCopyBuffer + "\"");
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
            catch (Exception e)
            {
                UnityEngine.Debug.Log("error:\n" + e);
            }
        }

        [MenuItem("Assets/GF47 Editor/GetName", false, 0)]
        static void CopyName()
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

        [MenuItem("Assets/GF47 Editor/Get AssetPath #c", false, 0)]
        static void CopyAssetPath()
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
        static void CopyPath()
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
        static void CopyAbsolutePath()
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

        static Object GetSelectedObject()
        {
            if (Selection.objects.Length == 0)
            {
                return null;
            }
            return Selection.objects[0];
        }
    }
}