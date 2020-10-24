//************************************************************//
//      Author      :       GF47
//      DataTime    :       2014/2/24 23:01:18
//      Edited      :       2014/2/24 23:01:18
//************************************************************//

namespace GRT.Tween
{
    /// <summary>
    /// 缓动算法
    /// </summary>
    internal class EaseAlgorithm
    {
        public EaseAlgorithm(TweenEase easeType)
        {
            SetEaseType(easeType);
        }

        public TweenEase EaseType
        {
            get { return _easeEaseType; }
            set
            {
                SetEaseType(value);
            }
        }

        public float Result(float percent)
        {
            return _easeAlgorithm.Algorithm(percent);
        }
        private TweenEase _easeEaseType;
        private TweenLinear _easeAlgorithm;

        private void SetEaseType(TweenEase type)
        {
            _easeEaseType = type;
            switch (_easeEaseType)
            {
                case TweenEase.Linear:
                    _easeAlgorithm = new TweenLinear();
                    break;
                case TweenEase.CubicEaseIn:
                    _easeAlgorithm = new TweenCubicEaseIn();
                    break;
                case TweenEase.CubicEaseOut:
                    _easeAlgorithm = new TweenCubicEaseOut();
                    break;
                case TweenEase.CubicEaseInOut:
                    _easeAlgorithm = new TweenCubicEaseInOut();
                    break;
                case TweenEase.QuadEaseIn:
                    _easeAlgorithm = new TweenQuadEaseIn();
                    break;
                case TweenEase.QuadEaseOut:
                    _easeAlgorithm = new TweenQuadEaseOut();
                    break;
                case TweenEase.QuadEaseInOut:
                    _easeAlgorithm = new TweenQuadEaseInOut();
                    break;
                case TweenEase.QuartEaseIn:
                    _easeAlgorithm = new TweenQuartEaseIn();
                    break;
                case TweenEase.QuartEaseOut:
                    _easeAlgorithm = new TweenQuartEaseOut();
                    break;
                case TweenEase.QuartEaseInOut:
                    _easeAlgorithm = new TweenQuartEaseInOut();
                    break;
                default:
                    _easeAlgorithm = new TweenLinear();
                    break;
            }
        }
    }
}
