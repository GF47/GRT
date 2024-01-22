using System.Collections.Generic;
using UnityEngine;

namespace GRT.Tween
{
    public class TweenChain : Tween<float>
    {
        [SerializeField]
        private List<MonoBehaviour> _monoBehaviours;

        public List<IPercent> tweens;

        private float _step;

        public override float From { get => 0f; set { from = 0f; } }
        public override float To { get => 1f; set { to = 1f; } }

        public override float Current { get => Percent; protected set { } }

        public override float Interpolate(float percent)
        {
            var i = Mathf.FloorToInt(percent / _step);

            if (i < tweens.Count && tweens[i] != null)
            {
                tweens[i].Percent = (percent - i * _step) / _step;
            }
            return percent;
        }

        private void Awake()
        {
            from = 0f;
            to = 1f;

            ReCalculateStep();
        }

        private void ReCalculateStep()
        {
            tweens = _monoBehaviours.ConvertAll(uo => uo == null ? null : uo as IPercent);
            tweens.RemoveAll(t => t == null);

            if (tweens.Count < 1) { _step = float.PositiveInfinity; }
            else { _step = 1f / tweens.Count; }
        }
    }
}