using UnityEngine;

namespace GRT.GTween
{
    public abstract class GTween<T> : IGStartable, IGDisposable, IGTickable, IInterpolable<T>, IPercent
    {
        private float _lifetime = 1f;
        private float _timeElapsed = 0f;

        private Ease _ease = Ease.Linear;
        private Loop _loop = Loop.Once;
        private Direction _direction = Direction.Forward;

        private ITweenPercent _tpEase = new TweenLinear();
        private ITweenPercentLoop _tpLoop = new TweenOnce();
        private ITweenPercent _tpDirection = new TweenForward();

        public float Lifetime
        {
            get => _lifetime; set
            {
                _lifetime = Mathf.Max(value, 0.02f);
                Interpolate(_tpEase.Calculate(_tpDirection.Calculate(_tpLoop.Calculate(_timeElapsed / _lifetime))));
            }
        }

        public float TimeElapsed
        {
            get => _timeElapsed; protected set
            {
                _timeElapsed = Mathf.Max(value, 0f);
                Interpolate(_tpEase.Calculate(_tpDirection.Calculate(_tpLoop.Calculate(_timeElapsed / _lifetime))));
            }
        }

        public Ease Ease
        {
            get => _ease; set
            {
                ResetEase(value);
            }
        }

        public Loop Loop
        {
            get => _loop; set
            {
                ResetLoop(value);
            }
        }

        public Direction Direction
        {
            get => _direction; set
            {
                ResetDirection(value);
            }
        }

        public bool IsAlive { get; protected set; }

        public IGScope Scope { get; set; }

        public abstract T From { get; set; }

        public abstract T To { get; set; }

        public float Percent
        {
            get => _timeElapsed % _lifetime; set
            {
                _timeElapsed = value * _lifetime;

                Interpolate(_tpEase.Calculate(_tpDirection.Calculate(_tpLoop.Calculate(value))));
            }
        }

        public virtual void GStart()
        {
            IsAlive = true;
            TimeElapsed = 0f;
        }

        public virtual void GDispose() => IsAlive = false;

        public void GTick(float delta)
        {
            TimeElapsed += delta;
            if (_tpLoop.IsStopped(_timeElapsed / _lifetime))
            {
                this.DetachFromScope(true);
            }
        }

        public abstract T Interpolate(float percent);

        private void ResetEase(Ease value)
        {
            _ease = value;
            switch (_ease)
            {
                case Ease.Linear: _tpEase = new TweenLinear(); break;
                case Ease.QuadIn: _tpEase = new TweenQuadEaseIn(); break;
                case Ease.QuadOut: _tpEase = new TweenQuadEaseOut(); break;
                case Ease.QuadInOut: _tpEase = new TweenQuadEaseInOut(); break;
                case Ease.CubicIn: _tpEase = new TweenCubicEaseIn(); break;
                case Ease.CubicOut: _tpEase = new TweenCubicEaseOut(); break;
                case Ease.CubicInOut: _tpEase = new TweenCubicEaseInOut(); break;
                case Ease.QuartIn: _tpEase = new TweenQuartEaseIn(); break;
                case Ease.QuartOut: _tpEase = new TweenQuartEaseOut(); break;
                case Ease.QuartInOut: _tpEase = new TweenQuartEaseInOut(); break;
                case Ease.BackIn: _tpEase = new TweenBackEaseIn(); break;
                case Ease.BackOut: _tpEase = new TweenBackEaseOut(); break;
                case Ease.BackInOut: _tpEase = new TweenBackEaseInOut(); break;
                case Ease.BounceIn: _tpEase = new TweenBounceEaseIn(); break;
                case Ease.BounceOut: _tpEase = new TweenBounceEaseOut(); break;
                case Ease.BounceInOut: _tpEase = new TweenBounceEaseInOut(); break;
                // case Ease.ElasticIn: _tpEase = new TweenElasticEaseIn(); break;
                // case Ease.ElasticOut: _tpEase = new TweenElasticEaseOut(); break;
                // case Ease.ElasticInOut: _tpEase = new TweenElasticEaseInOut(); break;
                default: _tpEase = new TweenLinear(); break;
            }
        }

        private void ResetLoop(Loop value)
        {
            _loop = value;
            switch (_loop)
            {
                case Loop.Once: _tpLoop = new TweenOnce(); break;
                case Loop.Loop: _tpLoop = new TweenLoop(); break;
                case Loop.PingPong: _tpLoop = new TweenPingPong(); break;
                case Loop.PingPongOnce: _tpLoop = new TweenPingPongOnce(); break;
                default: _tpLoop = new TweenOnce(); break;
            }
        }

        private void ResetDirection(Direction value)
        {
            _direction = value;
            switch (_direction)
            {
                case Direction.Forward: _tpDirection = new TweenForward(); break;
                case Direction.Backward: _tpDirection = new TweenBackward(); break;
                case Direction.Toggle:
                    if (_tpDirection is TweenForward)
                    {
                        _tpDirection = new TweenBackward();
                    }
                    else if (_tpDirection is TweenBackward)
                    {
                        _tpDirection = new TweenForward();
                    }
                    break;

                default: _tpDirection = new TweenForward(); break;
            }
        }
    }
}