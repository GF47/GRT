/***************************************************************
 * @File Name       : FileUtility
 * @Author          : GF47
 * @Description     : 文件工具
 * @Date            : 2017/8/1/星期二 11:50:56
 * @Edit            : none
 **************************************************************/

using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace GRT
{
    /// <summary>
    /// 文件帮助类，基本上只能在 Windows 上工作
    /// </summary>
    public static class FileUtility
    {
        /// <summary>
        /// 获取文件的md5值
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>md5字符串</returns>
        public static string GetFileHash(string path)
        {
            string fileMD5;
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                int len = (int)fs.Length;
                byte[] data = new byte[len];
                fs.Read(data, 0, len);
                fs.Close();
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] result = md5.ComputeHash(data);
                fileMD5 = "";
                for (int i = 0; i < result.Length; i++)
                {
                    fileMD5 += System.Convert.ToString(result[i], 16);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                fileMD5 = "生成错误";
            }

            return fileMD5;
        }

        /// <summary> 将二进制数据写入文件，储存在Unity3D的持久化存储路径中
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="bytes">二进制数据</param>
        /// <returns>写入成功</returns>
        public static bool SaveAsFile(string fileName, byte[] bytes)
        {
            string path = Application.persistentDataPath + "/" + fileName;

            if (bytes == null)
            {
                if (File.Exists(path)) File.Delete(path);
                return true;
            }

            FileStream file;

            try
            {
                file = File.Create(path);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                return false;
            }

            file.Write(bytes, 0, bytes.Length);
            file.Close();
            return true;
        }

        /// <summary> 从Unity3D的持久化存储路径中读取名为 fileName 的文件，并以二进制数据的形式返回文件中的数据
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>二进制数据</returns>
        public static byte[] LoadFromFile(string fileName)
        {

            string path = Application.persistentDataPath + "/" + fileName;

            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            return null;
        }
    }
}
