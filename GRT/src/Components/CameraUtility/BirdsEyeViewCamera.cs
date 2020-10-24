//************************************************************//
//      Author      :       GF47
//      DataTime    :       2014/5/15 星期四 16:34:17
//      Edited      :       2014/5/15 星期四 16:34:17
//************************************************************//

using UnityEngine;

namespace GRT.Components.CameraUtility
{
    /// <summary>
    /// Author          :GF47
    /// DataTime        :2014/5/15 星期四 16:34:17
    /// [Panorama] Introduction  :鸟瞰相机
    /// </summary>
    public class BirdsEyeViewCamera : MonoBehaviour
    {
        public Transform target; // 焦点
        public float minDistance = 10f; // 最小距离
        public float maxDistance = 200f; // 最大距离
        public float wheelSpeed = 20f; // 滚轮速度
        public float minOrthographicSize = 0.01f;
        public float maxOrthographicSize = 50f;
        [Range(0, 2)]
        public int rotateButton = 1; // 旋转的按钮
        public float rotateSpeedX = 1f; // x轴旋转的速度
        public float rotateSpeedY = 1f; // y轴旋转的速度
        public float rotateLimitMinY = 10f; // y轴最小值
        public float rotateLimitMaxY = 80f; // y轴最大值
        public float smoothTime = 0.5f; // 旋转插值时间
        public float smoothDampMaxSpeed = 10000f; // 旋转插值

        [Range(0, 2)]
        public int moveButton = 2; // 位移的按钮
        public float moveSpeedX = 0.25f; // x轴位移的速度
        public float moveSpeedY = 0.25f; // y轴位移的速度

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

        /// <summary>
        /// 响应鼠标消息的X轴最小值
        /// </summary>
        [HideInInspector]
        public float ActivatedRectXMin;

        /// <summary>
        /// 响应鼠标消息的X轴最大值
        /// </summary>
        [HideInInspector]
        public float ActivatedRectXMax;

        /// <summary>
        /// 响应鼠标消息的Y轴最小值
        /// </summary>
        [HideInInspector]
        public float ActivatedRectYMin;

        /// <summary>
        /// 响应鼠标消息的Y轴最大值
        /// </summary>
        [HideInInspector]
        public float ActivatedRectYMax;

        void OnEnable()
        {
            _camera = GetComponent<Camera>();

            Reset();
        }

        void LateUpdate()
        {
            // 拦截Rect外的鼠标消息
            float percent = Input.mousePosition.x / Screen.width;
            if (percent < ActivatedRectXMin || percent > ActivatedRectXMax) return;
            percent = Input.mousePosition.y / Screen.height;
            if (percent < ActivatedRectYMin || percent > ActivatedRectYMax) return;

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

