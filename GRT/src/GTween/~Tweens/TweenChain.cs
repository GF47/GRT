using System.Collections.Generic;
using UnityEngine;

namespace GRT.GTween
{
    public class TweenChain : GTween<float>
    {
        private float _step;
        private List<IPercent> _tweens;

        public override float From { get => 0f; set { } }
        public override float To { get => 1f; set { } }

        public List<IPercent> Tweens
        {
            get => _tweens; set
            {
                _tweens = value;

                if (_tweens.Count < 1)
                {
                    _step = float.PositiveInfinity;
                }
                else
                {
                    _step = 1f / _tweens.Count;
                }
            }
        }

        public override float Interpolate(float percent)
        {
            var i = Mathf.FloorToInt(percent / _step);

            if (i < Tweens.Count && Tweens[i] != null)
            {
                Tweens[i].Percent = (percent - i * _step) / _step;
            }
            return percent;
        }
    }
}