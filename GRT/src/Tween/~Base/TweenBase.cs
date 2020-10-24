//************************************************************//
//      Author      :       GF47
//      DataTime    :       2014/3/2 2:44:32
//      Edited      :       2014/3/2 2:44:32
//************************************************************//

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GRT.Tween
{
    /// <summary>
    /// Author              :GF47
    /// DataTime            :2014/3/2 2:44:32
    /// Introduction        :缓动基类
    /// </summary>
    public class TweenBase : MonoBehaviour, IPercent
    {
        /// <summary>
        /// 公共组
        /// </summary>
        public const int PUBLIC_GROUP = 0;

        /// <summary>
        /// 缓动方式
        /// </summary>
        public TweenEase easeType = TweenEase.Linear;
        /// <summary>
        /// 循环方式
        /// </summary>
        public TweenLoop loopType = TweenLoop.Once;

        /// <summary>
        /// 是否使用真实时间
        /// </summary>
        public bool useRealTime = true;

        /// <summary>
        /// 延迟
        /// </summary>
        public float delay = 0.0f;

        /// <summary>
        /// 缓动一次持续的时间
        /// </summary>
        public float duration = 1.0f;

        /// <summary>
        /// 当一个GameObject上有不同的[TweenBase]脚本的时候进行的分组
        /// </summary>
        public int tweenGroup = 1;

        /// <summary>
        /// 是否是在LateUpdate中更新
        /// </summary>
        public bool isLateUpdate = false;

        /// <summary>
        /// 完成一个缓动行为后的回调
        /// </summary>
        public Action<TweenBase> OnFinished;

        public UnityEvent OnFinishedUEvent;

        public List<MonoBehaviour> _iPercentTargets;
        public List<IPercent> targets;

        private bool _started;
        private float _startTime;
        private float _duration;
        private float _amountPerDelta = 1.0f;
        private float _factor;
        private Factor _result;

        private EaseAlgorithm _ease;
        private DirectionAlgorithm _direction;
        private LoopAlgorithm _loop;

        float IPercent.Percent
        {
            get { return _result.factor; }
            set { _result.factor = value; }
        }

        void Awake()
        {
            if (_iPercentTargets != null)
            {
                targets = new List<IPercent>(_iPercentTargets.Count);
                for (int i = 0; i < _iPercentTargets.Count; i++)
                {
                    IPercent temp = _iPercentTargets[i] as IPercent;
                    if (temp != null)
                    {
                        targets.Add(temp);
                    }
                }
                //_iPercentTargets.Clear();
            }
            else
            {
                targets = new List<IPercent>();
            }
        }
        void Start()
        {
            ResetAlgorithm(easeType, loopType, TweenDirection.Forward);

            Update();
            LateUpdate();
        }
        void Update()
        {
            if (!isLateUpdate)
            {
                float delta = useRealTime ? Updater.MonoUpdater.RealDelta : Time.deltaTime;
                float time = useRealTime ? Updater.MonoUpdater.RealTime : Time.time;

                Update_(delta, time);
            }
        }

        void LateUpdate()
        {
            if (isLateUpdate)
            {
                float delta = useRealTime ? Updater.MonoUpdater.RealLateDelta : Time.deltaTime;
                float time = useRealTime ? Updater.MonoUpdater.RealLateTime : Time.time;
                Update_(delta, time);
            }
        }

        private void Update_(float delta, float time)
        {
            if (!_started)
            {
                _started = true;
                _startTime = time + delay;
            }
            if (time < _startTime) return;

            _factor += GetAmountPerDelta() * delta;
            _result = _loop.Result(_factor);
            _result.factor = _direction.Result(_result.factor);
            _result.factor = _ease.Result(_result.factor);

            _factor -= Mathf.Floor(_factor);

            Sample(_result.factor, _result.isFinished);
        }

        protected void Sample(float factor, bool finished)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[0] != null)
                {
                    targets[i].Percent = factor;
                }
            }
            if (finished)
            {
                OnFinishedBehaviour();
                _result.isFinished = false;
                enabled = false;
            }
        }

        protected void OnFinishedBehaviour()
        {
            OnFinished?.Invoke(this);
            OnFinishedUEvent.Invoke();
        }

        protected float GetAmountPerDelta()
        {
            if (Math.Abs(_duration - duration) > 0.0001f)
            {
                _duration = duration;
                _amountPerDelta = _duration > 0 ? (1.0f / _duration) : 1000f;
            }
            return _amountPerDelta;
        }

        public void ResetAlgorithm(TweenEase ease, TweenLoop loop, TweenDirection dir)
        {
            if (_ease == null) { _ease = new EaseAlgorithm(ease); }
            else { _ease.EaseType = ease; }
            easeType = ease;

            if (_loop == null) { _loop = new LoopAlgorithm(loop); }
            else { _loop.LoopType = loop; }
            loopType = loop;

            if (_direction == null) { _direction = new DirectionAlgorithm(dir); }
            else { _direction.DirectionType = dir; }
        }

        public void Reset(TweenDirection direction, bool resetDelay)
        {
            ResetAlgorithm(easeType, loopType, direction);
            _factor = 0.0f;
            if (resetDelay)
            {
                _started = false;
            }
        }

        public float ToggleFactor()
        {
            _factor = 1.0f - _factor;
            return _factor;
        }

        public void Play(TweenDirection direction)
        {
            ResetAlgorithm(easeType, loopType, direction);
            enabled = true;
        }
        public void Play(int direction)
        {
            Play((TweenDirection)direction);
        }
        public void Play(bool direction)
        {
            Play(direction ? TweenDirection.Forward : TweenDirection.Backward);
        }

        public void ResetAndPlay(TweenDirection direction, bool resetDelay)
        {
            Reset(direction, resetDelay);
            enabled = true;
        }
        public void ResetAndPlay(int direction, bool resetDelay)
        {
            Reset((TweenDirection)direction, resetDelay);
            enabled = true;
        }
        public void ResetAndPlay(bool direction, bool resetDelay)
        {
            Reset(direction ? TweenDirection.Forward : TweenDirection.Backward, resetDelay);
            enabled = true;
        }

        public static TweenBase Begin(GameObject go, float duration)
        {
            TweenBase temp = go.GetComponent<TweenBase>() ?? go.AddComponent<TweenBase>();
            temp._started = false;
            temp.duration = duration;
            temp._factor = 0.0f;
            temp._amountPerDelta = Mathf.Abs(temp.GetAmountPerDelta());
            temp.ResetAlgorithm(temp.easeType, TweenLoop.Once, TweenDirection.Forward);
            temp.OnFinished = null;
            temp.enabled = true;
            return temp;
        }
        public static TweenBase Begin<T, T2>(float duration, T from, T to, GameObject root, params GameObject[] targets)
            where T2 : Tween<T>
        {
            TweenBase tb = Begin(root, duration);
            if (targets != null)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    T2 t = targets[i].GetComponent<T2>() ?? targets[i].AddComponent<T2>();

                    t.from = from;
                    t.to = to;
                    if (!tb.targets.Contains(t))
                    {
                        tb.targets.Add(t);
                    }
                }
                if (duration <= 0f)
                {
                    tb.Sample(1.0f, true);
                    tb.enabled = false;
                }
            }
            return tb;
        }
    }
}

