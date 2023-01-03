/***************************************************************
 * @File Name       : FileUtility
 * @Author          : GF47
 * @Description     : 文件工具
 * @Date            : 2017/8/1/星期二 11:50:56
 * @Edit            : none
 **************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace GRT
{
    /// <summary>
    /// 文件帮助类，基本上只能在 Windows 上工作
    /// </summary>
    public static class FileUtility
    {
        public enum RootDirectory
        {
            CurrentEnvironmentDirectory,
            DataPath,
            PersistentDataPath,
            StreamingAssetsPath,
            TemporaryCachePath,
            AbsolutePath,
        }

        public static string RootDirectoryToString(this RootDirectory root)
        {
            switch (root)
            {
                case RootDirectory.DataPath:
                    return Application.dataPath;

                case RootDirectory.PersistentDataPath:
                    return Application.persistentDataPath;

                case RootDirectory.StreamingAssetsPath:
                    return Application.streamingAssetsPath;

                case RootDirectory.TemporaryCachePath:
                    return Application.temporaryCachePath;

                case RootDirectory.AbsolutePath:
                    return string.Empty;

                case RootDirectory.CurrentEnvironmentDirectory:
                default:
                    return CurrentDirectory.Replace(DirectorySpearator, '/');
            }
        }

        public static string NewLine => Environment.NewLine;
        public static char DirectorySpearator => Path.DirectorySeparatorChar;
        public static string CurrentDirectory => Environment.CurrentDirectory;

        /// <summary>
        /// 获取文件的md5值
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>md5字符串</returns>
        public static string GetFileHash(string path)
        {
            string md5;
            try
            {
                byte[] filedata;
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var lenth = fs.Length;
                    filedata = new byte[lenth];
                    fs.Read(filedata, 0, (int)lenth);
                }
                var provider = new MD5CryptoServiceProvider();
                var bytes = provider.ComputeHash(filedata);
                var sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(System.Convert.ToString(bytes[i], 16));
                }
                md5 = sb.ToString();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                md5 = "生成错误";
            }

            return md5;
        }

        /// <summary>
        /// 将二进制数据写入文件，储存在Unity3D的持久化存储路径中
        /// </summary>
        public static void SaveBytes(string fileName, byte[] bytes, RootDirectory root = RootDirectory.DataPath)
        {
            var path = Path.Combine(root.RootDirectoryToString(), fileName).Replace('/', DirectorySpearator);

            var dir = path.Substring(0, path.Length - Path.GetFileName(path).Length);
            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// 从Unity3D的持久化存储路径中读取名为 fileName 的文件，并以二进制数据的形式返回文件中的数据
        /// </summary>
        public static byte[] LoadBytes(string fileName, RootDirectory root = RootDirectory.DataPath)
        {
            var path = Path.Combine(root.RootDirectoryToString(), fileName).Replace('/', DirectorySpearator);

            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            return null;
        }

        public static void SaveString(string fileName, string text, RootDirectory root = RootDirectory.DataPath)
        {
            var path = Path.Combine(root.RootDirectoryToString(), fileName).Replace('/', DirectorySpearator);

            var dir = path.Substring(0, path.Length - Path.GetFileName(path).Length);
            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var data = Encoding.UTF8.GetBytes(text);
                fs.Write(data, 0, data.Length);
            }
        }

        public static string LoadString(string fileName, RootDirectory root = RootDirectory.DataPath)
        {
            var path = Path.Combine(root.RootDirectoryToString(), fileName).Replace('/', DirectorySpearator);

            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return string.Empty;
        }

        /// <summary>
        /// 读取所有行
        /// </summary>
        public static string[] LoadAllLines(string fileName, RootDirectory root = RootDirectory.DataPath)
        {
            var path = Path.Combine(root.RootDirectoryToString(), fileName).Replace('/', DirectorySpearator);

            if (File.Exists(path))
            {
                return File.ReadAllLines(path);
            }
            return null;
        }

        /// <summary>
        /// 每次读取一行
        /// </summary>
        public static IEnumerable<string> LoadLines(string fileName, RootDirectory root = RootDirectory.DataPath)
        {
            var path = Path.Combine(root.RootDirectoryToString(), fileName).Replace('/', DirectorySpearator);

            if (File.Exists(path))
            {
                return File.ReadLines(path);
            }
            return null;
        }
    }
}