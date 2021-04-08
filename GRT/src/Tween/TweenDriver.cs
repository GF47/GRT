using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GRT.Tween
{
    public class TweenDriver : MonoBehaviour, IPercent
    {
        public const int PUBLIC_GROUP = 0;

        [SerializeField] private bool _useTimeScale;
        [SerializeField] private float _delay;
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private int _group = 1;
        [SerializeField] private bool _isLateUpdate;
        [SerializeField] private List<MonoBehaviour> _iPercentTargets;
        [SerializeField] private Ease _ease = Ease.Linear;
        [SerializeField] private Loop _loop = Loop.Once;
        [SerializeField] private Direction _direction = Direction.Forward;

        private ITweenPercent _tpEase, _tpDirection;
        private ITweenPercentLoop _tpLoop;

        public Ease Ease
        {
            get => _ease;
            set
            {
                ResetEase(value);
            }
        }

        public Loop Loop
        {
            get => _loop;
            set
            {
                ResetLoop(value);
            }
        }

        public Direction Direction
        {
            get => _direction;
            set
            {
                ResetDirection(value);
            }
        }

        public bool UseTimeScale { get => _useTimeScale; set => _useTimeScale = value; }
        public float Delay { get => _delay; set => _delay = value; }
        public float Duration { get => _duration; set => _delta = 1f / (_duration = Math.Max(value, 0.02f)); }
        private float _delta;
        public int Group { get => _group; set => _group = value; }
        public bool IsLateUpdate { get => _isLateUpdate; set => _isLateUpdate = value; }
        public List<IPercent> Targets { get => _targets; }
        private List<IPercent> _targets;

        public event Action<TweenDriver> Stopping;
        public UnityEvent StoppingUEvent;

        float IPercent.Percent { get => _percent; set => _percent = Mathf.Clamp(value, 0f, 1f); }
        private float _percent;

        #region flags for delay

        private bool _isStarted;
        private float _startTime;

        #endregion flags for delay

        private void Awake()
        {
            if (_iPercentTargets != null)
            {
                _targets = _iPercentTargets.ConvertAll(uo => uo as IPercent);
                _targets.RemoveAll(p => p == null);
            }
            else
            {
                _targets = new List<IPercent>();
            }
        }

        private void Start()
        {
            Ease = _ease;
            Loop = _loop;
            Direction = _direction;

            _delta = 1f / _duration;
            Update_();
        }

        private void Update() { if (!IsLateUpdate) { Update_(); } }
        private void LateUpdate() { if (IsLateUpdate) { Update_(); } }

        private void Update_()
        {
            var delta = _useTimeScale ? Time.deltaTime : Time.unscaledDeltaTime;
            var time = _useTimeScale ? Time.time : Time.unscaledTime;

            if (!_isStarted)
            {
                _startTime = time + _delay;
                _isStarted = true;
            }
            if (time < _startTime) return;

            _percent += _delta * delta;

            Sample(_tpEase.Calculate(_tpDirection.Calculate(_tpLoop.Calculate(_percent))), _tpLoop.IsStopped(_percent));

            _percent -= (int)_percent; // math.floor
        }

        private void Sample(float percent, bool stopped)
        {
            _targets.ForEach(t => { if (t != null) { t.Percent = percent; } });
            if (stopped)
            {
                Stopping?.Invoke(this);
                StoppingUEvent.Invoke();

                enabled = false;
            }
        }
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
            _tpLoop.Reset();
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

        public TweenDriver Reset(bool resetDelay = true)
        {
            _tpLoop.Reset();

            if (resetDelay) { _isStarted = false; }
            Duration = _duration;
            _percent = 0f;
            return this;
        }

        public TweenDriver Play(Direction direction)
        {
            ResetDirection(direction);
            enabled = true;
            return this;
        }

        public TweenDriver Play(int direction) => Play((Direction)direction);
        public TweenDriver Play(bool direction) => Play(direction ? Direction.Forward : Direction.Backward);

        public static TweenDriver Play(GameObject go, float duration)
        {
            var td = go.GetComponent<TweenDriver>() ?? go.AddComponent<TweenDriver>();
            td.Duration = duration;
            td.ResetEase(td._ease);
            td.ResetLoop(td._loop);
            td.ResetDirection(td._direction);
            td.Reset();
            td.Stopping = null;
            td.enabled = true;
            return td;
        }

        public static TweenDriver Play<T, T2>(GameObject go, float duration, T from, T to, params GameObject[] targets) where T2 : Tween<T>
        {
            var tb = Play(go, duration);
            if (targets != null)
            {
                foreach (var tg in targets)
                {
                    var ts = tg.GetComponents<T2>();
                    if (ts == null || ts.Length < 1)
                    {
                        ts = new T2[] { tg.AddComponent<T2>() };
                    }

                    foreach (var t in ts)
                    {
                        t.From = from;
                        t.To = to;

                        if (!tb.Targets.Contains(t))
                        {
                            tb.Targets.Add(t);
                        }
                    }
                }
            }
            return tb;
        }
    }
}