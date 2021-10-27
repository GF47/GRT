using UnityEngine;

namespace GRT.Tween
{
    public class TweenRectPosition : Tween<Vector3>
    {
        [SerializeField] private RectTransform _target;

        public override Vector3 Current { get => _target.anchoredPosition3D; protected set => _target.anchoredPosition3D = value; }

        public override Vector3 Project(float percent) => Current = Vector3.Lerp(From, To, percent);

        private void Awake()
        {
            if (_target == null)
            {
                _target = GetComponent<RectTransform>();
            }
        }
    }
}