using UnityEngine;

namespace GRT.GTween
{
    public class TweenCameraMatrix : GTween<float>
    {
        private int _row;
        private int _column;

        public override float From { get; set; }
        public override float To { get; set; }

        public Camera Camera { get; set; }

        public int Row { get => _row; set => _row = Mathf.Clamp(value, 0, 3); }
        public int Column { get => _column; set => _column = Mathf.Clamp(value, 0, 3); }

        public override float Interpolate(float percent)
        {
            var p = Mathf.Lerp(From, To, percent);

            var matrix = Camera.projectionMatrix;
            matrix[Row, Column] = p;
            Camera.projectionMatrix = matrix;

            return p;
        }
    }
}