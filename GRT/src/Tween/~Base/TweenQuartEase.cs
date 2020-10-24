namespace GRT.Tween
{
    internal class TweenQuartEaseIn : TweenLinear
    {
        public override float Algorithm(float percent)
        {
            return percent * percent * percent * percent;
        }
    }
    internal class TweenQuartEaseOut : TweenLinear
    {
        public override float Algorithm(float percent)
        {
            percent -= 1f;
            return 1 - percent * percent * percent * percent;
        }
    }
    internal class TweenQuartEaseInOut : TweenLinear
    {
        public override float Algorithm(float percent)
        {
            if (percent < 0.5f)
            {
                return 8 * percent * percent * percent * percent;
            }
            //return 1 - (2 * (percent - 1)) ^ 4;
            percent -= 1f;
            return 1 - 8 * percent * percent * percent * percent;
        }
    }
}
