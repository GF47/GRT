using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GRT.Components
{
    public class ArcRotateCamera : MonoBehaviour
    {
        public enum MouseButton { Left = 0, Right = 1, Wheel = 2 }

        #region serialized field

        [InspectorDisplayAs("相机焦点")] private Transform _target;

        [InspectorDisplayAs("半径最小值")] private float _lowerRadius = 10f;
        [InspectorDisplayAs("半径最大值")] private float _upperRadius = 200f;
        [InspectorDisplayAs("相机半径")] private float _radius = 100f;
        [InspectorDisplayAs("推拉速率")] private float _zoomInFactor = 20f;

        [InspectorDisplayAs("旋转按钮")] private MouseButton _rotatingButton = MouseButton.Right;
        [InspectorDisplayAs("经速度")] private float _longitudeVFactor = 1f;
        [InspectorDisplayAs("纬速度")] private float _latitudeVFactor = 1f;
        [InspectorDisplayAs("纬度最小值")] private float _lowerLatitude = 10f;
        [InspectorDisplayAs("纬度最大值")] private float _upperLatitude = 80f;
        [InspectorDisplayAs("惯性持续时间")] private float _inertialDuration = 0.5f;
        [InspectorDisplayAs("惯性速率")] private float _inertialFactor = 10000f;

        [InspectorDisplayAs("移动按钮")] private MouseButton _panningButton = MouseButton.Wheel;
        [InspectorDisplayAs("移动速率")] private float _panningVFactor = 0.25f;
        [InspectorDisplayAs("仅限水平移动")] private bool _panningHorizontal = false;

        #endregion serialized field

        #region properties

        /// <summary> 相机焦点 </summary>
        public Transform Target { get => _target; set => _target = value; }

        /// <summary> 半径最小值 </summary>
        public float LowerRadius { get => _lowerRadius; set => _lowerRadius = Math.Min(_upperRadius, value); }
        /// <summary> 半径最大值 </summary>
        public float UpperRadius { get => _upperRadius; set => _upperRadius = Math.Max(_lowerRadius, value); }
        /// <summary> 相机半径 </summary>
        public float Radius { get => _radius; set => _radius = Math.Max(_lowerRadius, Math.Min(_upperRadius, value)); }
        /// <summary> 推拉速率 </summary>
        public float ZoomInFactor { get => _zoomInFactor; set => _zoomInFactor = Math.Max(0f, value); }

        /// <summary> 旋转按钮 </summary>
        public MouseButton RotatingButton { get => _rotatingButton; set => _rotatingButton = value; }
        /// <summary> 经速度 </summary>
        public float LongitudeVFactor { get => _longitudeVFactor; set => _longitudeVFactor = value; }
        /// <summary> 纬速度 </summary>
        public float LatitudeVFactor { get => _latitudeVFactor; set => _latitudeVFactor = value; }
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
        public float PanningVFactor { get => _panningVFactor; set => _panningVFactor = Math.Max(0f, value); }
        /// <summary> 仅限水平移动 </summary>
        public bool PanningHorizontal { get => _panningHorizontal; set => _panningHorizontal = value; }

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
        private Vector2 _lastEuler;
        private Vector2 _euler;
        private Vector2 _inertialEuler;
        private Vector3 _lastPoint;
        private Vector3 _cachedPoint;
        private Vector3 _buttonDownPoint;
        private float _inertialRadius;
        private float _zoomInVelocity;
        private Vector3 _velocity;
        private float _lastOrthographicRatio;

        private int _blockCount;
        private List<Vector4> _blockAreas = new List<Vector4>();

        public void AppendBlockArea(Vector4 a) { _blockAreas.Add(a); return; }
        public void RemoveBlockArea(Vector4 a) { _blockAreas.Remove(a); }
        public void ClearBlockArea() { _blockAreas.Clear(); }

        void LateUpdate()
        {
            if (!PointBlocked(Input.mousePosition)) return;

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
                if (Input.GetMouseButton((int)_rotatingButton))
                {
                    OnRotatingStart();
                }
                OnRotating();
            }
            if (Input.GetMouseButtonUp((int)_rotatingButton))
            {
                OnRotatingEnd();
            }

            // todo current

            _inertialEuler = Vector3.SmoothDamp(_inertialEuler, _euler, ref _velocity, _inertialDuration, _inertialFactor, Time.deltaTime);
            transform.rotation = Quaternion.Euler(_inertialEuler.y, _inertialEuler.x, 0f);

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

                _inertialRadius = Mathf.SmoothDamp(_inertialRadius, _radius, ref _zoomInVelocity, _inertialDuration, _inertialFactor, Time.deltaTime);
                transform.position = transform.rotation * new Vector3(0f, 0f, -_inertialRadius) + _target.position;
            }
        }

        private void OnRotatingStart()
        {
            onRotatingStart?.Invoke();
        }
        private void OnRotating()
        {
            onRotating?.Invoke();
        }
        private void OnRotatingEnd()
        {
            onRotatingEnd?.Invoke();
        }

        private void OnPanningStart()
        {
            onPanningStart?.Invoke();
        }
        private void OnPanning()
        {
            onPanning?.Invoke();
        }
        private void OnPanningEnd()
        {
            onPanningEnd.Invoke();
        }

        public void ResetCamera()
        {
            if (_target != null)
            {
                _radius = Vector3.Distance(transform.position, _target.position);
                _inertialRadius = _radius;
                transform.position = transform.rotation * new Vector3(0f, 0f, -_radius) + _target.position;
            }

            _lastEuler = Input.mousePosition;
            _euler = new Vector3(transform.eulerAngles.y, Mathf.Clamp(transform.eulerAngles.x, _lowerLatitude, _upperLatitude));
            _inertialEuler = _cachedPoint;

            _lastPoint = new Vector3(
                _panningVFactor * Input.mousePosition.x,
                _panningVFactor * Input.mousePosition.y,
                10f);
            _cachedPoint = _lastPoint;

            _lastOrthographicRatio = 0f;
        }

        public bool InvokeAtPoint(Action action) { return InvokeAtPoint(action, Input.mousePosition); }
        public bool InvokeAtPoint(Action action, Vector2 point)
        {
            if (PointBlocked(point)) return false;

            action?.Invoke();
            return true;
        }

        private bool PointBlocked(Vector2 point)
        {
            if (BlockCount > 0) return true;

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
