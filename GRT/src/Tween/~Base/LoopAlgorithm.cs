//************************************************************//
//      Author      :       GF47
//      DataTime    :       2014/3/2 3:11:30
//      Edited      :       2014/3/2 3:11:30
//************************************************************//

using UnityEngine;

namespace GRT.Tween
{
    /// <summary>
    /// 循环算法
    /// </summary>
    internal class LoopAlgorithm
    {
        public LoopAlgorithm(TweenLoop loopType)
        {
            _type = (int)loopType;
            _direction = true;
        }
        public Factor Result(float percent)
        {
            _factor = percent;

            switch (_type)
            {
                case 0: // Once
                    return new Factor(_factor.Clamp(0.0f, 1.0f), percent >= 1.0f);
                case 1: // Loop
                    // return new Factor(_factor % 1.0f, false); // 浮点数求余不精确
                    return new Factor(_factor - Mathf.Floor(_factor), false);
                case 2: // PingPong
                    if (_factor > 1.0f)
                    {
                        _direction = !_direction;
                        // _factor %= 1.0f; // 浮点数求余不精确
                        _factor -= Mathf.Floor(_factor);
                    }
                    _factor = _direction ? _factor : 1.0f - _factor;
                    return new Factor(_factor, false);
                case 3: // PingPongOnce
                    if (_factor > 1.0f)
                    {
                        _direction = true;
                        // _factor %= 1.0f;
                        _factor -= Mathf.Floor(_factor);
                    }
                    else if (_factor > 0.5f)
                    {
                        _direction = false;
                        // _factor %= 0.5f;
                        _factor = Mod(_factor, 0.5f);
                    }
                    _factor = _direction ? 2.0f * _factor : 1.0f - 2.0f * _factor;
                    return new Factor(_factor, percent >= 1.0f);
                default:
                    return new Factor(_factor.Clamp(0.0f, 1.0f), percent >= 1.0f); // the same as Once
            }
        }
        public TweenLoop LoopType
        {
            get { return (TweenLoop)_type; }
            set { _type = (int)value; }
        }

        private int _type;
        private bool _direction;
        private float _factor;

        private float Mod(float a, float b)
        {
            return a - Mathf.Floor(a / b) * b;
        }
    }
}
