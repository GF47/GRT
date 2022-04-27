using System;
using UnityEditor;
using UnityEngine;

namespace GRT.Editor
{
    public class GF47InstantiateOnTheSurface : ScriptableObject
    {
        private static GF47InstantiateOnTheSurface _instance;

        /// <summary>
        /// 需要被复制的物体
        /// </summary>
        private RectTransform _template;
        /// <summary>
        /// 新生成的物体的缩放值
        /// </summary>
        private float _scale = 0.1f;

        /// <summary>
        /// 屏幕绘制的面片顶点
        /// </summary>
        private Vector3[] _points = new Vector3[8];

        private string _name = string.Empty;
        private Vector3 _up = Vector3.up;

        /// <summary>
        /// 鼠标左键拖拽时，最近的点
        /// </summary>
        private Vector3 _closestPoint;

        private Vector3 _lastPoint;
        private float _width, _height;

        [MenuItem("Tools/GF47 Editor/Transform/Instantiate On The Surface")]
        public static void Init()
        {
            if (_instance != null)
            {
                DestroyImmediate(_instance);
            }
            _instance = CreateInstance<GF47InstantiateOnTheSurface>();
        }

        private void Awake()
        {
            SceneView.duringSceneGui += SceneView_duringSceneGui;
            var tempGO = new GameObject("~~~ temp Mesh Collider ~~~");
            tempGO.hideFlags = HideFlags.DontSave;
            _tempMeshCollider = tempGO.AddComponent<MeshCollider>();

            Tools.pivotMode = PivotMode.Pivot;
            Tools.pivotRotation = PivotRotation.Local;
        }

        private void OnDestroy()
        {
            SceneView.duringSceneGui -= SceneView_duringSceneGui;
            if (_tempMeshCollider != null)
            {
                DestroyImmediate(_tempMeshCollider.gameObject);
            }
        }

        private void SceneView_duringSceneGui(SceneView scene)
        {
            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(0, 0, 320, 152), EditorStyles.textArea);

            DrawSceneViewUI();

            GUILayout.EndArea();
            Handles.EndGUI();

            DrawSceneHandle(scene);
        }

