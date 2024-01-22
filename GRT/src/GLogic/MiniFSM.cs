using System;
using System.Collections.Generic;

namespace GRT.GLogic
{
    public interface IMiniState<T> : ICell<T>
    {
        void Enter(T args);

        void Exit(T args);
    }

    public class MiniState<T> : IMiniState<T>
    {
        public event Action<T> Entering;

        public event Action<T> Exiting;

        public int ID { get; private set; }

        public void Enter(T args) => Entering?.Invoke(args);

        public void Exit(T args) => Exiting?.Invoke(args);
    }

    public class MiniFSM<T>
    {
        private IMiniState<T> _current;

        private readonly Dictionary<IMiniState<T>, ICollection<KeyValuePair<int, ICondition<T>>>> _states = new Dictionary<IMiniState<T>, ICollection<KeyValuePair<int, ICondition<T>>>>();

        public IReadOnlyDictionary<IMiniState<T>, ICollection<KeyValuePair<int, ICondition<T>>>> States => _states;

        public IMiniState<T> Current
        {
            get => _current;
        }

        public void Update(T args)
        {
            if (Current == null)
            {
                foreach (var pair in States)
                {
                    if (pair.Key != null)
                    {
                        _current = pair.Key;
                        _current.Enter(args);

                        break;
                    }
                }

                if (Current == null)
                {
                    throw new Exception("there is no states in the fsm");
                }
            }

            var transitions = States[Current];

            foreach (var transition in transitions)
            {
                var condition = transition.Value;
                if (condition == null || condition.IsTrue(args))
                {
                    var state = GetState(transition.Key);
                    _current.Exit(args);
                    _current = state;
                    _current.Enter(args);

                    break;
                }
            }
        }

        private IMiniState<T> GetState(int id)
        {
            foreach (var state in States.Keys)
            {
                if (state.ID == id)
                {
                    return state;
                }
            }
            throw new Exception($"there is not a state that id is {id}");
        }

        public void Add(IMiniState<T> state, params (int, ICondition<T>)[] transitions)
        {
            if (_states.TryGetValue(state, out var dict))
            {
                foreach (var (id, cond) in transitions)
                {
                    dict.Add(new KeyValuePair<int, ICondition<T>>(id, cond));
                }
            }
            else
            {
                dict = new Dictionary<int, ICondition<T>>();
                foreach (var (id, cond) in transitions)
                {
                    dict.Add(new KeyValuePair<int, ICondition<T>>(id, cond));
                }
                _states.Add(state, dict);
            }
        }

        public void Add(IMiniState<T> state, params (int, Predicate<T>)[] transitions)
        {
            if (_states.TryGetValue(state, out var dict))
            {
                foreach (var (id, predicate) in transitions)
                {
                    dict.Add(new KeyValuePair<int, ICondition<T>>(id, new Predication<T>(predicate)));
                }
            }
            else
            {
                dict = new Dictionary<int, ICondition<T>>();
                foreach (var (id, predicate) in transitions)
                {
                    dict.Add(new KeyValuePair<int, ICondition<T>>(id, new Predication<T>(predicate)));
                }

                _states.Add(state, dict);
            }
        }
    }
}