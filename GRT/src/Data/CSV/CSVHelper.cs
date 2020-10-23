using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GRT.Data.CSV
{
    public class CSVHelper
    {
        public const char CSVS_EPARATOR = ',';

        /// <summary>
        /// 处理CSV数据行
        /// </summary>
        private static string FormatRow(IList<string> strList)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < strList.Count - 1; i++)
            {
                sb.Append(strList[i]);
                sb.Append(CSVS_EPARATOR);
            }
            sb.Append(strList[strList.Count - 1]);
            return sb.ToString();
        }

        public static List<string[]> Read(string filePath, Encoding encoding)
        {
            var data = new List<string[]>();

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, encoding);

            string line = sr.ReadLine();
            while (line != null)
            {
                data.Add(line.Split(CSVS_EPARATOR));
                line = sr.ReadLine();
            }

            sr.Close();
            fs.Close();

            return data;
        }

        public static void Write(string filePath, Encoding encoding, IList<IList<string>> data)
        {
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Directory.Exists) { fi.Directory.Create(); }

            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, encoding);

            for (int i = 0; i < data.Count; i++)
            {
                sw.WriteLine(FormatRow(data[i]));
            }
            sw.Close();
            fs.Close();
        }
    }
}