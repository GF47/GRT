//************************************************************//
//      Author      :       GF47
//      DataTime    :       2014/1/24 星期五 16:49:56
//      Edited      :       2014/1/24 星期五 16:49:56
//************************************************************//

using UnityEditor;
using UnityEngine;

namespace GF47Editor
{
    /// <summary>
    /// Author          :GF47
    /// DataTime        :2014/1/24 星期五 16:49:56
    /// [GF47SnapSettings] Introduction  :Nothing to introduce
    /// </summary>
    public class GF47SnapSettings : EditorWindow
    {
        private static bool _initialized;
        private static float _moveSnapX;
        private static float _moveSnapY;
        private static float _moveSnapZ;
        private static float _rotationSnap;
        private static float _scaleSnap;
        private static float _colliderCenter;
        private static float _colliderSize;

        private static Vector3 Move
        {
            get
            {
                Initialize();
                return new Vector3(_moveSnapX, _moveSnapY, _moveSnapZ);
            }
            set
            {
                EditorPrefs.SetFloat("MoveSnapX", value.x);
                _moveSnapX = value.x;
                EditorPrefs.SetFloat("MoveSnapY", value.y);
                _moveSnapY = value.y;
                EditorPrefs.SetFloat("MoveSnapZ", value.z);
                _moveSnapZ = value.z;
            }
        }

        private static float Rotation
        {
            get
            {
                Initialize();
                return _rotationSnap;
            }
            set
            {
                EditorPrefs.SetFloat("RotationSnap", value);
                _rotationSnap = value;
            }
        }

        private static float Scale
        {
            get
            {
                Initialize();
                return _scaleSnap;
            }
            set
            {
                EditorPrefs.SetFloat("ScaleSnap", value);
                _scaleSnap = value;
            }
        }

        private static float ColliderCenter
        {
            get
            {
                Initialize();
                return _colliderCenter;
            }
            set
            {
                EditorPrefs.SetFloat("ColliderCenter", value);
                _colliderCenter = value;
            }
        }

        private static float ColliderSize
        {
            get
            {
                Initialize();
                return _colliderSize;
            }
            set
            {
                EditorPrefs.SetFloat("ColliderSize", value);
                _colliderSize = value;
            }
        }

        private static void Initialize()
        {
            if (_initialized) return;
            _moveSnapX = EditorPrefs.GetFloat("MoveSnapX", 1f);
            _moveSnapY = EditorPrefs.GetFloat("MoveSnapY", 1f);
            _moveSnapZ = EditorPrefs.GetFloat("MoveSnapZ", 1f);
            _scaleSnap = EditorPrefs.GetFloat("ScaleSnap", 0.1f);
            _rotationSnap = EditorPrefs.GetFloat("RotationSnap", 15f);
            _colliderCenter = EditorPrefs.GetFloat("ColliderCenter", 0.1f);
            _colliderSize = EditorPrefs.GetFloat("ColliderSize", 0.1f);
            _initialized = true;
        }

        [MenuItem("Tools/GF47 Editor/Snap Settings")]
        private static void ShowSnapSettings()
        {
            GF47SnapSettings window = GetWindow<GF47SnapSettings>("Snap Settings");
            window.position = new Rect(300f, 300f, 380f, 200f);
            window.minSize = new Vector2(380f, 200f);
            window.Show();
        }

