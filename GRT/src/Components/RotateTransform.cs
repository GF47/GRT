using System;
using UnityEngine;

namespace GRT.Components
{
    public class RotateTransform : MonoBehaviour
    {
        [InspectorDisplayAs("旋转按钮")] [SerializeField] private int _rotateButton = 1;
        [InspectorDisplayAs("经速度")] [SerializeField] private float _longitudeFactor = 1f;
        [InspectorDisplayAs("纬速度")] [SerializeField] private float _latitudeFactor = 1f;
        [InspectorDisplayAs("纬度最小值")] [SerializeField] private float _lowerLatitude = 10f;
        [InspectorDisplayAs("纬度最大值")] [SerializeField] private float _upperLatitude = 80f;
        [InspectorDisplayAs("惯性持续时间")] [SerializeField] private float _inertialDuration = 0.5f;
        [InspectorDisplayAs("惯性速率")] [SerializeField] private float _inertialFactor = 10000f;

        /// <summary> /// 旋转按钮 /// </summary>
        public int RotateButton { get => _rotateButton; set => _rotateButton = value; }

        /// <summary> /// 经速度 /// </summary>
        public float LongitudeFactor { get => _longitudeFactor; set => _longitudeFactor = value; }

        /// <summary> /// 纬速度 /// </summary>
        public float LatitudeFactor { get => _latitudeFactor; set => _latitudeFactor = value; }

        /// <summary> /// 纬度最小值 /// </summary>
        public float LowerLatitude { get => _lowerLatitude; set => _lowerLatitude = value; }

        /// <summary> /// 纬度最大值 /// </summary>
        public float UpperLatitude { get => _upperLatitude; set => _upperLatitude = value; }

        /// <summary> /// 惯性持续时间 /// </summary>
        public float InertialDuration { get => _inertialDuration; set => _inertialDuration = value; }

        /// <summary> /// 惯性速率 /// </summary>
        public float InertialFactor { get => _inertialFactor; set => _inertialFactor = value; }

        public event Action rotateStarting;

        public event Action rotating;

        public event Action rotateEnding;

        private Vector2 _lastScreenPoint; // 上一帧的鼠标坐标
        private Vector2 _screenPointOffset; // 鼠标坐标偏移
        private Vector2 _angle; // 目标角度
        private Vector2 _currentAngle; // 实际角度
        private Vector2 _palstance; // 角速度

        private Vector2 _initialAngle; // 初始角度

        private void Start() { _initialAngle = transform.eulerAngles; }

        private void OnEnable()
        {
            var x = transform.eulerAngles.x;
            if (x >= 180f) { x -= 360f; }
            _angle = new Vector2(Mathf.Clamp(x, _lowerLatitude, _upperLatitude), transform.eulerAngles.y);

            _currentAngle = _angle;
            transform.rotation = Quaternion.Euler(_angle);

            _screenPointOffset = Vector2.zero;
            _lastScreenPoint = Input.mousePosition;
        }

        private void Update()
        {
            _screenPointOffset = new Vector2(Input.mousePosition.x - _lastScreenPoint.x, Input.mousePosition.y - _lastScreenPoint.y);

            if (Input.GetMouseButton(_rotateButton))
            {
                if (Input.GetMouseButtonDown(_rotateButton))
                {
                    RotateStarting();
                }
                Rotating();
            }
            if (Input.GetMouseButtonUp(_rotateButton))
            {
                RotateEnding();
            }

            _currentAngle = Vector2.SmoothDamp(_currentAngle, _angle, ref _palstance, _inertialDuration, _inertialFactor, Time.deltaTime);
            transform.rotation = Quaternion.Euler(_currentAngle);

            _lastScreenPoint = Input.mousePosition;
        }

        private void RotateStarting()
        {
            _screenPointOffset = Vector2.zero;

            var x = transform.eulerAngles.x;
            if (x >= 180f) { x -= 360f; }

            _angle = new Vector2(Mathf.Clamp(x, _lowerLatitude, _upperLatitude), transform.eulerAngles.y);

            _currentAngle = _angle;

            rotateStarting?.Invoke();
        }

        private void Rotating()
        {
            _angle += new Vector2(
                _latitudeFactor * _screenPointOffset.y,
                -_longitudeFactor * _screenPointOffset.x);

            _angle.x = Mathf.Clamp(_angle.x, _lowerLatitude, _upperLatitude);

            rotating?.Invoke();
        }

        private void RotateEnding()
        {
            rotateEnding?.Invoke();
        }

        public void ResetRotation()
        {
            _angle = _initialAngle;
        }
    }
}