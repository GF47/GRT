//************************************************************//
//      Author      :       GF47
//      DataTime    :       2014/2/24 22:12:34
//      Edited      :       2014/2/24 22:12:34
//************************************************************//

namespace GRT.Tween
{
    internal class TweenCubicEaseIn : TweenLinear
    {
        public override float Algorithm(float percent)
        {
            return percent * percent * percent;
        }
    }
    internal class TweenCubicEaseOut : TweenLinear
    {
        public override float Algorithm(float percent)
        {
            // return 1 + (_percent - 1) ^ 3;
            percent -= 1f;
            return 1 + percent * percent * percent;
        }
    }
    internal class TweenCubicEaseInOut : TweenLinear
    {
        public override float Algorithm(float percent)
        {
            if (percent < 0.5f)
            {
                // return (2 * _percent) ^ 3 / 2;
                return 4 * percent * percent * percent;
            }
            // return 1 + ((_percent - 1) * 2) ^ 3 / 2;
            percent -= 1f;
            return 1 + percent * percent * percent * 4;
        }
    }
}
