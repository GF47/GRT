//************************************************************//
//      Author      :       GF47
//      DataTime    :       2014/5/15 星期四 16:34:17
//      Edited      :       2014/5/15 星期四 16:34:17
//************************************************************//

using System.Collections.Generic;
using UnityEngine;

namespace GRT.Components.CameraUtility
{
    /// <summary>
    /// Author          :GF47
    /// DataTime        :2014/5/15 星期四 16:34:17
    /// [Panorama] Introduction  :鸟瞰相机
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class BirdsEyeViewCamera : MonoBehaviour
    {
        [InspectorDisplayAs("相机焦点")]
        public Transform target;
        [InspectorDisplayAs("相机最小距离")]
        public float minDistance = 10f;
        [InspectorDisplayAs("相机最大距离")]
        public float maxDistance = 200f;
        [InspectorDisplayAs("滚轮速度")]
        public float wheelSpeed = 20f;
        [InspectorDisplayAs("鼠标拖拽旋转按钮", "0-鼠标左键/1-鼠标右键/2-鼠标中键（滚轮按下）")]
        [Range(0, 2)]
        public int rotateButton = 1;
        [InspectorDisplayAs("水平旋转的速度")]
        public float rotateSpeedX = 1f;
        [InspectorDisplayAs("垂直旋转的速度")]
        public float rotateSpeedY = 1f;
        [InspectorDisplayAs("相机竖直方向最小夹角")]
        public float rotateLimitMinY = 10f;
        [InspectorDisplayAs("相机竖直方向最大夹角")]
        public float rotateLimitMaxY = 80f;
        [InspectorDisplayAs("缓动持续时间")]
        public float smoothTime = 0.5f;
        [InspectorDisplayAs("缓动倍率")]
        public float smoothDampMaxSpeed = 10000f;

        [InspectorDisplayAs("鼠标拖拽焦点移动按钮", "0-鼠标左键/1-鼠标右键/2-鼠标中键（滚轮按下）")]
        [Range(0, 2)]
        public int moveButton = 2;
        [InspectorDisplayAs("焦点水平位移速度")]
        public float moveSpeedX = 0.25f;
        [InspectorDisplayAs("焦点竖直位移速度")]
        public float moveSpeedY = 0.25f;

        private float _distance = 100f;
        private Vector3 _lastMousePos;
        private Vector3 _saveMousePos;
        private float _smoothDistance;
        private float _velocityDistance;
        private Vector3 _smooth;
        private Vector3 _velocity;

        private float _lastOrthographicRatio;

        private Camera _camera;
        private Vector3 _lastMousePos2;
        private Vector3 _saveMousePos2;
        private Vector3 _posWhenButtonDown;

        private List<Vector4> _blockAreas;
        /// <summary>
        /// <para>...............(z,w)</para>
        /// <para>...............</para>
        /// <para>...............</para>
        /// <para>...............</para>
        /// <para>...............</para>
        /// <para>...............</para>
        /// <para>...............</para>
        /// <para>(x,y)</para>
        /// </summary>
        public void AppendBlockArea(Vector4 v) { _blockAreas?.Add(v); }
        public void RemoveBlockArea(Vector4 v) { _blockAreas?.Remove(v); }
        public void ClearBlockAreas() { _blockAreas.Clear(); }

        void OnEnable()
        {
            _camera = GetComponent<Camera>();

            Reset();
        }

        void LateUpdate()
        {
            // 拦截BlockArea内的鼠标消息
            for (int i = 0; i < _blockAreas.Count; i++)
            {
                Vector4 area = _blockAreas[i];
                float px = Input.mousePosition.x / Screen.width;
                float py = Input.mousePosition.y / Screen.height;
                if (px > area.x && py > area.y && px < area.z && py < area.w)
                {
                    return;
                }
            }

            // 移动
            if (Input.GetMouseButton(moveButton))
            {
                if (Input.GetMouseButtonDown(moveButton))
                {
                    _lastMousePos2.x = Input.mousePosition.x * moveSpeedX;
                    _lastMousePos2.y = Input.mousePosition.y * moveSpeedY;
                    _lastMousePos2.z = 10f;
                    _saveMousePos2 = _lastMousePos2;
                    _posWhenButtonDown = target.position;
                }
                _saveMousePos2.x = Input.mousePosition.x * moveSpeedX;
                _saveMousePos2.y = Input.mousePosition.y * moveSpeedY;
                Vector3 p0 = _camera.ScreenToWorldPoint(_saveMousePos2);
                Vector3 p1 = _camera.ScreenToWorldPoint(_lastMousePos2);
                Vector3 move = p0 - p1;
                target.position = _posWhenButtonDown - move;
            }

            // 旋转
            if (Input.GetMouseButton(rotateButton))
            {
                if (Input.GetMouseButtonDown(rotateButton))
                {
                    _lastMousePos = Input.mousePosition;
                }
                _saveMousePos.x += (Input.mousePosition.x - _lastMousePos.x) * rotateSpeedX;
                _saveMousePos.y -= (Input.mousePosition.y - _lastMousePos.y) * rotateSpeedY;
                _lastMousePos = Input.mousePosition;
                _saveMousePos.y = Mathf.Clamp(_saveMousePos.y, rotateLimitMinY, rotateLimitMaxY);
            }
            _smooth = Vector3.SmoothDamp(_smooth, _saveMousePos, ref _velocity, smoothTime, smoothDampMaxSpeed, Time.deltaTime);
            transform.rotation = Quaternion.Euler(_smooth.y, _smooth.x, 0);
            if (target != null)
            {
                float scrollWheelDelta = Input.GetAxis("Mouse ScrollWheel") * wheelSpeed;
                if (_camera.orthographic)
                {
                    _lastOrthographicRatio -= scrollWheelDelta;

                    _camera.orthographicSize = Mathf.Pow(1.25f, _lastOrthographicRatio);
                }
                else
                {
                    _distance -= scrollWheelDelta;
                }
                _distance = Mathf.Clamp(_distance, minDistance, maxDistance);

                _smoothDistance = Mathf.SmoothDamp(_smoothDistance, _distance, ref _velocityDistance, smoothTime, smoothDampMaxSpeed, Time.deltaTime);
                transform.position = transform.rotation * new Vector3(0, 0, -_smoothDistance) + target.position;

            }
        }

        public void Reset()
        {
            if (target != null)
            {
                _distance = Vector3.Distance(transform.position, target.position);
                _smoothDistance = _distance;
                transform.position = transform.rotation * new Vector3(0, 0, -_smoothDistance) + target.position;
            }
            _lastMousePos = Input.mousePosition;
            _saveMousePos = new Vector3(transform.eulerAngles.y, transform.eulerAngles.x, 0f);
            _saveMousePos.y = Mathf.Clamp(_saveMousePos.y, rotateLimitMinY, rotateLimitMaxY);
            _smooth = _saveMousePos;

            _lastMousePos2.x = Input.mousePosition.x * moveSpeedX;
            _lastMousePos2.y = Input.mousePosition.y * moveSpeedY;
            _lastMousePos2.z = 10f;
            _saveMousePos2 = _lastMousePos2;

            _lastOrthographicRatio = 0f;
        }
    }
}

