/* ****************************************************************
 * @File Name   :   GF47GetRelativeTransform
 * @Author      :   GF47
 * @Date        :   2015/8/27 14:20:50
 * @Description :   to do
 * @Edit        :   2015/8/27 14:20:50
 * ***************************************************************/

using System.Text;
using UnityEditor;
using UnityEngine;

namespace GF47Editor
{
    /// <summary>
    /// A example class in Unity
    /// </summary>
    public class GF47GetRelativeTransform : EditorWindow
    {
        private Transform _father;
        private Transform _child;

        [MenuItem("Tools/GF47 Editor/Transform/Get Relative Transform")]
        static void Init()
        {
            GF47GetRelativeTransform window = GetWindow<GF47GetRelativeTransform>();
            window.position = new Rect(200f, 200f, 400f, 400f);
            window.Show();
        }

        void OnGUI()
        {
            _father = EditorGUILayout.ObjectField("父物体", _father, typeof(Transform), true) as Transform;
            _child = EditorGUILayout.ObjectField("子物体", _child, typeof(Transform), true) as Transform;
            if (GUILayout.Button("获取"))
            {
                if (_father != null && _child != null)
                {
                    Matrix4x4 world2FatherMatrix = _father.worldToLocalMatrix;
                    Vector3 pos = world2FatherMatrix.MultiplyPoint(_child.position);
                    Vector3 eulerAngles = (Quaternion.Inverse(_father.rotation) * _child.rotation).eulerAngles;
                    Vector3 scale = world2FatherMatrix.MultiplyVector(_child.lossyScale);

                    StringBuilder tmp = new StringBuilder();
                    tmp.Append("<position>");
                    tmp.Append(pos.x.ToString("F4"));
                    tmp.Append(",");
                    tmp.Append(pos.y.ToString("F4"));
                    tmp.Append(",");
                    tmp.Append(pos.z.ToString("F4"));
                    tmp.Append("</position>\n");

                    tmp.Append("<eulerAngles>");
                    tmp.Append(eulerAngles.x.ToString("F4"));
                    tmp.Append(",");
                    tmp.Append(eulerAngles.y.ToString("F4"));
                    tmp.Append(",");
                    tmp.Append(eulerAngles.z.ToString("F4"));
                    tmp.Append("</eulerAngles>\n");

                    tmp.Append("<scale>");
                    tmp.Append(scale.x.ToString("F4"));
                    tmp.Append(",");
                    tmp.Append(scale.y.ToString("F4"));
                    tmp.Append(",");
                    tmp.Append(scale.z.ToString("F4"));
                    tmp.Append("</scale>\n");

                    EditorGUIUtility.systemCopyBuffer = tmp.ToString();
                    Debug.Log(string.Format("子物体{0}相对父物体{1}的Transform为:", _child, _father));
                    Debug.Log(tmp);
                }
                else if(_child != null)
                {
                    Vector3 pos = _child.position;
                    Vector3 eulerAngles = _child.eulerAngles;
                    Vector3 scale = _child.lossyScale;

                    StringBuilder tmp = new StringBuilder();
                    tmp.Append("<position>");
                    tmp.Append(pos.x.ToString("F4"));
                    tmp.Append(",");
                    tmp.Append(pos.y.ToString("F4"));
                    tmp.Append(",");
                    tmp.Append(pos.z.ToString("F4"));
                    tmp.Append("</position>\n");

                    tmp.Append("<eulerAngles>");
                    tmp.Append(eulerAngles.x.ToString("F4"));
                    tmp.Append(",");
                    tmp.Append(eulerAngles.y.ToString("F4"));
                    tmp.Append(",");
                    tmp.Append(eulerAngles.z.ToString("F4"));
                    tmp.Append("</eulerAngles>\n");

                    tmp.Append("<scale>");
                    tmp.Append(scale.x.ToString("F4"));
                    tmp.Append(",");
                    tmp.Append(scale.y.ToString("F4"));
                    tmp.Append(",");
                    tmp.Append(scale.z.ToString("F4"));
                    tmp.Append("</scale>\n");

                    EditorGUIUtility.systemCopyBuffer = tmp.ToString();
                    Debug.Log(string.Format("物体{0}的Transform为:", _child));
                    Debug.Log(tmp);
                }
            }
        }
    }
}