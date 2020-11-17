/***************************************************************
 * @File Name       : BezierSplineInspector
 * @Author          : GF47
 * @Description     : [BezierSpline]的编辑器界面
 * @Date            : 2017/6/8/星期四 11:47:51
 * @Edit            : none
 **************************************************************/
#define LAST_ACTIVE_SCENE_VIEW
// #undef LAST_ACTIVE_SCENE_VIEW

using GRT.Geometry.Bezier;
using UnityEditor;
using UnityEngine;

namespace GRT.Editor.Inspectors
{
    [CustomEditor(typeof(BezierSpline))]
    public class BezierSplineInspector : UnityEditor.Editor
    {
        // private const float MIN_OFFSET =
        //     //*/
        //     0.0000001f;
        //     /*/
        //     float.Epsilon;
        //     //*/

        // private const int DRAW_POINTS_COUNT = 32;

        private const float HANDLE_SIZE = 0.04f;
        private const float PICK_SIZE = 0.06f;

        // private bool _smooth;

        private int _selectedIndex = -1;

        private BezierSpline _target;
        // private readonly Vector3[] _points = new Vector3[DRAW_POINTS_COUNT + 1];

        // private float _minMaxThreshold = 1f;

        private void OnEnable()
        {
            _target = (BezierSpline)target;
            SceneView.onSceneGUIDelegate += Draw;
        }

        private void OnDisable()
        {
            AssetDatabase.SaveAssets();
            SceneView.onSceneGUIDelegate -= Draw;
        }