        private void OnGUI()
        {
            Vector3 move = Move;
            move.x = EditorGUILayout.FloatField("Snap.MoveX", Move.x);
            move.y = EditorGUILayout.FloatField("Snap.MoveY", Move.y);
            move.z = EditorGUILayout.FloatField("Snap.MoveZ", Move.z);
            if (GUI.changed)
            {
                if (move.x <= 0f)
                {
                    move.x = Move.x;
                }
                if (move.y <= 0f)
                {
                    move.y = Move.y;
                }
                if (move.z <= 0f)
                {
                    move.z = Move.z;
                }
                Move = move;
            }
            Rotation = EditorGUILayout.FloatField("Snap.Rotation", Rotation);
            Scale = EditorGUILayout.FloatField("Snap.Scale", Scale);
            ColliderCenter = EditorGUILayout.FloatField("Snap.ColliderCenter", ColliderCenter);
            ColliderSize = EditorGUILayout.FloatField("Snap.ColliderSize", ColliderSize);
            GUILayout.Space(5f);

            bool flagX = false;
            bool flagY = false;
            bool flagZ = false;
            bool flagRotation = false;
            bool flagScale = false;
            bool flagColliderCenter = false;
            bool flagColliderCenterZeroSetting = false;
            bool flagColliderSize = false;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("AllPosition", EditorStyles.miniButtonLeft))
            {
                flagX = true;
                flagY = true;
                flagZ = true;
            }
            if (GUILayout.Button("X", EditorStyles.miniButtonMid))
            {
                flagX = true;
            }
            if (GUILayout.Button("Y", EditorStyles.miniButtonMid))
            {
                flagY = true;
            }
            if (GUILayout.Button("Z", EditorStyles.miniButtonRight))
            {
                flagZ = true;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Rotation", EditorStyles.miniButtonLeft))
            {
                flagRotation = true;
            }
            if (GUILayout.Button("Scale", EditorStyles.miniButtonRight))
            {
                flagScale = true;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Collider.Center", EditorStyles.miniButtonLeft))
            {
                flagColliderCenter = true;
            }
            if (GUILayout.Button("Collider.CenterZeroSetting", EditorStyles.miniButtonMid))
            {
                flagColliderCenterZeroSetting = true;
            }
            if (GUILayout.Button("Collider.Size", EditorStyles.miniButtonRight))
            {
                flagColliderSize = true;
            }
            GUILayout.EndHorizontal();

            if (Selection.transforms != null)
            {
                if ((flagX | flagY) | flagZ)
                {
                    Vector3 vector2 = new Vector3(1f / Move.x, 1f / Move.y, 1f / Move.z);
                    foreach (Transform trans in Selection.transforms)
                    {
                        Vector3 pos = trans.localPosition;
                        if (flagX)
                        {
                            pos.x = Mathf.Round(pos.x * vector2.x) / vector2.x;
                        }
                        if (flagY)
                        {
                            pos.y = Mathf.Round(pos.y * vector2.y) / vector2.y;
                        }
                        if (flagZ)
                        {
                            pos.z = Mathf.Round(pos.z * vector2.z) / vector2.z;
                        }
                        trans.localPosition = pos;
                    }
                }
                if (flagRotation)
                {
                    float r = 1f / Rotation;
                    foreach (Transform trans in Selection.transforms)
                    {
                        Vector3 rot = trans.localEulerAngles;
                        rot = new Vector3(
                            Mathf.Round(rot.x * r) / r,
                            Mathf.Round(rot.y * r) / r,
                            Mathf.Round(rot.z * r) / r);
                        trans.localEulerAngles = rot;
                    }
                }
                if (flagScale)
                {
                    float s = 1f / Scale;
                    foreach (Transform trans in Selection.transforms)
                    {
                        Vector3 sle = trans.localScale;
                        sle = new Vector3(
                            Mathf.Round(sle.x * s) / s,
                            Mathf.Round(sle.y * s) / s,
                            Mathf.Round(sle.z * s) / s);
                        trans.localScale = sle;
                    }
                }
                if (flagColliderCenter | flagColliderCenterZeroSetting | flagColliderSize)
                {
                    float c = 1f / ColliderCenter;
                    foreach (GameObject obj in Selection.gameObjects)
                    {
                        Collider cTmp = obj.GetComponent<Collider>();
                        if (cTmp != null)
                        {
                            BoxCollider cBoxTmp = cTmp as BoxCollider;
                            if (cBoxTmp != null)
                            {
                                if (flagColliderCenterZeroSetting)
                                {
                                    cBoxTmp.center = Vector3.zero;
                                }
                                else if (flagColliderCenter)
                                {
                                    Vector3 center = cBoxTmp.center;
                                    center = new Vector3(
                                        Mathf.Round(center.x * c) / c,
                                        Mathf.Round(center.y * c) / c,
                                        Mathf.Round(center.z * c) / c);
                                    cBoxTmp.center = center;
                                }

                                if (flagColliderSize)
                                {
                                    Vector3 size = cBoxTmp.size;
                                    size = new Vector3(
                                        Mathf.Round(size.x * c) / c,
                                        Mathf.Round(size.y * c) / c,
                                        Mathf.Round(size.z * c) / c);
                                    cBoxTmp.size = size;
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}