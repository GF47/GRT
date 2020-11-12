using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.Tween
{
    public class TweenChain : Tween<float>
    {
        [SerializeField]
        [InheritFrom(typeof(IPercent))]
        private List<MonoBehaviour> _unityObjects;
        public List<IPercent> tweens;

        private float _step;

        private void Awake()
        {
            Reset();

            setValue = (f) =>
            {
                var i = Mathf.FloorToInt(f / _step);

                if (i < tweens.Count && tweens[i] != null)
                {
                    tweens[i].Percent = (f - i * _step) / _step;
                }

                return f;
            };
        }

        public void Reset()
        {
            from = 0f;
            to = 1f;

            tweens = _unityObjects.ConvertAll((uo) => uo == null ? null : uo as IPercent);

            if (tweens.Count < 1) { _step = Mathf.Infinity; }
            else { _step = 1f / tweens.Count; }
        }
    }
}
