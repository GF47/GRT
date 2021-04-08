using GRT.Geometry;
using UnityEditor;
using UnityEngine;

namespace GRT.Editor
{
    public class CreateLineRenderFromBezier : EditorWindow
    {
        private BezierSpline _bspline;
        private int _count = 50;

        [MenuItem("Tools/GF47 Editor/根据贝塞尔曲线生成LineRenderer")]
        private static void Init()
        {
            var window = GetWindow<CreateLineRenderFromBezier>();
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            _bspline = (BezierSpline)EditorGUILayout.ObjectField("贝塞尔曲线", _bspline, typeof(BezierSpline), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _count = EditorGUILayout.IntField("顶点数", _count);
            if (_count < 2 || _count > 256)
            {
                Debug.Log("请将顶点数设置在 [2,256] 的范围内");
                _count = 2;
                return;
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10f);

            EditorGUILayout.BeginHorizontal();
            LineRenderer lineRenderer = null;
            if (GUILayout.Button("生成新物体", EditorStyles.miniButtonLeft, GUILayout.MinWidth(20f)))
            {
                var go = new GameObject("Bezier Line");
                lineRenderer = go.AddComponent<LineRenderer>();
                lineRenderer.material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Line.mat");
                SetLineRendererPositions(lineRenderer);
            }
            if (GUILayout.Button("附加到所选物体上", EditorStyles.miniButtonRight, GUILayout.MinWidth(20f)))
            {
                UnityEngine.Object selected = GetSelectedObject();
                if (selected == null)
                {
                    Debug.Log("Nothing Selected");
                    return;
                }
                var go = selected as GameObject;
                lineRenderer = go.GetComponent<LineRenderer>();
                if (lineRenderer == null)
                {
                    lineRenderer = go.AddComponent<LineRenderer>();
                }
                lineRenderer.material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Line.mat");
                SetLineRendererPositions(lineRenderer);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SetLineRendererPositions(LineRenderer lineRenderer)
        {
            if (lineRenderer != null)
            {
                lineRenderer.positionCount = _count;
                float n = 1f / (_count - 1);
                for (int i = 0; i < _count - 1; i++)
                {
                    lineRenderer.SetPosition(i, _bspline.GetResult(i * n).position);
                }
                lineRenderer.SetPosition(_count - 1, _bspline.GetResult(1f).position);
            }
        }

        private static UnityEngine.Object GetSelectedObject()
        {
            if (Selection.objects.Length == 0)
            {
                return null;
            }
            return Selection.objects[0];
        }
    }
}