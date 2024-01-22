using UnityEngine;

namespace GRT.Tween
{
    public class TweenCameraFOV : Tween<float>
    {
        [SerializeField] private Camera _camera;

        public Camera Camera { get => _camera; set => _camera = value; }

        public override float Current { get => _camera.fieldOfView; protected set => _camera.fieldOfView = value; }

        public override float Interpolate(float percent)
        {
            Current = Mathf.Lerp(From, To, percent);
            return Current;
        }

        private void Awake()
        {
            if (_camera == null)
            {
                _camera = gameObject.GetComponent<Camera>();
            }
        }
    }
}