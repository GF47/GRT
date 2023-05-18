using UnityEditor;
using UnityEngine;

namespace GRT.Editor
{
    public class GF47InstantiateOnTheSurface : ScriptableObject
    {
        private static bool _isActive;

        /// <summary>
        /// 需要被复制的物体
        /// </summary>
        private static Transform _template;

        /// <summary>
        /// 屏幕绘制的面片顶点
        /// </summary>
        private static readonly Vector3[] _points = new Vector3[8];

        private static Vector3 _up = Vector3.up;

        /// <summary>
        /// 鼠标左键拖拽时，最近的点
        /// </summary>
        private static Vector3 _closestPoint;

        private static Vector3 _lastPoint;

        [MenuItem("Tools/GF47 Editor/Instantiate On The Surface")]
        public static void Init()
        {
            SceneView.duringSceneGui -= SceneView_duringSceneGui;
            _isActive = !_isActive;
            if (_isActive)
            {
                SceneView.duringSceneGui += SceneView_duringSceneGui;

                Tools.pivotMode = PivotMode.Pivot;
                Tools.pivotRotation = PivotRotation.Local;
            }
        }

        private static void SceneView_duringSceneGui(SceneView scene)
        {
            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(0, 0, 320, 90), EditorStyles.textArea);

            DrawSceneViewUI();

            GUILayout.EndArea();
            Handles.EndGUI();

            DrawSceneHandle(scene);
        }

        private static void DrawSceneViewUI()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Instantiate On The Surface\npress [Left Control + Left Mouse Button]\nto pick surface", EditorStyles.boldLabel, GUILayout.Width(270f));
                if (GUILayout.Button("X", GUILayout.Height(20f)))
                {
                    SceneView.duringSceneGui -= SceneView_duringSceneGui;
                    _isActive = false;
                }
            }
            GUILayout.EndHorizontal();

            _template = EditorGUILayout.ObjectField("Template", _template, typeof(Transform), true) as Transform;

            if (GUILayout.Button("Replace"))
            {
                if (_template == null)
                {
                    Debug.LogWarning("Template is null");
                }
                else
                {
                    var pos = _points[0] + _points[1] + _points[2] + _points[4] + _points[5] + _points[6];
                    pos /= 6f;

                    var normal = Vector3.Cross(_points[1] - _points[0], _points[3] - _points[2]);
                    normal += Vector3.Cross(_points[5] - _points[4], _points[7] - _points[6]);
                    normal /= 2f;

                    var led = Instantiate(_template, pos, Quaternion.LookRotation(normal, _up));
                    led.name = _template.name + "_Clone";

                    Undo.RegisterCreatedObjectUndo(led.gameObject, "instantiate on the surface");
                }
            }
        }

        private static void DrawSceneHandle(SceneView scene)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if (IsPicking(Event.current) && Selection.activeGameObject != null)
            {
                var mpos = Event.current.mousePosition;
                mpos.y = scene.camera.pixelHeight - mpos.y;
                var ray = scene.camera.ScreenPointToRay(mpos);

                Pick(ray, Selection.activeGameObject);
                scene.Repaint();
            }

            if (IsDraging(Event.current))
            {
                Vector3 closest = Vector3.zero;
                float closestDistance = float.PositiveInfinity;
                var mpos = Event.current.mousePosition;

                for (int i = 0; i < _points.Length; i++)
                {
                    var p = _points[i];
                    var d = Vector2.Distance(mpos, HandleUtility.WorldToGUIPoint(p));
                    if (d < closestDistance)
                    {
                        closest = p;
                        closestDistance = d;
                    }
                }
                var distance = Vector3.Distance(closest, _closestPoint);
                if (distance > 1e-2f) { _lastPoint = _closestPoint; _closestPoint = closest; }

                Handles.Label((_closestPoint - _lastPoint) / 2f + _lastPoint, distance.ToString("F3"));
            }

            Handles.color = Color.yellow;
            Handles.DrawPolyLine(_points);
        }

        private static int _triangleIndex;

        private static void Pick(Ray ray, GameObject go)
        {
            var meshFilter = go.GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                var mesh = meshFilter.sharedMesh;
                var transform = go.transform;
                if (GF47AlignWithSurface.IntersectRayMesh(ray, mesh, transform.localToWorldMatrix, out var hit))
                {
                    _up = transform.up;

                    var vertices = mesh.vertices;
                    var triangles = mesh.triangles;
                    if (_triangleIndex != hit.triangleIndex)
                    {
                        _triangleIndex = hit.triangleIndex;
                        _points[0] = _points[4];
                        _points[1] = _points[5];
                        _points[2] = _points[6];
                        _points[3] = _points[7];

                        var index = _triangleIndex * 3;
                        var matrix = transform.localToWorldMatrix;

                        _points[7] = matrix.MultiplyPoint3x4(vertices[triangles[index]]);
                        _points[4] = matrix.MultiplyPoint3x4(vertices[triangles[index++]]);
                        _points[5] = matrix.MultiplyPoint3x4(vertices[triangles[index++]]);
                        _points[6] = matrix.MultiplyPoint3x4(vertices[triangles[index]]);
                    }

                    return;
                }
            }

            _triangleIndex = -1;
        }

        #region Key & Mouse

        /// <summary>
        /// 是否在 pick 状态
        /// <para>
        /// 按住 Control 和 鼠标左键
        /// </para>
        /// </summary>
        private static bool IsPicking(Event e) =>
            e.control
            && e.button == 0
            && e.type == EventType.MouseDrag;

        /// <summary>
        /// 是否在获取长或宽
        /// <para>
        /// 按住 鼠标左键
        /// </para>
        /// <para>
        /// 不要按 Alt 和 Control
        /// </para>
        /// </summary>
        private static bool IsDraging(Event e) =>
            !e.alt && !e.control
            && e.button == 0
            && (e.type == EventType.MouseDrag || e.type == EventType.MouseDown);

        #endregion Key & Mouse
    }
}