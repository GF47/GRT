using GRT.Data.CSV;
using GRT;
using GRT.Geometry.Bezier;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GF47Editor
{
    public class CsvBezierConvert : ScriptableObject
    {
        [MenuItem("Assets/GF47 Editor/将 CSV 文件转换成为 BezierSpline", false, 0)]
        private static void Csv2Bezier()
        {
            if (Selection.objects.Length == 0)
            {
                Debug.Log("请选择csv文件");
                return;
            }

            var data = CSVHelper.Read(AssetDatabase.GetAssetPath(Selection.objects[0]), Encoding.UTF8);
            var bezier = CreateInstance<BezierSpline>();
            for (int i = 0; i < data.Count; i++)
            {
                var p = new BezierPoint(new Vector3(
                    Convert.ToFloat(data[i][0]),
                    Convert.ToFloat(data[i][1]),
                    Convert.ToFloat(data[i][2])
                    ));
                p.HandleR = new Vector3(
                    Convert.ToFloat(data[i][3]),
                    Convert.ToFloat(data[i][4]),
                    Convert.ToFloat(data[i][5])
                    ) + p.Point;
                bezier.Add(p);
            }
            var path = AssetDatabase.GetAssetPath(Selection.objects[0]);
            path = Path.ChangeExtension(path, "asset");
            AssetDatabase.CreateAsset(bezier, path);
            Debug.Log(path);
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/GF47 Editor/将 BezierSpline Asset 文件转换成为 CSV", false, 0)]
        private static void Bezier2Csv()
        {
            if (Selection.objects.Length == 0)
            {
                Debug.Log("请选择Bezier文件");
                return;
            }

            var bezier = Selection.objects[0] as BezierSpline;
            if (bezier != null)
            {
                var map = new List<IList<string>>();
                for (int i = 0; i < bezier.Count; i++)
                {
                    var p = bezier[i];
                    var pd = p.HandleR - p.Point;
                    var row = new string[] 
                    {
                        p.Point.x.ToString(),
                        p.Point.y.ToString(),
                        p.Point.z.ToString(),
                        pd.x.ToString(),
                        pd.y.ToString(),
                        pd.z.ToString(),
                    };
                    map.Add(row);
                }
                var path = Path.GetFullPath(AssetDatabase.GetAssetPath(Selection.objects[0]));
                path = Path.ChangeExtension(path, "csv");
                CSVHelper.Write(path, Encoding.UTF8, map);
                Debug.Log(path);
                AssetDatabase.Refresh();
            }
        }
    }
}
