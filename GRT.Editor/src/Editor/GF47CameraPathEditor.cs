/***************************************************************
 * @File Name       : GF47CameraPathEditor
 * @Author          : GF47
 * @Description     : 编辑[相机-焦点]路径
 * @Date            : 2017/6/14/星期三 10:31:37
 * @Edit            : none
 **************************************************************/

#define LAST_ACTIVE_SCENE_VIEW

using GRT.Geometry;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GRT.Editor
{
    public class GF47CameraPathEditor : EditorWindow
    {
        private const float PREVIEW_WIDTH = 400f;
        private const float PREVIEW_HEIGHT = 300f;

        private const float HANDLE_SIZE = 0.04f;
        private const float HANDLE_LENGTH = 0.64f;
        private const float PICK_SIZE = 0.06f;
        private const float BEZIERSPLINE_WIDTH = 2f;
        private static readonly Color BackgroundColor = GUI.backgroundColor;

        private BezierSpline _cameraBSpline; private BezierSpline _targetBSpline;
        private int _cameraSelectedID = -1; private int _targetSelectedID = -1;

        private float _percent;
        private bool _showPreviewCamera;
        private RenderTexture _previewRenderTexture;
        private Camera _previewCamera;
        private Transform _previewTarget;
        private float _previewDuration;
        private bool _autoPreview;
        private float _autoPreviewStartTime;
        private Vector3 _previewCameraPos;
        private Vector3 _previewTargetPos;
        private Vector2 _scrollViewPos;

        private static bool _showHandles;

        [MenuItem("Tools/GF47 Editor/Camera Path Editor")]
        private static void Init()
        {
            GF47CameraPathEditor window = GetWindow<GF47CameraPathEditor>();
            window.position = new Rect(200f, 200f, 400f, 600f);
            window.Show();
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += Draw;
            GameObject cam = new GameObject("__PreviewCamera") { hideFlags = HideFlags.HideAndDontSave };
            _previewCamera = cam.AddComponent<Camera>();
            _previewRenderTexture = new RenderTexture(400, 300, 16) { hideFlags = HideFlags.DontSave };
            _previewCamera.targetTexture = _previewRenderTexture;
            GameObject target = new GameObject("__PreviewTarget") { hideFlags = HideFlags.HideAndDontSave };
            _previewTarget = target.transform;
        }

        private void OnDisable()
        {
            if (_previewCamera.gameObject != null) DestroyImmediate(_previewCamera.gameObject);
            if (_previewRenderTexture != null) DestroyImmediate(_previewRenderTexture);
            if (_previewTarget != null) DestroyImmediate(_previewTarget.gameObject);
            AssetDatabase.SaveAssets();
            SceneView.duringSceneGui -= Draw;
            GetSceneView().Repaint();
        }

        private void OnGUI()
        {
            bool refreshSv = false;

            EditorGUILayout.BeginHorizontal();
            BezierSpline tmpBs = (BezierSpline)EditorGUILayout.ObjectField("相机路径", _cameraBSpline, typeof(BezierSpline), false);
            if (_cameraBSpline != tmpBs)
            {
                _cameraBSpline = tmpBs;
                refreshSv = true;
            }
            tmpBs = (BezierSpline)EditorGUILayout.ObjectField("焦点路径", _targetBSpline, typeof(BezierSpline), false);
            if (_targetBSpline != tmpBs)
            {
                _targetBSpline = tmpBs;
                refreshSv = true;
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10f);

            _scrollViewPos = EditorGUILayout.BeginScrollView(_scrollViewPos, false, false);

            int insertID_C = -1; int insertID_T = -1;
            int removeID_C = -1; int removeID_T = -1;

            int rowCount = -1;
            if (_cameraBSpline != null) { rowCount = _cameraBSpline.Count; }
            if (_targetBSpline != null) { rowCount = Mathf.Max(rowCount, _targetBSpline.Count); }
            for (int i = 0; i < rowCount; i++)
            {
                EditorGUILayout.BeginHorizontal();

                // 相机贝塞尔点
                refreshSv |= DrawBezierPointInspector(_cameraBSpline, i, ref insertID_C, ref removeID_C, ref _cameraSelectedID);

                // 焦点贝塞尔点
                refreshSv |= DrawBezierPointInspector(_targetBSpline, i, ref insertID_T, ref removeID_T, ref _targetSelectedID);

                EditorGUILayout.EndHorizontal();
                GUILayout.Space(10f);
            }
            EditorGUILayout.EndScrollView();

            if (_cameraBSpline != null)
            {
                if (insertID_C > -1)
                {
                    Point p = _cameraBSpline.GetResult((insertID_C - 0.5f) / (_cameraBSpline.Count - 1));
                    Undo.RecordObject(_cameraBSpline, "insert bezier point");
                    Vector3 pp = p.position;
                    Vector3 pn = p.velocity.normalized;
                    _cameraBSpline.Insert(insertID_C, new BezierPoint(pp, pp - pn, pp + pn, BezierPoint.PointType.Smooth));
                    EditorUtility.SetDirty(_cameraBSpline);
                }
                if (removeID_C > -1)
                {
                    Undo.RecordObject(_cameraBSpline, "remove bezier point");
                    _cameraBSpline.RemoveAt(removeID_C);
                    EditorUtility.SetDirty(_cameraBSpline);
                }
            }
            if (_targetBSpline != null)
            {
                if (insertID_T > -1)
                {
                    Point p = _targetBSpline.GetResult((insertID_T - 0.5f) / (_targetBSpline.Count - 1));
                    Undo.RecordObject(_targetBSpline, "insert bezier point");
                    Vector3 pp = p.position;
                    Vector3 pn = p.velocity.normalized;
                    _targetBSpline.Insert(insertID_T, new BezierPoint(pp, pp - pn, pp + pn, BezierPoint.PointType.Smooth));
                    EditorUtility.SetDirty(_targetBSpline);
                }
                if (removeID_T > -1)
                {
                    Undo.RecordObject(_targetBSpline, "remove bezier point");
                    _targetBSpline.RemoveAt(removeID_T);
                    EditorUtility.SetDirty(_targetBSpline);
                }
            }

            EditorGUILayout.BeginHorizontal();

            if (_cameraBSpline != null)
            {
                if (GUILayout.Button("Add", EditorStyles.miniButtonLeft, GUILayout.MinWidth(20f)))
                {
                    Undo.RecordObject(_cameraBSpline, "add bezier point");
                    _cameraBSpline.Insert(_cameraBSpline.Count, new BezierPoint(GetSceneView().camera.transform.position));
                    EditorUtility.SetDirty(_cameraBSpline);
                    refreshSv = true;
                }

                if (GUILayout.Button("Set", EditorStyles.miniButtonMid, GUILayout.MinWidth(20f)))
                {
                    if (_cameraSelectedID > -1)
                    {
                        Undo.RecordObject(_cameraBSpline, "change pos");
                        _cameraBSpline[_cameraSelectedID].Position = GetSceneView().camera.transform.position;
                        EditorUtility.SetDirty(_cameraBSpline);
                        refreshSv = true;
                    }
                }

                if (GUILayout.Button("Set SceneView", EditorStyles.miniButtonRight, GUILayout.MinWidth(20f)))
                {
                    if (_cameraSelectedID > -1)
                    {
                        SceneView sv = GetSceneView();
                        sv.FixNegativeSize();
                        Vector3 pivot = sv.pivot;
                        Vector3 pos = pivot;
                        Quaternion quaternion = Quaternion.LookRotation(pivot - _cameraBSpline[_cameraSelectedID].Position);

                        #region 不要打开

                        MethodInfo mf = typeof(SceneView).GetMethod("CalcCameraDist", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                        if (mf != null)
                        {
                            pos -= (quaternion * Vector3.forward).normalized * (float)mf.Invoke(sv, null);// 这个位置一直不对
                        }

                        #endregion 不要打开

                        sv.LookAtDirect(pos, quaternion);
                        sv.pivot = pivot;
                    }
                }
            }
            if (_targetBSpline != null)
            {
                if (GUILayout.Button("Add", EditorStyles.miniButtonLeft, GUILayout.MinWidth(20f)))
                {
                    Undo.RecordObject(_targetBSpline, "add bezier point");
                    _targetBSpline.Insert(_targetBSpline.Count, new BezierPoint(GetSceneView().pivot));
                    EditorUtility.SetDirty(_targetBSpline);
                    refreshSv = true;
                }
                if (GUILayout.Button("Set", EditorStyles.miniButtonMid, GUILayout.MinWidth(20f)))
                {
                    if (_targetSelectedID > -1)
                    {
                        Undo.RecordObject(_targetBSpline, "change pos");
                        _targetBSpline[_targetSelectedID].Position = GetSceneView().pivot;
                        EditorUtility.SetDirty(_targetBSpline);
                        refreshSv = true;
                    }
                }
                if (GUILayout.Button("Set SceneTarget", EditorStyles.miniButtonRight, GUILayout.MinWidth(20f)))
                {
                    if (_targetSelectedID > -1)
                    {
                        SceneView sv = GetSceneView();
                        sv.FixNegativeSize();
                        sv.pivot = _targetBSpline[_targetSelectedID].Position;
                        // sv.size = 10f;
                        // sv.LookAt(_targetBSpline[_targetSelectedID].Point);
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            var showHandles = GUILayout.Toggle(_showHandles, _showHandles ? "Hide Control Handles" : "Show Control Handles", EditorStyles.toolbarButton);
            if (_showHandles != showHandles)
            {
                _showHandles = showHandles;
                refreshSv = true;
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Sort", EditorStyles.miniButton, GUILayout.MinWidth(20f)))
            {
                _cameraBSpline.Sort();
                if (_cameraBSpline.Count > 0)
                {
                    _cameraBSpline[0].Percent = 0f;
                    _cameraBSpline[_cameraBSpline.Count - 1].Percent = 1f;
                }

                _targetBSpline.Sort();
                if (_targetBSpline.Count > 0)
                {
                    _targetBSpline[0].Percent = 0f;
                    _targetBSpline[_targetBSpline.Count - 1].Percent = 1f;
                }

                AssetDatabase.SaveAssets();
            }

            if (refreshSv) { GetSceneView().Repaint(); }

            EditorGUILayout.Space();

            _showPreviewCamera = GUILayout.Toggle(_showPreviewCamera, _showPreviewCamera ? "Hide Preview Camera" : "ShowPreview Camera", EditorStyles.toolbarButton);
            if (_showPreviewCamera)
            {
                EditorGUILayout.BeginHorizontal();
                _previewDuration = EditorGUILayout.FloatField(_previewDuration, EditorStyles.toolbarTextField);
                if (_previewDuration <= 0f) { _previewDuration = 5f; }
                _autoPreview = GUILayout.Toggle(_autoPreview, _autoPreview ? "Stop" : "Go", EditorStyles.toolbarButton);
                _percent = EditorGUILayout.Slider(_percent, 0f, 1f);
                EditorGUILayout.EndHorizontal();
                EditorGUI.DrawPreviewTexture(GUILayoutUtility.GetRect(PREVIEW_WIDTH, PREVIEW_HEIGHT), _previewRenderTexture, null, ScaleMode.ScaleToFit);
            }
        }

        private void Update()
        {
            if (_showPreviewCamera)
            {
                if (_autoPreview)
                {
                    _percent = (Time.realtimeSinceStartup - _autoPreviewStartTime) / _previewDuration;
                    if (_percent >= 1f)
                    {
                        _percent = 0f;
                        _autoPreviewStartTime = Time.realtimeSinceStartup;
                    }
                }
                GetSceneView().Repaint();
                _previewCamera.transform.position = _previewCameraPos;
                _previewTarget.position = _previewTargetPos;
                _previewCamera.transform.LookAt(_previewTarget);
                _previewCamera.Render();
                // EditorGUI.DrawPreviewTexture(GUILayoutUtility.GetRect(PREVIEW_WIDTH, PREVIEW_HEIGHT), _previewRenderTexture,null,ScaleMode.ScaleToFit);
            }

            Repaint();
        }

        private void Draw(SceneView sv)
        {
            bool refreshInspector = false;
            if (_cameraBSpline != null)
            {
                refreshInspector = DrawBezier(_cameraBSpline, ref _cameraSelectedID);
                if (_showPreviewCamera)
                {
                    Point br = _cameraBSpline.GetResult(_percent);
                    _previewCameraPos = br.position;
                    _previewTargetPos = _previewCameraPos + br.Direction;
                }
            }
            if (_targetBSpline != null)
            {
                refreshInspector |= DrawBezier(_targetBSpline, ref _targetSelectedID);
                if (_showPreviewCamera)
                {
                    Point br = _targetBSpline.GetResult(_percent);
                    _previewTargetPos = br.position;
                }
            }

            if (_cameraBSpline != null)
            {
                Handles.color = Color.red;
                Handles.DrawLine(_previewCameraPos, _previewTargetPos);
            }

            if (refreshInspector) { Repaint(); }
        }

        // 画贝塞尔线，返回值为是否刷新检视面板
        private static bool DrawBezier(BezierSpline bs, ref int selectedIndex)
        {
            bool refreshInspector = false;
            for (int i = 0; i < bs.Count - 1; i++)
            {
                refreshInspector |= DrawPoint(bs, i, ref selectedIndex);
                Handles.DrawBezier(bs[i].Position, bs[i + 1].Position, bs[i].HandleR, bs[i + 1].HandleL, Color.green, null, BEZIERSPLINE_WIDTH);
            }
            refreshInspector |= DrawPoint(bs, bs.Count - 1, ref selectedIndex);

            return refreshInspector;
        }

        // 画点，返回值为是否刷新检视面板
        private static bool DrawPoint(BezierSpline bs, int index, ref int selectedIndex)
        {
            if (bs.points == null || bs.Count < 1) { return false; }

            bool refreshInspector = false;

            BezierPoint point = bs[index];

            float size = HandleUtility.GetHandleSize(point.Position);

            Handles.color = Color.blue;
            if (Handles.Button(point.Position, Quaternion.identity, size * HANDLE_SIZE, size * PICK_SIZE, Handles.DotHandleCap))
            {
                selectedIndex = index;
                refreshInspector = true;
            }

            if (_showHandles)
            {
                Handles.color = Color.gray;
                if (Handles.Button(point.HandleL, Quaternion.identity, size * HANDLE_SIZE, size * PICK_SIZE, Handles.DotHandleCap) || Handles.Button(point.HandleR, Quaternion.identity, size * HANDLE_SIZE, size * PICK_SIZE, Handles.DotHandleCap))
                {
                    selectedIndex = index;
                    refreshInspector = true;
                }
            }

            if (selectedIndex == index)
            {
                Vector3 p = point.Position;
                Vector3 pl = point.HandleL;
                Vector3 pr = point.HandleR;

                p = Handles.PositionHandle(p, Quaternion.identity);
                if (_showHandles)
                {
                    pl = Handles.PositionHandle(pl, Quaternion.identity);
                    pr = Handles.PositionHandle(pr, Quaternion.identity);
                }

                if (p != point.Position)
                {
                    Undo.RecordObject(bs, "change pos");
                    point.Position = p;
                    EditorUtility.SetDirty(bs);
                    refreshInspector = true;
                }
                else if (pl != point.HandleL)
                {
                    Undo.RecordObject(bs, "change left handler");
                    point.HandleL = pl;
                    EditorUtility.SetDirty(bs);
                    refreshInspector = true;
                }
                else if (pr != point.HandleR)
                {
                    Undo.RecordObject(bs, "change right handler");
                    point.HandleR = pr;
                    EditorUtility.SetDirty(bs);
                    refreshInspector = true;
                }
            }
            if (_showHandles)
            {
                Handles.DrawLine(point.Position, point.HandleL);
                Handles.DrawLine(point.Position, point.HandleR);
            }
            return refreshInspector;
        }

        // 绘制贝塞尔点的面板，返回值为是否刷新检视面板
        private static bool DrawBezierPointInspector(BezierSpline bs, int index, ref int insertID, ref int removeID, ref int selectedID)
        {
            bool refreshSv = false;

            GUI.backgroundColor = index == selectedID ? Color.yellow : Color.cyan;
            if (bs != null && bs.Count > index)
            {
                EditorGUILayout.BeginVertical(EditorStyles.textField);
                BezierPoint point = bs[index];
                if (point == null)
                {
                    Undo.RecordObject(bs, "add bezier point");
                    bs[index] = new BezierPoint();
                    EditorUtility.SetDirty(bs);
                    point = bs[index];

                    refreshSv = true;
                }
                Vector3 pos = point.Position;
                pos = EditorGUILayout.Vector3Field("Postion", pos);
                if (point.Position != pos)
                {
                    point.Position = pos;

                    refreshSv = true;
                }
                EditorGUILayout.BeginHorizontal();
                point.type = (BezierPoint.PointType)EditorGUILayout.EnumPopup("Type", point.type);
                point.Percent = EditorGUILayout.FloatField("Percent", point.Percent);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                if (selectedID == index)
                {
                    if (GUILayout.Button("UnSelect", EditorStyles.miniButtonLeft)) { selectedID = -1; refreshSv = true; }
                }
                else
                {
                    if (GUILayout.Button("Select", EditorStyles.miniButtonLeft)) { selectedID = index; refreshSv = true; }
                }
                if (GUILayout.Button("Focus", EditorStyles.miniButtonMid)) { SceneView.lastActiveSceneView.LookAt(pos); }
                if (GUILayout.Button("Insert", EditorStyles.miniButtonMid)) { insertID = index; refreshSv = true; }
                if (GUILayout.Button("Remove", EditorStyles.miniButtonRight)) { removeID = index; refreshSv = true; }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            GUI.backgroundColor = BackgroundColor;

            return refreshSv;
        }

        private static SceneView GetSceneView()
        {
#if LAST_ACTIVE_SCENE_VIEW
            return SceneView.lastActiveSceneView;
#else
            return SceneView.currentDrawingSceneView;
#endif
        }
    }
}
