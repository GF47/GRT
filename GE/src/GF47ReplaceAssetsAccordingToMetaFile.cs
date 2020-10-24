/***************************************************************
 * @File Name       : GF47ReplaceAssetsAccordingToMetaFile
 * @Author          : GF47
 * @Description     : 根据meta文件替换资源，忽略文件路径
 * @Date            : 2017/9/28/星期四 13:29:16
 * @Edit            : none
 **************************************************************/

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GF47Editor.Editor
{
    public class GF47ReplaceAssetsAccordingToMetaFile : ScriptableObject
    {
        [MenuItem("Tools/GF47 Editor/Replace Assets According To Meta Files")]
        private static void Replace()
        {
            string sourceDir = EditorUtility.OpenFolderPanel("Select Source Directory", Application.dataPath, "");
            if (string.IsNullOrEmpty(sourceDir))
            {
                Debug.Log("nothing to do");
                return;
            }
            sourceDir = sourceDir.Replace('/', '\\');

            string[] files = Directory.GetFiles(sourceDir);
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fileInfo = new FileInfo(files[i]);
                if (fileInfo.Extension == ".meta")
                {
                    StreamReader streamReader = fileInfo.OpenText();
                    string guid = null;
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            if (line.StartsWith("guid: "))
                            {
                                string line2 = streamReader.ReadLine(); // 是否为目录的meta文件，理论上来说，这个属性如果存在，肯定是紧跟着guid这一行之的，我也不知道是不是一定这样
                                if (!string.IsNullOrEmpty(line2) && line2.Contains("folderAsset: yes"))
                                {
                                    break; // 如果是目录的meta文件，则跳过
                                }

                                guid = line.Substring(6 /*guid: */);
                                break;
                            }
                        }
                    }
                    streamReader.Close();
                    streamReader.Dispose();

                    if (!string.IsNullOrEmpty(guid))
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        Debug.Log(path);
                        if (string.IsNullOrEmpty(path))
                        {
                            break;
                        }

                        path = path.Substring(6 /*Assets*/);
                        path = Application.dataPath + path;
                        path = path.Replace('/', '\\');
                        try
                        {
                            File.Copy(files[i].Substring(0, files[i].Length - 5 /*.meta*/), path, true);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                    }
                }
            }

            AssetDatabase.Refresh();
        }
    }
}
