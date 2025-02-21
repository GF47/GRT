﻿using System;

namespace GRT.GTween
{
    internal interface ITweenPercent
    {
        float Calculate(float percent);
    }

    #region Ease

    internal struct TweenLinear : ITweenPercent
    {
        public float Calculate(float percent)
        {
            return percent;
        }
    }

    #region Quad

    internal struct TweenQuadEaseIn : ITweenPercent
    {
        public float Calculate(float percent)
        {
            return percent * percent;
        }
    }

    internal struct TweenQuadEaseOut : ITweenPercent
    {
        public float Calculate(float percent)
        {
            // return 1-(1-percent)^2;
            percent -= 1f;
            return 1f - percent * percent;
        }
    }

    internal struct TweenQuadEaseInOut : ITweenPercent
    {
        public float Calculate(float percent)
        {
            if (percent < 0.5f)
            {
                // return (2*percent)^2/2;
                return 2f * percent * percent;
            }
            else
            {
                // return 1-(2*(1-percent))^2/2;
                percent -= 1f;
                return 1f - 2f * percent * percent;
            }
        }
    }

    #endregion Quad

    #region Cubic

    internal struct TweenCubicEaseIn : ITweenPercent
    {
        public float Calculate(float percent)
        {
            return percent * percent * percent;
        }
    }

    internal struct TweenCubicEaseOut : ITweenPercent
    {
        public float Calculate(float percent)
        {
            // return 1-(1-percent)^3;
            percent -= 1f;
            return 1f + percent * percent * percent;
        }
    }

    internal struct TweenCubicEaseInOut : ITweenPercent
    {
        public float Calculate(float percent)
        {
            if (percent < 0.5f)
            {
                // return (2*percent)^3/2;
                return 4 * percent * percent * percent;
            }
            else
            {
                // return 1-(2*(1-percent))^3/2;
                percent -= 1f;
                return 1f + 4 * percent * percent * percent;
            }
        }
    }

    #endregion Cubic

    #region Quart

    internal struct TweenQuartEaseIn : ITweenPercent
    {
        public float Calculate(float percent)
        {
            return percent * percent * percent * percent;
        }
    }

    internal struct TweenQuartEaseOut : ITweenPercent
    {
        public float Calculate(float percent)
        {
            // return 1-(1-percent)^4;
            percent -= 1f;
            return 1f - percent * percent * percent * percent;
        }
    }

    internal struct TweenQuartEaseInOut : ITweenPercent
    {
        public float Calculate(float percent)
        {
            if (percent < 0.5f)
            {
                // return (2*percent)^4/2;
                return 8 * percent * percent * percent * percent;
            }
            else
            {
                // return 1-(2*(1-percent))^4/2;
                percent -= 1f;
                return 1f - 8 * percent * percent * percent * percent;
            }
        }
    }

    #endregion Quart

    #region Back

    internal struct TweenBackEaseIn : ITweenPercent
    {
        public float Calculate(float percent)
        {
            const float s = 1.70158f;
            return percent * percent * (percent * (s + 1f) - s);
        }
    }

    internal struct TweenBackEaseOut : ITweenPercent
    {
        public float Calculate(float percent)
        {
            const float s = 1.70158f;
            percent -= 1f;
            return 1f + percent * percent * (percent * (s + 1f) + s);
        }
    }

    internal struct TweenBackEaseInOut : ITweenPercent
    {
        public float Calculate(float percent)
        {
            const float s = 1.70158f;
            const float s_ = s * 1.525f;

            if (percent < 0.5f)
            {
                return 2f * percent * percent * (2f * percent * (s_ + 1f) - s_);
            }
            else
            {
                percent -= 1f;
                return 1f + 2 * percent * percent * (2f * percent * (s_ + 1f) + s_);
            }
        }
    }

    #endregion Back

    #region Bounce

    internal struct TweenBounceEaseIn : ITweenPercent
    {
        public float Calculate(float percent)
        {
            return 1f - TweenBounceEaseOut.Calculate_(1f - percent);
        }
    }

    internal struct TweenBounceEaseOut : ITweenPercent
    {
        public float Calculate(float percent)
        {
            return Calculate_(percent);
        }

        internal static float Calculate_(float percent)
        {
            if (percent < 1f / 2.75f)
            {
                return 7.5625f * percent * percent;
            }
            else if (percent < 2f / 2.75f)
            {
                percent -= 1.5f / 2.75f;
                return 7.5625f * percent * percent + 0.75f;
            }
            else if (percent < 2.5f / 2.75f)
            {
                percent -= 2.25f / 2.75f;
                return 7.5625f * percent * percent + 0.9375f;
            }
            else
            {
                percent -= 2.625f / 2.75f;
                return 7.5625f * percent * percent + 0.984375f;
            }
        }
    }

    internal struct TweenBounceEaseInOut : ITweenPercent
    {
        public float Calculate(float percent)
        {
            if (percent < 0.5f)
            {
                return 0.5f - 0.5f * TweenBounceEaseOut.Calculate_(1f - 2f * percent);
            }
            else
            {
                return 0.5f + 0.5f * TweenBounceEaseOut.Calculate_(2f * percent - 1f);
            }
        }
    }

    #endregion Bounce

#if Elastic

    #region Elastic

    internal struct TweenElasticEaseIn : ITweenPercent
    {
        public float Calculate(float percent)
        {
            throw new NotImplementedException();
        }
    }

    internal struct TweenElasticEaseOut : ITweenPercent
    {
        public float Calculate(float percent)
        {
            throw new NotImplementedException();
        }
    }

    internal struct TweenElasticEaseInOut : ITweenPercent
    {
        public float Calculate(float percent)
        {
            throw new NotImplementedException();
        }
    }

    #endregion Elastic

#endif

    #endregion Ease

    #region Loop

    internal interface ITweenPercentLoop : ITweenPercent
    {
        bool IsStopped(float percent);
    }

    internal struct TweenOnce : ITweenPercentLoop
    {
        public float Calculate(float percent)
        {
            return percent.Clamp(0f, 1f);
        }

        public bool IsStopped(float percent)
        {
            return percent >= 1f;
        }
    }

    internal struct TweenLoop : ITweenPercentLoop
    {
        public float Calculate(float percent)
        {
            return percent - (float)Math.Floor(percent);
        }

        public bool IsStopped(float percent)
        {
            return false;
        }
    }

    internal struct TweenPingPong : ITweenPercentLoop
    {
        public float Calculate(float percent)
        {
            return Calculate_(percent);
        }

        internal static float Calculate_(float percent)
        {
            percent %= 2f;
            if (percent < 0f) { percent += 2f; }
            return percent < 1f ? percent : 2f - percent;
        }

        public bool IsStopped(float percent)
        {
            return false;
        }
    }

    internal struct TweenPingPongOnce : ITweenPercentLoop
    {
        public float Calculate(float percent)
        {
            return TweenPingPong.Calculate_(2f * percent);
        }

        public bool IsStopped(float percent)
        {
            return percent >= 1f;
        }
    }

    #endregion Loop

    #region Direction

    internal struct TweenForward : ITweenPercent
    {
        public float Calculate(float percent)
        {
            return percent;
        }
    }

    internal struct TweenBackward : ITweenPercent
    {
        public float Calculate(float percent)
        {
            return 1f - percent;
        }
    }

    #endregion Direction
}