        private void DrawSceneViewUI()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Instantiate On The Surface\npress [Left Control + Left Mouse Button]\nto pick surface", EditorStyles.boldLabel, GUILayout.Width(270f));
                if (GUILayout.Button("X", GUILayout.Height(20f)))
                {
                    DestroyImmediate(_instance);
                }
            }
            GUILayout.EndHorizontal();

            _template = EditorGUILayout.ObjectField("Template", _template, typeof(RectTransform), true) as RectTransform;
            _scale = Math.Max(EditorGUILayout.FloatField("Instantiate Scale", _scale), 0.01f);

            SetTargetMesh(EditorGUILayout.ObjectField("Target Mesh Filter", _targetMesh, typeof(MeshFilter), true) as MeshFilter);

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

                    var led = GameObject.Instantiate(_template, pos, Quaternion.LookRotation(normal, _up));
                    led.name = _name + "_LED";
                    led.localScale = _scale * Vector3.one;
                    led.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _width / _scale);
                    led.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _height / _scale);

                    Undo.RegisterCreatedObjectUndo(led.gameObject, "instantiate on the surface");
                    Selection.activeGameObject = led.gameObject;
                    SetTargetMesh(null);
                }
            }

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Set Width")) { _width = Vector3.Distance(_lastPoint, _closestPoint); Debug.LogWarning($"Set Width [{_width}]"); }
                if (GUILayout.Button("Set Height")) { _height = Vector3.Distance(_lastPoint, _closestPoint); Debug.LogWarning($"Set Height [{_height}]"); }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawSceneHandle(SceneView scene)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if (IsPicking(Event.current))
            {
                Pick(scene);
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
                // var closestPoint = HandleUtility.ClosestPointToPolyLine(_triangles);
                var distance = Vector3.Distance(closest, _closestPoint);
                if (distance > 1e-2f) { _lastPoint = _closestPoint; _closestPoint = closest; }

                Handles.Label((_closestPoint - _lastPoint) / 2f + _lastPoint, distance.ToString("F3"));
            }

            Handles.color = Color.yellow;
            Handles.DrawPolyLine(_points);

            Handles.color = Color.blue;
            Handles.DotHandleCap(0, _lastPoint, Quaternion.identity, 0.05f * HandleUtility.GetHandleSize(_lastPoint), EventType.Repaint);
            Handles.color = Color.red;
            Handles.DotHandleCap(0, _closestPoint, Quaternion.identity, 0.05f * HandleUtility.GetHandleSize(_closestPoint), EventType.Repaint);
        }

        private Mesh _mesh;
        private int _triangleIndex;
        private void Pick(SceneView scene)
        {
            var mpos = Event.current.mousePosition;
            mpos.y = scene.camera.pixelHeight - mpos.y;
            var ray = scene.camera.ScreenPointToRay(mpos);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider is MeshCollider mc)
                {
                    if (_mesh != mc.sharedMesh)
                    {
                        _mesh = mc.sharedMesh;
                        _triangleIndex = -1;

                        _name = (mc == _tempMeshCollider ? _targetMesh.gameObject : mc.gameObject).name;
                        _up = hit.transform.up;
                    }

                    var vertices = _mesh.vertices;
                    var triangles = _mesh.triangles;
                    if (_triangleIndex != hit.triangleIndex)
                    {
                        _triangleIndex = hit.triangleIndex;
                        _points[0] = _points[4];
                        _points[1] = _points[5];
                        _points[2] = _points[6];
                        _points[3] = _points[7];

                        var index = _triangleIndex * 3;
                        var matrix = hit.transform.localToWorldMatrix;

                        _points[7] = matrix.MultiplyPoint3x4(vertices[triangles[index]]);
                        _points[4] = matrix.MultiplyPoint3x4(vertices[triangles[index++]]);
                        _points[5] = matrix.MultiplyPoint3x4(vertices[triangles[index++]]);
                        _points[6] = matrix.MultiplyPoint3x4(vertices[triangles[index]]);

                        scene.Repaint();
                    }

                    return;
                }
            }
            _mesh = null;
            _triangleIndex = -1;
        }


        // TODO 如果目标本身带有 BoxCollider 等其他非 MeshCollider 则会出问题
        // 解决办法：滚蛋。
        private MeshFilter _targetMesh; // 如果当前鼠标所在物体没有MeshCollider，则射线无法获取顶点和三角形，所以需要手动生成一个MeshCollider
        private MeshCollider _tempMeshCollider; // 随 _instance 生成和销毁
        private void SetTargetMesh(MeshFilter target)
        {
            if (target != _targetMesh)
            {
                _targetMesh = target;

                if (_targetMesh != null)
                {
                    _tempMeshCollider.transform.SetPositionAndRotation(_targetMesh.transform.position, _targetMesh.transform.rotation);
                    _tempMeshCollider.transform.localScale = _targetMesh.transform.lossyScale;

                    var targetCollider = _targetMesh.GetComponent<Collider>();
                    if (targetCollider is MeshCollider)
                    {
                        // 如果物体本身已经有MeshCollider了，则不需要手动生成，否则自动生成一个
                        _tempMeshCollider.sharedMesh = null;
                    }
                    else
                    {
                        _tempMeshCollider.sharedMesh = _targetMesh.sharedMesh;
                    }
                    // Debug.LogWarning($"{_targetMesh.name} has a collider but not a mesh collider, please resolve it");
                }
                else
                {
                    _tempMeshCollider.sharedMesh = null;
                }
            }
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

        private static bool IsCtrlUp(Event e) => e.keyCode == KeyCode.LeftControl && e.type == EventType.KeyUp;
        private static bool IsDragingUp(Event e) => e.button == 0 && e.type == EventType.MouseUp;

        #endregion
    }
}