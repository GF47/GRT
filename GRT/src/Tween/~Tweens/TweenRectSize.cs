using UnityEngine;

namespace GRT.Tween
{
    public class TweenRectSize : Tween<Vector2>
    {
        [SerializeField] private RectTransform _target;

        public override Vector2 Current { get => _target.sizeDelta; protected set => _target.sizeDelta = value; }

        public override Vector2 Project(float percent) => Current = Vector2.Lerp(From, To, percent);

        private void Awake()
        {
            if (_target == null)
            {
                _target = GetComponent<RectTransform>();
            }
        }
    }
}