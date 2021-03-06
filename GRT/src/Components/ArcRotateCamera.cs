﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.Components
{
    public class ArcRotateCamera : MonoBehaviour
    {
        public enum MouseButton { Left = 0, Right = 1, Wheel = 2 }

        #region serialized field

        [SerializeField] [InspectorDisplayAs("是否为局部坐标")] private bool _isLocal;

        [SerializeField] [InspectorDisplayAs("相机焦点")] private Transform _target;

        [SerializeField] [InspectorDisplayAs("半径最小值")] private float _lowerRadius = 10f;
        [SerializeField] [InspectorDisplayAs("半径最大值")] private float _upperRadius = 200f;
        [SerializeField] [InspectorDisplayAs("相机半径")] private float _radius = 100f;
        [SerializeField] [InspectorDisplayAs("推拉速率")] private float _zoomInFactor = 20f;

        [SerializeField] [InspectorDisplayAs("旋转按钮")] private MouseButton _rotatingButton = MouseButton.Right;
        [SerializeField] [InspectorDisplayAs("经速度")] private float _longitudeFactor = 1f;
        [SerializeField] [InspectorDisplayAs("纬速度")] private float _latitudeFactor = 1f;
        [SerializeField] [InspectorDisplayAs("纬度最小值")] private float _lowerLatitude = 10f;
        [SerializeField] [InspectorDisplayAs("纬度最大值")] private float _upperLatitude = 80f;
        [SerializeField] [InspectorDisplayAs("惯性持续时间")] private float _inertialDuration = 0.5f;
        [SerializeField] [InspectorDisplayAs("惯性速率")] private float _inertialFactor = 10000f;

        [SerializeField] [InspectorDisplayAs("移动按钮")] private MouseButton _panningButton = MouseButton.Wheel;
        [SerializeField] [InspectorDisplayAs("移动速率")] private float _panningFactor = 0.25f;
        [SerializeField] [InspectorDisplayAs("仅限水平移动")] private bool _panningHorizontal = false;

        #endregion serialized field

        #region properties

        /// <summary> 相机焦点 </summary>
        public Transform Target { get => _target; set { _target = value; ResetCamera(); } }

        /// <summary> 半径最小值 </summary>
        public float LowerRadius { get => _lowerRadius; set => _lowerRadius = Math.Min(_upperRadius, value); }

        /// <summary> 半径最大值 </summary>
        public float UpperRadius { get => _upperRadius; set => _upperRadius = Math.Max(_lowerRadius, value); }

        /// <summary> 相机半径 </summary>
        public float Radius { get => _radius; set => _radius = Mathf.Clamp(value, _lowerRadius, _upperLatitude); }

        /// <summary> 推拉速率 </summary>
        public float ZoomInFactor { get => _zoomInFactor; set => _zoomInFactor = value; }

        /// <summary> 旋转按钮 </summary>
        public MouseButton RotatingButton { get => _rotatingButton; set => _rotatingButton = value; }

        /// <summary> 经速度 </summary>
        public float LongitudeFactor { get => _longitudeFactor; set => _longitudeFactor = value; }

        /// <summary> 纬速度 </summary>
        public float LatitudeFactor { get => _latitudeFactor; set => _latitudeFactor = value; }

        /// <summary> 纬度最小值 </summary>
        public float LowerLatitude { get => _lowerLatitude; set => _lowerLatitude = Math.Min(_upperLatitude, value); }

        /// <summary> 纬度最大值 </summary>
        public float UpperLatitude { get => _upperLatitude; set => _upperLatitude = Math.Max(_lowerLatitude, value); }

        /// <summary> 惯性持续时间 </summary>
        public float InertialDuration { get => _inertialDuration; set => _inertialDuration = Math.Max(0f, value); }

        /// <summary> 惯性速率 </summary>
        public float InertialFactor { get => _inertialFactor; set => _inertialFactor = Math.Max(0f, value); }

        /// <summary> 移动按钮 </summary>
        public MouseButton PanningButton { get => _panningButton; set => _panningButton = value; }

        /// <summary> 移动速率 </summary>
        public float PanningFactor { get => _panningFactor; set => _panningFactor = Math.Max(0f, value); }

        /// <summary> 仅限水平移动 </summary>
        public bool PanningHorizontal { get => _panningHorizontal; set => _panningHorizontal = value; }

        /// <summary> 值大于0时，忽略鼠标事件 </summary>
        public int BlockCount { get => _blockCount; set { _blockCount = Math.Max(0, value); } }

        #endregion properties

        #region events

        public event Action onRotatingStart;

        public event Action onRotating;

        public event Action onRotatingEnd;

        public event Action onPanningStart;

        public event Action onPanning;

        public event Action onPanningEnd;

        #endregion events

        private Camera _camera;

        private Vector2 _lastScreenPoint; // 上一帧的鼠标坐标
        private Vector2 _screenPointOffset; // 鼠标坐标偏移
        private Vector2 _angle; // 目标角度
        private Vector2 _currentAngle; // 实际角度
        private Vector2 _palstance; // 角速度

        private Vector3 _position;

        private float _currentRadius; // 实际半径
        private float _zoomInVelocity; // 推拉速度

        private float _lastOrthographicRatio; // 正交状态下的放大比例

        private int _blockCount;
        private readonly List<Vector4> _blockAreas = new List<Vector4>();

        public void AppendBlockArea(Vector4 a) { _blockAreas.Add(a); return; }
        public void RemoveBlockArea(Vector4 a) { _blockAreas.Remove(a); }
        public void ClearBlockArea() { _blockAreas.Clear(); }

        private Vector3 TransformPosition
        {
            get => _isLocal ? transform.localPosition : transform.position;
            set { if (_isLocal) { transform.localPosition = value; } else { transform.position = value; } }
        }
        private Quaternion TransformRotation
        {
            get => _isLocal ? transform.localRotation : transform.rotation;
            set { if (_isLocal) { transform.localRotation = value; } else { transform.rotation = value; } }
        }
        private Vector3 TransformEulerAngles
        {
            get => _isLocal ? transform.localEulerAngles : transform.eulerAngles;
            set { if (_isLocal) { transform.localEulerAngles = value; } else { transform.eulerAngles = value; } }
        }
        private Vector3 TargetPosition
        {
            get => _isLocal ? _target.localPosition : _target.position;
            set { if (_isLocal) { _target.localPosition = value; } else { _target.position = value; } }
        }

        private void OnEnable()
        {
            ResetCamera();
        }

        private void LateUpdate()
        {
            if (PointBlocked(Input.mousePosition)) return;

            _screenPointOffset = new Vector2(Input.mousePosition.x - _lastScreenPoint.x, Input.mousePosition.y - _lastScreenPoint.y);

            #region 鼠标响应
            if (Input.GetMouseButton((int)_panningButton))
            {
                if (Input.GetMouseButtonDown((int)_panningButton))
                {
                    OnPanningStart();
                }
                OnPanning();
            }
            if (Input.GetMouseButtonUp((int)_panningButton))
            {
                OnPanningEnd();
            }

            if (Input.GetMouseButton((int)_rotatingButton))
            {
                if (Input.GetMouseButtonDown((int)_rotatingButton))
                {
                    OnRotatingStart();
                }
                OnRotating();
            }
            if (Input.GetMouseButtonUp((int)_rotatingButton))
            {
                OnRotatingEnd();
            }
            #endregion 鼠标响应

            _currentAngle = Vector2.SmoothDamp(_currentAngle, _angle, ref _palstance, _inertialDuration, _inertialFactor, Time.deltaTime);
            TransformRotation = Quaternion.Euler(_currentAngle);

            if (_target != null)
            {
                var radiusDelta = _zoomInFactor * Input.GetAxis("Mouse ScrollWheel");
                if (_camera.orthographic)
                {
                    _lastOrthographicRatio -= radiusDelta;
                    _camera.orthographicSize = Mathf.Pow(1.25f, _lastOrthographicRatio);
                }
                else
                {
                    _radius -= radiusDelta;
                }

                _radius = Mathf.Clamp(_radius, _lowerRadius, _upperRadius);

                _currentRadius = Mathf.SmoothDamp(_currentRadius, _radius, ref _zoomInVelocity, _inertialDuration, _inertialFactor, Time.deltaTime);
                TargetPosition = _position;
                TransformPosition = TransformRotation * new Vector3(0f, 0f, -_currentRadius) + TargetPosition;
            }
            else
            {
                TransformPosition = _position;
            }

            _lastScreenPoint = Input.mousePosition;
        }

        /// <summary> 重置 </summary>
        public void ResetCamera()
        {
            _camera = GetComponent<Camera>();

            _angle = new Vector3(
                Mathf.Clamp(TransformEulerAngles.x, _lowerLatitude, _upperLatitude),
                TransformEulerAngles.y);
            _currentAngle = _angle;
            TransformRotation = Quaternion.Euler(_angle);


            if (_target != null)
            {
                _radius = Vector3.Distance(TransformPosition, TargetPosition);
                _currentRadius = _radius;
                TransformPosition = TransformRotation * new Vector3(0f, 0f, -_currentRadius) + TargetPosition;

                _position = TargetPosition;
            }
            else
            {
                _radius = 0f;
                _currentRadius = _radius;

                _position = TransformPosition;
            }

            _screenPointOffset = Vector2.zero;
            _lastScreenPoint = Input.mousePosition;

            _lastOrthographicRatio = 0f;
        }

        /// <summary> 旋转开始 </summary>
        private void OnRotatingStart()
        {
            _screenPointOffset = Vector2.zero;

            var x = TransformEulerAngles.x;
            x = x >= 180f ? x - 360f : x;

            _angle = new Vector2(Mathf.Clamp(x, _lowerLatitude, _upperLatitude), TransformEulerAngles.y);
            _currentAngle = _angle;

            onRotatingStart?.Invoke();
        }

        /// <summary> 旋转 </summary>
        private void OnRotating()
        {
            _angle += new Vector2(
                -_latitudeFactor * _screenPointOffset.y,
                _longitudeFactor * _screenPointOffset.x);
            _angle.x = Mathf.Clamp(_angle.x, _lowerLatitude, _upperLatitude);

            onRotating?.Invoke();
        }

        /// <summary> 旋转终止 </summary>
        private void OnRotatingEnd()
        {
            onRotatingEnd?.Invoke();
        }

        /// <summary> 平移开始 </summary>
        private void OnPanningStart()
        {
            _screenPointOffset = Vector2.zero;

            _position = _target == null ? TransformPosition : TargetPosition;

            onPanningStart?.Invoke();
        }

        /// <summary> 平移 </summary>
        private void OnPanning()
        {
            var from = _camera.ScreenToWorldPoint(new Vector3(_lastScreenPoint.x, _lastScreenPoint.y, _currentRadius));
            var offset = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _currentRadius)) - from;
            _position -= _panningFactor * offset;

            onPanning?.Invoke();
        }

        /// <summary> 平移终止 </summary>
        private void OnPanningEnd()
        {
            onPanningEnd?.Invoke();
        }

        /// <summary>
        /// 在当前鼠标位置处触发事件，顺利执行会返回 True，被忽略则返回 False
        /// </summary>
        /// <param name="action">被触发的事件</param>
        /// <returns>事件是否被执行</returns>
        public bool InvokeAtPoint(Action action)
        {
            return InvokeAtPoint(action, Input.mousePosition);
        }

        /// <summary>
        /// 在当前屏幕位置处触发事件，顺利执行会返回 True，被忽略则返回 False
        /// </summary>
        /// <param name="action">被触发的事件</param>
        /// <param name="point">当前屏幕位置</param>
        /// <returns>事件是否被执行</returns>
        public bool InvokeAtPoint(Action action, Vector2 point)
        {
            if (PointBlocked(point)) return false;

            action?.Invoke();
            return true;
        }

        /// <summary>
        /// 检测当前屏幕位置的鼠标事件是否应被忽略
        /// </summary>
        /// <param name="point">当前的屏幕位置，一般是鼠标</param>
        /// <returns>是否应被忽略</returns>
        private bool PointBlocked(Vector2 point)
        {
            if (_blockCount > 0) return true;

            for (int i = 0; i < _blockAreas.Count; i++)
            {
                var a = _blockAreas[i];
                var px = point.x / Screen.width;
                var py = point.y / Screen.height;

                if (px > a.x && py > a.y && px < a.z && py < a.w) return true;
            }
            return false;
        }
    }
}
