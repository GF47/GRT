//************************************************************//
//      Author      :       GF47
//      DataTime    :       2014/3/18 星期二 13:53:26
//      Edited      :       2014/3/18 星期二 13:53:26
//************************************************************//

namespace GRT.Tween
{
    internal class TweenQuadEaseIn : TweenLinear
    {
        public override float Algorithm(float percent)
        {
            return percent * percent;
        }
    }
    internal class TweenQuadEaseOut : TweenLinear
    {
        public override float Algorithm(float percent)
        {
            //return 1 - (percent - 1) ^ 2;
            percent -= 1f;
            return 1 - percent * percent;
        }
    }
    internal class TweenQuadEaseInOut : TweenLinear
    {
        public override float Algorithm(float percent)
        {
            if (percent < 0.5f)
            {
                return 2 * percent * percent;
            }
            // return 1 - 2 * (percent - 1) ^ 2;
            percent -= 1f;
            return 1 - 2 * percent * percent;
        }
    }
}
