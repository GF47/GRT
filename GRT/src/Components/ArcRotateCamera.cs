using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.Components
{
    public class ArcRotateCamera : MonoBehaviour
    {
        public enum MouseButton { Left = 0, Right = 1, Wheel = 2 }

        #region serialized field

        [SerializeField][InspectorAlias("是否为局部坐标")] private bool _isLocal;

        [SerializeField][InspectorAlias("相机焦点")] private Transform _target;

        [SerializeField][InspectorAlias("半径最小值")] private float _lowerRadius = 10f;
        [SerializeField][InspectorAlias("半径最大值")] private float _upperRadius = 200f;
        [SerializeField][InspectorAlias("相机半径")] private float _radius = 100f;
        [SerializeField][InspectorAlias("推拉速率")] private float _zoomInFactor = 20f;
        [SerializeField][InspectorAlias("是否允许推拉")] private bool _zoomable = true;

        [SerializeField][InspectorAlias("旋转按钮")] private MouseButton _rotatingButton = MouseButton.Right;
        [SerializeField][InspectorAlias("经速度")] private float _longitudeFactor = 1f;
        [SerializeField][InspectorAlias("纬速度")] private float _latitudeFactor = 1f;
        [SerializeField][InspectorAlias("纬度最小值")] private float _lowerLatitude = 10f;
        [SerializeField][InspectorAlias("纬度最大值")] private float _upperLatitude = 80f;
        [SerializeField][InspectorAlias("惯性持续时间")] private float _inertialDuration = 0.5f;
        [SerializeField][InspectorAlias("惯性速率")] private float _inertialFactor = 10000f;

        [SerializeField][InspectorAlias("移动按钮")] private MouseButton _panningButton = MouseButton.Wheel;
        [SerializeField][InspectorAlias("移动速率")] private float _panningFactor = 0.25f;
        [SerializeField][InspectorAlias("仅限水平移动")] private bool _panningHorizontal = false;

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

        /// <summary> 是否允许推拉 </summary>
        public bool Zoomable { get => _zoomable; set => _zoomable = value; }

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

        public event Action RotatingStart;

        public event Action Rotating;

        public event Action RotatingEnd;

        public event Action PanningStart;

        public event Action Panning;

        public event Action PanningEnd;

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

        public Vector3 Position
        {
            get => _isLocal ? transform.localPosition : transform.position;
            private set { if (_isLocal) { transform.localPosition = value; } else { transform.position = value; } }
        }

        public Vector3 TargetPosition
        {
            get => _isLocal ? _target.localPosition : _target.position;
            private set { if (_isLocal) { _target.localPosition = value; } else { _target.position = value; } }
        }

        public Quaternion Rotation
        {
            get => _isLocal ? transform.localRotation : transform.rotation;
            private set { if (_isLocal) { transform.localRotation = value; } else { transform.rotation = value; } }
        }

        public Vector3 EulerAngles
        {
            get => _isLocal ? transform.localEulerAngles : transform.eulerAngles;
            private set { if (_isLocal) { transform.localEulerAngles = value; } else { transform.eulerAngles = value; } }
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
            Rotation = Quaternion.Euler(_currentAngle);

            if (_target != null)
            {
                var radiusDelta = _zoomable ? _zoomInFactor * Input.GetAxis("Mouse ScrollWheel") : 0f;
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
                Position = Rotation * new Vector3(0f, 0f, -_currentRadius) + TargetPosition;
            }
            else
            {
                Position = _position;
            }

            _lastScreenPoint = Input.mousePosition;
        }

        /// <summary> 重置 </summary>
        public void ResetCamera()
        {
            if (_camera == null)
            {
                _camera = GetComponent<Camera>();
            }

            var x = EulerAngles.x;
            if (x > 180f) { x -= 360f; }

            _angle = new Vector2(Mathf.Clamp(x, _lowerLatitude, _upperLatitude), EulerAngles.y);
            _currentAngle = _angle;
            Rotation = Quaternion.Euler(_angle);

            if (_target != null)
            {
                _radius = Vector3.Distance(Position, TargetPosition);
                _currentRadius = _radius;
                Position = Rotation * new Vector3(0f, 0f, -_currentRadius) + TargetPosition;

                _position = TargetPosition;
            }
            else
            {
                _radius = 0f;
                _currentRadius = _radius;

                _position = Position;
            }

            _screenPointOffset = Vector2.zero;
            _lastScreenPoint = Input.mousePosition;

            _lastOrthographicRatio = 0f;
        }

        public void SetPosition(Vector3 pos)
        {
            Position = pos;

            if (_target != null)
            {
                var direction = TargetPosition - Position;

                Rotation = Quaternion.LookRotation(direction);
                _angle = Rotation.eulerAngles;
                _currentAngle = _angle;

                _radius = direction.magnitude;
                _currentRadius = _radius;

                _position = TargetPosition;
            }
            else
            {
                _radius = 0f;
                _currentRadius = 0f;

                _position = Position;
            }
        }

        public void SetTargetPosition(Vector3 pos)
        {
            if (_target != null)
            {
                TargetPosition = pos;
            }
        }

        public void SetRotation(Vector3 rot)
        {
            _angle = rot;
            _currentAngle = _angle;
            Rotation = Quaternion.Euler(_angle);
        }

        /// <summary> 旋转开始 </summary>
        private void OnRotatingStart()
        {
            _screenPointOffset = Vector2.zero;

            var x = EulerAngles.x;
            if (x >= 180f) { x -= 360f; }

            _angle = new Vector2(Mathf.Clamp(x, _lowerLatitude, _upperLatitude), EulerAngles.y);
            _currentAngle = _angle;

            RotatingStart?.Invoke();
        }

        /// <summary> 旋转 </summary>
        private void OnRotating()
        {
            _angle += new Vector2(
                -_latitudeFactor * _screenPointOffset.y,
                _longitudeFactor * _screenPointOffset.x);
            _angle.x = Mathf.Clamp(_angle.x, _lowerLatitude, _upperLatitude);

            Rotating?.Invoke();
        }

        /// <summary> 旋转终止 </summary>
        private void OnRotatingEnd()
        {
            RotatingEnd?.Invoke();
        }

        /// <summary> 平移开始 </summary>
        private void OnPanningStart()
        {
            _screenPointOffset = Vector2.zero;

            _position = _target == null ? Position : TargetPosition;

            PanningStart?.Invoke();
        }

        /// <summary> 平移 </summary>
        private void OnPanning()
        {
            var from = _camera.ScreenToWorldPoint(new Vector3(_lastScreenPoint.x, _lastScreenPoint.y, _currentRadius));
            var offset = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _currentRadius)) - from;
            _position -= _panningFactor * offset;

            Panning?.Invoke();
        }

        /// <summary> 平移终止 </summary>
        private void OnPanningEnd()
        {
            PanningEnd?.Invoke();
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
