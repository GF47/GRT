//************************************************************//
//      Author      :       GF47
//      DataTime    :       2014/2/24 23:29:17
//      Edited      :       2014/2/24 23:29:17
//************************************************************//

namespace GRT.Tween
{
    /// <summary>
    /// 方向算法
    /// </summary>
    internal class DirectionAlgorithm
    {
        public DirectionAlgorithm(TweenDirection directionType)
        {
            SetDirectionType(directionType);
        }
        public TweenDirection DirectionType
        {
            get { return (TweenDirection)_type; }
            set { SetDirectionType(value); }
        }
        public float Result(float percent)
        {
            switch (_type)
            {
                case 1:
                    return percent;
                case -1:
                    return 1 - percent;
            }
            return 0.0f;
        }

        private int _type;
        private void SetDirectionType(TweenDirection directionType)
        {
            if (directionType != TweenDirection.Toggle)
            {
                _type = (int)directionType;
                return;
            }
            _type = -_type;
        }
    }
}