        public override void OnInspectorGUI()
        {
            // EditorGUILayout.BeginHorizontal();
            // BezierSplineAsset bsa = (BezierSplineAsset)EditorGUILayout.ObjectField("serializedBezier", _asset, typeof(BezierSplineAsset), false);
            // if (_asset != bsa)
            // {
            //     _asset = bsa;
            //     _target.spline = _asset.bezierSpline;
            // }
            // if (GUILayout.Button("serialize", EditorStyles.miniButton))
            // {
            //     BezierSplineAsset bsa_ = ScriptableObject.CreateInstance<BezierSplineAsset>();
            //     bsa_.bezierSpline = _target.spline;
            //     string path = EditorUtility.SaveFilePanelInProject("Select bezier spline asset path", "bezierSpline", "asset", "");
            //     if (!string.IsNullOrEmpty(path)) { AssetDatabase.CreateAsset(bsa_, path); }
            // }
            // EditorGUILayout.EndHorizontal();

            int insertId = -1;
            int removeId = -1;
            GUILayout.Space(10f);
            // bool tmpSmooth = _smooth;
            // tmpSmooth = GUILayout.Toggle(tmpSmooth, tmpSmooth ? "Smooth" : "PolyLine", EditorStyles.toggle);
            // if (tmpSmooth != _smooth)
            // {
            //     _smooth = tmpSmooth;
            //     RepaintSceneView();
            // }
            // GUILayout.Space(10f);
            Color bg = GUI.backgroundColor;
            for (int i = 0; i < _target.Count; i++)
            {
                GUI.backgroundColor = i == _selectedIndex ? Color.yellow : Color.cyan;
                EditorGUILayout.BeginVertical(EditorStyles.textField);
                BezierPoint point = _target[i];
                if (point == null)
                {
                    Undo.RecordObject(_target, "add bezier point");
                    _target[i] = new BezierPoint();
                    EditorUtility.SetDirty(_target);
                    point = _target[i];
                }
                Vector3 position = point.Point;
                position = EditorGUILayout.Vector3Field("Postion", position);
                if (point.Point != position)
                {
                    point.Point = position;
                }
                point.type = (BezierPoint.PointType)EditorGUILayout.EnumPopup("Type", point.type);
                EditorGUILayout.BeginHorizontal();
                if (_selectedIndex == i)
                {
                    if (GUILayout.Button("UnSelect", EditorStyles.miniButtonLeft))
                    {
                        _selectedIndex = -1;
                        RepaintSceneView();
                    }
                }
                else
                {
                    if (GUILayout.Button("Select", EditorStyles.miniButtonLeft))
                    {
                        _selectedIndex = i;
                        RepaintSceneView();
                    }
                }
                if (GUILayout.Button("Insert", EditorStyles.miniButtonMid))
                {
                    insertId = i;
                }
                if (GUILayout.Button("Remove", EditorStyles.miniButtonRight))
                {
                    removeId = i;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                GUI.backgroundColor = bg;
                GUILayout.Space(10f);
            }
            if (insertId > -1)
            {
                float percent = (insertId - 0.5f) / (_target.Count - 1);
                BezierResult result = _target.GetResult(percent);
                Undo.RecordObject(_target, "insert bezier point");
                _target.Insert(insertId, new BezierPoint(result.position, 1f, BezierPoint.PointType.Bezier));
                EditorUtility.SetDirty(_target);
                RepaintSceneView();
            }
            if (removeId > -1)
            {
                Undo.RecordObject(_target, "remove bezier point");
                _target.RemoveAt(removeId);
                EditorUtility.SetDirty(_target);
                RepaintSceneView();
            }

            // _minMaxThreshold = EditorGUILayout.FloatField("缩放", _minMaxThreshold);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add", EditorStyles.miniButtonLeft))
            {
                Undo.RecordObject(_target, "add bezier point");
                _target.Add(new BezierPoint());
                EditorUtility.SetDirty(_target);
                RepaintSceneView();
            }
            if (GUILayout.Button("Save", EditorStyles.miniButtonRight))
            {
                // AssetDatabase.SaveAssets();
                AssetDatabase.SaveAssets();
            }
            EditorGUILayout.EndHorizontal();

            // 粒子
            // if (GUILayout.Button("Apply", EditorStyles.miniButton))
            // {
            //     OnPressApplyButton();
            // }
        }

        private void Draw(SceneView sv)
        {
            for (int i = 0; i < _target.Count - 1; i++)
            {
                DrawPoint(i);
                Handles.DrawBezier(_target[i].Point, _target[i + 1].Point, _target[i].HandleR, _target[i + 1].HandleL, Color.green, null, 2f);
                // if (_smooth)
                // {
                //     Handles.DrawBezier(_target[i].Point, _target[i + 1].Point, _target[i].HandleR, _target[i + 1].HandleL, Color.green, null, 2f);
                // }
            }
            DrawPoint(_target.Count - 1);
            // if (!_smooth)
            // {
            //     float step = 1f / DRAW_POINTS_COUNT;
            //     float total = 0f;
            //     for (int i = 0; i < _points.Length; i++, total += step)
            //     {
            //         BezierResult result = _target.GetResult(total);
            //         _points[i] = result.position;
            //     }
            //     Handles.color = Color.green;
            //     Handles.DrawPolyLine(_points);
            // }
        }

        // 画点
        private void DrawPoint(int index)
        {
            if (_target.points == null || _target.Count < 1)
            {
                return;
            }
            BezierPoint point = _target[index];

            float size = HandleUtility.GetHandleSize(point.Point);

            Handles.color = Color.blue;
            if (Handles.Button(point.Point, Quaternion.identity, size * HANDLE_SIZE, size * PICK_SIZE, Handles.DotHandleCap))
            {
                _selectedIndex = index;
                Repaint();
            }

            Handles.color = Color.gray;
            if (Handles.Button(point.HandleL, Quaternion.identity, size * HANDLE_SIZE, size * PICK_SIZE, Handles.DotHandleCap) || Handles.Button(point.HandleR, Quaternion.identity, size * HANDLE_SIZE, size * PICK_SIZE, Handles.DotHandleCap))
            {
                _selectedIndex = index;
                Repaint();
            }

            if (_selectedIndex == index)
            {
                Vector3 p = point.Point;
                Vector3 pl = point.HandleL;
                Vector3 pr = point.HandleR;

                p = Handles.PositionHandle(p, Quaternion.identity);
                pl = Handles.PositionHandle(pl, Quaternion.identity);
                pr = Handles.PositionHandle(pr, Quaternion.identity);

                if (p != point.Point)
                {
                    Undo.RecordObject(_target, "change pos");
                    point.Point = p;
                    EditorUtility.SetDirty(_target);
                }
                else if (pl != point.HandleL)
                {
                    Undo.RecordObject(_target, "change left handle");
                    point.HandleL = pl;
                    EditorUtility.SetDirty(_target);
                }
                else if (pr != point.HandleR)
                {
                    Undo.RecordObject(_target, "change right handle");
                    point.HandleR = pr;
                    EditorUtility.SetDirty(_target);
                }
            }
            Handles.DrawLine(point.Point, point.HandleL);
            Handles.DrawLine(point.Point, point.HandleR);
        }

        // 重新绘制Scene视图
        private static void RepaintSceneView()
        {
            // SceneView.RepaintAll();
#if LAST_ACTIVE_SCENE_VIEW
            SceneView.lastActiveSceneView.Repaint();
#else
            SceneView.currentDrawingSceneView.Repaint();
#endif
        }

        // 粒子
        // private void OnPressApplyButton()
        // {
        //     ParticleSystem p = _target.GetComponent<ParticleSystem>();
        //     if (p != null)
        //     {
        //         AnimationCurve curveX = new AnimationCurve();
        //         AnimationCurve curveY = new AnimationCurve();
        //         AnimationCurve curveZ = new AnimationCurve();

        //         BezierSpline bs = _target.spline;
        //         int segmentCount = bs.points.Count - 1;
        //         float segment = 1f / segmentCount;
        //         float errorLeftTangent = 0f;
        //         float errorRightTangent = 0f;

        //         for (int i = 0; i < segmentCount; i++)
        //         {
        //             Vector3 leftVelocity;
        //             Vector3 leftTangent;
        //             Vector3 rightVeolcity;
        //             Vector3 rightTangent;

        //             Vector3 v0 = bs[i].Point;
        //             Vector3 v1 = bs[i].HandleR;
        //             Vector3 v2 = bs[i + 1].HandleL;
        //             Vector3 v3 = bs[i + 1].Point;
        //             GetVelocity(v0, v1, v2, v3, 0f, out leftVelocity, out leftTangent);
        //             GetVelocity(v0, v1, v2, v3, 1f, out rightVeolcity, out rightTangent);
        //
        //             curveX.AddKey(new Keyframe(i * segment + MIN_OFFSET, leftVelocity.x, errorLeftTangent, leftTangent.x * segmentCount));
        //             curveY.AddKey(new Keyframe(i * segment + MIN_OFFSET, leftVelocity.y, errorLeftTangent, leftTangent.y * segmentCount));
        //             curveZ.AddKey(new Keyframe(i * segment + MIN_OFFSET, leftVelocity.z, errorLeftTangent, leftTangent.z * segmentCount));

        //             curveX.AddKey(new Keyframe((i + 1) * segment, rightVeolcity.x, rightTangent.x * segmentCount, errorRightTangent));
        //             curveY.AddKey(new Keyframe((i + 1) * segment, rightVeolcity.y, rightTangent.y * segmentCount, errorRightTangent));
        //             curveZ.AddKey(new Keyframe((i + 1) * segment, rightVeolcity.z, rightTangent.z * segmentCount, errorRightTangent));
        //         }

        //         p.startSpeed = 0f;

        //         ParticleSystem.VelocityOverLifetimeModule vel = p.velocityOverLifetime;
        //         float pStartLifetime = p.startLifetime;
        //         vel.enabled = true;
        //         vel.space = ParticleSystemSimulationSpace.Local;
        //         _minMaxThreshold = segmentCount / pStartLifetime;
        //         vel.x = new ParticleSystem.MinMaxCurve(_minMaxThreshold, curveX);
        //         vel.y = new ParticleSystem.MinMaxCurve(_minMaxThreshold, curveY);
        //         vel.z = new ParticleSystem.MinMaxCurve(_minMaxThreshold, curveZ);
        //     }
        // }

        // private static void GetVelocity(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, float t, out Vector3 velocity, out Vector3 tangent)
        // {
        //     ////////////////////////////////////////////////////////////////
        //     // v = -3s^2(v0-3v1+3v2-v3)t^2+6s(v0-2v1+v2)t-3(v0-v1)
        //     // v' = -6s^2(v0-3v1+3v2-v3)t+6(v0-2v1+v2)
        //     ////////////////////////////////////////////////////////////////
        //     Vector3 a = -3 * (v0 - 3 * v1 + 3 * v2 - v3);
        //     Vector3 b = 6 * (v0 - 2 * v1 + v2);
        //     Vector3 c = - 3 * (v0 - v1);
        //     velocity = a * t * t + b * t + c;
        //     tangent = 2 * a * t + b;
        // }
    }
}