using UnityEngine;

namespace GRT.Tween
{
    public class TweenCameraMatrix : Tween<float>
    {
        [SerializeField] private Camera _camera;
        [SerializeField] [Range(0, 3)] private int _row;
        [SerializeField] [Range(0, 3)] private int _column;

        public Camera Camera { get => _camera; set => _camera = value; }
        public int Row { get => _row; set => _row = Mathf.Clamp(value, 0, 3); }
        public int Column { get => _column; set => _column = Mathf.Clamp(value, 0, 3); }

        public override float Current
        {
            get => _camera.projectionMatrix[Row, Column];
            protected set
            {
                var m = _camera.projectionMatrix;
                m[Row, Column] = value;
                _camera.projectionMatrix = m;
            }
        }

        public override float Project(float percent)
        {
            var p = Mathf.Lerp(From, To, percent);
            Current = p;
            return p;
        }
    }
}