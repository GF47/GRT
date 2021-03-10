using System;

namespace GRT.Tween
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
            return 1 + percent * percent * percent;
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
                return 1 + 4 * percent * percent * percent;
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

    #endregion Ease

    #region Loop

    internal interface ITweenPercentLoop : ITweenPercent
    {
        bool IsStopped(float percent);
        void Reset();
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

        public void Reset() { }
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

        public void Reset() { }
    }

    internal struct TweenPingPong : ITweenPercentLoop
    {
        private bool _isForward;

        public float Calculate(float percent)
        {
            if (percent > 1f)
            {
                _isForward = !_isForward;
                percent -= (float)Math.Floor(percent);
            }
            return _isForward ? percent : 1f - percent;
        }

        public bool IsStopped(float percent)
        {
            return false;
        }

        public void Reset()
        {
            _isForward = true;
        }
    }

    internal struct TweenPingPongOnce : ITweenPercentLoop
    {
        private bool _isForward;

        public float Calculate(float percent)
        {
            if (percent > 1f)
            {
                _isForward = true;
                percent -= (float)Math.Floor(percent);
            }
            else if (percent > 0.5f)
            {
                _isForward = false;
                percent -= (float)Math.Floor(percent / 0.5f) * 0.5f;
            }
            return _isForward ? 2f * percent : 1f - 2f * percent;
        }

        public bool IsStopped(float percent)
        {
            return percent >= 1f;
        }

        public void Reset()
        {
            _isForward = true;
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
